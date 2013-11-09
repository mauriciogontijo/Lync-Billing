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

        private static void CreateOrAlterFunction(string functionName, string SQLStatement)
        {
            string functionCreateUpdateQuery = string.Empty;
            string QueryType = string.Empty;
            string FunctionVariables = string.Empty;

            // SQL Statement to Check if the function already exisits in the Database or not and it will return empty string of not
            string sqlValidationQuery = string.Format("select OBJECT_ID('{0}')", functionName);

            using (OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString))
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

                    if (SQLStatement.Contains(@"@DepartmentName") && SQLStatement.Contains(@"@OfficeName"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @DepartmentName nvarchar(450) ");
                    }
                    else if (SQLStatement.Contains(@"@OfficeName"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName	nvarchar(450)");
                    }
                    else if (SQLStatement.Contains(@"@SipAccount"))
                    {
                        FunctionVariables = string.Format("\t @SipAccount	nvarchar(450)");
                    }
                 

                    functionCreateUpdateQuery =
                           string.Format("{0} FUNCTION [dbo].[{1}] \r\n" +
                                         "( \r\n" +
                                         "{2}\r\n" +
                                         ") \r\n" +
                                         "RETURNS TABLE \r\n" +
                                         "AS \r\n" +
                                         "RETURN \r\n" +
                                         "(\r\n" +
                                         "{3} \r\n" +
                                         ") "
                                         , QueryType, functionName, FunctionVariables, SQLStatement);

                    comm.CommandText = functionCreateUpdateQuery;
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string x = string.Empty;
                }
            }

        }

        #region Chargeable Calls Functions        
       
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
            
            sqlStatement.Remove(sqlStatement.Length - 12, 12);

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
            
        }

        //High  1
        //Returns List of Chargeable Calls For users in Specific Site
        public static void Get_ChargeableCalls_ForSite() { }

        
        #endregion
  
        #region Destinations Countries Summaries Functions

        // High 2
        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationsSummary_ForUser() { }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationsSummary_ForSite() { }
        
        // High 3
        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationsSummary_ForSiteDepartment() { }

        //Get Destinations with duration/cost/count  per gateway by cost
        public static void Get_DestinationsSummary_PerGateway() { }

        #endregion

        #region Destinations Numbers Functions

        //High 4
        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationsNumbers_ForUser() { }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationsNumbers_ForSite() { }

        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationsNumbers_ForSiteDepartment() { }

        #endregion

        #region Calls Summaries Functions

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
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsDuration) + "], \r\n" +
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
        public static void Get_CallsSummary_ForUsers_PerSite() 
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
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName COLLATE SQL_Latin1_General_CP1_CI_AS AND \r\n" +
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
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] COLLATE SQL_Latin1_General_CP1_CI_AS \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);

            
            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_UserID) + "] AS [" + Enums.GetDescription(Enums.Users.AD_UserID) + "], \r\n" +
			        "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "], \r\n" +
			        "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t (CAST(CAST(YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" +  @" + '/' + " +  "CAST(MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' +" + "CAST(1 AS VARCHAR) AS DATETIME)) AS Date, \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsCost) + "] \r\n" + 
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" +  Enums.GetDescription(Enums.UsersCallsSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "] COLLATE SQL_Latin1_General_CP1_CI_AS, \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_UserID) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "]COLLATE SQL_Latin1_General_CP1_CI_AS , \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "] COLLATE SQL_Latin1_General_CP1_CI_AS, \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] COLLATE SQL_Latin1_General_CP1_CI_AS, \r\n" +
                     "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n"+
                     "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                     "\t ORDER BY YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n",subSelect.ToString()
                ));

          

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Calls Summary for A Department in A Site
        public static void Get_CallsSummary_ForSiteDepartment()
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
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName COLLATE SQL_Latin1_General_CP1_CI_AS AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName COLLATE SQL_Latin1_General_CP1_CI_AS AND \r\n" +
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
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] COLLATE SQL_Latin1_General_CP1_CI_AS \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);


            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] COLLATE SQL_Latin1_General_CP1_CI_AS AS [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t (CAST(CAST(YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' + " + "CAST(MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' +" + "CAST(1 AS VARCHAR) AS DATETIME)) AS Date, \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.UnmarkedCallsCost) + "] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.UsersCallsSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "] COLLATE SQL_Latin1_General_CP1_CI_AS, \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] COLLATE SQL_Latin1_General_CP1_CI_AS, \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        #endregion

        #region Gateways Summaries Functions

        //Get Gateways Summary(Total Duration/Cost/Count) for a user
        public static void Get_GatewaySummary_PerUser() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for a site
        public static void Get_GatewaySummary_PerSite() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for users per site
        public static void Get_GatewaySummary_ForUsers_PerSite() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for department in a site
        public static void Get_GatewaySummary_PerSiteDepartment() { }
        
        #endregion

        #region Invoiced Calls Function

        //Returns List of invoiced Calls For A specific User
        public static void Get_ChargedCalls_ForUser() { }

        //Returns List of invoiced Calls For users in Specific Site
        public static void Get_ChargedCalls_ForSite() { }

        //Returns List of invoiced Calls for  users specific deparment in a specific Site
        public static void Get_ChargedCalls_ForSiteDepartment() { }

        #endregion

    }
}
