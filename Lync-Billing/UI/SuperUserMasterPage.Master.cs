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
using Lync_Billing.Backend;

namespace Lync_Billing.ui
{
    public partial class SuperUserMasterPage : System.Web.UI.MasterPage
    {
        public UserSession current_session { get; set; }
        public string HTML_SELECTED = string.Empty;
        public string PAGE_NAME = string.Empty;
        public string DROP_ACCESS_BUTTON_TEXT = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Initialize the local cope of the current user's session
            current_session = (UserSession)HttpContext.Current.Session.Contents["UserData"];


            //Initialize the sidebar-selected css class string
            HTML_SELECTED = "class='selected'";


            //Filter the page name text
            PAGE_NAME = this.Page.Request.FilePath.ToString().Replace("/", "_").Replace(".aspx", ""); //this converts the string from "/ui/example.aspx" to "_ui_example"
            if (PAGE_NAME[0] == '_') {
                PAGE_NAME = PAGE_NAME.Remove(0, 1); //this removes the first underscore (_), the final string will look like: "ui_example"
            }


            //Initialize the hidden element's value
            this.ThisPageReferrer.Value = PAGE_NAME;


            //Initialize the DropAccess button's text
            if (PAGE_NAME.Contains("ui_accounting"))
            {
                DROP_ACCESS_BUTTON_TEXT = "Drop Site Accountant Access";
            }
            else if(PAGE_NAME.Contains("ui_admin"))
            {
                DROP_ACCESS_BUTTON_TEXT = "Drop Site Administrator Access";
            }
            else if (PAGE_NAME.Contains("ui_sysadmin"))
            {
                DROP_ACCESS_BUTTON_TEXT = "Drop System Administrator Access";
            }
            else if (PAGE_NAME.Contains("ui_dephead"))
            {
                DROP_ACCESS_BUTTON_TEXT = "Drop Department Head Access";
            }
            else if (PAGE_NAME.Contains("ui_delegee_department"))
            {
                DROP_ACCESS_BUTTON_TEXT = "Drop Department Delegee Access";
            }
            else if (PAGE_NAME.Contains("ui_delegee_site"))
            {
                DROP_ACCESS_BUTTON_TEXT = "Drop Site Delegee Access";
            }
        }
    }
}