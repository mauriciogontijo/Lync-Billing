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

namespace Lync_Billing.ui.admin.notifications
{
    public partial class calls : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

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

                if (session.ActiveRoleName != "admin")
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

        protected void GetUnmarkedCallsForSite(object sender, DirectEventArgs e)
        {
            string site = FilterUsersBySite.SelectedItem.Value;
            UnmarkedCallsGrid.GetStore().DataSource = PeriodicalReport(DateTime.Now.AddYears(-1), DateTime.Now, site);
            UnmarkedCallsGrid.GetStore().DataBind();
        }

        List<UsersCallsSummary> PeriodicalReport(DateTime startingDate, DateTime endingDate, string site)
        {
            List<UsersCallsSummary> tmp = new List<UsersCallsSummary>();

            tmp.AddRange(UsersCallsSummary.GetUsersCallsSummary(startingDate, endingDate, site).AsEnumerable<UsersCallsSummary>());

            var sipAccounts =
                (
                    from data in tmp.AsEnumerable()

                    group data by new { data.SipAccount, data.EmployeeID, data.FullName, data.SiteName } into res

                    select new UsersCallsSummary
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

            List<UsersCallsSummary> usersSummary = JSON.Deserialize<List<UsersCallsSummary>>(json);

            MailTemplates mailTemplate = MailTemplates.GetMailTemplates().First(item => item.TemplateID == 1);

            subject = mailTemplate.TemplateSubject;
            Body = mailTemplate.TemplateBody;

            foreach (UsersCallsSummary userSummary in usersSummary)
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