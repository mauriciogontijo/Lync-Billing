using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Lync_Billing.Libs;
using Lync_Billing.Backend;

using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.session
{
    public partial class login : System.Web.UI.Page
    {
        public AdLib athenticator = new AdLib();
        public string AuthenticationMessage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                Response.Redirect("~/ui/user/dashboard.aspx");
            }

            //Check if a redirect_to value has been passed and validate it's link
            if (Request.QueryString["redirect_to"] != null && Request.QueryString["redirect_to"] != string.Empty)
            {
                //This statement validates that the link must contain the application root path and the page extension at the end of it
                if (Request.QueryString["redirect_to"].Contains(@"~/ui/") && Request.QueryString["redirect_to"].Contains(@".aspx"))
                {
                    this.redirect_to_url.Value = Request.QueryString["redirect_to"];
                }
                else
                {
                    this.redirect_to_url.Value = string.Empty;
                }
            }

            AuthenticationMessage = string.Empty;
        }


        /// <summary>
        /// Session managemenet routine. This is called from the SignButton_Click procedure.
        /// </summary>
        /// <param name="session">The current user session, sent by reference.</param>
        /// <param name="userInfo">The current user info</param>
        private void SetUserSessionFields(ref UserSession session, ADUserInfo userInfo)
        {
            //First and foremost initialize the user's most basic and mandatory fields
            session.NormalUserInfo = Users.GetUser(userInfo.SipAccount.Replace("sip:", ""));
            session.NormalUserInfo.DisplayName = HelperFunctions.FormatUserDisplayName(userInfo.DisplayName, userInfo.SipAccount);
            
            session.DelegeeAccount = null;

            //Initialize his/her ROLES AND THEN DELEGEES information
            session.InitializeAllRolesInformation(session.NormalUserInfo.SipAccount);
            
            //Lastly, get some additional information about the user.
            session.TelephoneNumber = userInfo.Telephone;
            session.IPAddress = HttpContext.Current.Request.UserHostAddress;
            session.UserAgent = HttpContext.Current.Request.UserAgent;
        }


        protected void SigninButton_Click(object sender, EventArgs e)
        {
            UserSession session = new UserSession();
            ADUserInfo userInfo = new ADUserInfo();
            List<SystemRole> userSystemRoles = new List<SystemRole>();

            //START
            bool status = false;
            string msg = string.Empty;

            status = athenticator.AuthenticateUser(email.Text, password.Text, out msg);
            AuthenticationMessage = msg;
            

            // TMP BLOCK TO IMPORT ALL USERS FROM AD
            //TmpUsers.InsertUsers();
            //END TMP BLOCK


            //email.Text = "aalhour@ccc.gr";
            //status = true;


            if (status == true)
            {
                userInfo = Users.GetUserInfo(email.Text);
                
                /** ----
                 * To impersonate user identity 
                 *  userInfo = Users.GetUserInfo("aalhour@ccc.gr");
                 /* -------
                 */
                
                // Users Information was found in active directory
                if (!userInfo.Equals(null))
                {
                    //Try to get user from the database
                    Users DatabaseUserRecord = Users.GetUser(userInfo.SipAccount.Replace("sip:", ""));
                    
                    //Update the user, if exists and if his/her info has changed... Insert te Users if s/he doesn't exist
                    if (DatabaseUserRecord != null)
                    {

                        //Make sure the user record was updated by ActiveDirectory and not by the System Admin
                        //If the system admin has updated this user then you cannot update his record from Active Directory
                        if (DatabaseUserRecord.UpdatedByAD == true)
                        {

                            //If user information from Active directory doesnt match the one in Users Table : update user table 
                            if (DatabaseUserRecord.EmployeeID.ToString() != userInfo.EmployeeID ||
                                DatabaseUserRecord.FullName != String.Format("{0} {1}", userInfo.FirstName, userInfo.LastName) ||
                                DatabaseUserRecord.SiteName != userInfo.physicalDeliveryOfficeName ||
                                DatabaseUserRecord.Department != userInfo.department ||
                                DatabaseUserRecord.TelephoneNumber != HelperFunctions.FormatUserTelephoneNumber(userInfo.Telephone))
                            {
                                Users user = new Users();
                                int employeeID = 0;

                                // Validate employeeID if it could be parsed as integer or not
                                bool result = Int32.TryParse(userInfo.EmployeeID, out employeeID);

                                if (result)
                                    user.EmployeeID = employeeID;
                                else
                                    user.EmployeeID = 0;

                                user.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                                user.FullName = userInfo.FirstName + " " + userInfo.LastName;
                                user.TelephoneNumber = HelperFunctions.FormatUserTelephoneNumber(userInfo.Telephone);
                                user.Department = userInfo.department;
                                user.SiteName = userInfo.physicalDeliveryOfficeName;
                                user.UpdatedByAD = true;

                                Users.UpdateUser(user);
                            }
                        }
                    }
                    else
                    {
                        // If user not found in Users tables that means this is his first login : insert his information into Users table
                        Users user = new Users();
                        
                        int employeeID = 0;

                        bool result = Int32.TryParse(userInfo.EmployeeID, out employeeID);

                        if (result)
                            user.EmployeeID = employeeID;
                        else
                            user.EmployeeID = 0;

                        user.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                        user.FullName = userInfo.FirstName + " " + userInfo.LastName;
                        user.TelephoneNumber = HelperFunctions.FormatUserTelephoneNumber(userInfo.Telephone);
                        user.Department = userInfo.department;
                        user.SiteName = userInfo.physicalDeliveryOfficeName;
                        user.UpdatedByAD = true;
                     
                        Users.InsertUser(user);
                    }

                    //Assign the current userInfo to the UserSession fields.
                    SetUserSessionFields(ref session, userInfo);

                    Session.Add("UserData", session);

                    if (this.redirect_to_url != null && this.redirect_to_url.Value != string.Empty)
                    {
                        if (Request.QueryString["redirect_to"].Contains(@"~/ui/") && Request.QueryString["redirect_to"].Contains(@".aspx"))
                        {
                            Response.Redirect(this.redirect_to_url.Value);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/ui/user/dashboard.aspx");
                    }
                }
            }

            if (AuthenticationMessage.ToString() != string.Empty)
            {
                AuthenticationMessage = "* " + AuthenticationMessage;
            }
        }//END OF FUNCTION
    }
}