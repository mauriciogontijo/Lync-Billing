using Ext.Net;
using Lync_Billing.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.ui.dephead.users
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        private string sipAccount = string.Empty;
        private UserSession session;
        private List<Department> departments;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/dephead/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != "dephead")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=dephead");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            BindDepartmentsForThisUser();
        }


        private void BindDepartmentsForThisUser()
        {
            departments = DepartmentHead.GetDepartmentsForHead(sipAccount);

            if (departments.Count == 1)
            {
                FilterDepartments.SetValueAndFireSelect(departments.First().DepartmentName);
                FilterDepartments.ReadOnly = true;
            }
            else
            {
                FilterDepartments.ReadOnly = false;
                FilterDepartments.GetStore().DataSource = departments;
                FilterDepartments.GetStore().DataBind();
            }
        }


        private List<Users> GetUsers(string departmentName)
        {
            //List<Users> users = new List<Users>();
            List<string> columns = new List<string>();
            Dictionary<string, object> whereClause = new Dictionary<string, object>() 
            { 
                { "AD_Department", departmentName } 
            };

            return Users.GetUsers(columns, whereClause, 0);
        }


        protected void GetUsersPerDepartment(object sender, DirectEventArgs e)
        {
            FilterUsersByDepartment.Clear();
            FilterUsersByDepartment.ReadOnly = false;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                List<Users> users = GetUsers(FilterDepartments.SelectedItem.Value);

                FilterUsersByDepartment.Disabled = false;
                FilterUsersByDepartment.GetStore().DataSource = users;
                FilterUsersByDepartment.GetStore().DataBind();

                if (users.Count == 1)
                {
                    FilterUsersByDepartment.SetValueAndFireSelect(users.First().SipAccount);
                    FilterUsersByDepartment.ReadOnly = true;
                }
                else
                {
                    FilterUsersByDepartment.ReadOnly = false;
                }
            }
        }


    }
}