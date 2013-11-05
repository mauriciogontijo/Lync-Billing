using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Configuration;


namespace Lync_Backend.Helpers
{
    class CreateDBStatistics
    {
        private OleDbDataReader dataReader;
        private OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

        //TODO: every Get_ChargeableCalls function should return a new column represents which table the phone call object is located 

        #region Chargeable Calls Functions
        
        //Returns List of Chargeable Calls For A specific User
        public static void Get_ChargeableCalls_ForUser() { }

        //Returns List of Chargeable Calls For users in Specific Site
        public static void Get_ChargeableCalls_ForSite() { }

        //Returns List of chargeable Calls for  users specific deparment in a specific Site
        public static void Get_ChargeableCalls_ForSiteDepartment() { }
        
        #endregion


        #region Invoiced Calls Function
        
        //Returns List of invoiced Calls For A specific User
        public static void Get_ChargedCalls_ForUser() { }

        //Returns List of invoiced Calls For users in Specific Site
        public static void Get_ChargedCalls_ForSite() { }

        //Returns List of invoiced Calls for  users specific deparment in a specific Site
        public static void Get_ChargedCalls_ForSiteDepartment() { }

        #endregion


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


        #region Destinations Numbers Functions

        //Get Destinations with duration/cost/count Per User
        public static void Get_DestinationsNumbers_ForUser() { }

        //Get Destinations with duration/cost/count Per Site
        public static void Get_DestinationsNumbers_ForSite() { }

        //Get Destinations with duration/cost/count Per Department Per Site
        public static void Get_DestinationsNumbers_ForSiteDepartment() { }

        #endregion


        #region Calls Summaries Functions

        //Get Calls Summary Per User
        public static void Get_CallsSummary_ForUser() { }

        //Get Calls Summary for Users Per Site
        public static void Get_CallsSummary_ForUsers_PerSite() { }

        //Get Calls Summary for Users in a department in a site
        public static void Get_CallsSummary_ForUsers_PerSiteDepartment() { }

        //Get Calls Summary for A Department in A Site
        public static void Get_CallsSummary_ForSiteDepartment() { }

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

        //Get Gateways Summary(Total Duration/Cost/Count) for users in a department per site
        public static void Get_GatewaySummary_ForUsers_PerSiteDepartment() { }
        
        #endregion
    }
}
