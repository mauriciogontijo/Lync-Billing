using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lync_Billing.DB;

namespace Lync_Billing.ui
{
    public partial class MasterPage: System.Web.UI.MasterPage
    {
        public UserSession current_session { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            current_session = (UserSession)HttpContext.Current.Session.Contents["UserData"];
        }
    }
}