﻿using System;
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

        public override void MarkCalls(string tableName)
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
                    column = string.Empty;

                    //Apply Rate
                    phoneCall = new PhoneCall();
                    updateStatementValues = new Dictionary<string, object>();

                    phoneCall.SessionIdTime = Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]);
                    phoneCall.SessionIdSeq = (int)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)];

                    column = Enums.GetDescription(Enums.PhoneCalls.ResponseTime);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.ResponseTime = Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.ResponseTime)]);

                    column = Enums.GetDescription(Enums.PhoneCalls.SessionEndTime);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.SessionEndTime = Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionEndTime)]);

                    column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.SourceUserUri = dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceUserUri)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.SourceNumberUri = dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.DestinationUserUri = dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.DestinationNumberUri = dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.FromMediationServer = dataReader[Enums.GetDescription(Enums.PhoneCalls.FromMediationServer)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.ToMediationServer = dataReader[Enums.GetDescription(Enums.PhoneCalls.ToMediationServer)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.FromGateway = dataReader[Enums.GetDescription(Enums.PhoneCalls.FromGateway)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.ToGateway = dataReader[Enums.GetDescription(Enums.PhoneCalls.ToGateway)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.SourceUserEdgeServer = dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.DestinationUserEdgeServer = dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.ServerFQDN);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.ServerFQDN = dataReader[Enums.GetDescription(Enums.PhoneCalls.ServerFQDN)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.PoolFQDN);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.PoolFQDN = dataReader[Enums.GetDescription(Enums.PhoneCalls.PoolFQDN)].ToString();

                    column = Enums.GetDescription(Enums.PhoneCalls.Duration);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.Duration = Convert.ToDecimal(dataReader[column]);

                    column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
                    if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                        phoneCall.Marker_CallTypeID = Convert.ToInt32(dataReader[column]);


                    phoneCall = PhoneCall.SetCallType(phoneCall);
                    updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);


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


        public override void MarkExclusion(string tableName)
        {

        }


        public override void ApplyRates(string tableName)
        {
            ////Get Gateways for that Marker
            //List<Gateways> gateways = Gateways.GetGateways("Gateways2010");

            ////Get Gateway IDs from Gateways
            //List<int> gatewaysIds = gateways.Select(item => item.GatewayId).ToList<int>();

            ////Get Rates Tables for that marker
            //List<GatewaysRates> ratesTables = GatewaysRates.GetGatewaysRates().Where(item => gatewaysIds.Contains(item.GatewayID)).ToList<GatewaysRates>();

            //List<string> ratesTablesName = ratesTables.Select(item => item.RatesTableName).ToList<string>();


            ////Get Rates for those Gateways for that marker
            //Dictionary<string, List<Rates>> ratesPerGatway =
            //        Rates.GetAllGatewaysRates().
            //            Where(item => ratesTablesName.Contains(item.Key)).
            //            ToDictionary(p => p.Key, p => p.Value);

            ////Get Dialing Prefixes Information
            //List<NumberingPlan> numberingPlan = NumberingPlan.GetNumberingPlan();
        }


        public override void ResetPhoneCallsAttributes(string tableName)
        {

        }
    }
}