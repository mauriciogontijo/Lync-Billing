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
using Lync_Billing.DB;
using Lync_Billing.Libs;
using Lync_Billing.DB.Summaries;

namespace Lync_Billing.ui.admin.notifications
{
    public partial class calls : System.Web.UI.Page
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
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=admin");
                }
            }

            sipAccount = session.EffectiveSipAccount;

            FilterUsersBySite.GetStore().DataSource = DB.Site.GetUserRoleSites(session.Roles, Enums.GetDescription(Enums.ValidRoles.IsSiteAdmin));
            FilterUsersBySite.GetStore().DataBind();
        }

        public List<Site> GetAdminSites()
        {
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                DB.Site tmpSite = new DB.Site();

                if (role.SiteID != 0 && (role.IsSiteAdmin() || role.IsDeveloper()))
                {
                    tmpSite.SiteID = role.SiteID;
                    sites.Add(tmpSite);
                }
            }

            List<Site> tmpSites = DB.Site.GetSites();

            foreach (DB.Site site in sites)
            {
                DB.Site tmpSite = new DB.Site();

                tmpSite = tmpSites.First(e => e.SiteID == site.SiteID);

                site.SiteName = tmpSite.SiteName;
                site.CountryCode = tmpSite.CountryCode;
            }

            return sites;
        }

        protected void GetUnmarkedCallsForSite(object sender, DirectEventArgs e)
        {
            string siteName = FilterUsersBySite.SelectedItem.Value;
            UnmarkedCallsGrid.GetStore().DataSource = PeriodicalReport(siteName, DateTime.Now.AddYears(-1), DateTime.Now);
            UnmarkedCallsGrid.GetStore().DataBind();
        }

        List<UserCallsSummary> PeriodicalReport(string siteName, DateTime startingDate, DateTime endingDate)
        {
            List<UserCallsSummary> tmp = new List<UserCallsSummary>();

            tmp.AddRange(UserCallsSummary.GetUsersCallsSummary(siteName, startingDate, endingDate).AsEnumerable<UserCallsSummary>());

            var sipAccounts =
                (
                    from data in tmp.AsEnumerable()

                    group data by new { data.SipAccount, data.EmployeeID, data.FullName, data.SiteName } into res

                    select new UserCallsSummary
                    {
                        EmployeeID = res.Key.EmployeeID,
                        FullName = res.Key.FullName,
                        SipAccount = res.Key.SipAccount,
                        SiteName = res.Key.SiteName,

                        BusinessCallsCost = res.Sum(x => x.BusinessCallsCost),
                        BusinessCallsDuration = res.Sum(x => x.BusinessCallsDuration),
                        BusinessCallsCount = res.Sum(x => x.BusinessCallsCount),

                        PersonalCallsCost = res.Sum(x => x.PersonalCallsCost),
                        PersonalCallsDuration = res.Sum(x => x.PersonalCallsDuration),
                        PersonalCallsCount = res.Sum(x => x.PersonalCallsCount),

                        UnmarkedCallsCost = res.Sum(x => x.UnmarkedCallsCost),
                        UnmarkedCallsDuration = res.Sum(x => x.UnmarkedCallsDuration),
                        UnmarkedCallsCount = res.Sum(x => x.UnmarkedCallsCount),
                    }
                ).Where(e => e.UnmarkedCallsCount > 0).ToList();

            return sipAccounts;
        }

        protected void NotifyUsers(object sender, DirectEventArgs e)
        {
            string Body = string.Empty;
            string subject = string.Empty;

            string sipAccount = string.Empty;

            string json = e.ExtraParams["Values"];

            List<UserCallsSummary> usersSummary = JSON.Deserialize<List<UserCallsSummary>>(json);

            MailTemplates mailTemplate = MailTemplates.GetMailTemplates().First(item => item.TemplateID == 1);

            subject = mailTemplate.TemplateSubject;
            Body = mailTemplate.TemplateBody;

            foreach (UserCallsSummary userSummary in usersSummary)
            {
                sipAccount = userSummary.SipAccount;

                string RealBody =
                    string.Format(
                            Body,
                            userSummary.FullName,
                            userSummary.UnmarkedCallsCount,
                            userSummary.UnmarkedCallsCost,
                            DB.Misc.ConvertSecondsToReadable(userSummary.UnmarkedCallsDuration));

                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }

        }

    }
}