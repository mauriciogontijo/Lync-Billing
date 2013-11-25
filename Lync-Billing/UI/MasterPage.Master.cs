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

        public string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        public string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        //public variable made available for the view
        public string DisplayName = string.Empty;

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

        //Get the user displayname.
        private string GetEffectiveDisplayName()
        {
            string userDisplayName = string.Empty;
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the user is a normal one, just return the normal user sipaccount.
            if (session.ActiveRoleName == normalUserRoleName)
            {
                userDisplayName = session.NormalUserInfo.DisplayName;
            }
            //if the user is a user-delegee return the delegate sipaccount.
            else if (session.ActiveRoleName == userDelegeeRoleName)
            {
                userDisplayName = session.DelegeeAccount.DelegeeUserAccount.DisplayName;
            }

            return userDisplayName;
        }
    }
}