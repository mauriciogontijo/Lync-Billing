﻿using System;
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
        public Dictionary<string, string> InfoOfUserDelegates { get; set; }
        public Dictionary<string, string> InfoOfDepartmentDelegates { get; set; }
        public Dictionary<string, string> InfoOfSiteDelegates { get; set; }
        
        //SystemRoles Related
        public List<SystemRole> SystemRoles { set; get; }
        public string ActiveRoleName { set; get; }
        public Dictionary<string, string> ClientData { set; get; }
               
        //Generic User SystemRoles
        //public bool IsAdmin { set; get; }
        //public bool IsAccountant { set; get; }
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
            InfoOfUserDelegates = new Dictionary<string, string>();
            InfoOfDepartmentDelegates = new Dictionary<string, string>();
            InfoOfSiteDelegates = new Dictionary<string, string>();
        }

        
        private void InitializeSystemRoles(List<SystemRole> UserRoles = null)
        {
            this.SystemRoles = new List<SystemRole>();

            if (UserRoles != null && UserRoles.Count > 0) 
            {
                this.SystemRoles = UserRoles;

                foreach (SystemRole role in UserRoles)
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
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.PrimarySipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.PrimarySipAccount))
            {
                userSipAccount = this.PrimarySipAccount;
            }

            //Initialize the Delegees SystemRoles Flags
            this.IsUserDelegate = DelegateRole.IsUserDelegate(userSipAccount);
            this.IsSiteDelegate = DelegateRole.IsSiteDelegate(userSipAccount);
            this.IsDepartmentDelegate = DelegateRole.IsDepartmentDelegate(userSipAccount);

            this.IsDelegee = this.IsUserDelegate || this.IsDepartmentDelegate || this.IsSiteDelegate;

            //Initialize the Delegees Information Lists
            if (IsUserDelegate)
                this.InfoOfUserDelegates = DelegateRole.GetDelegeesNames(userSipAccount, DelegateRole.UserDelegeeTypeID);

            if (IsDepartmentDelegate)
                this.InfoOfDepartmentDelegates = DelegateRole.GetDelegeesNames(userSipAccount, DelegateRole.DepartmentDelegeeTypeID);

            if (IsSiteDelegate)
                this.InfoOfSiteDelegates = DelegateRole.GetDelegeesNames(userSipAccount, DelegateRole.SiteDelegeeTypeID);

        }


        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */
        private void InitializeDepartmentHeadRoles(string userSipAccount = "")
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


        public void InitializeAllRolesInformation(string userSipAccount, List<SystemRole> userRoles)
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

            InitializeSystemRoles(userRoles);
            InitializeDelegeesInformation(userSipAccount);
            InitializeDepartmentHeadRoles(userSipAccount);

            ActiveRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        }

    }
}