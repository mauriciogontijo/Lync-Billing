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
        private List<UserCallsSummary> listOfUsersCallsSummary = new List<UserCallsSummary>();
        private List<string> sites = new List<string>();
        private string sipAccount = string.Empty;
        private UserSession session;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAccountant);

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/accounting/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=accounting");
                }
            }

            sipAccount = session.EffectiveSipAccount;
            
            //Get the list of sites for this accountant
            FilterReportsBySite.GetStore().DataSource = DB.Site.GetUserRoleSites(session.Roles, Enums.GetDescription(Enums.ValidRoles.IsSiteAccountant));
            FilterReportsBySite.GetStore().DataBind();
        }

        List<UserCallsSummary> PeriodicalReport(string site, DateTime startingDate, DateTime endingDate) 
        {
            List<UserCallsSummary> tmp = new List<UserCallsSummary>();

            
            tmp.AddRange(UserCallsSummary.GetUsersCallsSummary(startingDate, endingDate, site).AsEnumerable<UserCallsSummary>());
            
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
                ).Where(e=> e.PersonalCallsCost > 0 || e.BusinessCallsCost > 0 || e.UnmarkedCallsCost > 0).ToList();

            return sipAccounts;
        }

        protected void PeriodicalReportsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            DateTime startDate;
            DateTime endDate;
            Document pdfDocument;
            Dictionary<string, string> pdfDocumentHeaders;
            List<string> SipAccountsList;
            Dictionary<string, Dictionary<string, object>> UsersCollection;

            //These are created to hold the data submitted through the grid as JSON
            List<Users> usersData;
            Dictionary<string, object> tempUserDataContainer;

            XmlNode xml = e.Xml;
            string siteName = FilterReportsBySite.SelectedItem.Value;
            string format = this.FormatType.Value.ToString();
            string pdfReportFileName = string.Empty;

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
                    UsersCollection = new Dictionary<string, Dictionary<string, object>>();
                    usersData = (new JavaScriptSerializer()).Deserialize<List<Users>>(e.Json);

                    foreach (Users user in usersData)
                    {
                        //SipAccountsList.Add(user.SipAccount);
                        tempUserDataContainer = new Dictionary<string, object>();
                        tempUserDataContainer.Add("FullName", user.FullName);
                        tempUserDataContainer.Add("EmployeeID", user.EmployeeID);
                        tempUserDataContainer.Add("SipAccount", user.SipAccount);

                        UsersCollection.Add(user.SipAccount, tempUserDataContainer);
                    }

                    pdfReportFileName = string.Format(
                        "{0}_Periodical_Summary_Report_{1}.pdf", 
                        siteName.ToUpper(), StartingDate.SelectedDate.Month + "-" + StartingDate.SelectedDate.Year + "--" + EndingDate.SelectedDate.Month + "-" + EndingDate.SelectedDate.Year
                    );
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    Dictionary<string, string> headers = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Perdiodical Report [Summary]"},
                        {"subTitle", "From " + StartingDate.SelectedDate.Month + "-" + StartingDate.SelectedDate.Year + ", to " + EndingDate.SelectedDate.Month + "-" + EndingDate.SelectedDate.Year}
                    };

                    Document doc = new Document();
                    UserCallsSummary.ExportUsersCallsSummaryToPDF(StartingDate.SelectedDate, EndingDate.SelectedDate, siteName, UsersCollection, Response, out doc, headers);
                    Response.Write(doc);
                    break;

                case "pdf-d":
                    SipAccountsList = new List<string>();
                    UsersCollection = new Dictionary<string, Dictionary<string, object>>();
                    usersData = (new JavaScriptSerializer()).Deserialize<List<Users>>(e.Json);

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
                    pdfReportFileName = string.Format(
                        "{0}_Periodical_Detailed_Report_{1}.pdf",
                        siteName.ToUpper(), StartingDate.SelectedDate.Month + "-" + StartingDate.SelectedDate.Year + "--" + EndingDate.SelectedDate.Month + "-" + EndingDate.SelectedDate.Year
                    );
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Monthly Report [Detailed]"},
                        {"subTitle", "From: " + startDate.Month + "-" + startDate.Year + ", to: " + endDate.Month + "-" + endDate.Year + "."}
                    };

                    pdfDocument = new Document();
                    UserCallsSummary.ExportUsersCallsDetailedToPDF(startDate, endDate, siteName, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders);
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