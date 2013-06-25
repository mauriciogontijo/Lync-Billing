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


namespace Lync_Billing.ui.user
{
    public partial class history : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        
        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();
        private StoreReadDataEventArgs e = new StoreReadDataEventArgs();
        private string sipAccount = string.Empty;
        private string pageData = string.Empty;
        //private string filter = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/history.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
        }

        protected void PhoneCallStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);
            
            this.Response.End();
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
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            getPhoneCalls();

            IQueryable<PhoneCall> result;

            if (filter == null)
                result = userSession.PhoneCallsHistory.Where(phoneCall => phoneCall.UI_CallType != null).AsQueryable();
            else
                result = userSession.PhoneCallsHistory.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();

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
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = userSession.EffectiveSipAccount;

            if (userSession.PhoneCallsHistory == null || userSession.PhoneCallsHistory.Count == 0 || force == true)
            {
                wherePart.Add("SourceUserUri", sipAccount);
                wherePart.Add("marker_CallTypeID", 1);
                //wherePart.Add("ac_IsInvoiced", "YES");

                columns.Add("SessionIdTime");
                columns.Add("marker_CallToCountry");
                columns.Add("DestinationNumberUri");
                columns.Add("Duration");
                columns.Add("marker_CallCost");
                columns.Add("ui_CallType");
                columns.Add("ui_MarkedOn");
                columns.Add("ac_IsInvoiced");

                userSession.PhoneCallsHistory = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
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