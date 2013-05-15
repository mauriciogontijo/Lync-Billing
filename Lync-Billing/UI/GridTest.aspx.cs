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

namespace Lync_Billing.UI
{
    public partial class GridTest : System.Web.UI.Page
    {

       Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string SipAccount = "SGhaida@ccc.gr"; //((UserSession)Session.Contents["UserData"]).SipAccount;


            wherePart.Add("SourceUserUri", SipAccount);
            wherePart.Add("marker_CallTypeID", 1);

            columns.Add("SessionIdTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_IsPersonal");
            columns.Add("ui_MarkedOn");
            columns.Add("ui_IsInvoiced");

            PhoneCallStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            PhoneCallStore.DataBind();
        }

     
        protected void GridSubmitChanges_Click(object sender, DirectEventArgs e)
        {

        }

        protected void PhoneCallStore_AfterRecordUpdated(object sender, AfterRecordUpdatedEventArgs e)
        {

        }

        protected void PhoneCallStore_AfterStoreChanged(object sender, AfterStoreChangedEventArgs e)
        {

        }

        protected void PhoneCallStore_AfterDirectEvent(object sender, AfterDirectEventArgs e)
        {

        }

        protected void PhoneCallStore_BeforeDirectEvent(object sender, BeforeDirectEventArgs e)
        {

        }

        protected void PhoneCallStore_BeforeRecordUpdated(object sender, BeforeRecordUpdatedEventArgs e)
        {

        }

        protected void PhoneCallStore_BeforeStoreChanged(object sender, BeforeStoreChangedEventArgs e)
        {

        }
        public void refreshStore(string Field, string value) 
        {
            PhoneCallsHistoryGrid.GetStore().Filters.Clear();
            PhoneCallsHistoryGrid.GetStore().Filter(Field, value);
            DataBind();
        }

        protected void FilterTypeChange(object sender, EventArgs e)
        {
            PhoneCallsHistoryGrid.GetStore().Filters.Clear();
            switch(Convert.ToInt32(FilterTypeComboBox.SelectedItem.Value))
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    refreshStore("UI_IsPersonal", "NO");
                    break;
                case 5:
                    refreshStore("UI_IsPersonal", "YES");
                    break;
                case 6:
                    break;
            }
        }
        
    }
}
