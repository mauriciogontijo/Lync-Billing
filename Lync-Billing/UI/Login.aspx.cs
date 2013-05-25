using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Lync_Billing.Libs;
using Lync_Billing.DB;

namespace Lync_Billing.UI
{
    public partial class Login : System.Web.UI.Page
    {
        public AdLib authinticator = new AdLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Contents["UserData"] != null)
            {
                Response.Redirect("~/UI/User_Dashboard.aspx");
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
                    }
                    else
                    {
                        session.ActiveRoleName = "USER";
                        session.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                        session.SiteName = userInfo.physicalDeliveryOfficeName;
                        session.EmployeeID = userInfo.EmployeeID;

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
                    Response.Redirect("~/UI/User_Dashboard.aspx");
                }

            }
        }//END OF FUNCTION
    }
}