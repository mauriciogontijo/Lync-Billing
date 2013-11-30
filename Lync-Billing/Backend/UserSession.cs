﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

using Lync_Billing.Backend.Roles;

namespace Lync_Billing.Backend
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
        public List<DelegateRole> UserDelegateRoles { get; set; }
        public List<DelegateRole> SiteDelegateRoles { get; set; }
        public List<DelegateRole> DepartmentDelegateRoles { get; set; }

        public DelegeeAccountInfo DelegeeAccount { get; set; }
        

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

        //Get the user sipaccount.
        public string GetEffectiveSipAccount()
        {
            List<string> DelegeesRoleNames = new List<string>()
            {
                Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };

            //if the user is a user-delegee return the delegate sipaccount.
            if (DelegeesRoleNames.Contains(this.ActiveRoleName) && this.DelegeeAccount != null)
            {
                return (this.DelegeeAccount.DelegeeUserAccount.SipAccount);
            }
            //else then the user is a normal one, just return the normal user sipaccount.
            else
            {
                return (this.NormalUserInfo.SipAccount);
            }
        }

        //Get the user displayname.
        public string GetEffectiveDisplayName()
        {
            List<string> DelegeesRoleNames = new List<string>()
            {
                Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };

            //if the user is a user-delegee return the delegate sipaccount.
            if (DelegeesRoleNames.Contains(this.ActiveRoleName) && this.DelegeeAccount != null)
            {
                return (this.DelegeeAccount.DelegeeUserAccount.DisplayName);
            }
            //else then the user is a normal one, just return the normal user sipaccount.
            else
            {
                return (this.NormalUserInfo.DisplayName);
            }
        }

        //Get the user session phonecalls
        //Handle normal user mode and user delegee mode
        public List<PhoneCall> GetUserSessionPhoneCalls(bool force = false)
        {
            string sipAccount = this.GetEffectiveSipAccount();
            
            List<string> DelegeesRoleNames = new List<string>()
            {
                Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };

            //Delegee Mode
            if (DelegeesRoleNames.Contains(this.ActiveRoleName))
            {
                //Initialize the addressbook if it was not initialized already
                if (this.DelegeeAccount.DelegeeUserAddressbook == null || this.DelegeeAccount.DelegeeUserAddressbook.Count == 0)
                {
                    this.DelegeeAccount.DelegeeUserAddressbook = PhoneBook.GetAddressBook(sipAccount);
                }

                //Initialize and/or return phonecalls.
                if (this.DelegeeAccount.DelegeeUserPhonecalls == null || this.DelegeeAccount.DelegeeUserPhonecalls.Count == 0 || force == true)
                {
                    //var userPhoneCalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var userPhoneCalls = PhoneCall.GetPhoneCallsFast(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var addressbook = this.DelegeeAccount.DelegeeUserAddressbook;

                    //Skip the adding addressbook contact names to the phonecalls if there are no entries in the addressbook
                    if (addressbook.Count > 0)
                    {
                        foreach (var phoneCall in userPhoneCalls)
                        {
                            if (addressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                            {
                                phoneCall.PhoneBookName = ((PhoneBook)addressbook[phoneCall.DestinationNumberUri]).Name;
                            }
                        }
                    }

                    this.DelegeeAccount.DelegeeUserPhonecalls = userPhoneCalls.ToList();
                }

                return this.DelegeeAccount.DelegeeUserPhonecalls;
            }
            //Normal User Mode
            else
            {
                //Initialize the addressbook if it was not initialized already
                if (this.Addressbook == null || this.Addressbook.Count == 0)
                {
                    this.Addressbook = PhoneBook.GetAddressBook(sipAccount);
                }

                //Initialize and/or return phonecalls.
                if (this.Phonecalls == null || this.Phonecalls.Count == 0 || force == true)
                {
                    //var userPhoneCalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var userPhoneCalls = PhoneCall.GetPhoneCallsFast(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var addressbook = this.Addressbook;

                    //Skip the adding addressbook contact names to the phonecalls if there are no entries in the addressbook
                    if (addressbook.Count > 0)
                    {
                        foreach (var phoneCall in userPhoneCalls)
                        {
                            if (phoneCall.DestinationNumberUri != null && addressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                            {
                                phoneCall.PhoneBookName = ((PhoneBook)addressbook[phoneCall.DestinationNumberUri]).Name;
                            }
                        }
                    }

                    this.Phonecalls = userPhoneCalls.ToList();
                }

                return this.Phonecalls;
            }
        }

        //Get the user session phonecalls data, such as: PhoneCalls list, AddressBook and PhoneCallsPerPage JSON String
        //Handle normal user mode and user delegee mode
        public void FetchSessionPhonecallsAndAddressbookData(out List<PhoneCall> userSessionPhoneCalls, out Dictionary<string, PhoneBook> userSessionAddressBook, out string userSessionPhoneCallsPerPageJson)
        {
            //Initialize the passed varaibles
            userSessionPhoneCalls = new List<PhoneCall>();
            userSessionAddressBook = new Dictionary<string, PhoneBook>();
            userSessionPhoneCallsPerPageJson = string.Empty;

            List<string> DelegeesRoleNames = new List<string>()
            {
                Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };
            
            //This part is off-loaded to another procedure due to size of code
            userSessionPhoneCalls = GetUserSessionPhoneCalls();

            if (DelegeesRoleNames.Contains(this.ActiveRoleName))
            {
                userSessionAddressBook = this.DelegeeAccount.DelegeeUserAddressbook;
                userSessionPhoneCallsPerPageJson = this.DelegeeAccount.DelegeeUserPhonecallsPerPage;
            }
            else
            {
                userSessionAddressBook = this.Addressbook;
                userSessionPhoneCallsPerPageJson = this.PhonecallsPerPage;
            }

        }

        //Pass any of the three variables to this function and it will assign it's data to the respective UserSession container
        //The functions respectively stand for the the list of user phonecalls, his/her addressbook contacts, and the phonecalls grid json string
        //This handles the normal user mode and the user delegee mode.
        public void AssignSessionPhonecallsAndAddressbookData(List<PhoneCall> userSessionPhoneCalls, Dictionary<string, PhoneBook> userSessionAddressBook, string userSessionPhoneCallsPerPageJson)
        {
            List<string> DelegeesRoleNames = new List<string>()
            {
                Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };

            if (DelegeesRoleNames.Contains(this.ActiveRoleName))
            {
                if (userSessionPhoneCalls != null && userSessionPhoneCalls.Count > 0)
                    this.DelegeeAccount.DelegeeUserPhonecalls = userSessionPhoneCalls;

                if (userSessionAddressBook != null && userSessionAddressBook.Count > 0)
                    this.DelegeeAccount.DelegeeUserAddressbook = userSessionAddressBook;

                if (!string.IsNullOrEmpty(userSessionPhoneCallsPerPageJson))
                    this.DelegeeAccount.DelegeeUserPhonecallsPerPage = userSessionPhoneCallsPerPageJson;
            }
            else
            {
                if (userSessionPhoneCalls != null && userSessionPhoneCalls.Count > 0)
                    this.Phonecalls = userSessionPhoneCalls;

                if (userSessionAddressBook != null && userSessionAddressBook.Count > 0)
                    this.Addressbook = userSessionAddressBook;

                if (!string.IsNullOrEmpty(userSessionPhoneCallsPerPageJson))
                    this.PhonecallsPerPage = userSessionPhoneCallsPerPageJson;
            }
        }


    }
}