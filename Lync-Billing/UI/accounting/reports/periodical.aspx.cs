using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections;
using Ext.Net;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Data;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Lync_Billing.ui.accounting.reports
{
    public partial class periodical : System.Web.UI.Page
    {

        List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();
        List<string> sites = new List<string>(); 

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/accounting/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if (session.ActiveRoleName != "accounting")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=accounting");
                }
            }
        }

        List<UsersCallsSummary> PeriodicalReport(DateTime startingDate, DateTime endingDate) 
        {
            List<UsersCallsSummary> tmp = new List<UsersCallsSummary>();

            sites = GetAccountantSiteName();

            foreach (string site in sites)
            {
                tmp.AddRange(UsersCallsSummary.GetUsersCallsSummary(startingDate, endingDate, site).AsEnumerable<UsersCallsSummary>());
            }

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
                ).Where(e=> e.PersonalCallsCost > 0).ToList();

            return sipAccounts;
        }

        public string GetSiteName(int siteID)
        {
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SiteID", siteID);

            List<Site> sites = DB.Site.GetSites(null, wherePart, 0);

            return sites[0].SiteName;
        }

        public List<string> GetAccountantSiteName()
        {
            UserSession session = (UserSession)Session.Contents["UserData"];

            List<string> sites = new List<string>();

            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                if (role.RoleID == 7 || role.RoleID == 1)
                    sites.Add(GetSiteName(role.SiteID));
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


        protected void ViewMonthlyBills_DirectClick(object sender, DirectEventArgs e)
        {
            if (StartingDate.SelectedValue != null && EndingDate.SelectedValue != null )
            {
                listOfUsersCallsSummary = PeriodicalReport(StartingDate.SelectedDate,EndingDate.SelectedDate);
                PeriodicalReportsGrid.GetStore().DataSource = listOfUsersCallsSummary;
                PeriodicalReportsGrid.GetStore().LoadData(listOfUsersCallsSummary);
            }
        }

        protected void PeriodicalReportsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.FormatType.Value.ToString();
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);

            this.Response.End();
        }

        protected void ExportDetailedReportButton_DirectClick(object sender, DirectEventArgs e)
        {

        }
    }
}