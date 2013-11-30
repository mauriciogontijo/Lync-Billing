using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Build.Tasks;
using Ext.Net;
using Lync_Billing.Backend;
using Lync_Billing.Libs;
using System.Linq.Expressions;
using Newtonsoft.Json;
using iTextSharp.text;


namespace Lync_Billing.ui.user
{
    public partial class history : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        
        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();
        private StoreReadDataEventArgs e = new StoreReadDataEventArgs();
        private string pageData = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/history.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            sipAccount = session.GetEffectiveSipAccount();
        }

        protected void PhoneCallStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            this.PhoneCallStore.DataBind();
        }

        protected void CallsHistoryDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
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

        protected void CallsHistoryDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.PhoneCallStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        public List<PhoneCall> GetCallsHistoryFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            IQueryable<PhoneCall> filteredPhoneCalls;
            List<PhoneCall> userSessionPhoneCallsHistory;
            int filteredPhoneCallsCount;

            //Get user session data
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            //Get user session phonecalls history; handle normal user mode and delegee mode
            userSessionPhoneCallsHistory = session.GetUserSessionPhoneCalls().Where(phoneCall => phoneCall.AC_InvoiceDate != DateTime.MinValue && (!string.IsNullOrEmpty(phoneCall.AC_IsInvoiced) && phoneCall.AC_IsInvoiced != "NO")).ToList();

            //Start filtering process
            if (filter == null)
                filteredPhoneCalls = userSessionPhoneCallsHistory.Where(phoneCall => !string.IsNullOrEmpty(phoneCall.UI_CallType)).AsQueryable();
            else
                filteredPhoneCalls = userSessionPhoneCallsHistory.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();

            //Start sorting process
            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                Expression<Func<PhoneCall, object>> sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.Property(param, sort.Property), param);
                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    filteredPhoneCalls = filteredPhoneCalls.OrderByDescending(sortExpression);
                else
                    filteredPhoneCalls = filteredPhoneCalls.OrderBy(sortExpression);
            }

            filteredPhoneCallsCount = filteredPhoneCalls.Count(); 

            if (start >= 0 && limit > 0)
                filteredPhoneCalls = filteredPhoneCalls.Skip(start).Take(limit);

            count = filteredPhoneCallsCount;

            return filteredPhoneCalls.ToList();
        }


        [DirectMethod]
        protected void PhoneCallsHistoryFilter(object sender, DirectEventArgs e) 
        {
            PhoneCallStore.ClearFilter();

            if (FilterTypeComboBox.SelectedItem.Value != "Everything")
                PhoneCallStore.Filter("UI_CallType", FilterTypeComboBox.SelectedItem.Value);

            PhoneCallStore.LoadPage(1);
        }
    }
}