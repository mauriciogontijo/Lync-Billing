using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Lync_Billing
{
    public class Global : System.Web.HttpApplication
    {
        private static List<string> sessionInfo;
        private static readonly object padlock = new object();

        public static List<string> Sessions
        {
            get
            {
                lock (padlock)
                {
                    if (sessionInfo == null)
                        sessionInfo = new List<string>();
                }
                return sessionInfo;
           }
        }

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Sessions.Add(Session.SessionID);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Sessions.Remove(Session.SessionID);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}