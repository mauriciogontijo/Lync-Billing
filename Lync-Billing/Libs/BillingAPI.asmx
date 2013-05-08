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
        public object GetJsonUserAttributes(string mailAddress)
        {
            ADUserInfo userInfo = GetUserAttributes(mailAddress);
           return JsonTranslator.Serialize<ADUserInfo>(userInfo);
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

        [WebMethod]
        public List<PhoneCall> GetPhoneCalls(object jsonColumns, object jsonWhereStatement, int limits) 
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<string> columns = new List<string>();
            Dictionary<string, object> whereStatement = new Dictionary<string,object>();
            
            if(jsonColumns != null)
                columns = JsonDeserializer<List<string>>(jsonColumns.ToString());
            
            if(jsonWhereStatement != null)
                whereStatement = serializer.Deserialize<Dictionary<string,object>>(jsonWhereStatement.ToString());

            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            PhoneCall phoneCall = new PhoneCall();

            return phoneCall.GetPhoneCalls(columns, whereStatement, limits);
        }

        [WebMethod]
        public string JsonSerializer<T>(T t) 
        {
            return JsonTranslator.Serialize<T>(t);
        }

        [WebMethod]
        public T JsonDeserializer<T>(string JsonString) 
        {
            return (T)JsonTranslator.Deserialize<T>(JsonString);
        } 
    }
}

