<%@ WebService Language="C#"  Class="Lync_Billing.Libs.BillingAPI" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Lync_Billing.DB;

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
        
        [WebMethod]
        public bool authenticateUser(string emailAddress, string password)
        {
            bool status = false;

            AdLib adConnector = new AdLib();
            status = adConnector.AuthenticateUser(emailAddress, password);

            return status;
        }

        [WebMethod]
        public ADUserInfo GetUserAttributes(string emailAddress) 
        {
            AdLib adConnector = new AdLib();
           
            return adConnector.getUserAttributes(emailAddress);
        }

        [WebMethod]
        public List<UserRole> GetUserRoles(string sipAccount)
        {
            return Employee.GetUserRoles(sipAccount);
        }

        [WebMethod]
        public bool ValidateUsersRoles(string SipAccount, int RoleID)
        {
            UserRole userRole = new UserRole();
            return userRole.ValidateUsersRoles(SipAccount, RoleID);
        }
    }
}

