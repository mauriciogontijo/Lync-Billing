using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lync_Billing.DB;
using Ext.Net;

namespace Lync_Billing.UI
{
    public partial class GridTest2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sipAccount = "SGhaida@ccc.gr";
            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            wherePart.Add("SourceUserUri",sipAccount);

           Store store = DBStores.PhoneCallsStore(null,wherePart,100);

            Ext.Net.Panel panel = new Ext.Net.Panel();
            panel.ID = "GridPanel";


            store.DataSource = GetPhoneCalls("SGhaida@ccc.gr");
           // store.DataBind();
            GridPanel grid = new GridPanel()
            {
                Store = { store },
                ColumnModel =
                {
                    Columns = 
                    {
                        new Column 
                        {
                            ID = "SessionIdTime",
                            Text = "SessionIdTime",
                            DataIndex = "SessionIdTime"
                        },
                        new Column 
                        {
                            ID = "SessionIdSeq",
                            Text = "SessionIdSeq",
                            DataIndex = "SessionIdSeq"
                        },
                        new Column 
                        {
                            ID = "ResponseTime",
                            Text = "ResponseTime",
                            DataIndex = "ResponseTime"
                        },
                        new Column 
                        {
                            ID = "SessionEndTime",
                            Text = "SessionEndTime",
                            DataIndex = "SessionEndTime"
                        },
                        new Column 
                        {
                            ID = "SourceUserUri",
                            Text = "SourceUserUri",
                            DataIndex = "SourceUserUri"
                        },
                        new Column 
                        {
                            ID = "SourceNumberUri",
                            Text = "SourceNumberUri",
                            DataIndex = "SourceNumberUri"
                        },
                        new Column 
                        {
                            ID = "DestinationNumberUri",
                            Text = "DestinationNumberUri",
                            DataIndex = "DestinationNumberUri"
                        },
                        new Column 
                        {
                            ID = "ServerFQDN",
                            Text = "ServerFQDN",
                            DataIndex = "ServerFQDN"
                        },
                        new Column 
                        {
                            ID = "PoolFQDN",
                            Text = "PoolFQDN",
                            DataIndex = "PoolFQDN"
                        },
                        new Column 
                        {
                            ID = "Marker_CallToCountry",
                            Text = "Marker_CallToCountry",
                            DataIndex = "Marker_CallToCountry"
                        },
                        new Column 
                        {
                            ID = "marker_CallType",
                            Text = "marker_CallType",
                            DataIndex = "marker_CallType"
                        },
                        new Column 
                        {
                            ID = "Duration",
                            Text = "Duration",
                            DataIndex = "Duration"
                        },
                        new Column 
                        {
                            ID = "Marker_CallCost",
                            Text = "Marker_CallCost",
                            DataIndex = "Marker_CallCost"
                        },
                        new Column 
                        {
                            ID = "UI_UpdatedByUser",
                            Text = "UI_UpdatedByUser",
                            DataIndex = "UI_UpdatedByUser"
                        },
                        new Column 
                        {
                            ID = "UI_MarkedOn",
                            Text = "UI_MarkedOn",
                            DataIndex = "UI_MarkedOn"
                        },
                        new Column 
                        {
                            ID = "UI_IsPersonal",
                            Text = "UI_IsPersonal",
                            DataIndex = "UI_IsPersonal"
                        },
                        new Column 
                        {
                            ID = "UI_Dispute",
                            Text = "UI_Dispute",
                            DataIndex = "UI_Dispute"
                        },
                        new Column 
                        {
                            ID = "UI_IsInvoiced",
                            Text = "UI_IsInvoiced",
                            DataIndex = "UI_IsInvoiced"
                        }
                    }
                }
            };

                       
            //grid.Store.Add(store);
            form1.Controls.Add(grid);
            
        }
        public static List<PhoneCall> GetPhoneCalls(string SourceUserUri)
        {
            Store PhoneCallsStore = new Store();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SourceUserUri", SourceUserUri);

            PhoneCallsStore.ID = "PhoneCallsStore";
            PhoneCallsStore.AutoLoad = false;
            //PhoneCallsStore.DataSource = serializer.Serialize(Lync_Billing.DB.PhoneCall.GetPhoneCalls(null, wherePart, 10));
            //DataBind();
            return Lync_Billing.DB.PhoneCall.GetPhoneCalls(null, wherePart, 10);
        }
    }
}