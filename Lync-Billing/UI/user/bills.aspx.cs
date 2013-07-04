using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using Microsoft.Build.Tasks;
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;


namespace Lync_Billing.ui.user
{
    public partial class bills : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();
        private string sipAccount = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/bills.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != "user" && session.ActiveRoleName != "delegee")
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
        }

        protected void BillsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = userSession.EffectiveSipAccount;

            List<UsersCallsSummary> UserSummariesList = new List<UsersCallsSummary>();
            CultureInfo provider = CultureInfo.InvariantCulture;
            List<Dictionary<string, object>> BillsList = new List<Dictionary<string, object>>();
            Dictionary<string, object> Bill;

            int year = 2013,
                start_month = 1,
                end_month = DateTime.Now.Month;

            //if the end month is not the beginning of the year, decrease it by 1, for the purpose of not including the current month
            if (end_month != start_month) { end_month -= 1; }

            UserSummariesList = UsersCallsSummary.GetUsersCallsSummary(sipAccount, year, start_month, end_month);
            foreach (UsersCallsSummary summary in UserSummariesList)
            {
                Bill = new Dictionary<string, object>();
                Bill.Add("BillDate", summary.MonthDate);
                Bill.Add("Cost", summary.PersonalCallsCost);
                Bill.Add("TotalCalls", summary.PersonalCallsCount);
                Bill.Add("TotalDuration", summary.PersonalCallsDuration);

                BillsList.Add(Bill);
            }

            BillsStore.DataSource = BillsList;
            BillsStore.DataBind();
        }

        protected void BillsStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = userSession.EffectiveSipAccount;

            List<UsersCallsSummary> UserSummariesList = new List<UsersCallsSummary>();
            List<UsersCallsSummary> BillsList = new List<UsersCallsSummary>();

            int year = 2013,
                start_month = 1,
                end_month = DateTime.Now.Month;

            //if the end month is not the beginning of the year, decrease it by 1, for the purpose of not including the current month
            if (end_month != start_month) { end_month -= 1; }

            UserSummariesList = UsersCallsSummary.GetUsersCallsSummary(sipAccount, year, start_month, end_month);
            foreach(UsersCallsSummary summary in UserSummariesList)
            {
                BillsList.Add(summary);
            }

            BillsStore.DataSource = BillsList;
            BillsStore.DataBind();
        }
    }
}