﻿using System;
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

            UsersCallsSummary userSummary = new UsersCallsSummary();
            userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);

            List<AbstractComponent> components = new List<AbstractComponent>();

            Ext.Net.Panel personalPanel = new Ext.Net.Panel()
            {
                Title = "Personal Calls Overview",
                Icon = Icon.Phone,
                Html = String.Format(
                    "<div class='block-body wauto m15 p5'><p>" +
                    "<p class='line-height-1-7 mb15'>You have a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                    "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                    userSummary.PersonalCallsCount, userSummary.PersonalCallsDuration /= 60, userSummary.PersonalCallsCost)
            };

            Ext.Net.Panel businessPanel = new Ext.Net.Panel()
            {
                Title = "Business Calls Overview",
                Icon = Icon.Phone,
                Html = String.Format(
                    "<div class='block-body wauto m15 p5'><p>" +
                    "<p class='line-height-1-7 mb15'>You have a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                    "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                    userSummary.BusinessCallsCount, userSummary.BusinessCallsDuration /= 60, userSummary.BusinessCallsCost)
            };

            Ext.Net.Panel unmarkedPanel = new Ext.Net.Panel()
            {

                Title = "Unmarked Calls Overview",
                Icon = Icon.Phone,
                Html = String.Format(
                    "<div class='block-body wauto m15 p5'><p>" +
                    "<p class='line-height-1-7 mb15'>You have a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                    "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                    userSummary.UnmarkedCallsCount, userSummary.UnmarkedCallsDuartion /= 60, userSummary.UnmarkedCallsCost)
            };

            
            components.Add(personalPanel);
            components.Add(businessPanel);
            components.Add(unmarkedPanel);

            return ComponentLoader.ToConfig(components);

        }

        public List<UsersCallsSummary> getChartData() 
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            return UsersCallsSummary.GetUsersCallsSummary1(((UserSession)Session.Contents["UserData"]).SipAccount, 2012, 1);
        }


    }



   
    public class PhoneCallsSummary
    {
        private int NumberOfCalls { get; set; }
        private int NumberOfSeconds { get; set; }
        private int TotalCost { get; set; }

        public PhoneCallsSummary() { }
       
    }

    
}