using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Interfaces;
using Lync_Backend.Helpers;
using System.Data.OleDb;
using System.Configuration;
using Lync_Backend.Libs;

namespace Lync_Backend.Implementation
{
    class CallMarker : AbDatabaseMarker
    {
        OleDbDataReader dataReader;
        OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

        private static DBLib DBRoutines = new DBLib();
       
        public override void MarkCalls(string tablename)
        {
            PhoneCalls phoneCall;
            string column = string.Empty;
            Dictionary<string, object> updateStatementValues;
            DateTime statusTimestamp;

            int dataRowsCounter = 0;
            string lastImportedPhoneCallDate;
            Dictionary<string, object> callMarkerStatusData;

            var markerStatus = CallMarkerStatus.GetCallMarkerStatus().SingleOrDefault(item => item.PhoneCallsTable == tablename && item.Type == "Marking");

            if (markerStatus != null)
            {
                statusTimestamp = markerStatus.Timestamp;
            }
            else
            {
                statusTimestamp = DateTime.MinValue;
            }

            sourceDBConnector.Open();

            if (statusTimestamp == DateTime.MinValue)
            {
                lastImportedPhoneCallDate = string.Empty;

                // Update phone calls from the begining by iterating through them from the start
                string SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename);

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                while (dataReader.Read())
                {
                    //Initialize the updateStatementValues variable
                    updateStatementValues = new Dictionary<string, object>();

                    //Fill the phoneCall Object
                    phoneCall = Misc.FillPhoneCallFromOleDataReader(dataReader);

                    //Call the SetType on the phoneCall Related table using class loader

                    Type type = Type.GetType("Lync_Backend.Implementation." + tablename);
                    string fqdn = typeof(Interfaces.IPhoneCalls).AssemblyQualifiedName;
                    object instance = Activator.CreateInstance(type);

                    //Call the correct set type
                    ((Interfaces.IPhoneCalls)instance).SetCallType(phoneCall);

                    //Set the updateStatementValues dictionary items with the phoneCall instance variables
                    updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);

                    //Update the phoneCall database record
                    DBRoutines.UPDATE(tablename, updateStatementValues);

                    lastImportedPhoneCallDate = phoneCall.SessionIdTime;

                    dataRowsCounter += 1;
                }
            }
            else
            {
                lastImportedPhoneCallDate = string.Empty;

                string SQL = Misc.CREATE_IMPORT_PHONE_CALLS_QUERY(Misc.ConvertDate(statusTimestamp));

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                while (dataReader.Read())
                {
                    //Initialize the updateStatementValues variable
                    updateStatementValues = new Dictionary<string, object>();

                    //Fill the phoneCall Object
                    phoneCall = Misc.FillPhoneCallFromOleDataReader(dataReader);

                    //Call the SetType on the phoneCall Related table using class loader

                    Type type = Type.GetType("Lync_Backend.Implementation." + tablename);
                    string fqdn = typeof(Interfaces.IPhoneCalls).AssemblyQualifiedName;
                    object instance = Activator.CreateInstance(type);

                    //Call the correct set type
                    ((Interfaces.IPhoneCalls)instance).SetCallType(phoneCall);

                    //Set the updateStatementValues dictionary items with the phoneCall instance variables
                    updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);

                    //Update the phoneCall database record
                    DBRoutines.UPDATE(tablename, updateStatementValues);

                    lastImportedPhoneCallDate = Misc.ConvertDate((DateTime)updateStatementValues[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]);
                }
            }


            //Close the database connection
            sourceDBConnector.Close();
        }


        public override void MarkExclusion(string tablename)
        {
            
        }


        public override void ApplyRates(string tableName)
        {
            string cost = Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost);
            string duration = Enums.GetDescription(Enums.PhoneCalls.Duration);
            string toGateway = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
            string callTypeID = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
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
            List<Gateways> ListofGateways = Gateways.GetGateways();

            //Get Gateway IDs from Gateways
            List<string> ListofGatewaysNames = ListofGateways.Select(item => item.GatewayName).ToList<string>();

            //Get Rates for those Gateways for that marker
            //Dictionary<string, List<Rates>> ratesPerGatway = Rates.GetAllGatewaysRatesDictionary();
            Dictionary<int, List<Rates>> ratesPerGatway = Rates.GetAllGatewaysRatesList();

            //Read the phone calls and apply the rates to them
            sourceDBConnector.Open();

            string SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tableName);

            dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

            while (dataReader.Read())
            {
                //Skip this step in the loop if this PhoneCall record is not rates-appliant
                if (dataReader[toGateway] == DBNull.Value || dataReader[callToCountry].ToString() == "N/A" || !ListofChargeableCallTypes.Contains(Convert.ToInt32(dataReader[callTypeID])))
                {
                    continue;
                }

                //Begin processing this PhoneCall
                //Initialize the phoneCallRecord variable
                phoneCallRecord = Misc.FillDictionaryFromOleDataReader(dataReader);

                // Check if we can apply the rates for this phone-call
                var gateway = ListofGateways.Find(g => g.GatewayName == phoneCallRecord[toGateway].ToString());
                var rates = (from keyValuePair in ratesPerGatway where keyValuePair.Key == gateway.GatewayId select keyValuePair.Value).SingleOrDefault<List<Rates>>() ?? (new List<Rates>());

                if (rates.Count > 0)
                {
                    //Apply the rate for this phone call
                    var rate = (from r in rates
                                where r.CountryCode == phoneCallRecord[callToCountry].ToString()
                                select r).First();

                    //if the call is of type national/international MOBILE then apply the Mobile-Rate, otherwise apply the Fixedline-Rate
                    phoneCallRecord[cost] = ((int)phoneCallRecord[callTypeID] == 3 || (int)phoneCallRecord[callTypeID] == 5) ?
                            Math.Ceiling(Convert.ToDecimal(phoneCallRecord[duration]) / 60) * rate.MobileLineRate :
                            Math.Ceiling(Convert.ToDecimal(phoneCallRecord[duration]) / 60) * rate.FixedLineRate;

                    DBRoutines.UPDATE(tableName, phoneCallRecord);
                }
            }//END-WHILE


            //Close the database conenction
            sourceDBConnector.Close();
        }


        public override void ResetPhoneCallsAttributes(string tablename)
        {
            
        }
    }
}
