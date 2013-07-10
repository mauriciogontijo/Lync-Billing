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
using Lync_Billing.Libs;

namespace Lync_Billing.ui.admin.notifications
{
    public partial class bills : System.Web.UI.Page
    {
       
        private string sipAccount = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/admin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if (session.ActiveRoleName != "admin")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=admin");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            FilterUsersBySite.GetStore().DataSource = GetAccountantSiteName();
            FilterUsersBySite.GetStore().DataBind();
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
            if (BillDateField.SelectedValue != null && FilterUsersBySite.SelectedItem != null)
            {
                string site = FilterUsersBySite.SelectedItem.Value;

                DateTime month_start = new DateTime(BillDateField.SelectedDate.Year, BillDateField.SelectedDate.Month, 1);
                DateTime month_end = month_start.AddMonths(1).AddDays(-1);

                UsersBillsGrid.GetStore().DataSource = GetUsersBills(month_start, month_end, site);
                UsersBillsGrid.GetStore().DataBind();
            }
        }

        private List<UsersCallsSummary> GetUsersBills(DateTime startingDate, DateTime endingDate, string site)
        {
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
                    MonthDate = endingDate,

                    PersonalCallsCost = res.Sum(x => x.PersonalCallsCost),
                    PersonalCallsDuration = res.Sum(x => x.PersonalCallsDuration),
                    PersonalCallsCount = res.Sum(x => x.PersonalCallsCount),
                }
            ).Where(e => e.PersonalCallsCount > 0).ToList();

            return UserBills;
        }

        protected void BillDateField_Selection(object sender, DirectEventArgs e)
        {
            if (BillDateField.SelectedDate != null)
            {
                FilterUsersBySite.Disabled = false;
            }
            else
            {
                FilterUsersBySite.Disabled = true;
            }
        }


        protected void NotifyUsers(object sender, DirectEventArgs e)
        {
            string Body = string.Empty;
            string subject = string.Empty;

            string sipAccount = string.Empty;

            string json = e.ExtraParams["Values"];

            List<UsersCallsSummary> usersSummary = JSON.Deserialize<List<UsersCallsSummary>>(json);

            MailTemplates mailTemplate = MailTemplates.GetMailTemplates().First(item => item.TemplateID == 2);

            subject = mailTemplate.TemplateSubject;
            Body = mailTemplate.TemplateBody;

            foreach (UsersCallsSummary userSummary in usersSummary)
            {
                sipAccount = userSummary.SipAccount;

                string RealBody =
                    string.Format(
                            Body,
                            userSummary.FullName,
                            userSummary.MonthDate.ToString("MMM", CultureInfo.InvariantCulture) + " " + userSummary.MonthDate.Year,
                            userSummary.PersonalCallsCount,
                            DB.Misc.ConvertSecondsToReadable(userSummary.PersonalCallsDuration),
                            userSummary.PersonalCallsCost);

                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }

        }
    }
}