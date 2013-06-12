using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using Lync_Billing.DB;

namespace Lync_Billing.Libs
{
    public class Dispatcher
    {
        public string CurrentURL { get; set; }
        public string RedirectToURL { get; private set; }

        private static bool PageAuthorization(UserSession session, string URL) 
        {
            bool status = false;

            if (session.IsDelegate)
            {
                if (URL.Contains(@"/manage_delegates") || URL.Contains(@"/user/"))
                    status = true;
            }

            if (session.IsAccountant)
            {
                if (URL.Contains(@"/accounting/") || URL.Contains(@"/user/"))
                    status = true;
            }

            if (session.IsAdmin)
            {
                if (URL.Contains(@"/admin/") || URL.Contains(@"/user/"))
                    status = true;
            }

            if (session.IsDeveloper)
                status = true;

            return status;
        }

        public static int Dispatch(UserSession session, string currentURL,string toURL) 
        {
            if (session != null)
            {
                //List<UserRole> roles = session.Roles;

                if (PageAuthorization(session, toURL) == true)
                    return 0;
                else
                    return 1;
            }
            else 
                return -1;
        }

    }
}