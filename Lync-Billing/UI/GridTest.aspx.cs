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

        protected void Page_Load(object sender, EventArgs e)
        {
            ObjectDataSource1.SelectParameters.Add("SourceUserUri","Sghaida@ccc.gr");
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