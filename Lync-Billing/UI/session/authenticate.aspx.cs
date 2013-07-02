﻿using System;
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
        public string HeaderAuthBoxMessage { get; set; }
        public string ParagraphAuthBoxMessage { get; set; }
        public string sipAccount = string.Empty;

        private string accessParam = string.Empty;
        private string identityParam = string.Empty;
        private string dropParam = string.Empty;
        private bool redirectionFlag = true;
        private List<string> AccessLevels = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            HeaderAuthBoxMessage = string.Empty;
            ParagraphAuthBoxMessage = string.Empty;
            AuthenticationMessage = string.Empty;

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
                redirectionFlag = true;

                /*
                 * The User must pass the following autentiaction criteria
                 * PrimarySipAccount = EffectiveSipAccount, which means he is not viewing another's user account (X Person) and this X person seems to have some elevated access permissions.
                 * The asked permission actually exists - in the query string and in the system!
                 * The asked permission was actually granted for the current user.
                 **/

                //Mode 1: The Normal User Mode
                if (session.PrimarySipAccount == session.EffectiveSipAccount)
                {
                    //Case 1: The user asks for Admin or Accounting access
                    //Should pass "access" and "access" should be coherent within our own system
                    //Shouldn't pass the other variables, such as: identity (see the case of "identity" below.
                    //The following condition covers the case in which the user is asking an elevated access-permission
                    if (!string.IsNullOrEmpty(Request.QueryString["access"]) && AccessLevels.Contains(Request.QueryString["access"].ToLower()) && string.IsNullOrEmpty(Request.QueryString["identity"]))
                    {
                        accessParam = Request.QueryString["access"].ToLower();
                        HeaderAuthBoxMessage = "You have requested an elevated access";
                        ParagraphAuthBoxMessage = "Please note that you must authenticate your information before proceeding any further.";

                        //if the user was authenticated already
                        if (session.ActiveRoleName != "user" && (session.IsAdmin || session.IsAccountant || session.IsDeveloper))
                        {
                            RedirectToElevatedAccessDasboard(session.ActiveRoleName);
                        }

                        //if the user has the elevated-access-permission s/he is asking for, we fill the access text value in a hidden field in this page's form
                        else if ((accessParam == "admin" && session.IsAdmin) || (accessParam == "accounting" && session.IsAccountant) || session.IsDeveloper)
                        {
                            //set the value of hidden field in this page to the value of passed access variable.
                            this.access_level.Value = accessParam;

                            //The user WOULD HAvE BEEN redirected if s/he weren't granted the elevated-access-permission s/he is asking for. But in this case, they passed the redirection.
                            redirectionFlag = false;
                        }
                    }

                    //Case 2: The user asks for Delegee access
                    if (!string.IsNullOrEmpty(Request.QueryString["access"]) && !string.IsNullOrEmpty(Request.QueryString["identity"]))
                    {
                        accessParam = Request.QueryString["access"].ToLower();
                        identityParam = Request.QueryString["identity"]; //NEVER LOWER CASE - This is SipAccount
                        HeaderAuthBoxMessage = "You have requested to manage a delegee account";
                        ParagraphAuthBoxMessage = "Please note that you must authenticate your information before proceeding any further.";

                        //if the user has the elevated-access-permission s/he is asking for, we fill the access text value in a hidden field in this page's form
                        if ((session.IsDelegate && accessParam == "delegee" && session.ListOfDelegees.Keys.Contains(identityParam)) || session.IsDeveloper)
                        {
                            //set the value of hidden field in this page to the value of passed access variable.
                            this.access_level.Value = accessParam;
                            this.delegee_identity.Value = identityParam;
                            //SwitchToDelegee(identityParam);

                            //The user WOULD HAVE BEEN redirected if s/he weren't granted the elevated-access-permission s/he is asking for. But in this case, they passed the redirection.
                            redirectionFlag = false;
                        }
                    }

                    //The following condition covers the case in which the user is asking to drop the already granted elevated-access-permission
                    else if (!string.IsNullOrEmpty(Request.QueryString["drop"]))
                    {
                        dropParam = Request.QueryString["drop"].ToLower();

                        //Case 1: Drop Admin or Accounting Access
                        if (AccessLevels.Contains(dropParam))
                        {
                            if ((dropParam == "admin" && session.IsAdmin && session.ActiveRoleName == "admin") || 
                                (dropParam == "accounting" && session.IsAccountant && session.ActiveRoleName == "accounting") || 
                                session.IsDeveloper)
                            {
                                DropAccess(dropParam);

                                //Notice the redirection which means that the user passed all the previous criteria and therefore, they shouldn't be redirected.
                                //However if they didn't pass this last condition, the redirection flag will still hold the value FALSE and the last if condition will redirect
                                //the user to the User Dashboard page.
                                redirectionFlag = false;
                            }
                        }
                        else
                        {
                            //The user was already authenticated, redirect him/het to the respective elevated access dashboard
                            if (session.ActiveRoleName != "user" && session.ActiveRoleName != "delegee")
                            {
                                RedirectToElevatedAccessDasboard(session.ActiveRoleName);
                            }
                            else redirectionFlag = true;
                        }
                    }
                }

                //Mode 2: The Delegee Mode
                else if (session.PrimarySipAccount != session.EffectiveSipAccount)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["drop"]))
                    {
                        dropParam = Request.QueryString["drop"].ToLower();

                        if (dropParam == "delegee" && session.ActiveRoleName == "delegee")
                        {
                            DropAccess(dropParam);

                            //Notice the redirection which means that the user passed all the previous criteria and therefore, they shouldn't be redirected.
                            //However if they didn't pass this last condition, the redirection flag will still hold the value FALSE and the last if condition will redirect
                            //the user to the User Dashboard page.
                            redirectionFlag = false;
                        }
                    }
                }

                //if the user was not granted any elevated-access permission or he is currently in a manage-delegee mode, redirect him/her to the User Dashboard page.
                //Or if the redirection_flag was not set to FALSE so far, we redurect the user to the USER DASHBOARD
                if(redirectionFlag == true)
                {
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
        }//END OF PAGE_LOAD


        //This function handles the 
        public void RedirectToElevatedAccessDasboard(string role)
        {
            if (role == "admin")
            {
                Response.Redirect("~/ui/admin/main/dashboard.aspx");
            }
            else if (role == "accounting")
            {
                Response.Redirect("~/ui/accounting/main/dashboard.aspx");
            }
        }


        //This function is responsilbe for initializing the value of the AccessLevels List instance variable
        private void InitAccessLevels()
        {
            AccessLevels.Add("accounting");
            AccessLevels.Add("admin");
        }

        //This function handles the switching to delegees
        private void SwitchToDelegee(string delegeeSipAccount)
        {
            //Initialize a temp copy of the User Session
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //Switch identity
            session.EffectiveSipAccount = delegeeSipAccount;
            session.EffectiveDisplayName = session.ListOfDelegees[session.EffectiveSipAccount];

            //Initialize the PhoneBook in the session
            session.PhoneBook = new Dictionary<string, PhoneBook>();
            session.PhoneBook = PhoneBook.GetAddressBook(session.EffectiveSipAccount);

            //Clear the PhoneCalls containers in the session
            session.PhoneCalls = new List<PhoneCall>();
            session.PhoneCallsPerPage = string.Empty;

            //Set the ActiveRoleName to "delegee"
            session.ActiveRoleName = "delegee";

            //Redirect to Uer Dashboard
            Response.Redirect("~/ui/user/dashboard.aspx");
        }


        //This function is responsible for dropping the already-granted elevated-access-permission
        private void DropAccess(string access)
        {
            //Initialize a temp copy of the User Session
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            if (access == "delegee")
            {
                //Switch back to original identity
                session.EffectiveSipAccount = session.PrimarySipAccount;
                session.EffectiveDisplayName = session.PrimaryDisplayName;

                //Initialize the PhoneBook in the session
                session.PhoneBook = new Dictionary<string, PhoneBook>();
                session.PhoneBook = PhoneBook.GetAddressBook(session.EffectiveSipAccount);

                //Clear the PhoneCalls containers in the session
                session.PhoneCalls = new List<PhoneCall>();
                session.PhoneCallsPerPage = string.Empty;
            }
            
            //Always set the ActiveRoleName to "user"
            session.ActiveRoleName = "user";

            //Redirect to the User Dashboard
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
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];
                user_email = session.EffectiveSipAccount.ToLower();

                if (this.access_level != null) //!string.IsNullOrEmpty(this.access_level.Value)
                {
                    status = athenticator.AuthenticateUser(user_email, this.password.Text, out msg);
                    AuthenticationMessage = msg;

                    if (status == true)
                    {
                        if (this.access_level.Value == "admin")
                        {
                            session.ActiveRoleName = "admin";
                            Response.Redirect("~/ui/admin/main/dashboard.aspx");
                        }
                        else if (this.access_level.Value == "accounting")
                        {
                            session.ActiveRoleName = "accounting";
                            Response.Redirect("~/ui/accounting/main/dashboard.aspx");
                        }
                        else if (this.access_level.Value == "delegee" && this.delegee_identity != null)
                        {
                            if (session.ListOfDelegees.Keys.Contains(this.delegee_identity.Value))
                            {
                                SwitchToDelegee(this.delegee_identity.Value);
                            }
                            else
                            {
                                //the value of the access_level hidden field has changed - fraud value!
                                session.ActiveRoleName = "user";
                                Response.Redirect("~/ui/user/dashboard.aspx");
                            }
                        }
                        else
                        {
                            //the value of the access_level hidden field has changed - fraud value!
                            session.ActiveRoleName = "user";
                            Response.Redirect("~/ui/user/dashboard.aspx");
                        }
                    }
                }
                else
                {
                    //the value of the access_level hidden field has changed - fraud value!
                    session.ActiveRoleName = "user";
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }

                if (AuthenticationMessage.ToString() != string.Empty)
                {
                    AuthenticationMessage = "* " + AuthenticationMessage;
                }
            }
            else
            {
                Response.Redirect("~/ui/session/login.aspx");
            }
        }//END OF FUNCTION
    }
}