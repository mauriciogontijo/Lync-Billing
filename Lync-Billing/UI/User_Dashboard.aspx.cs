using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using Ext.Net;
using System.Web.SessionState;
using Lync_Billing.Libs;

namespace Lync_Billing.UI
{
    public partial class User_Dashboard : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri", SipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ui_IsInvoiced", "NO");

            columns.Add("SessionIdTime");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallToCountry");

            PhoneCallsHistoryStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart,7);
            PhoneCallsHistoryStore.DataBind();
        }

        [DirectMethod]
        public static string GetSummaryData()
        {
            string SipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;

            //UsersCallsSummary userSummary = new UsersCallsSummary();

            if (UsersCallsSummary.UserSummary == null) 
                UsersCallsSummary.UserSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);
            

            List<AbstractComponent> components = new List<AbstractComponent>();

            Ext.Net.Panel personalPanel = new Ext.Net.Panel()
            {
                Title = "Personal Calls Overview",
                Icon = Icon.Phone,
                Html = String.Format(
                    "<div class='block-body wauto m15 p5'><p>" +
                    "<p class='line-height-1-7 mb15'>You have a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                    "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                    UsersCallsSummary.UserSummary.PersonalCallsCount, UsersCallsSummary.UserSummary.PersonalCallsDuration /= 60, UsersCallsSummary.UserSummary.PersonalCallsCost)
            };

            Ext.Net.Panel businessPanel = new Ext.Net.Panel()
            {
                Title = "Business Calls Overview",
                Icon = Icon.Phone,
                Html = String.Format(
                    "<div class='block-body wauto m15 p5'><p>" +
                    "<p class='line-height-1-7 mb15'>You have a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                    "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                    UsersCallsSummary.UserSummary.BusinessCallsCount, UsersCallsSummary.UserSummary.BusinessCallsDuration /= 60, UsersCallsSummary.UserSummary.BusinessCallsCost)
            };

            Ext.Net.Panel unmarkedPanel = new Ext.Net.Panel()
            {

                Title = "Unmarked Calls Overview",
                Icon = Icon.Phone,
                Html = String.Format(
                    "<div class='block-body wauto m15 p5'><p>" +
                    "<p class='line-height-1-7 mb15'>You have a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                    "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                    UsersCallsSummary.UserSummary.UnmarkedCallsCount, UsersCallsSummary.UserSummary.UnmarkedCallsDuartion /= 60, UsersCallsSummary.UserSummary.UnmarkedCallsCost)
            };

            
            components.Add(personalPanel);
            components.Add(businessPanel);
            components.Add(unmarkedPanel);

            return ComponentLoader.ToConfig(components);

        }

        public List<UsersCallsSummaryChartData> getChartData() 
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            return UsersCallsSummaryChartData.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, DateTime.Now.AddMonths(-3), DateTime.Now);
            
        }

        protected void PhoneCallsCostChartStore_Load(object sender, EventArgs e)
        {
            if (Stores.phoneCallsSummaryChartData.Count == 0)
            {
                Stores.phoneCallsSummaryChartData = getChartData();
                PhoneCallsCostChartStore.DataSource = Stores.phoneCallsSummaryChartData;
                PhoneCallsCostChartStore.DataBind();
            }
            else 
            {
                PhoneCallsCostChartStore.DataSource = Stores.phoneCallsSummaryChartData;
                PhoneCallsCostChartStore.DataBind();
            }
        }

        protected void PhoneCallsDuartionChartStore_Load(object sender, EventArgs e)
        {
            if (Stores.phoneCallsSummaryChartData.Count == 0)
            {
                Stores.phoneCallsSummaryChartData = getChartData();
                PhoneCallsDuartionChartStore.DataSource = Stores.phoneCallsSummaryChartData;
                PhoneCallsDuartionChartStore.DataBind();
            }
            else
            {
                PhoneCallsDuartionChartStore.DataSource = Stores.phoneCallsSummaryChartData;
                 PhoneCallsDuartionChartStore.DataBind();
            }
        }
    }
}