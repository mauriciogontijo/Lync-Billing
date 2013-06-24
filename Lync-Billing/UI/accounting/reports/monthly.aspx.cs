using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using Ext.Net;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Data;
using System.IO;
using System.Text;

namespace Lync_Billing.ui.accounting.reports
{
    public partial class monthly : System.Web.UI.Page
    {

        Store UserBusinessCallsStore = new Store();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/accounting/reports/monthly.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if (!session.IsDeveloper && !session.IsAccountant)
                {
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }
        }

        public string GetSiteName(int siteID) 
        {
            
            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            wherePart.Add("SiteID", siteID);

            List<Site> sites = DB.Site.GetSites(null, wherePart, 0);

            return sites[0].SiteName;
        }

        public bool ValidateAccountantSite(string employeeID) 
        {
            UserSession session = (UserSession)Session.Contents["UserData"];

            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles) 
            {
                if ((GetSiteName(role.SiteID) == GetSipAccountSite(employeeID)) && (role.RoleID == 7 || role.RoleID == 1)) 
                    return true;
            }
            return false;
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

        protected void FilterReportButton_DirectClick(object sender, DirectEventArgs e)
        {
            int year = 0;
            int month = 0;
            decimal BusinessCallCost;
            string sipAccount = string.Empty;
            DataSet ds = new DataSet();
            List<UsersCallsSummary> userSummary;
            DateTime date = DateField.SelectedDate;
            UserSession session = (UserSession)Session.Contents["UserData"];

            year = date.Year;
            month = date.Month;

            if (ValidateAccountantSite(UserSearch.Text) == true)
            {
                sipAccount = GetSipAccount(UserSearch.Text);
                userSummary = UsersCallsSummary.GetUsersCallsSummary(sipAccount, year, month, month);
                BusinessCallCost = userSummary[0].BusinessCallsCost;

                XElement eml = new XElement(
                    "records",
                    new XElement(
                        "record",
                        new XElement("EmployeeID", UserSearch.Text),
                        new XElement("SipAccount", sipAccount),
                        new XElement("Cost", BusinessCallCost)
                    )
                );

                XmlDocument xml = new XmlDocument();
                xml.Load(eml.CreateReader());

                ds.ReadXml(eml.CreateReader());
                DataTable dt = ds.Tables[0];

                this.Response.Clear();
                this.Response.ContentType = "application/vnd.ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");

                XslCompiledTransform xtExcel = new XslCompiledTransform();
                xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
                xtExcel.Transform(xml, null, Response.OutputStream);

                this.Response.End();
            }
        }//END OF FUNCTION
    }
}