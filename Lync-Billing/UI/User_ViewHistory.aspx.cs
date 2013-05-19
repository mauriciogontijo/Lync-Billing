using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using Ext.Net;
using Lync_Billing.DB;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Build.Tasks;


namespace Lync_Billing.UI
{
    public partial class User_ViewHistory : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
           
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
            this.PhoneCallStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            this.PhoneCallStore.DataBind();
        }

        protected void PhoneCallStore_Load(object sender, EventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            wherePart.Add("SourceUserUri", SipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ui_IsInvoiced", "YES");
            columns.Add("SessionIdTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_IsPersonal");
            columns.Add("ui_MarkedOn");
            columns.Add("ui_IsInvoiced");

            if (PhoneCall.PhoneCalls.Count == 0)
            {
                PhoneCall.PhoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
                PhoneCallStore.DataSource = PhoneCall.PhoneCalls;
                PhoneCallStore.DataBind();
            }
            else
            {
                PhoneCallStore.DataSource = PhoneCall.PhoneCalls;
                PhoneCallStore.DataBind();
            }
        }
    }
}