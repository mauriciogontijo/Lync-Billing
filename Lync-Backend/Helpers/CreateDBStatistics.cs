using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Configuration;
using Lync_Backend.Libs;
using System.Reflection;



namespace Lync_Backend.Helpers
{
    class CreateDBStatistics
    {
        
        private static OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

        //TODO: every Get_ChargeableCalls function should return a new column represents which table the phone call object is located 
       
      
        #region Chargeable Calls Functions
        
        private static void CreateOrAlterFunction(string functionName,string SQLStatement)
        {
            string functionCreateUpdateQuery = string.Empty;
            string QueryType = string.Empty;

            // SQL Statement to Check if the function already exisits in the Database or not and it will return empty string of not
            string sqlValidationQuery = string.Format("select OBJECT_ID('{0}')", functionName);

            using (sourceDBConnector)
            {
                OleDbCommand comm = new OleDbCommand(sqlValidationQuery, sourceDBConnector);

                try
                {
                    sourceDBConnector.Open();

                    string result = comm.ExecuteScalar().ToString();

                    //Define the query Type
                    if (!string.IsNullOrEmpty(result))
                        QueryType = "ALTER";
                    else
                        QueryType = "CREATE";

                    functionCreateUpdateQuery =
                           string.Format("{0} FUNCTION [dbo].[{1}] \r\n"+ 
                                         "( \r\n" +
                                         "\t @SipAccount	nvarchar(450)\r\n" +
                                         ") \r\n" +
                                         "RETURNS TABLE \r\n" + 
                                         "AS \r\n" + 
                                         "RETURN \r\n" +
                                         "(\r\n" + 
                                         "{2} \r\n"+
                                         ") "
                                         , QueryType, functionName, SQLStatement);

                    comm.CommandText = functionCreateUpdateQuery;
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }

        }

        //Returns List of Chargeable Calls For A specific User
        public static void Get_ChargeableCalls_ForUser()
        {

            StringBuilder sqlStatement = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            //convert BillableCallTypesIds to strings 1,2,3,4,5 ...etc
            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement = 
                string.Format(
                    "WHERE " + 
                    "[" +Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "]=@SipAccount COLLATE SQL_Latin1_General_CP1_CI_AS AND " + 
                    "[" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0})"
                    , BillableCallTypesIdsList);
           
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                sqlStatement.Append(
                    string.Format(
                        "\t SELECT *,'" + ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable + "' AS " + Enums.GetDescription(Enums.PhoneCalls.PhoneCallsTableName) + "\r\n" + 
                        "\t FROM [{0}] \r\n"+
                        "\t {1} \r\n"+
                        "\t UNION \r\n ", 
                        ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }
            
            sqlStatement.Remove(sqlStatement.Length - 9, 9);

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
            
        }

        //Returns List of Chargeable Calls For users in Specific Site
        public static void Get_ChargeableCalls_ForSite() { }

        //Returns List of chargeable Calls for  users specific deparment in a specific Site
        public static void Get_ChargeableCalls_ForSiteDepartment() { }
        
        #endregion

        //STEP 2
        #region Invoiced Calls Function

        //High Priority

        //Returns List of invoiced Calls For A specific User
        public static void Get_ChargedCalls_ForUser() { }

        //Returns List of invoiced Calls For users in Specific Site
        public static void Get_ChargedCalls_ForSite() { }

        //Returns List of invoiced Calls for  users specific deparment in a specific Site
        public static void Get_ChargedCalls_ForSiteDepartment() { }

        #endregion

        //STEP 5
        #region Destinations Countries Summaries Functions

        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationsSummary_ForUser() { }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationsSummary_ForSite() { }

        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationsSummary_ForSiteDepartment() { }

        //Get Destinations with duration/cost/count  per gateway by cost
        public static void Get_DestinationsSummary_PerGateway() { }

        #endregion

        //STEP 6
        #region Destinations Numbers Functions

        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationsNumbers_ForUser() { }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationsNumbers_ForSite() { }

        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationsNumbers_ForSiteDepartment() { }

        #endregion

        //STEP 3
        #region Calls Summaries Functions

        //High Priority

        //Get Calls Summary Per User
        public static void Get_CallsSummary_ForUser() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            //convert BillableCallTypesIds to strings 1,2,3,4,5 ...etc
            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE " +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "]=@SipAccount COLLATE SQL_Latin1_General_CP1_CI_AS AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) \r\n" 
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 11, 11);

            
            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsDuartion) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsCost) + "] \r\n" + 
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" +  Enums.GetDescription(Enums.UsersCallsSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "] COLLATE SQL_Latin1_General_CP1_CI_AS, \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n"+
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n",subSelect.ToString()
                ));

          

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Calls Summary for Users Per Site
        public static void Get_CallsSummary_ForUsers_PerSite() { }

        //Get Calls Summary for Users in a department in a site
        public static void Get_CallsSummary_ForUsers_PerSiteDepartment() { }

        //Get Calls Summary for A Department in A Site
        public static void Get_CallsSummary_ForSiteDepartment() { }

        #endregion

        //STEP 4
        #region Gateways Summaries Functions

        //Get Gateways Summary(Total Duration/Cost/Count) for a user
        public static void Get_GatewaySummary_PerUser() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for a site
        public static void Get_GatewaySummary_PerSite() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for users per site
        public static void Get_GatewaySummary_ForUsers_PerSite() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for department in a site
        public static void Get_GatewaySummary_PerSiteDepartment() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for users in a department per site
        public static void Get_GatewaySummary_ForUsers_PerSiteDepartment() { }
        
        #endregion
    }
}
