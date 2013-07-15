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

        private StoreReadDataEventArgs e;
        List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();


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


        protected List<UsersCallsSummary> MonthlyReports( DateTime date)
        {
            List<string> sites = new List<string>();
            List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();
           
            sites = GetAccountantSiteName();

            foreach (string site in sites) 
            {
                listOfUsersCallsSummary.AddRange(UsersCallsSummary.GetUsersCallsSummary(date, date, site).Where(e => e.PersonalCallsCost !=0).AsEnumerable<UsersCallsSummary>());
            }
            
            return listOfUsersCallsSummary;
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
            if (reportDateField.SelectedValue != null)
            {
                listOfUsersCallsSummary = MonthlyReports(reportDateField.SelectedDate);
                MonthlyReportsGrids.GetStore().DataSource = listOfUsersCallsSummary;
                MonthlyReportsGrids.GetStore().LoadData(listOfUsersCallsSummary);
            }
        }

        protected void MonthlyReportsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
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

            List<string> columns = new List<string>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            columns.Add("SourceUserUri");
            columns.Add("SourceNumberUri");
            columns.Add("DestinationNumberUri");
            columns.Add("ResponseTime");
            columns.Add("Duration");
            columns.Add("ui_CallType");
            columns.Add("marker_CallCost");

            wherePart.Add("marker_CallTypeID", "1");
            wherePart.Add("ac_IsInvoiced", "NO");
            wherePart.Add("ui_CallType", "Personal");

            DBLib dbRoutines = new DBLib();
            DataTable dt = new DataTable();

            dt = dbRoutines.SELECT(Enums.GetDescription(Enums.PhoneCalls.TableName), columns, wherePart, 0);

            Document pdfDoc = new Document(PageSize.A4, 30, 30, 40, 25);
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, mStream);
            int cols = dt.Columns.Count;
            int rows = dt.Rows.Count;
            pdfDoc.Open();

            PdfPTable pdfTable = new PdfPTable(cols);

            //creating table headers
            for (int i = 0; i < cols; i++)
            {
                PdfPCell cellCols = new PdfPCell();
                Font ColFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
                Chunk chunkCols = new Chunk(dt.Columns[i].ColumnName, ColFont);
                cellCols.AddElement(chunkCols);
                pdfTable.AddCell(cellCols);
            }

            //creating table data (actual result)
            for (int k = 0; k < rows; k++)
            {
                for (int j = 0; j < cols; j++)
                {
                    PdfPCell cellRows = new PdfPCell();
                    Font RowFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    Chunk chunkRows = new Chunk(dt.Rows[k][j].ToString(), RowFont);
                    cellRows.AddElement(chunkRows);
                    pdfTable.AddCell(cellRows);

                }
            }

            pdfDoc.Add(pdfTable);
            pdfDoc.Close();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf");
            Response.Clear();
            Response.BinaryWrite(mStream.ToArray());
            Response.End();
        }
    }
}