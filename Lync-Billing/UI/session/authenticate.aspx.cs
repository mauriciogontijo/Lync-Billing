using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using Lync_Billing.Libs;
using Lync_Billing.DB;
using Lync_Billing.DB.Roles;

namespace Lync_Billing.ui.session
{
    public partial class authenticate : System.Web.UI.Page
    {
        public AdLib athenticator = new AdLib();
        public string AuthenticationMessage { get; set; }
        public string HeaderAuthBoxMessage { get; set; }
        public string ParagraphAuthBoxMessage { get; set; }
        public string sipAccount = string.Empty;

        private string accessParam = string.Empty;
        private string identityParam = string.Empty;
        private string dropParam = string.Empty;
        private bool redirectionFlag = true;
        private List<string> AccessLevels = new List<string>();

        //SystemRoles lookup variables
        private string systemAdminRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);
        private string siteAdminRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAdmin);
        private string siteAccountantRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAccountant);
        private string departmentHeadRoleName = Enums.GetDescription(Enums.ActiveRoleNames.DepartmentHead);
        
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);
        private string departmentDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee);
        private string siteDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee);

        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);


        protected void Page_Load(object sender, EventArgs e)
        {
            HeaderAuthBoxMessage = string.Empty;
            ParagraphAuthBoxMessage = string.Empty;
            AuthenticationMessage = string.Empty;

            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                Response.Redirect(getHomepageLink("login"));
            }
            //but if the user is actually logged in we only need to check if he was granted elevated-access(s)
            else
            {
                //Get a local copy of the user's session.
                UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                //Initialize the list of current user-permissions (user-access-levels!)
                InitAccessLevels();

                //Initialize the redirection flag to true. This is responsible for redirecting the user.
                //In the default state, the user must be redirected unless the request was valid and the redirection_flag was set to false.
                redirectionFlag = true;

                /*
                 * The User must pass the following autentiaction criteria
                 * PrimarySipAccount = EffectiveSipAccount, which means he is not viewing another's user account (X Person) and this X person seems to have some elevated access permissions.
                 * The asked permission actually exists - in the query string and in the system!
                 * The asked permission was actually granted for the current user.
                 **/

                //Mode 1: The Normal User Mode
                if (!string.IsNullOrEmpty(session.NormalUserInfo.SipAccount) && (session.DelegeeAccount == null || session.DelegeeAccount.DelegeeUserAccount == null))
                {
                    //Case 1: The user asks for Admin or Accounting access
                    //Should pass "access" and "access" should be coherent within our own system
                    //Shouldn't pass the other variables, such as: identity (see the case of "identity" below.
                    //The following condition covers the case in which the user is asking an elevated access-permission
                    if (!string.IsNullOrEmpty(Request.QueryString["access"]) && AccessLevels.Contains(Request.QueryString["access"].ToLower()) && string.IsNullOrEmpty(Request.QueryString["identity"]))
                    {
                        accessParam = Request.QueryString["access"].ToLower();
                        HeaderAuthBoxMessage = "You have requested an elevated access";
                        ParagraphAuthBoxMessage = "Please note that you must authenticate your information before proceeding any further.";

                        //if the user was authenticated already
                        if (session.ActiveRoleName != "user" && (session.IsSiteAdmin || session.IsSiteAccountant || session.IsDeveloper || session.IsDepartmentHead))
                        {
                            Response.Redirect(getHomepageLink(session.ActiveRoleName));
                        }

                        //if the user has the elevated-access-permission s/he is asking for, we fill the access text value in a hidden field in this page's form
                        else if (
                            (accessParam == siteAdminRoleName && session.IsSiteAdmin) ||
                            (accessParam == siteAccountantRoleName && session.IsSiteAccountant) ||
                            (accessParam == systemAdminRoleName && session.IsSystemAdmin) ||
                            (accessParam == departmentHeadRoleName && session.IsDepartmentHead) ||
                            session.IsDeveloper)
                        {
                            //set the value of hidden field in this page to the value of passed access variable.
                            this.ACCESS_LEVEL_FIELD.Value = accessParam;

                            //The user WOULD HAvE BEEN redirected if s/he weren't granted the elevated-access-permission s/he is asking for. But in this case, they passed the redirection.
                            redirectionFlag = false;
                        }
                    }

                    //Case 2: The user asks for Delegee access
                    if (!string.IsNullOrEmpty(Request.QueryString["access"]) && !string.IsNullOrEmpty(Request.QueryString["identity"]))
                    {
                        accessParam = Request.QueryString["access"].ToLower();
                        identityParam = Request.QueryString["identity"];
                        HeaderAuthBoxMessage = "You have requested to manage a delegee account";
                        ParagraphAuthBoxMessage = "Please note that you must authenticate your information before proceeding any further.";

                        bool accessParameterExists = (accessParam == userDelegeeRoleName || accessParam == departmentDelegeeRoleName || accessParam == siteDelegeeRoleName);

                        if (accessParameterExists)
                        {
                            bool userDelegeeCaseMatch = (session.IsUserDelegate && accessParam == userDelegeeRoleName && session.UserDelegateRoles.Find(user => user.SipAccount == identityParam) != null);
                            bool departmentDelegeeCaseMatch = (session.IsDepartmentDelegate && accessParam == departmentDelegeeRoleName && session.DepartmentDelegateRoles.Find(department => department.DelegeeDepartment.DepartmentName == identityParam) != null);
                            bool siteDelegeeCaseMatch = (session.IsSiteDelegate && accessParam == siteDelegeeRoleName && session.SiteDelegateRoles.Find(site => site.DelegeeSite.SiteName == identityParam) != null);

                            //if the user has the elevated-access-permission s/he is asking for, we fill the access text value in a hidden field in this page's form
                            if (userDelegeeCaseMatch || departmentDelegeeCaseMatch || siteDelegeeCaseMatch || session.IsDeveloper)
                            {
                                //set the value of hidden field in this page to the value of passed access variable.
                                this.ACCESS_LEVEL_FIELD.Value = accessParam;
                                this.DELEGEE_IDENTITY.Value = identityParam;
                                //SwitchToDelegeeAndRedirect(identityParam);

                                //The user WOULD HAVE BEEN redirected if s/he weren't granted the elevated-access-permission s/he is asking for. But in this case, they passed the redirection.
                                redirectionFlag = false;
                            }
                        }
                    }

                    //The following condition covers the case in which the user is asking to drop the already granted elevated-access-permission
                    else if (!string.IsNullOrEmpty(Request.QueryString["drop"]))
                    {
                        dropParam = Request.QueryString["drop"].ToLower();

                        //Case 1: Drop Admin or Accounting Access
                        if (AccessLevels.Contains(dropParam))
                        {
                            if ((session.IsSiteAdmin && dropParam == siteAdminRoleName && session.ActiveRoleName == siteAdminRoleName) ||
                                (session.IsSiteAccountant && dropParam == siteAccountantRoleName && session.ActiveRoleName == siteAccountantRoleName) ||
                                (session.IsSystemAdmin && dropParam == systemAdminRoleName && session.ActiveRoleName == systemAdminRoleName) ||
                                (session.IsDepartmentHead && dropParam == departmentHeadRoleName && session.ActiveRoleName == departmentHeadRoleName) ||
                                session.IsDeveloper)
                            {
                                DropAccess(dropParam);

                                //Notice the redirection which means that the user passed all the previous criteria and therefore, they shouldn't be redirected.
                                //However if they didn't pass this last condition, the redirection flag will still hold the value FALSE and the last if condition will redirect
                                //the user to the User Dashboard page.
                                redirectionFlag = false;
                            }
                        }
                        else
                        {
                            //The user was already authenticated, redirect him/het to the respective elevated access dashboard
                            if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
                            {
                                Response.Redirect(getHomepageLink(session.ActiveRoleName));
                            }
                            else redirectionFlag = true;
                        }
                    }
                }

                //Mode 2: The UserDelegee Mode
                else if (session.DelegeeAccount != null && session.DelegeeAccount.DelegeeTypeID == DelegateRole.UserDelegeeTypeID && session.DelegeeAccount.DelegeeUserAccount != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["drop"]))
                    {
                        dropParam = Request.QueryString["drop"].ToLower();

                        if (dropParam == userDelegeeRoleName && session.ActiveRoleName == userDelegeeRoleName)
                        {
                            DropAccess(dropParam);

                            //Notice the redirection which means that the user passed all the previous criteria and therefore, they shouldn't be redirected.
                            //However if they didn't pass this last condition, the redirection flag will still hold the value FALSE and the last if condition will redirect
                            //the user to the User Dashboard page.
                            redirectionFlag = false;
                        }
                    }
                }

                //if the user was not granted any elevated-access permission or he is currently in a manage-delegee mode, redirect him/her to the User Dashboard page.
                //Or if the redirection_flag was not set to FALSE so far, we redurect the user to the USER DASHBOARD
                if (redirectionFlag == true)
                {
                    Response.Redirect(getHomepageLink(normalUserRoleName));
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).NormalUserInfo.SipAccount;
        }//END OF PAGE_LOAD



        //This function is responsible for authenticating the user's information.
        protected void AuthenticateUser(object sender, EventArgs e)
        {
            bool status = false;
            ADUserInfo userInfo = new ADUserInfo();
            UserSession session = new UserSession();

            string msg = string.Empty;
            string user_email = string.Empty;

            //Get the requests from the view.
            string requestedAccessLevel = this.ACCESS_LEVEL_FIELD.Value ?? string.Empty;
            string requestedDelegeeIdentity = this.DELEGEE_IDENTITY.Value ?? string.Empty;

            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];
                user_email = session.NormalUserInfo.SipAccount.ToLower();

                if (this.ACCESS_LEVEL_FIELD != null)
                {
                    status = athenticator.AuthenticateUser(user_email, this.password.Text, out msg);
                    AuthenticationMessage = msg;

                    /** 
                     * -------
                     * To spoof identity for intermediate authentication
                     * status = true;
                     /* --------
                     **/

                    if (status == true)
                    {
                        //System Admin
                        if (requestedAccessLevel == systemAdminRoleName)
                        {
                            session.ActiveRoleName = systemAdminRoleName;
                            Response.Redirect(getHomepageLink(systemAdminRoleName));
                        }

                        //Site Admin
                        else if (requestedAccessLevel == siteAdminRoleName)
                        {
                            session.ActiveRoleName = siteAdminRoleName;
                            Response.Redirect(getHomepageLink(siteAdminRoleName));
                        }

                        //Site Accountant
                        else if (requestedAccessLevel == siteAccountantRoleName)
                        {
                            session.ActiveRoleName = siteAccountantRoleName;
                            Response.Redirect(getHomepageLink(siteAdminRoleName));
                        }

                        //Department Head
                        else if(requestedAccessLevel == departmentHeadRoleName)
                        {
                            session.ActiveRoleName = departmentHeadRoleName;
                            Response.Redirect(getHomepageLink(departmentHeadRoleName));
                        }

                        //Site Delegee
                        else if (requestedAccessLevel == siteDelegeeRoleName)
                        {
                            DelegateRole role = session.SiteDelegateRoles.Find(someRole => someRole.DelegeeSite != null && someRole.DelegeeSite.SiteName == requestedDelegeeIdentity);

                            if (role != null)
                            {
                                Site site = role.DelegeeSite;
                                SwitchToDelegeeAndRedirect(role.SipAccount, site, DelegateRole.SiteDelegeeTypeID);
                            }
                        }

                        //Department Delegee
                        else if (requestedAccessLevel == departmentDelegeeRoleName)
                        {
                            DelegateRole role = session.DepartmentDelegateRoles.Find(someRole => someRole.DelegeeDepartment != null && someRole.DelegeeDepartment.DepartmentName == requestedDelegeeIdentity);

                            if(role != null)
                            {
                                Department department = role.DelegeeDepartment;
                                SwitchToDelegeeAndRedirect(role.SipAccount, department, DelegateRole.DepartmentDelegeeTypeID);
                            }
                        }

                        //User Delegee
                        else if (requestedAccessLevel == userDelegeeRoleName && this.DELEGEE_IDENTITY != null)
                        {
                            DelegateRole role = session.UserDelegateRoles.Find(someRole => someRole.DelegeeUser != null && someRole.SipAccount == requestedDelegeeIdentity);

                            if(role != null)
                            {
                                Users user = role.DelegeeUser;
                                SwitchToDelegeeAndRedirect(role.SipAccount, user, DelegateRole.UserDelegeeTypeID);
                            }
                        }

                        //the value of the access_level hidden field has changed - fraud value!
                        session.ActiveRoleName = normalUserRoleName;
                        Response.Redirect(getHomepageLink(normalUserRoleName));
                    }
                }
                else
                {
                    //the value of the access_level hidden field has changed - fraud value!
                    session.ActiveRoleName = normalUserRoleName;
                    Response.Redirect(getHomepageLink(normalUserRoleName));
                }

                //Setup the authentication message.
                AuthenticationMessage = (!string.IsNullOrEmpty(AuthenticationMessage)) ? ("* " + AuthenticationMessage) : "";
            }
            else
            {
                Response.Redirect(getHomepageLink("login"));
            }
        }


        //This function is responsilbe for initializing the value of the AccessLevels List instance variable
        private void InitAccessLevels()
        {
            //role_id=20; system-admin
            AccessLevels.Add(systemAdminRoleName);

            //role_id=30; site-admin
            AccessLevels.Add(siteAdminRoleName);

            //role_id=40; site-accountant
            AccessLevels.Add(siteAccountantRoleName);

            //role_id=50; department-head
            AccessLevels.Add(departmentHeadRoleName);

            //delegee_type=1; user-delegates
            AccessLevels.Add(userDelegeeRoleName);
            
            //delegee_type=2; department-delegates
            AccessLevels.Add(departmentDelegeeRoleName);
            
            //delegee_type=3; site-delegates
            AccessLevels.Add(siteDelegeeRoleName);
        }


        //This function handles the switching to delegees
        //@param delegeeAddress could be a user sipAccount, a department name or a site name
        private void SwitchToDelegeeAndRedirect(string sipAccount, object delegee, int delegeeType)
        {
            //Initialize a temp copy of the User Session
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            if (delegee is Users && delegeeType == DelegateRole.UserDelegeeTypeID)
            {
                //Switch identity
                session.DelegeeAccount = new DelegeeAccountInfo();
                session.DelegeeAccount.DelegeeTypeID = DelegateRole.UserDelegeeTypeID;

                //Get the delegate user account
                session.DelegeeAccount.DelegeeUserAccount = (Users)delegee;
                session.DelegeeAccount.DelegeeUserAccount.DisplayName = HelperFunctions.FormatUserDisplayName(session.DelegeeAccount.DelegeeUserAccount.FullName, session.DelegeeAccount.DelegeeUserAccount.SipAccount, returnAddressPartIfExists: true);

                //Get the Delegee Phonecalls
                session.DelegeeAccount.DelegeeUserPhonecallsPerPage = string.Empty;
                session.DelegeeAccount.DelegeeUserPhonecalls = new List<PhoneCall>();
                //session.DelegeeAccount.DelegeeUserPhonecalls = PhoneCall.GetPhoneCalls(session.DelegeeAccount.DelegeeUserAccount.SipAccount);
                
                //Get the Delegee Addressbook
                session.DelegeeAccount.DelegeeUserAddressbook = new Dictionary<string,PhoneBook>();
                //session.DelegeeAccount.DelegeeUserAddressbook = PhoneBook.GetAddressBook(session.DelegeeAccount.DelegeeUserAccount.SipAccount);

                //Set the ActiveRoleName to "userdelegee"
                session.ActiveRoleName = userDelegeeRoleName;

                //Redirect to Uer Dashboard
                Response.Redirect(getHomepageLink(userDelegeeRoleName));
            }

            else if(delegee is Department && delegeeType == DelegateRole.DepartmentDelegeeTypeID)
            {
                //Get delegated department
                session.DelegeeAccount = new DelegeeAccountInfo();
                session.DelegeeAccount.DelegeeDepartmentAccount = (Department)delegee;
                session.DelegeeAccount.DelegeeTypeID = DelegateRole.DepartmentDelegeeTypeID;

                session.DelegeeAccount.DelegeeUserAccount = Users.GetUser(sipAccount);
                session.DelegeeAccount.DelegeeUserAccount.DisplayName = HelperFunctions.FormatUserDisplayName(session.DelegeeAccount.DelegeeUserAccount.FullName, session.DelegeeAccount.DelegeeUserAccount.SipAccount, returnAddressPartIfExists: true);

                //Switch ActiveRoleName to Department Delegee
                session.ActiveRoleName = departmentDelegeeRoleName;

                Response.Redirect(getHomepageLink(departmentDelegeeRoleName));
            }

            else if (delegee is Site && delegeeType == DelegateRole.SiteDelegeeTypeID)
            {
                //Get delegated site
                session.DelegeeAccount = new DelegeeAccountInfo();
                session.DelegeeAccount.DelegeeSiteAccount = (Site)delegee;
                session.DelegeeAccount.DelegeeTypeID = DelegateRole.SiteDelegeeTypeID;

                session.DelegeeAccount.DelegeeUserAccount = Users.GetUser(sipAccount);
                session.DelegeeAccount.DelegeeUserAccount.DisplayName = HelperFunctions.FormatUserDisplayName(session.DelegeeAccount.DelegeeUserAccount.FullName, session.DelegeeAccount.DelegeeUserAccount.SipAccount, returnAddressPartIfExists: true);

                //Switch ActiveRoleName to Site Delegee
                session.ActiveRoleName = siteDelegeeRoleName;

                Response.Redirect(getHomepageLink(siteDelegeeRoleName));
            }
        }


        //This function is responsible for dropping the already-granted elevated-access-permission
        private void DropAccess(string accessParameter)
        {
            //Initialize a temp copy of the User Session
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //Nullify the DelegeeAccount object.
            session.DelegeeAccount = null;

            //Always set the ActiveRoleName to "user"
            session.ActiveRoleName = normalUserRoleName;

            //Redirect to the User Dashboard
            Response.Redirect(getHomepageLink(normalUserRoleName));
        }


        //This function returns the homepage link of a specific role, if given, otherwise it returns the login link.
        private string getHomepageLink(string roleName = "")
        {
            if (roleName == systemAdminRoleName) return "~/ui/sysadmin/main/dashboard.aspx";

            else if (roleName == siteAdminRoleName) return "~/ui/admin/main/dashboard.aspx";
            else if (roleName == siteAccountantRoleName) return "~/ui/accounting/main/dashboard.aspx";
            else if (roleName == departmentHeadRoleName) return "~/ui/dephead/main/dashboard.aspx";

            else if (roleName == departmentDelegeeRoleName) return "~/ui/delegee/department/phonecalls.aspx";
            else if (roleName == siteDelegeeRoleName) return "~/ui/delegee/site/phonecalls.aspx";
            else if (roleName == userDelegeeRoleName) return "~/ui/user/dashboard.aspx";

            else if (roleName == normalUserRoleName) return "~/ui/user/dashboard.aspx";
            
            //default case
            else return "~/ui/session/login.aspx";

        }

    }
}