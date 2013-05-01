using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Users : ADUserInfo 
    {
        public ADUserInfo GetUserInfo(string emailAddress)
        {
            AdLib adConnector = new AdLib();
            return adConnector.getUserAttributes(emailAddress);
        }

        public int getUserRole(string emailAddress) 
        {

        }
       
    }
}