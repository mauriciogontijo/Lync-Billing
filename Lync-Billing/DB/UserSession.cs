using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lync_Billing.DB
{
    public class UserSession
    { 
        //User Related Data
        public string EmailAddress { set; get; }
        public string TelephoneNumber { set; get; }
        public string SiteName { set; get; }
        public string Department { set; get; }
        public string EmployeeID { set; get; }
        public string IpAddress {set;get;}
        public string UserAgent { set; get; }

        //Sip Addresses and Delegees Addresses Related
        public string PrimaryDisplayName { set; get; }
        public string EffectiveDisplayName { set; get; }
        public string PrimarySipAccount { set; get; }
        public string EffectiveSipAccount { set; get; }
        public Dictionary<string, string> ListOfDelegees { get; set; }
        
        //Roles Related
        public List<UserRole> Roles { set; get; }
        public string ActiveRoleName { set; get; }
        public Dictionary<string, string> ClientData { set; get; }
               
        //Generic User Roles
        //public bool IsAdmin { set; get; }
        //public bool IsAccountant { set; get; }
        public bool IsDeveloper { set; get; }

        //Specific User Roles
        public bool IsSystemAdmin { set; get; }
        public bool IsSiteAdmin { set; get; }
        public bool IsSiteAccountant { set; get; }
        
        //Delegate-capability check
        public bool IsDelegate { set; get; }

        //Redirection Related
        public string CurrentURL { set; get; }
        public string ToURL { set; get; }

        //Phone Calls and Phone Book Related
        public string PhoneCallsPerPage { set; get; }

        public List<PhoneCall> PhoneCalls { set; get; }

        public List<PhoneCall> PhoneCallsHistory { set; get; }
        public Dictionary<string, PhoneBook> PhoneBook { set; get; }

        private static List<UserSession> usersSessions = new List<UserSession>();


        public UserSession()
        {
            //By default the roles are set to false unless initialized as otherwise!
            //IsAdmin = false;
            //IsAccountant = false;
            IsDeveloper = false;
            IsSystemAdmin = false;
            IsSiteAdmin = false;
            IsSiteAccountant = false;
            IsDelegate = false;

            //Empty all of the sip accounts
            PrimarySipAccount = string.Empty;
            EffectiveSipAccount = string.Empty;
            ListOfDelegees = new Dictionary<string, string>();
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
            if (UserRoles != null && UserRoles.Count > 0) {
                foreach (UserRole role in UserRoles) {
                    if (role.IsDeveloper())
                    {
                        IsDeveloper = true;
                        break;
                    }

                    if (role.IsSystemAdmin())
                    {
                        IsSystemAdmin = true;
                        break;
                    }

                    if (role.IsSiteAdmin())
                    {
                        IsSiteAdmin = true;
                        break;
                    }

                    if (role.IsSiteAccountant())
                    {
                        IsSiteAccountant = true;
                        break;
                    }
                }
            } else {
                //do nothing
                //keeps default values as is, which were set by the constructor to FALSE
            }
        }//END PF FUNCTION

    }
}