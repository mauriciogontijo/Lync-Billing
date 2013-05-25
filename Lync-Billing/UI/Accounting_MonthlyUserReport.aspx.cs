﻿using System;
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

namespace Lync_Billing.UI
{
    public partial class Accounting_MonthlyUserReport : System.Web.UI.Page
    {

        Store UserBusinessCallsStore = new Store();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
            }
            else
            {
                UserSession session = (UserSession)Session.Contents["UserData"];

                if (!session.IsDeveloper && !session.IsAccountant)
                {
                    Response.Redirect("~/UI/User_Dashboard.aspx");
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

        protected void Button1_DirectClick(object sender, DirectEventArgs e)
        {
            UserSession session = (UserSession)Session.Contents["UserData"];

            int year = 0;
            int month = 0;

            DateTime date = DateField.SelectedDate;

            List<UsersCallsSummary> userSummary;
            decimal BusinessCallCost;
            string sipAccount = string.Empty;

            year = date.Year;
            month = date.Month;

            if (ValidateAccountantSite(GroupNumberField.Text) == true)
            {
                sipAccount = GetSipAccount(GroupNumberField.Text);
                userSummary = UsersCallsSummary.GetUsersCallsSummary(sipAccount, year, month, month);

                BusinessCallCost = userSummary[0].BusinessCallsCost;

                XElement eml = new XElement(
                    "Records",
                    new XElement(
                        "Record",
                        new XElement("EmployeeID", GroupNumberField.Text),
                        new XElement("SipAccount", sipAccount),
                        new XElement("Cost",BusinessCallCost)
                        )
                    );

                XmlDocument xmlDoc = new XmlDocument();

                using (System.Xml.XmlReader xmlReader = eml.CreateReader())
                {
                    xmlDoc.Load(xmlReader);
                }

                XmlNode xmlNode = xmlDoc; //.DocumentElement;

                this.Response.Clear();
                this.Response.ContentType = "application/vnd.ms-excel";
                this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                XslCompiledTransform xtExcel = new XslCompiledTransform();
                xtExcel.Load(Server.MapPath("Excel.xslt"));
                xtExcel.Transform(xmlNode, null, this.Response.OutputStream);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Response.Flush();
                this.Response.Close();

            }
        }
    }
}