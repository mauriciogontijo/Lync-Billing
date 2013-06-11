using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Lync_Billing.DB;

namespace Lync_Billing.Libs
{
    public class Dispatcher
    {

        public string URL { get; set; }
        public UserSession USession {get; set;}

        private static bool ValidateUserSession(string sipAccount,UserSession uSession) 
        {
            if (uSession != null && sipAccount == uSession.SipAccount)
                return true;
            else
                return false;
        }

        public string RedirectURL(string sipAccount, UserSession uSession, string url) 
        {
            if (ValidateUserSession(sipAccount, uSession) == true)
                return url;
            else
                return string.Empty;
        }

        
    }
}