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

        //Returns List of Chargeable Calls For A specific User
        public static void Get_ChargeableCalls_ForUser() { }

        //Returns List of Chargeable Calls For users in Specific Site
        public static void Get_ChargeableCalls_ForSite() { }

        //Returns List of chargeable Calls for  users specific deparment in a specific Site
        public static void Get_ChargeableCalls_ForSiteDepartment() { }

        //Top Destinations by Cost Per User
        public static void Get_TopDestinations_ForUser_ByCost() { }

        //Top Destinations by Cost Per Site
        public static void Get_TopDestinations_ForSite_ByCost() { }

        //Top Destinations by Cost Per Department Per Site
        public static void Get_TopDestinations_ForSiteDepartment_ByCost() { }

        //Top Destinations By Count by Cost Per User
        public static void Get_TopDestinations_ForUser_ByCount() { }

        //Top Destinations By Count by Cost Per Site
        public static void Get_TopDestinations_ForSite_ByCount() { }

        //Top Destinations By Count by Cost Per Department Per Site
        public static void Get_TopDestinations_ForSiteDepartment_ByCount() { }

        //Get Calls Summary Per User
        public static void Get_CallsSummary_ForUser() { }

        //Get Calls Summary for Users Per Site
        public static void Get_CallsSummary_ForUsers_PerSite() { }

        //Get Calls Summary for Users in a department in a site
        public static void Get_CallsSummary_ForUsers_PerSiteDepartment() { }

        //Get Calls Summary for A Department in A Site
        public static void Get_CallsSummary_ForSiteDepartment() { }
    }
}
