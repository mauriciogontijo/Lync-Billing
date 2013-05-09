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
        /// <summary>
        /// Authenticate user based on Email Address and Password
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="password">Domain Password</param>
        /// <returns></returns>
        [WebMethod]
        public bool authenticateUser(string emailAddress, string password)
        {
            bool status = false;

            AdLib adConnector = new AdLib();
            status = adConnector.AuthenticateUser(emailAddress, password);

            return status;
        }
        
        /// <summary>
        /// JGet User Related information from Active Directory
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        [WebMethod]
        public object GetJsonUserAttributes(string mailAddress)
        {
           AdLib adConnector = new AdLib();
           ADUserInfo userInfo = adConnector.getUserAttributes(mailAddress);
           return serializer.Serialize(userInfo);
        }

        [WebMethod]
        public object GetUserRoles(string sipAccount)
        {
            return  serializer.Serialize(Users.GetUserRoles(sipAccount));
        }

        [WebMethod]
        public bool ValidateUsersRoles(string SipAccount, int RoleID)
        {
            UserRole userRole = new UserRole();
            return userRole.ValidateUsersRoles(SipAccount, RoleID);
        }

        [WebMethod]
        public object GetPhoneCalls(object jsonColumns, object jsonWhereStatement, int limits) 
        {
            List<string> columns = new List<string>();
            Dictionary<string, object> whereStatement = new Dictionary<string,object>();
            
            if(jsonColumns != null)
                columns = serializer.Deserialize<List<string>>(jsonColumns.ToString());
            
            if(jsonWhereStatement != null)
                whereStatement = serializer.Deserialize<Dictionary<string,object>>(jsonWhereStatement.ToString());

            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            PhoneCall phoneCall = new PhoneCall();
            phoneCalls = phoneCall.GetPhoneCalls(columns, whereStatement, limits);

            return serializer.Serialize(phoneCalls);
        }
    }
}

