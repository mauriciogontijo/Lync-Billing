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
    public partial class periodical : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAccountant);

        private List<string> sites = new List<string>();
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


        List<UserCallsSummary> PeriodicalReport(string siteName, DateTime startingDate, DateTime endingDate) 
        {
            List<UserCallsSummary> data = UserCallsSummary.GetUsersCallsSummaryInSite(siteName, startingDate, endingDate, asTotals: true)
                                        .Where(e=> e.PersonalCallsCost > 0 || e.BusinessCallsCost > 0 || e.UnmarkedCallsCost > 0).ToList();

            return data;
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
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.DisplayName), user.FullName);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID), user.EmployeeID);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), user.SipAccount);

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
                    UserCallsSummary.ExportUsersCallsSummaryToPDF(siteName, StartingDate.SelectedDate, EndingDate.SelectedDate, UsersCollection, Response, out doc, headers);
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
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.DisplayName), user.FullName);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID), user.EmployeeID);
                        tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), user.SipAccount);

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
                    UserCallsSummary.ExportUsersCallsDetailedToPDF(siteName, startDate, endDate, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders);
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