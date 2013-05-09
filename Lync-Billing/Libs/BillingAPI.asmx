<%@ WebService Language="C#"  Class="Lync_Billing.Libs.BillingAPI" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Lync_Billing.DB;
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
        
        /// <summary>
        /// Authenticate user based on Email Address and Password
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="password">Domain Password</param>
        /// <returns></returns>
        [WebMethod]
        public object authenticateUser(string emailAddress, string password)
        {
            bool status = false;

            AdLib adConnector = new AdLib();
            status = adConnector.AuthenticateUser(emailAddress, password);

            return serializer.Serialize(status);
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

