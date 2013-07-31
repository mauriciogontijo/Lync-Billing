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
using Lync_Billing.Libs;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Data;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Lync_Billing.ui.accounting.reports
{
    public partial class periodical : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();
        List<string> sites = new List<string>();
        private string sipAccount = string.Empty;

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

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
            FilterReportsBySite.GetStore().DataSource = GetAccountantSites();
            FilterReportsBySite.GetStore().DataBind();
        }

        List<UsersCallsSummary> PeriodicalReport(string site, DateTime startingDate, DateTime endingDate) 
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
                ).Where(e=> e.PersonalCallsCost > 0 || e.BusinessCallsCost > 0 || e.UnmarkedCallsCost > 0).ToList();

            return sipAccounts;
        }

        public List<Site> GetAccountantSites()
        {
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                DB.Site tmpSite = new DB.Site();

                tmpSite.SiteID = userRoles.First(item => item.SiteID == role.SiteID && (item.RoleID == 7 || item.RoleID == 1)).SiteID;
                sites.Add(tmpSite);
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

        protected void PeriodicalReportsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            DateTime startDate;
            DateTime endDate;
            Document pdfDocument;
            Dictionary<string, string> pdfDocumentHeaders;
            List<string> SipAccountsList;
            Dictionary<string, Dictionary<string, object>> UsersCollection;
            JavaScriptSerializer jSerializer;

            XmlNode xml = e.Xml;
            string siteName = FilterReportsBySite.SelectedItem.Value;
            string format = this.FormatType.Value.ToString();

            this.Response.Clear();

            switch (format)
            {
                case "xls":
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "pdf":
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=AccountingPeriodicalReport_Summary.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    Dictionary<string, string> headers = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Perdiodical Report [Summary]"},
                        {"subTitle", "From " + StartingDate.SelectedDate.Month + "-" + StartingDate.SelectedDate.Year + ", to " + EndingDate.SelectedDate.Month + "-" + EndingDate.SelectedDate.Year}
                    };

                    Document doc = new Document();
                    UsersCallsSummary.ExportUsersCallsSummaryToPDF(StartingDate.SelectedDate, EndingDate.SelectedDate, siteName, Response, out doc, headers);
                    Response.Write(doc);
                    break;

                case "pdf-d":
                    SipAccountsList = new List<string>();
                    UsersCollection = new Dictionary<string, Dictionary<string, object>>();
                    jSerializer = new JavaScriptSerializer();
                    List<Users> usersData = jSerializer.Deserialize<List<Users>>(e.Json);

                    Dictionary<string, object> tempUserDataContainer;
                    foreach (Users user in usersData)
                    {
                        //SipAccountsList.Add(user.SipAccount);
                        tempUserDataContainer = new Dictionary<string, object>();
                        tempUserDataContainer.Add("FullName", user.FullName);
                        tempUserDataContainer.Add("EmployeeID", user.EmployeeID);
                        tempUserDataContainer.Add("SipAccount", user.SipAccount);

                        UsersCollection.Add(user.SipAccount, tempUserDataContainer);
                    }

                    startDate = StartingDate.SelectedDate;
                    endDate = EndingDate.SelectedDate;

                    //Initialize the response.
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=AccountingMonthlyReport_Detailed.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Monthly Report [Detailed]"},
                        {"subTitle", "From: " + startDate.Month + "-" + startDate.Year + ", to: " + endDate.Month + "-" + endDate.Year + "."}
                    };

                    pdfDocument = new Document();
                    UsersCallsSummary.ExportUsersCallsDetailedToPDF(startDate, endDate, siteName, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders);
                    Response.Write(pdfDocument);
                    break;
            }

            this.Response.End();
        }

        protected void FilterReportsBySite_Selecting(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1)
            {
                StartingDate.Disabled = false;

                //If the dates were previously chosen, jsut refresh the data!
                if (FilterReportsBySite.SelectedItem.Index != -1 && StartingDate.SelectedValue != null && EndingDate.SelectedValue != null)
                {
                    ExportExcelReport.Disabled = false;
                    ExportPDFReport.Disabled = false;

                    listOfUsersCallsSummary = PeriodicalReport(FilterReportsBySite.SelectedItem.Value, StartingDate.SelectedDate, EndingDate.SelectedDate);
                    PeriodicalReportsGrid.GetStore().DataSource = listOfUsersCallsSummary;
                    PeriodicalReportsGrid.GetStore().LoadData(listOfUsersCallsSummary);
                }
            }
            else
            {
                StartingDate.Disabled = true;
                EndingDate.Disabled = true;
                ExportExcelReport.Disabled = true;
                ExportPDFReport.Disabled = true;
            }
        }

        protected void StartingDate_Selection(object sender, DirectEventArgs e)
        {
            if (StartingDate.SelectedValue != null)
            {
                EndingDate.Disabled = false;
            }
            else
            {
                EndingDate.Disabled = true;
                ExportExcelReport.Disabled = true;
                ExportPDFReport.Disabled = true;
            }
        }

        protected void EndingDate_Selection(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1 && StartingDate.SelectedValue != null && EndingDate.SelectedValue != null)
            {
                ExportExcelReport.Disabled = false;
                ExportPDFReport.Disabled = false;

                listOfUsersCallsSummary = PeriodicalReport(FilterReportsBySite.SelectedItem.Value, StartingDate.SelectedDate, EndingDate.SelectedDate);
                PeriodicalReportsGrid.GetStore().DataSource = listOfUsersCallsSummary;
                PeriodicalReportsGrid.GetStore().LoadData(listOfUsersCallsSummary);
            }
            else
            {
                ExportExcelReport.Disabled = true;
                ExportPDFReport.Disabled = true;
            }
        }

    }
}