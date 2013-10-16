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
    class CallMarker_Lync2010 : ICallMarker
    {
        OleDbDataReader dataReader;
        OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

        private static DBLib DBRoutines = new DBLib();


        public void MarkCalls(string tableName)
        {
           
        }

        public void MarkExclusion(string tableName)
        {
            
        }

        public void ApplyRates(string tableName)
        {
            public PhoneCall phoneCall;

            var markerStatus = CallMarkerStatus.GetCallMarkerStatus().Where(item => item.PhoneCallsTable == "PhoneCalls2010").First() as CallMarkerStatus;

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

            sourceDBConnector.Open();

            if (markerStatus.Timestamp == DateTime.MinValue || markerStatus.Timestamp == null)
            {
                // Update phone calls from the begining by iterating through them from the start
                string SQL = Misc.CREATE_IMPORT_PHONE_CALLS_QUERY();

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                while (dataReader.Read())
                {
                    //Apply Rate
                    phoneCall = new PhoneCall();
                }



            }
            else 
            {
                string SQL = Misc.CREATE_IMPORT_PHONE_CALLS_QUERY(Misc.ConvertDate(markerStatus.Timestamp));

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                while (dataReader.Read())
                {
                    phoneCall = new PhoneCall();
                    //Apply Rate
                }
            
            }
        }

        public void ResetPhoneCallsAttributes(string tableName)
        {
            
        }
    }
}
