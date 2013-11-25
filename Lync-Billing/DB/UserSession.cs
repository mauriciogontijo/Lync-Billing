using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

using Lync_Billing.DB.Roles;

namespace Lync_Billing.DB
{
    public class UserSession
    {
        private static List<UserSession> usersSessions = new List<UserSession>();
        private List<DelegateRole> userDelegees = new List<DelegateRole>();

        //Normal user data
        public Users NormalUserInfo { get; set; }
        public string TelephoneNumber { set; get; }
        public string IPAddress { set; get; }
        public string UserAgent { set; get; }

        //Roles Related
        public string ActiveRoleName { set; get; }
        public List<SystemRole> SystemRoles { set; get; }
        public DelegeeAccountInfo DelegeeAccount { get; set; }
        public List<DelegateRole> UserDelegateRoles { get; set; }
        public List<DelegateRole> SiteDelegateRoles { get; set; }
        public List<DelegateRole> DepartmentDelegateRoles { get; set; }

        //Phone Calls and Phone Book Related
        public string PhonecallsPerPage { set; get; }
        public List<PhoneCall> Phonecalls { set; get; }
        public List<PhoneCall> PhonecallsHistory { set; get; }
        public Dictionary<string, PhoneBook> Addressbook { set; get; }
               
        //Generic User SystemRoles
        public bool IsDeveloper { set; get; }

        //System Roles
        public bool IsSystemAdmin { set; get; }
        public bool IsSiteAdmin { set; get; }
        public bool IsSiteAccountant { set; get; }

        //Department Head Role
        public bool IsDepartmentHead { get; set; }
        
        //Delegates Roles
        public bool IsDelegee { set; get; }
        public bool IsUserDelegate { set; get; }
        public bool IsDepartmentDelegate { set; get; }
        public bool IsSiteDelegate { set; get; }


        public UserSession()
        {
            NormalUserInfo = new Users();
            TelephoneNumber = string.Empty;
            IPAddress = string.Empty;
            UserAgent = string.Empty;

            ActiveRoleName = string.Empty;
            SystemRoles = new List<SystemRole>();
            DelegeeAccount = null;

            //Initialized other containers
            PhonecallsPerPage = string.Empty;
            Phonecalls = new List<PhoneCall>();
            PhonecallsHistory = new List<PhoneCall>();
            Addressbook = new Dictionary<string, PhoneBook>();

            //By default the roles are set to false unless initialized as otherwise!
            IsDeveloper = false;
            IsSystemAdmin = false;
            IsSiteAdmin = false;
            IsSiteAccountant = false;
            IsDepartmentHead = false;

            IsDelegee = false;
            IsUserDelegate = false;
            IsDepartmentDelegate = false;
            IsSiteDelegate = false;

            //Initialize the lists
            UserDelegateRoles = new List<DelegateRole>();
            DepartmentDelegateRoles = new List<DelegateRole>();
            SiteDelegateRoles = new List<DelegateRole>();
        }

        
        private void InitializeSystemRoles(List<SystemRole> SystemRoles = null)
        {
            if (SystemRoles != null && SystemRoles.Count > 0) 
            {
                this.SystemRoles = SystemRoles;

                foreach (SystemRole role in SystemRoles)
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
                }
            }
        }


        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */
        private void InitializeDelegeesInformation(string userSipAccount = "")
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

            //Initialize the Delegees SystemRoles Flags
            this.IsUserDelegate = DelegateRole.IsUserDelegate(userSipAccount);
            this.IsSiteDelegate = DelegateRole.IsSiteDelegate(userSipAccount);
            this.IsDepartmentDelegate = DelegateRole.IsDepartmentDelegate(userSipAccount);

            this.IsDelegee = this.IsUserDelegate || this.IsDepartmentDelegate || this.IsSiteDelegate;

            //Initialize the Delegees Information Lists
            if (IsUserDelegate)
                this.UserDelegateRoles = DelegateRole.GetDelegees(userSipAccount, DelegateRole.UserDelegeeTypeID);

            if (IsDepartmentDelegate)
                this.DepartmentDelegateRoles = DelegateRole.GetDelegees(userSipAccount, DelegateRole.DepartmentDelegeeTypeID);

            if (IsSiteDelegate)
                this.SiteDelegateRoles = DelegateRole.GetDelegees(userSipAccount, DelegateRole.SiteDelegeeTypeID);

        }


        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */
        private void InitializeDepartmentHeadRoles(string userSipAccount = "")
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

            IsDepartmentHead = DepartmentHeadRole.IsDepartmentHead(userSipAccount);
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


        public void InitializeAllRolesInformation(string userSipAccount)
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

            SystemRoles = SystemRole.GetSystemRolesPerSipAccount(userSipAccount);
            InitializeSystemRoles(SystemRoles);
            InitializeDelegeesInformation(userSipAccount);
            InitializeDepartmentHeadRoles(userSipAccount);

            ActiveRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        }

    }
}