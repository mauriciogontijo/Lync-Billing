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
           
            Dictionary<string, object> updateStatementValues;
            string statusTimestamp = string.Empty;
            
            string column = string.Empty;
            string SQL = string.Empty;

            int dataRowCounter = 0;
            string lastMarkedPhoneCallDate = string.Empty;

            statusTimestamp = GetLastMarked(tablename);

            if (statusTimestamp == "N/A")
                SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename);
            else
                SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tablename, statusTimestamp);

            sourceDBConnector.Open();

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

                lastMarkedPhoneCallDate = phoneCall.SessionIdTime;

                //Update the CallMarkerStatus table fro this PhoneCall table.
                if (dataRowCounter % 10000 == 0)
                    UpdateCallMarkerStatus(tablename, "Marking", lastMarkedPhoneCallDate);

                dataRowCounter += 1;
            }

            UpdateCallMarkerStatus(tablename, "Marking", lastMarkedPhoneCallDate);

            //Close the database connection
            sourceDBConnector.Close();
        }

        public override void MarkExclusion(string tablename)
        {
            
        }
       
        public override void ApplyRates(string tableName)
        {
            PhoneCalls phoneCall;

            Dictionary<string, object> updateStatementValues;
            string statusTimestamp = string.Empty;

            string column = string.Empty;
            string SQL = string.Empty;

            int dataRowCounter = 0;
            string lastRateAppliedOnPhoneCall = string.Empty;

            statusTimestamp = GetLastAppliedRate(tableName);

            if (statusTimestamp == "N/A")
                SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tableName);
            else
                SQL = Misc.CREATE_READ_PHONE_CALLS_QUERY(tableName, statusTimestamp);

            sourceDBConnector.Open();

            dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

            string cost = Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost);
            string duration = Enums.GetDescription(Enums.PhoneCalls.Duration);
            string toGateway = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
            string callTypeID = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
            string callToCountry = Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry);

            while (dataReader.Read())
            {

                //Initialize the updateStatementValues variable
                updateStatementValues = new Dictionary<string, object>();

                //Fill the phoneCall Object
                phoneCall = Misc.FillPhoneCallFromOleDataReader(dataReader);

                //Call the SetType on the phoneCall Related table using class loader
                Type type = Type.GetType("Lync_Backend.Implementation." + tableName);
                string fqdn = typeof(Interfaces.IPhoneCalls).AssemblyQualifiedName;
                object instance = Activator.CreateInstance(type);

                //Call the correct set type
                ((Interfaces.IPhoneCalls)instance).ApplyRate(phoneCall);

                //Set the updateStatementValues dictionary items with the phoneCall instance variables
                updateStatementValues = Misc.ConvertPhoneCallToDictionary(phoneCall);

                //Update the phoneCall database record
                DBRoutines.UPDATE(tableName, updateStatementValues);

                lastRateAppliedOnPhoneCall = phoneCall.SessionIdTime;

                //Update the CallMarkerStatus table fro this PhoneCall table.
                if (dataRowCounter % 10000 == 0)
                    UpdateCallMarkerStatus(tableName, "ApplyingRates", lastRateAppliedOnPhoneCall);

              
            }//END-WHILE

            UpdateCallMarkerStatus(tableName, "ApplyingRates", lastRateAppliedOnPhoneCall);

            //Close the database connection
            sourceDBConnector.Close();
        }

        public override void ResetPhoneCallsAttributes(string tablename)
        {
            
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

        private void UpdateCallMarkerStatus(string phoneCallTable, string type, string timestamp)
        {
            Dictionary<string, object> whereClause;

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
                whereClause = new Dictionary<string, object>
                {
                    {Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable), phoneCallTable}
                };

                DBRoutines.UPDATE(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData, whereClause);
            }
        }
    }
}
