using System;
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
using System.Globalization;

using Lync_Billing.Backend;
using Lync_Billing.Libs;
using Lync_Billing.Backend.Summaries;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.admin.notifications
{
    public partial class bills : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAdmin);

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
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=admin");
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;

            FilterUsersBySite.GetStore().DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSiteAdmin));
            FilterUsersBySite.GetStore().DataBind();
        }


        public List<Site> GetAdminSites()
        {
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            List<SystemRole> userRoles = session.SystemRoles;

            foreach (SystemRole role in userRoles)
            {
                Backend.Site tmpSite = new Backend.Site();

                if (role.SiteID != 0 && (role.IsSiteAdmin() || role.IsDeveloper()))
                {
                    tmpSite.SiteID = role.SiteID;
                    sites.Add(tmpSite);
                }
            }

            List<Site> tmpSites = Backend.Site.GetAllSites();

            foreach (Backend.Site site in sites)
            {
                Backend.Site tmpSite = new Backend.Site();

                tmpSite = tmpSites.First(e => e.SiteID == site.SiteID);

                site.SiteName = tmpSite.SiteName;
                site.CountryCode = tmpSite.CountryCode;
            }

            return sites;
        }

        protected void GetUsersBillsForSite(object sender, DirectEventArgs e)
        {
            if (BillDateField.SelectedValue != null && FilterUsersBySite.SelectedItem != null)
            {
                string siteName = FilterUsersBySite.SelectedItem.Value;

                DateTime beginningOfTheMonth = new DateTime(BillDateField.SelectedDate.Year, BillDateField.SelectedDate.Month, 1);
                DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                UsersBillsGrid.GetStore().DataSource = GetUsersBills(siteName, beginningOfTheMonth, endOfTheMonth);
                UsersBillsGrid.GetStore().DataBind();
            }
        }

        private List<UserCallsSummary> GetUsersBills(string siteName, DateTime startingDate, DateTime endingDate)
        {
            List<UserCallsSummary> tmp = new List<UserCallsSummary>();
            tmp.AddRange(UserCallsSummary.GetUsersCallsSummaryInSite(siteName, startingDate, endingDate).AsEnumerable<UserCallsSummary>());

            var UserBills =
            (
                from data in tmp.AsEnumerable()

                group data by new { data.SipAccount, data.EmployeeID, data.FullName, data.SiteName, MonthDate = data.Date } into res

                select new UserCallsSummary
                {
                    EmployeeID = res.Key.EmployeeID,
                    FullName = res.Key.FullName,
                    SipAccount = res.Key.SipAccount,
                    SiteName = res.Key.SiteName,
                    Date = endingDate,

                    PersonalCallsCost = res.Sum(x => x.PersonalCallsCost),
                    PersonalCallsDuration = res.Sum(x => x.PersonalCallsDuration),
                    PersonalCallsCount = res.Sum(x => x.PersonalCallsCount),
                }
            ).Where(e => e.PersonalCallsCount > 0).ToList();

            return UserBills;
        }

        protected void FilterUsersBySite_Selected(object sender, DirectEventArgs e)
        {
            if (FilterUsersBySite.SelectedItem.Index != -1 && !string.IsNullOrEmpty(FilterUsersBySite.SelectedItem.Value))
            {
                BillDateField.Disabled = false;
                if (BillDateField.SelectedValue != null)
                {
                    string siteName = FilterUsersBySite.SelectedItem.Value;

                    DateTime beginningOfTheMonth = new DateTime(BillDateField.SelectedDate.Year, BillDateField.SelectedDate.Month, 1);
                    DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    UsersBillsGrid.GetStore().DataSource = GetUsersBills(siteName, beginningOfTheMonth, endOfTheMonth);
                    UsersBillsGrid.GetStore().DataBind();
                }
            }
            else
            {
                BillDateField.Disabled = true;
                //BillDateField.Clear();
                //UsersBillsGrid.ClearContent();
            }
        }


        protected void NotifyUsers(object sender, DirectEventArgs e)
        {
            string Body = string.Empty;
            string subject = string.Empty;

            string sipAccount = string.Empty;

            string json = e.ExtraParams["Values"];

            List<UserCallsSummary> usersSummary = JSON.Deserialize<List<UserCallsSummary>>(json);

            MailTemplates mailTemplate = MailTemplates.GetMailTemplates().First(item => item.TemplateID == 2);

            subject = mailTemplate.TemplateSubject;
            Body = mailTemplate.TemplateBody;

            foreach (UserCallsSummary userSummary in usersSummary)
            {
                sipAccount = userSummary.SipAccount;

                string RealBody =
                    string.Format(
                            Body,
                            userSummary.FullName,
                            userSummary.Date.ToString("MMM", CultureInfo.InvariantCulture) + " " + userSummary.Date.Year,
                            userSummary.PersonalCallsCount,
                            SpecialDateTime.ConvertSecondsToReadable(userSummary.PersonalCallsDuration),
                            userSummary.PersonalCallsCost);

                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }

        }
    }
}