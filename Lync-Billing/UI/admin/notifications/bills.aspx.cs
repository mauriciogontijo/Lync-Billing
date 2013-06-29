﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using Ext.Net;
using Lync_Billing.DB;
using System.Globalization;

namespace Lync_Billing.ui.admin.notifications
{
    public partial class bills : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart;
        List<string> columns;
        private string sipAccount = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/admin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if ((!session.IsDeveloper || !session.IsAdmin) && session.PrimarySipAccount != session.EffectiveSipAccount)
                {
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            FilterUsersBySite.GetStore().DataSource = GetAccountantSiteName();
            FilterUsersBySite.GetStore().DataBind();
        }

        protected void UsersBillsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            
        }

        protected void UsersBillsStore_Load(object sender, EventArgs e)
        {
            
        }

        public string GetSiteName(int siteID)
        {
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SiteID", siteID);

            List<Site> sites = DB.Site.GetSites(null, wherePart, 0);

            return sites[0].SiteName;
        }

        public List<Site> GetAccountantSiteName()
        {
            UserSession session = (UserSession)Session.Contents["UserData"];
            List<Site> sites = new List<Site>();
            Site site;

            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                if (role.RoleID == 5 || role.RoleID == 1)
                {
                    site = new DB.Site();
                    site.SiteID = role.SiteID;
                    site.SiteName = GetSiteName(role.SiteID);

                    sites.Add(site);
                }
            }
            return sites;
        }


        public string GetSipAccount(string employeeID)
        {
            Dictionary<string, object> whereStatement = new Dictionary<string, object>();
            List<string> fields = new List<string>();
            List<Users> users = new List<Users>();

            whereStatement.Add("UserID", employeeID);
            fields.Add("SipAccount");

            users = Users.GetUsers(fields, whereStatement, 0);
            return users[0].SipAccount;
        }

        public string GetSipAccountSite(string employeeID)
        {
            Dictionary<string, object> whereStatement = new Dictionary<string, object>();
            // List<string> fields = new List<string>();
            List<Users> users = new List<Users>();

            whereStatement.Add("UserID", employeeID);

            users = Users.GetUsers(null, whereStatement, 0);
            return users[0].SiteName;
        }

        protected void GetUsersBillsForSite(object sender, DirectEventArgs e)
        {
            if (reportDateField.SelectedValue != null && FilterUsersBySite.SelectedItem != null)
            {
                string site = FilterUsersBySite.SelectedItem.Value;

                DateTime month_start = new DateTime(reportDateField.SelectedDate.Year, reportDateField.SelectedDate.Month, 1);
                DateTime month_end = month_start.AddMonths(1).AddDays(-1);

                UsersBillsGrid.GetStore().DataSource = GetUsersBills(month_start, month_end, site);
                UsersBillsGrid.GetStore().DataBind();
            }
        }

        private List<UsersCallsSummary> GetUsersBills(DateTime startingDate, DateTime endingDate, string site)
        {
            //UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = userSession.EffectiveSipAccount;

            //List<UsersCallsSummary> UserSummariesList = new List<UsersCallsSummary>();
            //List<UsersCallsSummary> BillsList = new List<UsersCallsSummary>();

            //int year = 2013,
            //    start_month = 1,
            //    end_month = DateTime.Now.Month;

            ////if the end month is not the beginning of the year, decrease it by 1, for the purpose of not including the current month
            //if (end_month != start_month) { end_month -= 1; }

            //UserSummariesList = UsersCallsSummary.GetUsersCallsSummary(sipAccount, year, start_month, end_month);
            //foreach (UsersCallsSummary summary in UserSummariesList)
            //{
            //    BillsList.Add(summary);
            //}

            //return BillsList;

            List<UsersCallsSummary> tmp = new List<UsersCallsSummary>();
            tmp.AddRange(UsersCallsSummary.GetUsersCallsSummary(startingDate, endingDate, site).AsEnumerable<UsersCallsSummary>());

            var UserBills =
            (
                from data in tmp.AsEnumerable()

                group data by new { data.SipAccount, data.EmployeeID, data.FullName, data.SiteName, data.MonthDate } into res

                select new UsersCallsSummary
                {
                    EmployeeID = res.Key.EmployeeID,
                    FullName = res.Key.FullName,
                    SipAccount = res.Key.SipAccount,
                    SiteName = res.Key.SiteName,
                    MonthDate = res.Key.MonthDate,

                    PersonalCallsCost = res.Sum(x => x.PersonalCallsCost),
                    PersonalCallsDuration = res.Sum(x => x.PersonalCallsDuration),
                    PersonalCallsCount = res.Sum(x => x.PersonalCallsCount),
                }
            ).Where(e => e.PersonalCallsCount > 0).ToList();

            return UserBills;
        }

        //List<UsersCallsSummary> PeriodicalReport(DateTime startingDate, DateTime endingDate, string site)
        //{
        //    List<UsersCallsSummary> tmp = new List<UsersCallsSummary>();

        //    tmp.AddRange(UsersCallsSummary.GetUsersCallsSummary(startingDate, endingDate, site).AsEnumerable<UsersCallsSummary>());

        //    var sipAccounts =
        //        (
        //            from data in tmp.AsEnumerable()

        //            group data by new { data.SipAccount, data.EmployeeID, data.FullName, data.SiteName } into res

        //            select new UsersCallsSummary
        //            {
        //                EmployeeID = res.Key.EmployeeID,
        //                FullName = res.Key.FullName,
        //                SipAccount = res.Key.SipAccount,
        //                SiteName = res.Key.SiteName,

        //                BusinessCallsCost = res.Sum(x => x.BusinessCallsCost),
        //                BusinessCallsDuration = res.Sum(x => x.BusinessCallsDuration),
        //                BusinessCallsCount = res.Sum(x => x.BusinessCallsCount),

        //                PersonalCallsCost = res.Sum(x => x.PersonalCallsCost),
        //                PersonalCallsDuration = res.Sum(x => x.PersonalCallsDuration),
        //                PersonalCallsCount = res.Sum(x => x.PersonalCallsCount),

        //                UnmarkedCallsCost = res.Sum(x => x.UnmarkedCallsCost),
        //                UnmarkedCallsDuration = res.Sum(x => x.UnmarkedCallsDuration),
        //                UnmarkedCallsCount = res.Sum(x => x.UnmarkedCallsCount),
        //            }
        //        ).Where(e => e.UnmarkedCallsCount > 0).ToList();

        //    return sipAccounts;
        //}


        //protected void GetUsersBills_DirectClick(object sender, DirectEventArgs e)
        //{
        //    if (reportDateField.SelectedValue != null)
        //    {
        //        //listOfUsersCallsSummary = MonthlyReports(reportDateField.SelectedDate);
        //        //MonthlyReportsGrids.GetStore().DataSource = listOfUsersCallsSummary;
        //        //MonthlyReportsGrids.GetStore().LoadData(listOfUsersCallsSummary);
        //    }
        //}
    }
}