using Lync_Billing.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.ui.dephead.main
{
    public partial class statistics : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart;
        private List<string> columns;
        private string sipAccount = string.Empty;
        private UserSession session;
        private List<Department> departments;
        public List<TopCountries> topCountries;

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

        protected void TopDestinationCountriesStore_Load(object sender, EventArgs e)
        {
            string departmentName = string.Empty;
            string siteName = string.Empty;

            siteName = "MOA";
            departmentName = "ISD";
            
            topCountries = TopCountries.GetTopDestinationsForDepartment(departmentName, siteName);

            TopDestinationCountriesStore.DataSource = topCountries;
            TopDestinationCountriesStore.DataBind();

            //if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            //{
            //    department = FilterDepartments.SelectedItem.Value;

            //    if (topCountries == null || topCountries.Count == 0)
            //    {
            //        topCountries = TopCountries.GetTopDestinationsForDepartment(department);
            //    }

            //    TopDestinationCountriesStore.DataSource = topCountries;
            //    TopDestinationCountriesStore.DataBind();
            //}
        }
    }
}