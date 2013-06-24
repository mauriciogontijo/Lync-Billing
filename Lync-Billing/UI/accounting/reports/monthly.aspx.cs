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
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Lync_Billing.ui.accounting.reports
{
    public partial class monthly : System.Web.UI.Page
    {

        private StoreReadDataEventArgs e;
        private DateTime date;

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

                if (!session.IsDeveloper && !session.IsAccountant && session.PrimarySipAccount != session.EffectiveSipAccount)
                {
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }
        }


        protected List<UsersCallsSummary> MonthlyReports(string sitename, DateTime date)
        {
            List<string> sites = new List<string>();
            List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();

            if (reportDateField.SelectedDate != DateTime.MinValue)
            {
                date = reportDateField.SelectedDate;
                sites = GetAccountantSiteName();

                foreach (string site in sites) 
                {
                    listOfUsersCallsSummary.AddRange( UsersCallsSummary.GetUsersCallsSummary(date, date, sitename).AsEnumerable<UsersCallsSummary>());
                }
            }
            return listOfUsersCallsSummary;
        }

        protected void MonthlyReportsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (this.e.Start != -1)
                e.InputParameters["start"] = this.e.Start;
            else
                e.InputParameters["start"] = 0;

            if (this.e.Limit != -1)
                e.InputParameters["limit"] = this.e.Limit;
            else
                e.InputParameters["limit"] = 25;

            if (!string.IsNullOrEmpty(this.e.Parameters["sort"]))
                e.InputParameters["sort"] = this.e.Sort[0];
            else
                e.InputParameters["sort"] = null;

            if (!string.IsNullOrEmpty(this.e.Parameters["filter"]))
                e.InputParameters["filter"] = this.e.Filter[0];
            else
                e.InputParameters["filter"] = null;
        }

        protected void MonthlyReportsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.MonthlyReportsStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        public List<UsersCallsSummary> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            List<UsersCallsSummary> listOfUsersCallsSummary = new List<UsersCallsSummary>();
            //MonthlyReports("MOA");

            IQueryable<UsersCallsSummary> result;

            if (filter == null)
                result = listOfUsersCallsSummary.AsQueryable();
            else
                result = listOfUsersCallsSummary.Where ( userEntry => userEntry.EmployeeID.Contains(filter.Value) ||  userEntry.FullName.Contains(filter.Value) ).AsQueryable();

            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                Expression<Func<UsersCallsSummary, object>> sortExpression = Expression.Lambda<Func<UsersCallsSummary, object>>(Expression.Property(param, sort.Property), param);
                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    result = result.OrderByDescending(sortExpression);
                else
                    result = result.OrderBy(sortExpression);
            }

            int resultCount = result.Count();

            if (start >= 0 && limit > 0)
                result = result.Skip(start).Take(limit);

            count = resultCount;

            return result.ToList();
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


    }
}