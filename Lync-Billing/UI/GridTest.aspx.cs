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

namespace Lync_Billing.UI
{
    public partial class GridTest : System.Web.UI.Page
    {

       Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;


            wherePart.Add("SourceUserUri", SipAccount);

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

        protected void FilterTypeChange(object sender, EventArgs e)
        {
            
            switch(Convert.ToInt32(FilterTypeComboBox.SelectedItem.Value))
            {
                case 1:
                    PhoneCallsHistoryGrid.GetStore().Filter("ui_IsPersonal", null);        
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }

            
            
        }
        
    }
}
