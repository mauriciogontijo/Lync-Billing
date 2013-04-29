using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Lync_Billing.Libs;

namespace Lync_Billing.WebServices
{
    [System.Web.Script.Services.ScriptService]
    [WebService(Namespace = "http://xmlforasp.net")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    public class UsersWebServices : System.Web.Services.WebService
    {
        public UsersWebServices() { }

        [WebMethod]
        public bool authenticateUser(string emailAddress, string password) 
        {
            bool status = false;

            AdLib adConnector = new AdLib();
            status = adConnector.AuthenticateUser(emailAddress, password);

            return status;
        }

    }
}