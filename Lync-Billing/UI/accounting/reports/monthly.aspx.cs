﻿using System;
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
    public partial class monthly : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        private StoreReadDataEventArgs e;
        List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();
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


        protected List<UsersCallsSummary> MonthlyReports(string site, DateTime date)
        {
            List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();

            DateTime month_start = new DateTime(date.Year, date.Month, 1);
            DateTime month_end = month_start.AddMonths(1).AddDays(-1);

            listOfUsersCallsSummary.AddRange(
                UsersCallsSummary.GetUsersCallsSummary(month_start, month_end, site).Where
                            (e => e.PersonalCallsCost != 0 || e.BusinessCallsCost != 0 || e.UnmarkedCallsCost != 0).AsEnumerable<UsersCallsSummary>());
            
            return listOfUsersCallsSummary;
        }

        public List<Site> GetAccountantSites()
        {
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            List<UserRole> userRoles = session.Roles;
            List<int> tmpUserSites = new List<int>();

            tmpUserSites = userRoles.Where(item => item.RoleID == 7 || item.RoleID == 1).Select(item => item.SiteID).ToList();

            foreach (int site in tmpUserSites)
            {
                sites.Add(DB.Site.getSite(site));
            }

            List<Site> tmpSites = DB.Site.GetSites();

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
                    beginningOfTheMonth = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                    endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=AccountingMonthlyReport_Summary.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Monthly Report [Summary]"},
                        {"subTitle", "As Per: " + beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year}
                    };

                    pdfDocument = new Document();
                    UsersCallsSummary.ExportUsersCallsSummaryToPDF(beginningOfTheMonth, endOfTheMonth, siteName, Response, out pdfDocument, pdfDocumentHeaders);
                    Response.Write(pdfDocument);
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

                    beginningOfTheMonth = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                    endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);
                    
                    //Initialize the response.
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=AccountingMonthlyReport_Detailed.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", siteName},
                        {"title", "Accounting Monthly Report [Detailed]"},
                        {"subTitle", "As Per: " + beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year}
                    };

                    pdfDocument = new Document();
                    UsersCallsSummary.ExportUsersCallsDetailedToPDF(beginningOfTheMonth, endOfTheMonth, siteName, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders);
                    Response.Write(pdfDocument);
                    break;
            }

            this.Response.End();
        }

    }
}