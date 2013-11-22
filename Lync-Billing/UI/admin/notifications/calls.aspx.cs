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
using Lync_Billing.DB.Roles;

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

            FilterUsersBySite.GetStore().DataSource = DB.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSiteAdmin));
            FilterUsersBySite.GetStore().DataBind();
        }

        public List<Site> GetAdminSites()
        {
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            List<SystemRole> userRoles = session.SystemRoles;

            foreach (SystemRole role in userRoles)
            {
                DB.Site tmpSite = new DB.Site();

                if (role.SiteID != 0 && (role.IsSiteAdmin() || role.IsDeveloper()))
                {
                    tmpSite.SiteID = role.SiteID;
                    sites.Add(tmpSite);
                }
            }

            List<Site> tmpSites = DB.Site.GetAllSites();

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
            return UserCallsSummary.GetUsersCallsSummaryInSite(siteName, startingDate, endingDate, asTotals: true)
                    .Where(summary => summary.UnmarkedCallsCount > 0)
                    .ToList();
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
                            HelperFunctions.ConvertSecondsToReadable(userSummary.UnmarkedCallsDuration));

                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }

        }

    }
}