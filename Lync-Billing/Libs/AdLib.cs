using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Configuration;

namespace Lync_Billing.Libs
{
    public class AdLib
    {
        //WEB.CONF AD RELATED FIELDS
        private static string LocalGCUri = ConfigurationManager.AppSettings["LocalDomainURI"];
        private static string LocalGCUsername = ConfigurationManager.AppSettings["LocalDomainUser"];
        private static string LocalGCPassword = ConfigurationManager.AppSettings["LocalDomainPassword"];
        private static string ResourceGCUri = ConfigurationManager.AppSettings["ResourceDomainURI"];
        private static string ResourceGCUsername = ConfigurationManager.AppSettings["ResourceDomainUser"];
        private static string ResourceGCPassword = ConfigurationManager.AppSettings["ResourceDomainPassword"];
        private static string ADSearchFilter = ConfigurationManager.AppSettings["ADSearchFilter"];
        

        //INIT LOCAL GC
        private static DirectoryEntry forestResource = new DirectoryEntry(ResourceGCUri, ResourceGCUsername, ResourceGCPassword);
        
        //INIT RESOURCE GC
        private static DirectoryEntry forestlocal = new DirectoryEntry(LocalGCUri, LocalGCUsername, LocalGCPassword);
        //INIT LOCAL SEARCHER
        private DirectorySearcher localSearcher = new DirectorySearcher(forestlocal);
        //INIT RESOURCE SEARCHER
        private DirectorySearcher resourceSearcher = new DirectorySearcher(forestResource);

        /// <summary>
        /// Get Lync Server Pool FQDN
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ADUserInfo setLyncPool(ADUserInfo userInfo)
        {
            string poolFilter = string.Format(@"(distinguishedName={0})", userInfo.PrimaryHomeServerDN);
            resourceSearcher.Filter = poolFilter;
            SearchResult resourceForestPoolResult = resourceSearcher.FindOne();

            if((string)resourceForestPoolResult.Properties["dnshostname"][0] != null)
                userInfo.PoolName = (string)resourceForestPoolResult.Properties["dnshostname"][0];

            return userInfo;
        }

        /// <summary>
        /// Authenticate user 
        /// </summary>
        /// <param name="EmailAddress">Email Address </param>
        /// <param name="password">Domain Controller Password</param>
        /// <returns></returns>
        public bool AuthenticateUser(string EmailAddress, string password,out string msg)
        {
            msg = string.Empty;

            if (password == null || password == string.Empty || EmailAddress == null || EmailAddress == string.Empty)
            {
                msg = "Email and/or password can't be empty!";
                return false;
            }
            
            try
            {
                ADUserInfo userInfo = GetUserAttributes(EmailAddress);
               
                if (userInfo == null)
                {
                    msg = "Error: Couldn't fetch user information!";
                    return false;
                }
                DirectoryEntry directoryEntry = new DirectoryEntry(LocalGCUri, userInfo.Upn, password);
                directoryEntry.AuthenticationType = AuthenticationTypes.None;
                string localFilter = string.Format(ADSearchFilter, EmailAddress);


                DirectorySearcher localSearcher = new DirectorySearcher(directoryEntry);
                localSearcher.PropertiesToLoad.Add("mail");
                localSearcher.Filter = localFilter;

                SearchResult result = localSearcher.FindOne();


                if (result != null)
                {
                    msg = "You have logged in successfully!";
                    return true;
                }
                else
                {
                    msg = "Login failed, please try again.";
                    return false;
                }
            }catch (Exception ex)
            {
                //System.ArgumentException argEx = new System.ArgumentException("Logon failure: unknown user name or bad password");
                //throw argEx;
                msg = "Wrong Email and/or Password!";
                return false;
            }
        }

