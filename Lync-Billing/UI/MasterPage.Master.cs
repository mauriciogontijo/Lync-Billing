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
        public string HTML_SELECTED = string.Empty;
        public string PAGE_NAME = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            current_session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            HTML_SELECTED = "class='selected'";

            PAGE_NAME = this.Page.Request.FilePath.ToString().Replace("/", "_").Replace(".aspx", ""); //this converts the string from "/ui/example.aspx" to "_ui_example"
            if (PAGE_NAME[0] == '_')
            {
                PAGE_NAME = PAGE_NAME.Remove(0, 1); //this removes the first underscore (_), the final string will look like: "ui_example"
            }

            this.ThisPageReferrer.Value = PAGE_NAME;
        }
    }
}