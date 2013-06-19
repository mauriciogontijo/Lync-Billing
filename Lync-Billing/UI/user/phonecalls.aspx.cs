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
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Lync_Billing.ui.user
{
    public partial class phonecalls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/phonecalls.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
        }

        protected void PhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }

        protected void PhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {

        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            count = 0;
            return null;
        }

        protected void AssignAllPersonal(object sender, DirectEventArgs e)
        {

        }

        protected void AssignAllBusiness(object sender, DirectEventArgs e)
        {

        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {

        }

        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {

        }

        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {

        }
    }
}