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


                    if (SQLStatement.Contains(@"@OfficeName") && SQLStatement.Contains(@"@DepartmentName") && SQLStatement.Contains(@"@FromDate") && SQLStatement.Contains(@"@ToDate") && SQLStatement.Contains(@"@Limits"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @DepartmentName nvarchar(450), \r\n" + "\t @FromDate datetime, \r\n " + "\t @ToDate dateTime, \r\n " + "\t @Limits int ");
                    }
                    else if (SQLStatement.Contains(@"@OfficeName") &&   SQLStatement.Contains(@"@DepartmentName") && SQLStatement.Contains(@"@FromDate") && SQLStatement.Contains(@"@ToDate"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @DepartmentName nvarchar(450), \r\n" + "\t @FromDate datetime, \r\n " + "\t @ToDate dateTime ");
                    }
                    else if (SQLStatement.Contains(@"@OfficeName") && SQLStatement.Contains(@"@DepartmentName") && SQLStatement.Contains(@"@Limits"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @DepartmentName nvarchar(450), \r\n " + "\t @Limits int ");
                    }
                    else if (SQLStatement.Contains(@"@OfficeName") && SQLStatement.Contains(@"@FromDate") && SQLStatement.Contains(@"@ToDate") && SQLStatement.Contains(@"@Limits"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @FromDate datetime, \r\n " + "\t @ToDate dateTime, \r\n " + "\t @Limits int ");
                    }
                    else  if (SQLStatement.Contains(@"@OfficeName") && SQLStatement.Contains(@"@FromDate") && SQLStatement.Contains(@"@ToDate"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @FromDate datetime, \r\n " + "\t @ToDate dateTime ");
                    }
                    else if (SQLStatement.Contains(@"@OfficeName") && SQLStatement.Contains(@"@Limits"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @Limits int ");
                    }
                    else if (SQLStatement.Contains(@"@DepartmentName") && SQLStatement.Contains(@"@OfficeName"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName  nvarchar(450), \r\n" + "\t @DepartmentName nvarchar(450) ");
                    }
                    else if (SQLStatement.Contains(@"@OfficeName"))
                    {
                        FunctionVariables = string.Format("\t @OfficeName	nvarchar(450)");
                    }
                    else if (SQLStatement.Contains(@"@sipAccount") && SQLStatement.Contains(@"@FromDate") && SQLStatement.Contains(@"@ToDate") && SQLStatement.Contains(@"@Limits"))
                    {
                        FunctionVariables = string.Format("\t @SipAccount  nvarchar(450), \r\n" + "\t @FromDate datetime, \r\n " + "\t @ToDate dateTime, \r\n " + "\t @Limits int ");
                    }
                    else if (SQLStatement.Contains(@"@sipAccount") && SQLStatement.Contains(@"@FromDate") && SQLStatement.Contains(@"@ToDate"))
                    {
                        FunctionVariables = string.Format("\t @SipAccount  nvarchar(450), \r\n" + "\t @FromDate datetime, \r\n " + "\t @ToDate dateTime ");
                    }
                    else if (SQLStatement.Contains(@"@SipAccount") && SQLStatement.Contains(@"@Limits"))
                    {
                        FunctionVariables = string.Format("\t @SipAccount  nvarchar(450), \r\n" + "\t @Limits int ");
                    }
                    else if (SQLStatement.Contains(@"@SipAccount"))
                    {
                        FunctionVariables = string.Format("\t @SipAccount	nvarchar(450)");
                    }
                    else if (SQLStatement.Contains(@"@Gateway"))
                    {
                        FunctionVariables = string.Format("\t @Gateway	nvarchar(450)");
                    }
                    else if (SQLStatement.Contains(@"@RatesTableName"))
                    {
                        FunctionVariables = string.Format("\t @RatesTableName	nvarchar(450)");
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
                    "[" +Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "]=@SipAccount AND " + 
                    "[" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) \r\n" 
                    , BillableCallTypesIdsList);
           
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                sqlStatement.Append(
                    string.Format(
                        "\t SELECT *,'" + ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable + "' AS " + Enums.GetDescription(Enums.PhoneCalls.PhoneCallsTableName) + "\r\n" + 
                        "\t FROM [{0}] \r\n"+
                        "\t {1} \r\n"+
                        "\t UNION ALL \r\n ", 
                        ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }
            
            sqlStatement.Remove(sqlStatement.Length - 13, 13);

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
            
        }

        //Returns List of Chargeable Calls For users in Specific Site
        public static void Get_ChargeableCalls_ForSite()
        {
            StringBuilder sqlStatement = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            //convert BillableCallTypesIds to strings 1,2,3,4,5 ...etc
            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "WHERE \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IN \r\n" +
                    "\t\t ( \r\n" +
                    "\t\t\t SELECT [" + Enums.GetDescription(Enums.Gateways.GatewayName) + "] \r\n" +
                    "\t\t\t FROM [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "] \r\n" +
                    "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Gateways.TableName) + "] ON [" + Enums.GetDescription(Enums.Gateways.TableName) + "].[" + Enums.GetDescription(Enums.Gateways.GatewayId) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.GatewayID) + "] \r\n" +
                    "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Sites.TableName) + "] ON [" + Enums.GetDescription(Enums.Sites.TableName) + "].[" + Enums.GetDescription(Enums.Sites.SiteID) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.SiteID) + "] \r\n" +
                    "\t\t\t WHERE [" + Enums.GetDescription(Enums.Sites.SiteName) + "]=@OfficeName \r\n" +
                    "\t\t )" 
                    , BillableCallTypesIdsList);

            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                sqlStatement.Append(
                    string.Format(
                        "\t SELECT *,'" + ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable + "' AS " + Enums.GetDescription(Enums.PhoneCalls.PhoneCallsTableName) +  " \r\n" +
                        "\t FROM [{0}] \r\n" +
                        "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                        "\t {1} \r\n" +
                        "\t UNION ALL \r\n ",
                        ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            sqlStatement.Remove(sqlStatement.Length - 13, 13);

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        #endregion
  
        #region Destinations Countries Functions

        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationCountries_ForUser() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "]=@SipAccount AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN {1} AND {2}  \r\n"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                        "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.NumberingPlan.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTo) + "] =   [" + Enums.GetDescription(Enums.NumberingPlan.TableName) + "].[" + Enums.GetDescription(Enums.NumberingPlan.DialingPrefix) + "] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP (@Limits) [" + Enums.GetDescription(Enums.NumberingPlan.CountryName) + "], \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS CallsDuration, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS CallsCost, \r\n" +
                    "\t\t COUNT ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS CallsCount \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.NumberingPlan.CountryName) + "] \r\n" +
                    "\t ORDER BY CallsCount DESC", subSelect.ToString()
                ));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationCountries_ForSite() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN {1} AND {2}  \r\n"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                        "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.NumberingPlan.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTo) + "] =   [" + Enums.GetDescription(Enums.NumberingPlan.TableName) + "].[" + Enums.GetDescription(Enums.NumberingPlan.DialingPrefix) + "] \r\n" +
                        "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP (@Limits) [" + Enums.GetDescription(Enums.NumberingPlan.CountryName) + "], \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS CallsDuration, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS CallsCost, \r\n" +
                    "\t\t COUNT ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS CallsCount \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.NumberingPlan.CountryName) + "] \r\n" +
                    "\t ORDER BY CallsCount DESC", subSelect.ToString()
                ));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        
        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationCountries_ForSiteDepartment() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN {1} AND {2}  \r\n"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                        "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.NumberingPlan.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTo) + "] =   [" + Enums.GetDescription(Enums.NumberingPlan.TableName) + "].[" + Enums.GetDescription(Enums.NumberingPlan.DialingPrefix) + "] \r\n" +
                        "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP (@Limits) [" + Enums.GetDescription(Enums.NumberingPlan.CountryName) + "], \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS CallsDuration, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS CallsCost, \r\n" +
                    "\t\t COUNT ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS CallsCount \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.NumberingPlan.CountryName) + "] \r\n" +
                    "\t ORDER BY CallsCount DESC", subSelect.ToString()
                ));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Destinations with duration/cost/count  per gateway by cost
        public static void Get_DestinationCountries_PerGateway() { }

        #endregion

        #region Destinations Numbers Functions

        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationsNumbers_ForUser() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "]=@SipAccount AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN {1} AND {2}  \r\n"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP (@Limits) ISNULL([" + Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) + "], [" + Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri) + "]) AS PhoneNumber, \r\n" +
                    "\t\t  [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "] AS Country, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS CallsDuration, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS CallsCost, \r\n" +
                    "\t\t COUNT ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS CallsCount \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t ISNULL([" + Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) + "], [" + Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri) + "]), \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "] \r\n" +
                    "\t ORDER BY CallsCount DESC", subSelect.ToString()
                ));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationsNumbers_ForSite() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN {1} AND {2}  \r\n"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP (@Limits) ISNULL([" + Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) + "], [" + Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri) + "]) AS PhoneNumber, \r\n" +
                    "\t\t  [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "] AS Country, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS CallsDuration, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS CallsCost, \r\n" +
                    "\t\t COUNT ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS CallsCount \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t ISNULL([" + Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) + "], [" + Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri) + "]), \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "] \r\n" +
                    "\t ORDER BY CallsCount DESC", subSelect.ToString()
                ));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationsNumbers_ForSiteDepartment() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            Dictionary<string, MonitoringServersInfo> monInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

            string BillableCallTypesIdsList = string.Join(",", section.BillableTypesList);

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN {1} AND {2}  \r\n"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP (@Limits) ISNULL([" + Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) + "], [" + Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri) + "]) AS PhoneNumber, \r\n" +
                    "\t\t  [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "] AS Country, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS CallsDuration, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS CallsCost, \r\n" +
                    "\t\t COUNT ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS CallsCount \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t ISNULL([" + Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) + "], [" + Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri) + "]), \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) + "] \r\n" +
                    "\t ORDER BY CallsCount DESC", subSelect.ToString()
                ));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

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
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "]=@SipAccount AND \r\n" +
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
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);

            
            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost) + "] \r\n" + 
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" +  Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] ASC ,YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
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
                   "WHERE \r\n" +
                   "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                   "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                   "\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                   "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IN \r\n" +
                   "\t\t ( \r\n" +
                   "\t\t\t SELECT [" + Enums.GetDescription(Enums.Gateways.GatewayName) + "] \r\n" +
                   "\t\t\t FROM [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "] \r\n" +
                   "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Gateways.TableName) + "] ON [" + Enums.GetDescription(Enums.Gateways.TableName) + "].[" + Enums.GetDescription(Enums.Gateways.GatewayId) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.GatewayID) + "] \r\n" +
                   "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Sites.TableName) + "] ON [" + Enums.GetDescription(Enums.Sites.TableName) + "].[" + Enums.GetDescription(Enums.Sites.SiteID) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.SiteID) + "] \r\n" +
                   "\t\t\t WHERE [" + Enums.GetDescription(Enums.Sites.SiteName) + "]=@OfficeName \r\n" +
                   "\t\t )"
                   , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
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
			        "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "] AS [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "], \r\n" +
			        "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] AS [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t (CAST(CAST(YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" +  @" + '/' + " +  "CAST(MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' +" + "CAST(1 AS VARCHAR) AS DATETIME)) AS Date, \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost) + "] \r\n" + 
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" +  Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_UserID) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                     "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                     "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                     "\t ORDER BY YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n",subSelect.ToString()
                ));

          

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Calls Summary For Users in Site for PDF
        public static void Get_CallsSummary_ForUsers_PerSite_PDF() 
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
                   "WHERE \r\n" +
                   "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                   "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                   "\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                   "\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "] BETWEEN @FromDate AND @ToDate) AND \r\n" +
                   "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IN \r\n" +
                   "\t\t ( \r\n" +
                   "\t\t\t SELECT [" + Enums.GetDescription(Enums.Gateways.GatewayName) + "] \r\n" +
                   "\t\t\t FROM [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "] \r\n" +
                   "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Gateways.TableName) + "] ON [" + Enums.GetDescription(Enums.Gateways.TableName) + "].[" + Enums.GetDescription(Enums.Gateways.GatewayId) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.GatewayID) + "] \r\n" +
                   "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Sites.TableName) + "] ON [" + Enums.GetDescription(Enums.Sites.TableName) + "].[" + Enums.GetDescription(Enums.Sites.SiteID) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.SiteID) + "] \r\n" +
                   "\t\t\t WHERE [" + Enums.GetDescription(Enums.Sites.SiteName) + "]=@OfficeName \r\n" +
                   "\t\t )"
                   , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
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
			        "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "] AS [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "], \r\n" +
			        "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] AS [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost) + "] \r\n" + 
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" +  Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_UserID) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "], \r\n" +
                     "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] \r\n" +
                    "\t ORDER BY " +Enums.GetDescription(Enums.PhoneCalls.ChargingParty) +  " ASC \r\n",subSelect.ToString()
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
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName AND \r\n" +
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
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL\r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 13, 13);


            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t (CAST(CAST(YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' + " + "CAST(MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' +" + "CAST(1 AS VARCHAR) AS DATETIME)) AS Date, \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost) + "] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Calls Summary for A users for a specific Gateway
        public static void Get_CallsSummary_ForUsers_PerGateway() 
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
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "]=@Gateway AND \r\n" +
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
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
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
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "] AS [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "] AS [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t (CAST(CAST(YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' + " + "CAST(MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS varchar)" + @" + '/' +" + "CAST(1 AS VARCHAR) AS DATETIME)) AS Date, \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Business' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] = 'Personal' THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN 1 END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount) + "], \r\n" +
                    "\t\t SUM(CASE WHEN [" + Enums.GetDescription(Enums.PhoneCalls.UI_CallType) + "] IS NULL THEN [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "] END) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost) + "] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_UserID) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_DisplayName) + "]COLLATE SQL_Latin1_General_CP1_CI_AS , \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                     "\t ORDER BY YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        #endregion

        #region Gateways Summaries Functions

        //Get Gateways Summary(Total Duration/Cost/Count) for a user
        public static void Get_GatewaySummary_PerUser() 
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
                    "\t\t WHERE \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "]=@SipAccount AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IS NOT NULL"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);


            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS [CallsDuration], \r\n" +
                    "\t\t COUNT([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS [CallsCount], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS [CallsCost] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "], \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] ASC, YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());

        }

        //Get Gateways Summary(Total Duration/Cost/Count) for a site
        public static void Get_GatewaySummary_PerSite() 
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
                    "WHERE \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IN \r\n" +
                    "\t\t ( \r\n" +
                    "\t\t\t SELECT [" + Enums.GetDescription(Enums.Gateways.GatewayName) + "] \r\n" +
                    "\t\t\t FROM [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "] \r\n" +
                    "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Gateways.TableName) + "] ON [" + Enums.GetDescription(Enums.Gateways.TableName) + "].[" + Enums.GetDescription(Enums.Gateways.GatewayId) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.GatewayID) + "] \r\n" +
                    "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Sites.TableName) + "] ON [" + Enums.GetDescription(Enums.Sites.TableName) + "].[" + Enums.GetDescription(Enums.Sites.SiteID) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.SiteID) + "] \r\n" +
                    "\t\t\t WHERE [" + Enums.GetDescription(Enums.Sites.SiteName) + "]=@OfficeName \r\n" +
                    "\t\t )"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "]  \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);


            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS [CallsDuration], \r\n" +
                    "\t\t COUNT([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS [CallsCount], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS [CallsCost] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] ASC, YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        //Get Gateways Summary(Total Duration/Cost/Count) for a All site
        public static void Get_GatewaySummary_ForAll_Sites()
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
                    "\t\t WHERE \r\n" +
                   "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IS NOT NULL"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);


            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                     "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS [CallsDuration], \r\n" +
                    "\t\t COUNT([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS [CallsCount], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS [CallsCost] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] ASC, YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }


        //Get Gateways Summary(Total Duration/Cost/Count) for users per site
        public static void Get_GatewaySummary_ForUsers_PerSite() { }

        //Get Gateways Summary(Total Duration/Cost/Count) for department in a site
        public static void Get_GatewaySummary_PerSiteDepartment() 
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
                    "WHERE \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID) + "] in ({0}) AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.Exclude) + "]=0 AND \r\n" +
                    "\t\t ([" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "]='Rejected' OR [" + Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) + "] IS NULL ) AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] IN \r\n" +
                    "\t\t ( \r\n" +
                    "\t\t\t SELECT [" + Enums.GetDescription(Enums.Gateways.GatewayName) + "] \r\n" +
                    "\t\t\t FROM [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "] \r\n" +
                    "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Gateways.TableName) + "] ON [" + Enums.GetDescription(Enums.Gateways.TableName) + "].[" + Enums.GetDescription(Enums.Gateways.GatewayId) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.GatewayID) + "] \r\n" +
                    "\t\t\t\t LEFT JOIN [" + Enums.GetDescription(Enums.Sites.TableName) + "] ON [" + Enums.GetDescription(Enums.Sites.TableName) + "].[" + Enums.GetDescription(Enums.Sites.SiteID) + "] = [" + Enums.GetDescription(Enums.GatewaysDetails.TableName) + "].[" + Enums.GetDescription(Enums.GatewaysDetails.SiteID) + "] \r\n" +
                    "\t\t\t WHERE [" + Enums.GetDescription(Enums.Sites.SiteName) + "]=@OfficeName \r\n" +
                    "\t\t )"
                    , BillableCallTypesIdsList);

            //Sub Select Construction
            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monInfo)
            {
                subSelect.Append(
                   string.Format(
                       "\t\t SELECT * FROM [{0}] \r\n" +
                       "\t\t LEFT OUTER JOIN [" + Enums.GetDescription(Enums.Users.TableName) + "]  ON [{0}].[" + Enums.GetDescription(Enums.PhoneCalls.ChargingParty) + "] =   [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "]  \r\n" +
                       "{1} \r\n" +
                       "\t\t UNION ALL \r\n\r\n",
                       ((MonitoringServersInfo)keyValue.Value).PhoneCallsTable, whereStatement));
            }

            subSelect.Remove(subSelect.Length - 14, 14);


            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT\r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] AS [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Year], \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") AS [Month], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Duration) + "]) AS [CallsDuration], \r\n" +
                    "\t\t COUNT([" + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + "]) AS [CallsCount], \r\n" +
                    "\t\t SUM([" + Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) + "]) AS [CallsCost] \r\n" +
                    "\t FROM \r\n" +
                    "\t (\r\n" +
                    "{0} \r\n" +
                    "\t) AS [" + Enums.GetDescription(Enums.PhoneCallSummary.TableName) + "] \r\n" +
                    "\t GROUP BY \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "], \r\n" +
                    "\t\t YEAR(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + "), \r\n" +
                    "\t\t MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") \r\n" +
                    "\t ORDER BY [" + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + "] ASC, YEAR( " + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC, MONTH(" + Enums.GetDescription(Enums.PhoneCalls.ResponseTime) + ") ASC \r\n", subSelect.ToString()
                ));



            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }
        
        #endregion

        #region Invoiced Calls Function

        //Returns List of invoiced Calls For A specific User
        public static void Get_ChargedCalls_ForUser() { }

        //Returns List of invoiced Calls For users in Specific Site
        public static void Get_ChargedCalls_ForSite() { }

        //Returns List of invoiced Calls for  users specific deparment in a specific Site
        public static void Get_ChargedCalls_ForSiteDepartment() { }

        #endregion

        #region MISC

        //Get Mail Statistics Per Siet Department
        public static void Get_MailStatistics_ForUsers_PerSiteDepartment() 
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t WHERE \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.MailStatistics.TimeStamp) + "] BETWEEN @FromDate and @ToDate \r\n"
                    );

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.MailStatistics.TableName) + "].[" + Enums.GetDescription(Enums.MailStatistics.EmailAddress) + "], \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.ReceivedCount) + "]) AS RecievedCount, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.ReceivedSize) + "]) AS RecievedSize, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.SentCount) + "]) AS SentCount, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.SentSize) + "]) AS SentSize \r\n" +
                    "\t FROM [" + Enums.GetDescription(Enums.MailStatistics.TableName) + "] \r\n" +
                    "\t\t LEFT OUTER JOIN  [" + Enums.GetDescription(Enums.Users.TableName) + "] ON [" + Enums.GetDescription(Enums.MailStatistics.TableName) + "].[" + Enums.GetDescription(Enums.MailStatistics.EmailAddress) + "] = [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                    "{0} " +
                    "\t GROUP BY [" + Enums.GetDescription(Enums.MailStatistics.TableName) + "].[" + Enums.GetDescription(Enums.MailStatistics.EmailAddress) + "] \r\n"
                ,whereStatement));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        public static void Get_MailStatistics_PerSiteDepartment()
        {
            StringBuilder sqlStatement = new StringBuilder();
            StringBuilder subSelect = new StringBuilder();

            //Get WhereStatemnet and append it to every Select 
            string whereStatement =
                string.Format(
                    "\t WHERE \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName) + "]=@OfficeName AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.Users.AD_Department) + "]=@DepartmentName AND \r\n" +
                    "\t\t [" + Enums.GetDescription(Enums.MailStatistics.TimeStamp) + "] BETWEEN @FromDate and @ToDate \r\n"
                    );

            //Outer Select 
            sqlStatement.Append(
                string.Format(
                    "\t SELECT TOP 100 PERCENT \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.ReceivedCount) + "]) AS RecievedCount, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.ReceivedSize) + "]) AS RecievedSize, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.SentCount) + "]) AS SentCount, \r\n" +
                    "\t\t SUM ([" + Enums.GetDescription(Enums.MailStatistics.SentSize) + "]) AS SentSize \r\n" +
                    "\t FROM [" + Enums.GetDescription(Enums.MailStatistics.TableName) + "] \r\n" +
                    "\t\t LEFT OUTER JOIN  [" + Enums.GetDescription(Enums.Users.TableName) + "] ON [" + Enums.GetDescription(Enums.MailStatistics.TableName) + "].[" + Enums.GetDescription(Enums.MailStatistics.EmailAddress) + "] = [" + Enums.GetDescription(Enums.Users.TableName) + "].[" + Enums.GetDescription(Enums.Users.SipAccount) + "] \r\n" +
                    "{0} "
                   , whereStatement));

            CreateOrAlterFunction(MethodBase.GetCurrentMethod().Name, sqlStatement.ToString());
        }

        #endregion 
    }
}
