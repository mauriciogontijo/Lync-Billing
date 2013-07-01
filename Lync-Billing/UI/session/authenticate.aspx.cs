using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Lync_Billing.Libs;
using Lync_Billing.DB;

namespace Lync_Billing.ui.session
{
    public partial class authenticate : System.Web.UI.Page
    {
        public AdLib athenticator = new AdLib();
        public string AuthenticationMessage { get; set; }
        public string sipAccount = string.Empty;
        private List<string> AccessLevels = new List<string>();
        private bool redirection_flag = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/ui/session/login.aspx");
            }
            //but if the user is actually logged in we only need to check if he was granted elevated-access(s)
            else
            {
                //Get a local copy of the user's session.
                UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                //Initialize the list of current user-permissions (user-access-levels!)
                InitAccessLevels();


                //Initialize the redirection flag to true. This is responsible for redirecting the user.
                //In the default state, the user must be redirected unless the request was valid and the redirection_flag was set to false.
                redirection_flag = true;

                /*
                 * The User must pass the following autentiaction criteria
                 * PrimarySipAccount = EffectiveSipAccount, which means he is not viewing another's user account (X Person) and this X person seems to have some elevated access permissions.
                 * The asked permission actually exists - in the query string and in the system!
                 * The asked permission was actually granted for the current user.
                 **/
                if ((session.IsAdmin == true || session.IsDeveloper == true || session.IsAccountant == true) && (session.PrimarySipAccount == session.EffectiveSipAccount))
                {
                    //The following condition covers the case in which the user is asking an elevated access-permission
                    if (!string.IsNullOrEmpty(Request.QueryString["access"]) && AccessLevels.Contains(Request.QueryString["access"].ToLower()))
                    {
                        //if the user has the elevated-access-permission s/he is asking for, we fill the access text value in a hidden field in this page's form
                        if ((Request.QueryString["access"] == "admin" && session.IsAdmin) || (Request.QueryString["access"] == "accounting" && session.IsAccountant) || session.IsDeveloper)
                        {
                            //set the value of hidden field in this page to the value of passed access variable.
                            this.access_level.Value = Request.QueryString["access"].ToString();

                            //The user WOULD HAvE BEEN redirected if s/he weren't granted the elevated-access-permission s/he is asking for. But in this case, they passed the redirection.
                            redirection_flag = false;
                        }
                    }

                    //The following condition covers the case in which the user is asking to drop the already granted elevated-access-permission
                    else if (!string.IsNullOrEmpty(Request.QueryString["drop"]))
                    {
                        if (AccessLevels.Contains(Request.QueryString["drop"].ToLower()))
                        {
                            if ((Request.QueryString["drop"] == "admin" && session.IsAdmin) || (Request.QueryString["drop"] == "accounting" && session.IsAccountant) || session.IsDeveloper)
                            {
                                drop_access(Request.QueryString["drop"].ToLower());

                                //Notice the redirection which means that the user passed all the previous criteria and therefore, they shouldn't be redirected.
                                //However if they didn't pass this last condition, the redirection flag will still hold the value FALSE and the last if condition will redirect
                                //the user to the User Dashboard page.
                                redirection_flag = false;
                            }
                        }
                        else
                        {
                            if (session.ActiveRoleName == "admin") Response.Redirect("~/ui/admin/main/dashboard.aspx");
                            else if (session.ActiveRoleName == "accounting") Response.Redirect("~/ui/accounting/main/dashboard.aspx");
                            else redirection_flag = true;
                        }
                    }
                }
                
                //if the user was not granted any elevated-access permission or he is currently in a manage-delegee mode, redirect him/her to the User Dashboard page.
                //Or if the redirection_flag was not set to FALSE so far, we redurect the user to the USER DASHBOARD
                if(redirection_flag == true)
                {
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
        }


        //This function is responsilbe for initializing the value of the AccessLevels List instance variable
        private void InitAccessLevels()
        {
            AccessLevels.Add("accounting");
            AccessLevels.Add("admin");
        }


        //This function is responsible for dropping the already-granted elevated-access-permission
        private void drop_access(string access)
        {
            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).ActiveRoleName = "user";

            Response.Redirect("~/ui/user/dashboard.aspx");
        }


        //This function is responsible for authenticating the user's information.
        protected void authenticate_user(object sender, EventArgs e)
        {
            bool status = false;
            ADUserInfo userInfo = new ADUserInfo();
            UserSession session = new UserSession();
            string msg = string.Empty;
            string user_email = string.Empty;

            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                user_email = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount.ToLower();

                if (!string.IsNullOrEmpty(this.access_level.Value))
                {
                    status = athenticator.AuthenticateUser(user_email, this.password.Text, out msg);
                    AuthenticationMessage = msg;

                    if (status == true)
                    {
                        if (this.access_level.Value == "admin")
                        {
                            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).ActiveRoleName = "admin";
                            Response.Redirect("~/ui/admin/main/dashboard.aspx");
                        }
                        else if (this.access_level.Value == "accounting")
                        {
                            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).ActiveRoleName = "accounting";
                            Response.Redirect("~/ui/accounting/main/dashboard.aspx");
                        }
                        else
                        {
                            //the value of the access_level hidden field has changed - fraud value!
                            Response.Redirect("~/ui/user/dashboard.aspx");
                        }
                    }
                }

                if (AuthenticationMessage.ToString() != string.Empty)
                {
                    AuthenticationMessage = "* " + AuthenticationMessage;
                }
            }
            else
            {
                Response.Redirect("~/ui/user/dashboard.aspx");
            }
        }//END OF FUNCTION
    }
}