﻿<%@ WebService Language="C#"  Class="Lync_Billing.Libs.BillingAPI" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Lync_Billing.Backend;
using System.Web.Script.Serialization;

namespace Lync_Billing.Libs
{
    /// <summary>
    /// Summary description for UserWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class BillingAPI : System.Web.Services.WebService
    {
        public BillingAPI() { }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        #region Users Related

        [WebMethod]
        public object GetUserSession() 
        {
            return ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;
        }  
        /// <summary>
        /// Authenticate user based on Email Address and Password
        /// If Success Get User Information
        /// If sucess Validate part of user information from Users Table
        /// Fill the session accross the way
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="password">Domain Password</param>
        /// <returns>User session if authenticated or null if not</returns>
        [WebMethod]
        public object authenticateUser(string emailAddress, string password,object env)
        {
            bool status = false;
            AdLib adConnector = new AdLib();
            ADUserInfo userInfo = new ADUserInfo();
            UserSession session = new UserSession();

            Dictionary<string, string> userEnv = serializer.Deserialize<Dictionary<string, string>>(env.ToString());
            
            status = adConnector.AuthenticateUser(emailAddress, password);

            if (status == true) 
            {
                userInfo = Users.GetUserInfo(emailAddress);
                
                // User Information was found in active directory
                if (!userInfo.Equals(null))
                {
                    session.EmailAddress = userInfo.EmailAddress;
                    session.DisplayName = userInfo.DisplayName;
                    session.TelephoneNumber = userInfo.Telephone;
                    session.ClientData = new Dictionary<string, string>();
                    
                    foreach (KeyValuePair<string, string> keyvalue in userEnv) 
                    {
                        if (keyvalue.Key == "IPAddress")
                            session.ClientData.Add("IPAddress", keyvalue.Value);

                        if (keyvalue.Key == "BrowserName")
                            session.ClientData.Add("BrowserName", keyvalue.Value);

                        if (keyvalue.Key == "BrowserVersion")
                            session.ClientData.Add("BrowserVersion", keyvalue.Value);

                        if (keyvalue.Key == "UserAgent")
                            session.ClientData.Add("UserAgent", keyvalue.Value);

                        if (keyvalue.Key == "OS")
                            session.ClientData.Add("OS", keyvalue.Value);
                    }
                    
                    List<string> columns = new List<string>();
                    Dictionary<string, object> whereStatement = new Dictionary<string, object>();
                    
                    whereStatement.Add("SipAccount", userInfo.SipAccount.Replace("sip:",""));
                   
                    List<Users> ListOfUsers =  Users.GetUsers(columns, whereStatement, 1);
                    List<UserRole> userRoles;

                    //User Exists in Users Table
                    if (ListOfUsers.Count > 0)
                    {
                        userRoles = Users.GetUserRoles(userInfo.SipAccount.Replace("sip:", ""));

                         session.ActiveRoleName = "USER";

                        if (userRoles.Count > 0)
                             session.Roles = userRoles;
                        else
                            session.Roles = null;
                        
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
                        // If user not found in Users tables that means this is his first login : insert his information into Users table
                        Users user = new Users();
                        user.SiteName = userInfo.physicalDeliveryOfficeName;
                        user.UserID = Convert.ToInt32(userInfo.EmployeeID);
                        user.SipAccount = userInfo.SipAccount.Replace("sip:", "");

                        Users.InsertUser(user);
                    }
                }
                
            }
            return serializer.Serialize(session);
        }
        
        /// <summary>
        /// JGet User Related information from Active Directory
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        [WebMethod]
        public object GetUserAttributes(string mailAddress)
        {
           ADUserInfo userInfo = Users.GetUserInfo(mailAddress);
           return serializer.Serialize(userInfo);
        }

        [WebMethod]
        public object GetUserByNumber(string sipAccount, string phoneNumber) 
        {
            Dictionary<string, PhoneBook> phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);
            if (phoneBookEntries.ContainsKey(phoneNumber))
                return serializer.Serialize(phoneBookEntries[phoneNumber].Name);
            else
                return serializer.Serialize("N-A");
        }

        /// <summary>
        /// Insert Users in Users Table only if Users are not there
        /// </summary>
        /// <param name="jsonUserInfo">Users Object in JSON Format</param>
        /// <returns></returns>
        [WebMethod]
        public object InsertUser(object jsonUserInfo)
        {
            Users userInfo = serializer.Deserialize<Users>(jsonUserInfo.ToString());
            return serializer.Serialize(Users.InsertUser(userInfo));
        }

        /// <summary>
        /// Get Users from Users Table 
        /// </summary>
        /// <param name="jsonColumns">Columns to be populated as a list of strings</param>
        /// <param name="jsonWhereStatement">SQL Condition </param>
        /// <param name="limits">Number of rows</param>
        /// <returns>serialized List of Users</returns>
        [WebMethod]
        public object GetUsers(object jsonColumns, object jsonWhereStatement, int limits)
        {
            List<string> columns = new List<string>();
            Dictionary<string, object> whereStatement = new Dictionary<string, object>();

            if (jsonColumns != null)
                columns = serializer.Deserialize<List<string>>(jsonColumns.ToString());

            if (jsonWhereStatement != null)
                whereStatement = serializer.Deserialize<Dictionary<string, object>>(jsonWhereStatement.ToString());

            return serializer.Serialize(Users.GetUsers(columns, whereStatement, limits));
        }

        /// <summary>
        /// Get User Roles
        /// </summary>
        /// <param name="sipAccount">Sip Account name</param>
        /// <returns>serialized List of UsersRoles </returns>
        [WebMethod]
        public object GetUserRoles(string sipAccount)
        {
            return  serializer.Serialize(Users.GetUserRoles(sipAccount));
        }

        /// <summary>
        /// Validate User Role
        /// </summary>
        /// <param name="SipAccount">Sip Account</param>
        /// <param name="RoleID">Role ID</param>
        /// <returns></returns>
        [WebMethod]
        public bool ValidateUsersRoles(string SipAccount, int RoleID)
        {
            return UserRole.ValidateUsersRoles(SipAccount, RoleID);
        }

        [WebMethod]
        public bool UpdateUser(object jsonUser) 
        {
            return Users.UpdateUser(serializer.Deserialize<Users>(jsonUser.ToString()));
        }
        
        #endregion

        /// <summary>
        /// Get User/s Phone calls
        /// </summary>
        /// <param name="jsonColumns">Columns to be populated from DB</param>
        /// <param name="jsonWhereStatement">Conditions </param>
        /// <param name="limits">Number of rows</param>
        /// <returns></returns>
        [WebMethod]
        public object GetPhoneCalls(object jsonColumns, object jsonWhereStatement, int limits) 
        {
            List<string> columns = new List<string>();
            Dictionary<string, object> whereStatement = new Dictionary<string,object>();
            
            if(jsonColumns != null)
                columns = serializer.Deserialize<List<string>>(jsonColumns.ToString());
            
            if(jsonWhereStatement != null)
                whereStatement = serializer.Deserialize<Dictionary<string,object>>(jsonWhereStatement.ToString());

            return serializer.Serialize(PhoneCall.GetPhoneCalls(columns, whereStatement, limits));
        }
    }
}

