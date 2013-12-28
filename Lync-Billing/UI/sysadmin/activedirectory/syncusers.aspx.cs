using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;

using Lync_Billing.Backend;

namespace Lync_Billing.ui.sysadmin.activedirectory
{
    public partial class syncusers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void SyncWithADButton_Click(object sender, DirectEventArgs e)
        {
            Users.InsertUsers();
        }
    }
}