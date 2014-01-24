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
            
            //Get the list of AllSites for this accountant
            FilterReportsBySite.GetStore().DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSiteAccountant));
            FilterReportsBySite.GetStore().DataBind();
        }

        private List<UserCallsSummary> MonthlyReports(int siteID, DateTime date)
        {
            Site site = Backend.Site.GetSite(siteID);

            DateTime beginningOfTheMonth = new DateTime(date.Year, date.Month, 1);
            DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

            listOfUsersCallsSummary = UserCallsSummary.GetUsersCallsSummaryInSite(site.SiteName, beginningOfTheMonth, endOfTheMonth, true)
                                        .Where(e => e.PersonalCallsCost > 0 || e.BusinessCallsCost > 0 || e.UnmarkedCallsCost > 0).ToList();

            //listOfUsersCallsSummary = UserCallsSummary.GetUsersCallsSummaryInSite(site.SiteName, beginningOfTheMonth, endOfTheMonth, true);
            
            return listOfUsersCallsSummary;
        }

        private void BindDataToReportsGrid(bool siteHasChanged = false, bool dateHasChanged = false, bool callTypeHasChanged = false)
        {
            int callsType, siteID;
            string siteName = string.Empty;
            List<UserCallsSummary> gridData = new List<UserCallsSummary>();

            if (listOfUsersCallsSummary.Count == 0 || (siteHasChanged == true || dateHasChanged == true))
            {
                siteID = Convert.ToInt32(FilterReportsBySite.SelectedItem.Value);
                siteName = Backend.Site.GetSiteName(siteID);
                listOfUsersCallsSummary = MonthlyReports(siteID, ReportDateField.SelectedDate);
            }
            
            callsType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);

            if(callsType == 1)
                gridData = listOfUsersCallsSummary.Where(summary => summary.AC_IsInvoiced == "NO" && summary.SiteName == siteName).ToList();
            else if (callsType == 2)
                gridData = listOfUsersCallsSummary.Where(summary => summary.AC_IsInvoiced == "N/A" && summary.SiteName == siteName).ToList();
            else if (callsType == 3)
                gridData = listOfUsersCallsSummary.Where(summary => summary.AC_IsInvoiced == "YES" && summary.SiteName == siteName).ToList();
            
            MonthlyReportsGrids.GetStore().DataSource = gridData;
            MonthlyReportsGrids.GetStore().LoadData(gridData);
        }

        protected void FilterReportsBySite_Selecting(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1)
            {
                ReportDateField.Disabled = false;

                if (ReportDateField.SelectedValue != null)
                {
                    BindDataToReportsGrid(siteHasChanged: true);
                }
            }
            else
            {
                ReportDateField.Disabled = true;
            }
        }

        protected void ReportDateField_Selection(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index > -1)
            {
                CallsTypesComboBox.Disabled = false;
                AdvancedToolsMenu.Disabled = false;

                BindDataToReportsGrid(siteHasChanged: true, dateHasChanged: true);
            }
            else
            {
                CallsTypesComboBox.Disabled = true;
                AdvancedToolsMenu.Disabled = true;
            }
        }

        protected void FilterReportsByCallsTypes_Select(object sender, DirectEventArgs e)
        {
            int callsType;
            InvoiceUsers.Disabled = true;

            if (CallsTypesComboBox.SelectedItem.Index > -1)
            {
                callsType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);

                BindDataToReportsGrid(callTypeHasChanged: true);

                if (callsType == 1 || callsType == 2)
                    InvoiceUsers.Disabled = false;
            }
        }

        protected void InvoiceUsers_Confirm_Click(object sender, DirectEventArgs e)
        {
            int siteID, callType;
            DateTime startingDate, endingDate;

            if (FilterReportsBySite.SelectedItem.Index > -1 && CallsTypesComboBox.SelectedItem.Index > -1 && ReportDateField.SelectedValue != null)
            {
                siteID = Convert.ToInt32(FilterReportsBySite.SelectedItem.Value);
                callType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);

                startingDate = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                endingDate = startingDate.AddMonths(1).AddDays(-1);

                if (callType == 1)
                {
                    PhoneCall.ChargePhoneCallsOfSite(siteID, startingDate, endingDate, chargeBusinessPersonal: true, chargePending: false);
                }
                else if (callType == 2)
                {
                    PhoneCall.ChargePhoneCallsOfSite(siteID, startingDate, endingDate, chargeBusinessPersonal: false, chargePending: true);
                }

                BindDataToReportsGrid(callTypeHasChanged: true);

            }//End-if
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
            int SiteID;
            Backend.Site SelectedSite;
            
            //These are created to hold the data submitted through the grid as JSON
            List<Users> usersData;
            Dictionary<string, object> tempUserDataContainer;
            
            XmlNode xml = e.Xml;
            string format = this.FormatType.Value.ToString();
            string pdfReportFileName = string.Empty;


            SiteID = Convert.ToInt32(FilterReportsBySite.SelectedItem.Value);
            SelectedSite = Backend.Site.GetSite(SiteID);

            int callsType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);
            string callsTypeString = string.Empty;

            
            this.Response.Clear();
            
            
            switch(callsType)
            {
                case 1:
                    callsTypeString = "Not Charged";
                    break;
                case 2:
                    callsTypeString = "Pending Charges";
                    break;
                case 3:
                    callsTypeString = "Charged";
                    break;
                default:
                    callsTypeString = "Not Charged";
                    break;
            }
            

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
                        "{0}_Monthly_Summary_Report_{1}_{2}.pdf",
                        SelectedSite.SiteName.ToUpper(), beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString
                    );
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", SelectedSite.SiteName},
                        {"title", "Accounting Monthly Report [Summary]"},
                        {"subTitle", String.Format("As Per: {0}; {1}", beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString)}
                    };

                    pdfDocument = new Document();
                    
                    if(callsType == 1)
                        UserCallsSummary.ExportUsersCallsSummaryToPDF(SelectedSite.SiteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, notChargedCalls: true);
                    else if(callsType == 2)
                        UserCallsSummary.ExportUsersCallsSummaryToPDF(SelectedSite.SiteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, pendingChargesCalls: true);
                    else if (callsType == 3)
                        UserCallsSummary.ExportUsersCallsSummaryToPDF(SelectedSite.SiteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, chargedCalls: true);

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
                        "{0}_Monthly_Detailed_Report_{1}_{2}.pdf",
                        SelectedSite.SiteName.ToUpper(), beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString
                    );
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", SelectedSite.SiteName},
                        {"title", "Accounting Monthly Report [Detailed]"},
                        {"subTitle", String.Format("As Per: {0}; {1}", beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString)}
                    };

                    pdfDocument = new Document();

                    if (callsType == 1)
                        UserCallsSummary.ExportUsersCallsDetailedToPDF(SelectedSite.SiteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, notChargedCalls: true);
                    else if (callsType == 2)
                        UserCallsSummary.ExportUsersCallsDetailedToPDF(SelectedSite.SiteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, pendingChargesCalls: true);
                    else if (callsType == 3)
                        UserCallsSummary.ExportUsersCallsDetailedToPDF(SelectedSite.SiteName, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, chargedCalls: true);

                    Response.Write(pdfDocument);
                    break;
            }

            this.Response.End();
        }//End-function

    }

}