        /// <summary>
        /// Get a list of all Domain Controllers IP Address and DNS Suffixes
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetLocalDomainController()
        {
            try
            {
                Dictionary<string, string> domainControllerInfo = new Dictionary<string, string>();
                Forest obj = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();
                DomainCollection collection = obj.Domains;
                foreach (Domain domain in collection)
                {
                    DomainControllerCollection domainControllers = domain.FindAllDiscoverableDomainControllers();
                    foreach (DomainController domainController in domainControllers)
                    {
                        if (!domainControllerInfo.ContainsKey(domain.Name))
                            domainControllerInfo.Add(domain.Name, domainController.IPAddress);
                    }
                }
                return domainControllerInfo;
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        /// Get All Users Related Attributes from Active Directory by Quering two forests 
        /// 1. for Users Related Information
        /// 2. Sip Related Information
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public ADUserInfo GetUserAttributes(string mailAddress)
        {
            ADUserInfo userInfo = new ADUserInfo();



            string localFilter = string.Format(ADSearchFilter, mailAddress);
            string resourceFilter = string.Format(ADSearchFilter, mailAddress);

            localSearcher.Filter = localFilter;
            resourceSearcher.Filter = resourceFilter;

            try
            {
                SearchResult localForestResult = localSearcher.FindOne();
                SearchResult resourceForestResult = resourceSearcher.FindOne();

                if (localForestResult != null && resourceForestResult != null)
                {
                    if (localForestResult.Properties.Contains("title"))
                        userInfo.Title = (string)localForestResult.Properties["title"][0];

                    if (localForestResult.Properties.Contains("givenName"))
                        userInfo.FirstName = (string)localForestResult.Properties["givenName"][0];

                    if (localForestResult.Properties.Contains("sn"))
                        userInfo.LastName = (string)localForestResult.Properties["sn"][0];

                    if (localForestResult.Properties.Contains("cn"))
                        userInfo.DisplayName = (string)localForestResult.Properties["cn"][0];

                    if (localForestResult.Properties.Contains("samaccountname"))
                        userInfo.SamAccountName = (string)localForestResult.Properties["samaccountname"][0];

                    if (localForestResult.Properties.Contains("department"))
                        userInfo.department = (string)localForestResult.Properties["department"][0];

                    if (localForestResult.Properties.Contains("userprincipalname"))
                        userInfo.Upn = (string)localForestResult.Properties["userprincipalname"][0];

                    if (localForestResult.Properties.Contains("mail"))
                        userInfo.EmailAddress = (string)localForestResult.Properties["mail"][0];

                    if (localForestResult.Properties.Contains("employeeid"))
                        userInfo.EmployeeID = (string)localForestResult.Properties["employeeid"][0];

                    if (localForestResult.Properties.Contains("department"))
                        userInfo.Department = (string)localForestResult.Properties["department"][0];

                    if (localForestResult.Properties.Contains("telephonenumber"))
                        userInfo.BusinessPhone = (string)localForestResult.Properties["telephonenumber"][0];

                    if (localForestResult.Properties.Contains("physicalDeliveryOfficeName"))
                        userInfo.physicalDeliveryOfficeName = (string)localForestResult.Properties["physicalDeliveryOfficeName"][0];

                    if (resourceForestResult.Properties.Contains("msrtcsip-primaryuseraddress"))
                        userInfo.SipAccount = (string)resourceForestResult.Properties["msrtcsip-primaryuseraddress"][0];

                    if (resourceForestResult.Properties.Contains("msrtcsip-line"))
                        userInfo.Telephone = (string)resourceForestResult.Properties["msrtcsip-line"][0];

                    if (resourceForestResult.Properties.Contains("msrtcsip-primaryhomeserver"))
                        userInfo.PrimaryHomeServerDN = ((string)resourceForestResult.Properties["msrtcsip-primaryhomeserver"][0]).Replace("CN=Lc Services,CN=Microsoft,", "");

                    return userInfo;
                }
                else { return null; }
                
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        public List<ADUserInfo> GetSipAccounts(string fullName) 
        {
            string localFilter = string.Format("(&(objectClass=user)(objectCategory=person)(cn={0}))",fullName);

            localSearcher.Filter = localFilter;
            
            List<ADUserInfo> listOfUsers = new List<ADUserInfo>();
            ADUserInfo userInfo;

            try
            {
                SearchResultCollection localForestResult = localSearcher.FindAll();

                foreach (SearchResult result in localForestResult) 
                {
                    userInfo = new ADUserInfo();

                    if( result.Properties.Contains("mail"))
                        userInfo.EmailAddress = (string)result.Properties["mail"][0];

                    if (result.Properties.Contains("cn"))
                        userInfo.DisplayName = (string)result.Properties["cn"][0];

                    if (result.Properties.Contains("employeeid"))
                        userInfo.EmployeeID = (string)result.Properties["employeeid"][0];

                    listOfUsers.Add(userInfo);
                }

                return listOfUsers;
            }
            catch (Exception ex) 
            {
                return null; 
            }
        }

        /// <summary>
        /// Get Users Attributes From Phone Number
        /// </summary>
        /// <param name="phoneNumber">Business Phone Number</param>
        /// <returns>ADUserInfo Object</returns>
        public ADUserInfo getUsersAttributesFromPhone(string phoneNumber)
        {
            ADUserInfo userInfo = new ADUserInfo();

            string searchFilter = "(&(objectClass=user)(objectCategory=person)(msrtcsip-line=Tel:{0}))";
            string resourceFilter = string.Format(searchFilter, phoneNumber);

            resourceSearcher.Filter = resourceFilter;

            try
            {
                SearchResult resourceForestResult = resourceSearcher.FindOne();

                if (resourceForestResult != null)
                {
                    userInfo.Title = (string)resourceForestResult.Properties["title"][0];
                    userInfo.FirstName = (string)resourceForestResult.Properties["givenName"][0];
                    userInfo.LastName = (string)resourceForestResult.Properties["sn"][0];
                    userInfo.DisplayName = (string)resourceForestResult.Properties["cn"][0];
                    userInfo.Telephone = (string)resourceForestResult.Properties["msrtcsip-line"][0];
                }
                return userInfo;
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        /// Get AD Domain NetBios Name
        /// </summary>
        /// <param name="dnsDomainName">DNS Suffix Name</param>
        /// <returns>Domain NetBios Name</returns>
        public string GetNetbiosDomainName(string dnsDomainName)
        {
            string netbiosDomainName = string.Empty;

            try
            {
                DirectoryEntry rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/RootDSE", dnsDomainName));

                string configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

                DirectoryEntry searchRoot = new DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext);

                DirectorySearcher searcher = new DirectorySearcher(searchRoot);
                searcher.SearchScope = SearchScope.OneLevel;
                // searcher.PropertiesToLoad.Add("netbiosname");
                searcher.Filter = string.Format("(&(objectcategory=Crossref)(dnsRoot={0})(netBIOSName=*))", dnsDomainName);

                SearchResult result = searcher.FindOne();

                if (result != null)
                {
                    netbiosDomainName = result.Properties["netbiosname"][0].ToString();
                }

                return netbiosDomainName;
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        /// Get DNS Name from AD Netbios Name
        /// </summary>
        /// <param name="netBiosName">AD Netbios Name</param>
        /// <returns>Domain FQDN</returns>
        public string GetFqdnFromNetBiosName(string netBiosName)
        {
            string FQDN = string.Empty;

            try
            {
                DirectoryEntry rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/RootDSE", netBiosName));

                string configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

                DirectoryEntry searchRoot = new DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext);

                DirectorySearcher searcher = new DirectorySearcher(searchRoot);
                searcher.SearchScope = SearchScope.OneLevel;
                //searcher.PropertiesToLoad.Add("dnsroot");
                searcher.Filter = string.Format("(&(objectcategory=Crossref)(netbiosname={0}))", netBiosName);

                SearchResult result = searcher.FindOne();
                if (result != null)
                    FQDN = result.Properties["dnsroot"][0].ToString();

                return FQDN;
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        public List<string> GetAllUsers() 
        {
            List<string> users = new List<string>();

            resourceSearcher.SizeLimit = 10000;
            resourceSearcher.PageSize = 250;

            string localFilter = string.Format(@"(&(objectClass=user)(objectCategory=person)(!(objectClass=contact))(msRTCSIP-PrimaryUserAddress=*))");

            resourceSearcher.Filter = localFilter;
            
            SearchResultCollection resourceForestResult;
           
            try
            {
                resourceForestResult = resourceSearcher.FindAll();

                if (resourceForestResult != null) 
                {

                    foreach (SearchResult result in resourceForestResult) 
                    {
                        if (result.Properties.Contains("mail"))
                            users.Add((string)result.Properties["mail"][0]);
                    }
                   
                }

            }
            catch (Exception ex) 
            {

            }

            return users;
        }
    }

    public class ADUserInfo
    {
        public string Title { set; get; }
        public string DisplayName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string SamAccountName { set; get; }
        public string Upn { set; get; }
        public string EmailAddress { set; get; }
        public string EmployeeID { set; get; }
        public string Department { set; get; }
        public string BusinessPhone { get; set; }
        public string department { set; get; }
        public string Telephone { get; set; }
        public string SipAccount { set; get; }
        public string PrimaryHomeServerDN { get; set; }
        public string PoolName { set; get; }
        public string physicalDeliveryOfficeName { set; get; }
    }

}
