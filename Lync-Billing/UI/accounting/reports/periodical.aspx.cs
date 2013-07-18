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

        protected void FilterReportsBySite_Selecting(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1)
            {
                StartingDate.Disabled = false;

                //If the dates were previously chosen, jsut refresh the data!
                if (FilterReportsBySite.SelectedItem.Index != -1 && StartingDate.SelectedValue != null && EndingDate.SelectedValue != null)
                {
                    ReportExportOptions.Disabled = false;

                    listOfUsersCallsSummary = PeriodicalReport(FilterReportsBySite.SelectedItem.Value, StartingDate.SelectedDate, EndingDate.SelectedDate);
                    PeriodicalReportsGrid.GetStore().DataSource = listOfUsersCallsSummary;
                    PeriodicalReportsGrid.GetStore().LoadData(listOfUsersCallsSummary);
                }
            }
            else
            {
                StartingDate.Disabled = true;
                EndingDate.Disabled = true;
                ReportExportOptions.Disabled = true;
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
                ReportExportOptions.Disabled = true;
            }
        }

        protected void EndingDate_Selection(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1 && StartingDate.SelectedValue != null && EndingDate.SelectedValue != null)
            {
                ReportExportOptions.Disabled = false;

                listOfUsersCallsSummary = PeriodicalReport(FilterReportsBySite.SelectedItem.Value, StartingDate.SelectedDate, EndingDate.SelectedDate);
                PeriodicalReportsGrid.GetStore().DataSource = listOfUsersCallsSummary;
                PeriodicalReportsGrid.GetStore().LoadData(listOfUsersCallsSummary);
            }
            else
            {
                ReportExportOptions.Disabled = true;
            }
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

            Response.Buffer = false;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "Application/pdf";
            Response.BinaryWrite(mStream.ToArray());
            Response.Flush();
            Response.End();
            //Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf");
            //Response.Clear();
            //Response.BinaryWrite(mStream.ToArray());
            //Response.End();
        }

    }
}