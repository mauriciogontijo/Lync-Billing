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

        private static bool PageAuthorization(List<UserRole> roles, string URL) 
        {
            bool status = false;
            
            foreach (UserRole role in roles)
            {
                if (role.RoleID.ToString() == Enums.GetDescription(Enums.TypeOfUser.USER))
                {
                    if (URL.Contains(@"/user/"))
                        status = true;
                }

                if (role.RoleID.ToString() == Enums.GetDescription(Enums.TypeOfUser.ACCOUNTANT))
                {
                    if (URL.Contains(@"/accounting/"))
                        status = true;
                }

                if (role.RoleID.ToString() == Enums.GetDescription(Enums.TypeOfUser.ADMIN))
                {
                    if (URL.Contains(@"/admin/"))
                        status = true;
                }

                if (role.RoleID.ToString() == Enums.GetDescription(Enums.TypeOfUser.DEVELOPER))
                    status = true;
            }
            return status;
        }

        public static int Dispatch(UserSession session, string currentURL,string toURL) 
        {
            if (session != null)
            {
                List<UserRole> roles = session.Roles;

                if (PageAuthorization(roles, toURL) == true)
                    return 0;
                else
                    return 1;
            }
            else 
            {
                session.CurrentURL = currentURL;
                session.ToURL = toURL;
                return -1;
            }

        }

    }
}