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


namespace Lync_Billing.ui.user
{
    public partial class history : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        
        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();
        private StoreReadDataEventArgs e;
        private string sipAccount = string.Empty;
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
        }

        public void refreshStore(string Field, string value)
        {
            PhoneCallsHistoryGrid.GetStore().Filters.Clear();
            PhoneCallsHistoryGrid.GetStore().Filter(Field, value);
            DataBind();
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
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            userSession.PhoneCallsPerPage = PhoneCallStore.JsonData;
        }

        protected void PhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["start"] = this.e.Start;
            e.InputParameters["limit"] = this.e.Limit;
            e.InputParameters["sort"] = this.e.Sort[0];
        }

        protected void PhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.PhoneCallStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, string filter = "none")
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            getPhoneCalls();

            IQueryable<PhoneCall> result = userSession.PhoneCallsHistory.Select(e => e).AsQueryable();

            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                Expression<Func<PhoneCall, object>> sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.Property(param, sort.Property), param);
                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    result = result.OrderByDescending(sortExpression);
                else
                    result = result.OrderBy(sortExpression);
            }

            if (start >= 0 && limit > 0)
                result = result.Skip(start).Take(limit);

            count = userSession.PhoneCallsHistory.Count();
            return result.ToList();
        }

        protected void getPhoneCalls(bool force = false)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            if (userSession.PhoneCallsHistory == null || userSession.PhoneCallsHistory.Count == 0 || force == true)
            {
                string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

                wherePart.Add("SourceUserUri", SipAccount);
                wherePart.Add("marker_CallTypeID", 1);

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
        public void PhoneCallsHistoryFilter(object sender, EventArgs e)
        {
            
        }
    }
}