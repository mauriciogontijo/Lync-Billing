using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lync_Billing.DB
{
    public class UserSession
    {
        private static List<UserSession> usersSessions = new List<UserSession>();
        private List<DB.Delegates> userDelegees = new List<DB.Delegates>();

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
        public string EffectiveDelegatedDepartmentName { get; set; }
        public string EffectiveDelegatedSiteName { get; set; }
        public Dictionary<string, string> ListOfUserDelegates { get; set; }
        public Dictionary<string, string> ListOfDepartmentDelegates { get; set; }
        public Dictionary<string, string> ListOfSiteDelegates { get; set; }
        
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
        public bool IsDelegee { set; get; }
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


        public UserSession()
        {
            //By default the roles are set to false unless initialized as otherwise!
            //IsAdmin = false;
            //IsAccountant = false;
            IsDeveloper = false;
            IsSystemAdmin = false;
            IsSiteAdmin = false;
            IsSiteAccountant = false;
            IsDepartmentHead = false;

            IsDelegee = false;
            IsUserDelegate = false;
            IsDepartmentDelegate = false;
            IsSiteDelegate = false;
            
            //Empty all of the sip accounts
            PrimarySipAccount = string.Empty;
            EffectiveSipAccount = string.Empty;
            ListOfUserDelegates = new Dictionary<string, string>();
            ListOfDepartmentDelegates = new Dictionary<string, string>();
            ListOfSiteDelegates = new Dictionary<string, string>();
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


        public void InitializeRolesFlags(List<UserRole> UserRoles = null)
        {
            this.Roles = new List<UserRole>();

            if (UserRoles != null && UserRoles.Count > 0) 
            {
                this.Roles = UserRoles;

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
        }


        /***
         * Please note that this function depends on the state of some instance variables, such as: 
         * 1. PrimarySipAccount
         * 2. The roles boolean flags
         * So, don't call this function before at least initializing these instance variables
         */
        public void InitializeDelegeesInformation(string userSipAccount="")
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.PrimarySipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.PrimarySipAccount))
            {
                userSipAccount = this.PrimarySipAccount;
            }

            //userDelegees = Delegates.GetDelegees(userSipAccount);

            //Initialize the Delegees Roles Flags
            this.IsUserDelegate = Delegates.IsUserDelegate(userSipAccount);
            this.IsSiteDelegate = Delegates.IsSiteDelegate(userSipAccount);
            this.IsDepartmentDelegate = Delegates.IsDepartmentDelegate(userSipAccount);

            this.IsDelegee = this.IsUserDelegate || this.IsDepartmentDelegate || this.IsSiteDelegate;


            //Initialize the Delegees Information Lists
            if (IsUserDelegate)
                this.ListOfUserDelegates = Delegates.GetDelegeesNames(userSipAccount, Delegates.UserDelegeeTypeID);

            if (IsDepartmentDelegate)
                this.ListOfDepartmentDelegates = Delegates.GetDelegeesNames(userSipAccount, Delegates.DepartmentDelegeeTypeID);

            if (IsSiteDelegate)
                this.ListOfSiteDelegates = Delegates.GetDelegeesNames(userSipAccount, Delegates.SiteDelegeeTypeID);

        }//END PF FUNCTION

    }
}