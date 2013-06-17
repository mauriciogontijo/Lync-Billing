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
    public partial class login : System.Web.UI.Page
    {
        public AdLib authinticator = new AdLib();

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
        }

        protected void SigninButton_Click(object sender, EventArgs e)
        {
            bool status = false;
            ADUserInfo userInfo = new ADUserInfo();
            UserSession session = new UserSession();

            status = authinticator.AuthenticateUser(email.Text, password.Text);
            
            if (status == true)
            {
                userInfo = Users.GetUserInfo(email.Text);

                // User Information was found in active directory
                if (!userInfo.Equals(null))
                {
                    session.EmailAddress = userInfo.EmailAddress;
                    session.DisplayName = userInfo.DisplayName;
                    session.TelephoneNumber = userInfo.Telephone;
                    session.IpAddress = HttpContext.Current.Request.UserHostAddress;
                    session.UserAgent = HttpContext.Current.Request.UserAgent;

                    List<string> columns = new List<string>();
                    Dictionary<string, object> whereStatement = new Dictionary<string, object>();

                    whereStatement.Add("SipAccount", userInfo.SipAccount.Replace("sip:", ""));

                    List<Users> ListOfUsers = Users.GetUsers(columns, whereStatement, 1);
                    List<UserRole> userRoles;

                    //User Exists in Users Table
                    if (ListOfUsers.Count > 0)
                    {
                        userRoles = Users.GetUserRoles(userInfo.SipAccount.Replace("sip:", ""));

                        session.ActiveRoleName = "USER";

                        if (userRoles.Count > 0)
                        {
                            session.Roles = userRoles;
                            session.InitializeRoles(userRoles);
                        }
                        else
                        {
                            session.Roles = null;
                            //By default, the Roles map (IsDeveloper....etc) is initialized to false.
                        }

                        //If user information from Active directory doesnt match the one in Users Table : update user table 
                        if ((ListOfUsers[0]).SipAccount != userInfo.SipAccount.Replace("sip:", "") ||
                            (ListOfUsers[0]).UserID.ToString() != userInfo.EmployeeID ||
                            (ListOfUsers[0]).SiteName != userInfo.physicalDeliveryOfficeName)
                        {
                            Users user = new Users();
                            user.SiteName = userInfo.physicalDeliveryOfficeName;
                            user.UserID = Convert.ToInt32(userInfo.EmployeeID);
                            user.SipAccount = userInfo.SipAccount.Replace("sip:", "");

                            Users.UpdateUser(user);
                        }

                        session.SiteName = userInfo.physicalDeliveryOfficeName;
                        session.EmployeeID = userInfo.EmployeeID;
                        session.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                        session.IsDelegate = UsersDelegates.IsDelegate(userInfo.SipAccount.Replace("sip:", ""));
                    }
                    else
                    {
                        session.ActiveRoleName = "USER";
                        session.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                        session.SiteName = userInfo.physicalDeliveryOfficeName;
                        session.EmployeeID = userInfo.EmployeeID;
                        session.IsDelegate = UsersDelegates.IsDelegate(userInfo.SipAccount.Replace("sip:", ""));

                        userRoles = Users.GetUserRoles(userInfo.SipAccount.Replace("sip:", ""));


                        if (userRoles.Count > 0)
                        {
                            session.Roles = userRoles;
                            session.InitializeRoles(userRoles);
                        }
                        else
                        {
                            session.Roles = null;
                            //By default, the Roles map (IsDeveloper....etc) is initialized to false.
                        }

                        // If user not found in Users tables that means this is his first login : insert his information into Users table
                        Users user = new Users();
                        user.SiteName = userInfo.physicalDeliveryOfficeName;
                        user.UserID = Convert.ToInt32(userInfo.EmployeeID);
                        user.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                        Users.InsertUser(user);
                    }

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
        }//END OF FUNCTION
    }
}