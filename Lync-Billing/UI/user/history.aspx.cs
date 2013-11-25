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
using Lync_Billing.DB;
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

            sipAccount = GetEffectiveSipAccount();
        }


        //Get the user sipaccount.
        private string GetEffectiveSipAccount()
        {
            string userSipAccount = string.Empty;
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the user is a normal one, just return the normal user sipaccount.
            if (session.ActiveRoleName == normalUserRoleName)
            {
                userSipAccount = session.NormalUserInfo.SipAccount;
            }
            //if the user is a user-delegee return the delegate sipaccount.
            else if (session.ActiveRoleName == userDelegeeRoleName)
            {
                userSipAccount = session.DelegeeAccount.DelegeeUserAccount.SipAccount;
            }

            return userSipAccount;
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
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            getPhoneCalls();

            IQueryable<PhoneCall> result;

            if (filter == null)
                result = session.PhonecallsHistory.Where(phoneCall => phoneCall.UI_CallType != null).AsQueryable();
            else
                result = session.PhonecallsHistory.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();

            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                Expression<Func<PhoneCall, object>> sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.Property(param, sort.Property), param);
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

        protected void getPhoneCalls(bool force = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();

            wherePart = new Dictionary<string, object>();

            if (session.PhonecallsHistory == null || session.PhonecallsHistory.Count == 0 || force == true)
            {
                wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced), "YES");

                session.PhonecallsHistory = PhoneCall.GetPhoneCalls(sipAccount, wherePart, 0);
            }
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