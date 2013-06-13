using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class UserSession
    { 
        //User Related Data
        public string EmailAddress { set; get; }
        public string DisplayName { set; get; }
        public string TelephoneNumber { set; get; }
        public string ActiveRoleName { set; get; }
        public string SiteName { set; get; }
        public string EmployeeID { set; get; }
        public string SipAccount { set; get; }
        public string IpAddress {set;get;}
        public string UserAgent { set; get; }

        public List<UserRole> Roles { set; get; }
        public Dictionary<string, string> ClientData { set; get; }
        public List<PhoneCall> phoneCalls { set; get; }

        //Generic User Roles
        public bool IsDeveloper { set; get; }
        public bool IsAdmin { set; get; }
        public bool IsAccountant { set; get; }

        //Specific User Roles
        public bool IsSuperAdmin { set; get; }
        public bool IsCountryAdmin { set; get; }
        public bool IsProjectAdmin { set; get; }
        public bool IsSuperAccountant { set; get; }
        public bool IsCountryAccountant { set; get; }
        public bool IsProjectAccountant { set; get; }
        public string CurrentURL { set; get; }
        public string ToURL { set; get; }

        //Delegate-capability check
        public bool IsDelegate { set; get; }

        private static List<UserSession> usersSessions = new List<UserSession>();


        public UserSession()
        {
            IsDeveloper = false;
            IsAdmin = false;
            IsAccountant = false;

            IsSuperAdmin = false;
            IsCountryAdmin = false;
            IsProjectAdmin = false;

            IsSuperAccountant = false;
            IsCountryAccountant = false;
            IsProjectAccountant = false;

            IsDelegate = false;
        }

        
        public void AddUserSession(UserSession userSession)
        {
            if (!usersSessions.Contains(userSession))
            {
                usersSessions.Add(userSession);
            }
        }

        
        public void RemoveUserSession(UserSession userSession)
        {
            if (!usersSessions.Contains(userSession))
            {
                usersSessions.Remove(userSession);
            }
        }


        public void InitializeRoles(List<UserRole> UserRoles = null)
        {
            if (UserRoles != null) {
                foreach (UserRole role in UserRoles) {
                    switch (role.RoleID) {
                        case 1:
                            IsDeveloper = true;
                            break;

                        case 2:
                            IsAccountant = true;
                            IsSuperAccountant = true;
                            break;

                        case 3:
                            IsAdmin = true;
                            IsSuperAdmin = true;
                            break;

                        case 4:
                            IsAdmin = true;
                            IsCountryAdmin = true;
                            break;

                        case 5:
                            IsAdmin = true;
                            IsProjectAdmin = true;
                            break;

                        case 6:
                            IsAccountant = true;
                            IsCountryAccountant = true;
                            break;

                        case 7:
                            IsAccountant = true;
                            IsProjectAccountant = true;
                            break;
                    }
                }
            } else {
                //do nothing
            }
        }

       
    }
    
}