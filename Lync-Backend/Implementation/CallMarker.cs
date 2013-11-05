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

        public override void MarkCalls(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {

            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;

            PhoneCalls phoneCall;
           
            Dictionary<string, object> updateStatementValues;
            string statusTimestamp = string.Empty;
            
            string column = string.Empty;
            string SQL = string.Empty;

            int dataRowCounter = 0;
            string lastMarkedPhoneCallDate = string.Empty;

            bool saveState = false;


            //Call the SetType on the phoneCall Related table using class loader
            Type type = Type.GetType("Lync_Backend.Implementation." + tablename);
            string fqdn = typeof(Interfaces.IPhoneCalls).AssemblyQualifiedName;
            object instance = Activator.CreateInstance(type);

            statusTimestamp = GetLastMarked(tablename);

            if (DateTime.Compare(from, DateTime.MinValue) != 0 || DateTime.Compare(to, DateTime.MaxValue) != 0 || !string.IsNullOrEmpty(gateway))
            {
                if (DateTime.Compare(from, DateTime.MinValue) != 0)
                    from = from.AddYears(1799);

                if (DateTime.Compare(to, DateTime.MaxValue) != 0)
                    to = to.AddDays(-1);

                SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename, gateway, from, to);

                saveState = false;
            }
            else 
            {
                if (statusTimestamp == "N/A")
                    SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename);
                else
                    SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename, statusTimestamp);

                saveState = true;
            }

            sourceDBConnector.Open();

            dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

            while (dataReader.Read())
            {
                //Initialize the updateStatementValues variable
                updateStatementValues = new Dictionary<string, object>();

                //Fill the phoneCall Object
                phoneCall = Misc.FillPhoneCallFromOleDataReader(dataReader);

                //Call the correct set type
                ((Interfaces.IPhoneCalls)instance).SetCallType(phoneCall);


                //Set the updateStatementValues dictionary items with the phoneCall instance variables
                updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);

                //Update the phoneCall database record
                DBRoutines.UPDATE(tablename, updateStatementValues);

                lastMarkedPhoneCallDate = phoneCall.SessionIdTime;

                //Update the CallMarkerStatus table fro this PhoneCall table.
                if (dataRowCounter % 10000 == 0)
                    UpdateCallMarkerStatus(tablename, "Marking", lastMarkedPhoneCallDate, saveState);

                dataRowCounter += 1;
            }

            UpdateCallMarkerStatus(tablename, "Marking", lastMarkedPhoneCallDate, saveState);

            //Close the database connection
            sourceDBConnector.Close();
        }

        public override void MarkExclusion(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {
            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;
            //TODO: manipluate exclusions 
        }

        public override void ApplyRates(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {
            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;

            PhoneCalls phoneCall;

            Dictionary<string, object> updateStatementValues;
            
            string statusTimestamp = string.Empty;
            string lastRateAppliedOnPhoneCall = string.Empty;
            string column = string.Empty;
            string SQL = string.Empty;

            int dataRowCounter = 0;
            
            bool saveState = false;
           
            //Call the SetType on the phoneCall Related table using class loader
            Type type = Type.GetType("Lync_Backend.Implementation." + tablename);
            string fqdn = typeof(Interfaces.IPhoneCalls).AssemblyQualifiedName;
            object instance = Activator.CreateInstance(type);

            statusTimestamp = GetLastAppliedRate(tablename);

           if (DateTime.Compare(from, DateTime.MinValue) != 0 || DateTime.Compare(to, DateTime.MaxValue) != 0 || !string.IsNullOrEmpty(gateway))
            {
                if (DateTime.Compare(from, DateTime.MinValue) != 0)
                    from = from.AddYears(1799);

                if (DateTime.Compare(to, DateTime.MaxValue) != 0)
                    to = to.AddDays(-1);

                SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename, gateway, from, to);

                saveState = false;
            }
            else 
            {
                if (statusTimestamp == "N/A")
                    SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename);
                else
                    SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename, statusTimestamp);

                saveState = true;
            }

            sourceDBConnector.Open();

            dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

            while (dataReader.Read())
            {
                //Initialize the updateStatementValues variable
                updateStatementValues = new Dictionary<string, object>();

                //Fill the phoneCall Object
                phoneCall = Misc.FillPhoneCallFromOleDataReader(dataReader);

                //Call the correct set type
                ((Interfaces.IPhoneCalls)instance).ApplyRate(phoneCall);

                //Set the updateStatementValues dictionary items with the phoneCall instance variables
                updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);

                //Update the phoneCall database record
                DBRoutines.UPDATE(tablename, updateStatementValues);

                lastRateAppliedOnPhoneCall = phoneCall.SessionIdTime;

                //Update the CallMarkerStatus table fro this PhoneCall table.
                if (dataRowCounter % 10000 == 0)
                    UpdateCallMarkerStatus(tablename, "ApplyingRates", lastRateAppliedOnPhoneCall, saveState);

                dataRowCounter += 1;

            }//END-WHILE

            UpdateCallMarkerStatus(tablename, "ApplyingRates", lastRateAppliedOnPhoneCall, saveState);

            //Close the database connection
            sourceDBConnector.Close();
        }

        public override void ResetPhoneCallsAttributes(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {
            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;
            //TODO: Reset marking or rates 
        }

        private string GetLastMarked(string phoneCallsTable) 
        {
            var status = CallMarkerStatus.GetCallMarkerStatus(phoneCallsTable, "Marking");

            if (status != null)
                return Misc.ConvertDate(status.Timestamp);
            else
                return "N/A";
        }

        private string GetLastAppliedRate(string phoneCallsTable) 
        {
            var status = CallMarkerStatus.GetCallMarkerStatus(phoneCallsTable, "ApplyingRates");

            if (status != null)
                return Misc.ConvertDate(status.Timestamp);
            else
                return "N/A";
        }

        private void UpdateCallMarkerStatus(string phoneCallTable, string type, string timestamp,bool Update=true)
        {
            if (Update == true)
            {
                Dictionary<string, object> callMarkerStatusData = new Dictionary<string, object>
                {
                    {Enums.GetDescription(Enums.CallMarkerStatus.Type), type},
                    {Enums.GetDescription(Enums.CallMarkerStatus.Timestamp), timestamp},
                    {Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable), phoneCallTable}

                };

                var existingMarkerStatus = CallMarkerStatus.GetCallMarkerStatus(phoneCallTable, type);

                if (existingMarkerStatus == null)
                {
                    DBRoutines.INSERT(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData);
                }
                else
                {
                    //DBRoutines.UPDATE(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData, whereClause);
                    DBRoutines.UPDATE(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData, Enums.GetDescription(Enums.CallMarkerStatus.MarkerId), existingMarkerStatus.MarkerId);
                }
            }
        }
    }
}
