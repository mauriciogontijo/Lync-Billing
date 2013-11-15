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
        public Dictionary<string, string> ListOfUserDelegees { get; set; }
        
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
        public bool IsDepartmentHead { get; set; }
        
        //Delegate-capability check
        public bool IsUserDelegate { set; get; }
        public bool IsDepartmentDelegate { set; get; }
        public bool IsSiteDelegate { set; get; }

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
            IsUserDelegate = false;
            IsDepartmentDelegate = false;
            IsSiteDelegate = false;
            IsDepartmentHead = false;

            //Empty all of the sip accounts
            PrimarySipAccount = string.Empty;
            EffectiveSipAccount = string.Empty;
            ListOfUserDelegees = new Dictionary<string, string>();
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
            if (UserRoles != null && UserRoles.Count > 0) 
            {
                foreach (UserRole role in UserRoles)
                {
                    if (role.IsDeveloper())
                    {
                        IsDeveloper = true;
                    }

                    else if (role.IsSystemAdmin())
                    {
                        IsSystemAdmin = true;
                    }

                    else if (role.IsSiteAdmin())
                    {
                        IsSiteAdmin = true;
                    }

                    else if (role.IsSiteAccountant())
                    {
                        IsSiteAccountant = true;
                    }

                    else if (role.IsDepartmentHead())
                    {
                        IsDepartmentHead = true;
                    }
                }
            }
            else
            {
                //do nothing
                //keeps default values as is, which were set by the constructor to FALSE
            }
        }//END PF FUNCTION

    }
}