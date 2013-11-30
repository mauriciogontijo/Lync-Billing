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

using Lync_Billing.Backend;
using Lync_Billing.Libs;
using Lync_Billing.Backend.Summaries;

namespace Lync_Billing.ui.accounting.reports
{
    public partial class monthly : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAccountant);
        private List<UserCallsSummary> listOfUsersCallsSummary = new List<UserCallsSummary>();


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

            sipAccount = session.NormalUserInfo.SipAccount;
            
            //Get the list of sites for this accountant
            FilterReportsBySite.GetStore().DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSiteAccountant));
            FilterReportsBySite.GetStore().DataBind();
        }

        protected List<UserCallsSummary> MonthlyReports(string siteName, DateTime date)
        {
            List<UserCallsSummary> listOfUsersCallsSummary;

            DateTime beginningOfTheMonth = new DateTime(date.Year, date.Month, 1);
            DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

            listOfUsersCallsSummary = UserCallsSummary.GetUsersCallsSummaryInSite(siteName, beginningOfTheMonth, endOfTheMonth, true)
                                        .Where(e => e.PersonalCallsCost > 0 || e.BusinessCallsCost > 0 || e.UnmarkedCallsCost > 0).ToList();
            
            return listOfUsersCallsSummary;
        }

        protected void FilterReportsBySite_Selecting(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1)
            {
                ReportDateField.Disabled = false;

                if (ReportDateField.SelectedValue != null)
                {
                    listOfUsersCallsSummary = MonthlyReports(FilterReportsBySite.SelectedItem.Value, ReportDateField.SelectedDate);
                    MonthlyReportsGrids.GetStore().DataSource = listOfUsersCallsSummary;
                    MonthlyReportsGrids.GetStore().LoadData(listOfUsersCallsSummary);
                }
            }
            else
            {
                ReportDateField.Disabled = true;
            }
        }

        protected void ReportDateField_Selection(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1 && ReportDateField.SelectedValue != null)
            {
                ExportExcelReport.Disabled = false;
                ExportPDFReport.Disabled = false;

                listOfUsersCallsSummary = MonthlyReports(FilterReportsBySite.SelectedItem.Value, ReportDateField.SelectedDate);
                MonthlyReportsGrids.GetStore().DataSource = listOfUsersCallsSummary;
                MonthlyReportsGrids.GetStore().LoadData(listOfUsersCallsSummary);
            }
            else
            {
                ExportExcelReport.Disabled = true;
                ExportPDFReport.Disabled = true;
            }
        }

        protected void MonthlyReportsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            DateTime beginningOfTheMonth;
            DateTime endOfTheMonth;
            Document pdfDocument;
            Dictionary<string, string> pdfDocumentHeaders;
            List<string> SipAccountsList;
            Dictionary<string, Dictionary<string, object>> UsersCollection;
            JavaScriptSerializer jSerializer;
            
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
                    jSerializer = new JavaScriptSerializer();
                    usersData = jSerializer.Deserialize<List<Users>>(e.Json);

                    foreach (Users user in usersData)
                    {
                        //SipAccountsList.Add(user.SipAccount);
                        tempUserDataContainer = new Dictionary<string, object>();
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.DisplayName), user.FullName);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID), user.EmployeeID);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), user.SipAccount);

                        UsersCollection.Add(user.SipAccount, tempUserDataContainer);
                    }

                    beginningOfTheMonth = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                    endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    //Initialize the response.
                    pdfReportFileName = string.Format(
                        "{0}_Monthly_Summary_Report_{1}.pdf",
                        siteName.ToUpper(), beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year
                    );
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Monthly Report [Summary]"},
                        {"subTitle", "As Per: " + beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year}
                    };

                    pdfDocument = new Document();
                    UserCallsSummary.ExportUsersCallsSummaryToPDF(siteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders);
                    Response.Write(pdfDocument);
                    break;

                case "pdf-d":
                    SipAccountsList = new List<string>();
                    UsersCollection = new Dictionary<string, Dictionary<string, object>>();
                    jSerializer = new JavaScriptSerializer();
                    usersData = jSerializer.Deserialize<List<Users>>(e.Json);

                    foreach (Users user in usersData)
                    {
                        //SipAccountsList.Add(user.SipAccount);
                        tempUserDataContainer = new Dictionary<string, object>();
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.DisplayName), user.FullName);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID), user.EmployeeID);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), user.SipAccount);

                        UsersCollection.Add(user.SipAccount, tempUserDataContainer);
                    }

                    beginningOfTheMonth = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                    endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    //Initialize the response.
                    pdfReportFileName = string.Format(
                        "{0}_Monthly_Detailed_Report_{1}.pdf",
                        siteName.ToUpper(), beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year
                    );
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Monthly Report [Detailed]"},
                        {"subTitle", "As Per: " + beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year}
                    };

                    pdfDocument = new Document();
                    UserCallsSummary.ExportUsersCallsDetailedToPDF(siteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders);
                    Response.Write(pdfDocument);
                    break;
            }

            this.Response.End();
        }

    }
}