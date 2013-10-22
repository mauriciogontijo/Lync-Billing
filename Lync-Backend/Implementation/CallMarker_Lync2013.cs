using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.OleDb;
using Lync_Backend.Helpers;
using Lync_Backend.Libs;
using Lync_Backend.Interfaces;


namespace Lync_Backend.Implementation
{
    class CallMarker_Lync2013 : AbDatabaseMarker
    {
        OleDbDataReader dataReader;
        OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

        private static DBLib DBRoutines = new DBLib();

        public override string PhoneCallsTableName
        {
            get { return "PhoneCalls2013"; }
        }

        public override string GatewaysTableName
        {
            get { return "Gateways2013"; }
        }

        public override void MarkCalls()
        {
            PhoneCall phoneCall;
            string column = string.Empty;
            Dictionary<string, object> updateStatementValues;
            DateTime statusTimestamp;

            var markerStatus = CallMarkerStatus.GetCallMarkerStatus().Where(item => item.PhoneCallsTable == PhoneCallsTableName);

            if (markerStatus.Count() != 0)
            {
                statusTimestamp = ((CallMarkerStatus)markerStatus.First()).Timestamp;
            }
            else
            {
                statusTimestamp = DateTime.MinValue;
            }

            sourceDBConnector.Open();

            if (statusTimestamp == DateTime.MinValue || statusTimestamp == null)
            {
                // Update phone calls from the begining by iterating through them from the start
                string SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(PhoneCallsTableName);

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                while (dataReader.Read())
                {
                    //Initialize the updateStatementValues variable
                    updateStatementValues = new Dictionary<string, object>();

                    //Fill the phoneCall Object
                    var readerInstance = dataReader;
                    phoneCall = Misc.FillPhoneCallFromOleDataReader(readerInstance);

                    //Call the SetType on the phoneCall object
                    phoneCall = PhoneCall.SetCallType(phoneCall);

                    //Set the updateStatementValues dictionary items with the phoneCall instance variables
                    updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);

                    //Update the phoneCall database record
                    DBRoutines.UPDATE(PhoneCallsTableName, updateStatementValues);
                }
            }
            else
            {
                string SQL = Misc.CREATE_IMPORT_PHONE_CALLS_QUERY(Misc.ConvertDate(statusTimestamp));

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                while (dataReader.Read())
                {
                    phoneCall = new PhoneCall();
                    //Apply Rate
                }
            }
        }


        public override void MarkExclusion()
        {

        }


        public override void ApplyRates()
        {
            string cost = Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost);
            string callTypeID = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
            string toGateway = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
            string callToCountry = Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry);

            //This is the data container to hold the data of the phonecall row from the database.
            Dictionary<string, object> phoneCallRecord;

            /***
             * List of Chargeable Phonecalls Types include:
             * 1 = LOCAL PHONECALL
             * 2 = NATIONAL-FIXEDLINE
             * 3 = NATIONAL-MOBILE
             * 4 = INTERNATIONAL-FIXEDLINE
             * 5 = INTERNATIONAL-MOBILE
             */
            List<int> ListofChargeableCallTypes = new List<int>() { 1, 2, 3, 4, 5 };

            //Get Gateways for that Marker
            List<Gateways> ListofGateways = Gateways.GetGateways(GatewaysTableName);

            //Get Gateway IDs from Gateways
            List<int> ListofGatewaysIds = ListofGateways.Select(item => item.GatewayId).ToList<int>();

            //Get Rates Tables for that marker
            //List<GatewaysRates> ListofRatesTables = GatewaysRates.GetGatewaysRates().Where(item => ListofGatewaysIds.Contains(item.GatewayID)).ToList<GatewaysRates>();
            //List<string> ratesTablesName = ListofRatesTables.Select(item => item.RatesTableName).ToList<string>();
            
            //Get Rates for those Gateways for that marker
            Dictionary<int, List<Rates>> ratesPerGatway =
                    Rates.GetAllGatewaysRates();
                    //.Where(item => ratesTablesName.Contains(item.Key.ToString()));
                    //.ToDictionary(p => p.Key.ToString(), p => p.Value);

            //Get Dialing Prefixes Information
            List<NumberingPlan> numberingPlan = NumberingPlan.GetNumberingPlan();


            //Read the phone calls and apply the rates to them
            sourceDBConnector.Open();

            string SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(PhoneCallsTableName);

            dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

            while (dataReader.Read())
            {
                //Initialize the updateStatementValues variable
                phoneCallRecord = new Dictionary<string, object>();

                //Skip this step in the loop if this PhoneCall record is not rates-appliant
                if (dataReader[callTypeID] == DBNull.Value || string.IsNullOrEmpty(dataReader[callTypeID].ToString()) || 
                    dataReader[toGateway] == DBNull.Value || string.IsNullOrEmpty(dataReader[toGateway].ToString()) )
                {
                    continue;
                }
                else if ( ! ListofChargeableCallTypes.Contains(Convert.ToInt32(dataReader[callTypeID])) )
                {
                    continue;
                }

                //Begin processing this PhoneCall
                var readerInstance = dataReader;
                phoneCallRecord = Misc.FillDictionaryFromOleDataReader(readerInstance);

                // Check if we can apply the rates for this phone-call
                if (!string.IsNullOrEmpty(phoneCallRecord[toGateway].ToString()) && (int)phoneCallRecord[callTypeID] != 0)
                {
                    var gateway = ListofGateways.Find(g => g.GatewayName == phoneCallRecord[toGateway].ToString());

                    if (gateway != null)
                    {
                        var rates = (from keyValuePair in ratesPerGatway where keyValuePair.Key == gateway.GatewayId select keyValuePair.Value).SingleOrDefault<List<Rates>>() ?? (new List<Rates>());

                        if (rates.Count > 0 && ListofChargeableCallTypes.Contains((int)phoneCallRecord[callTypeID]))
                        {
                            var rate = (from r in rates
                                        where r.CountryCode == phoneCallRecord[callToCountry].ToString()
                                        select r).First();

                            //if the call is of type national/international fixedline then apply the Fixedline Rate, otherwise apply the Mobile Rate
                            phoneCallRecord[cost] = ((int)phoneCallRecord[callTypeID] == 1 || (int)phoneCallRecord[callTypeID] == 2 || (int)phoneCallRecord[callTypeID] == 4) ? rate.FixedLineRate : rate.MobileLineRate;

                            DBRoutines.UPDATE(PhoneCallsTableName, phoneCallRecord);
                        }
                    }
                }
            }//END-WHILE


            //Close the database conenction
            sourceDBConnector.Close();
        }


        public override void ResetPhoneCallsAttributes()
        {

        }
    }
}
