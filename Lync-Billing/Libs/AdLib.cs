using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

namespace Lync_Billing.Libs
{
    public class AdLib
    {
        //WEB.CONF AD RELATED FIELDS
        private static string LocalGCUri = System.Configuration.ConfigurationManager.AppSettings["LocalDomainURI"];
        private static string LocalGCUsername = System.Configuration.ConfigurationManager.AppSettings["LocalDomainUser"];
        private static string LocalGCPassword = System.Configuration.ConfigurationManager.AppSettings["LocalDomainPassword"];
        private static string ResourceGCUri = System.Configuration.ConfigurationManager.AppSettings["ResourceDomainURI"];
        private static string ResourceGCUsername = System.Configuration.ConfigurationManager.AppSettings["ResourceDomainUser"];
        private static string ResourceGCPassword = System.Configuration.ConfigurationManager.AppSettings["ResourceDomainPassword"];
        private static string ADSearchFilter = System.Configuration.ConfigurationManager.AppSettings["ADSearchFilter"];

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

            userInfo.PoolName = (string)resourceForestPoolResult.Properties["dnshostname"][0];

            return userInfo;
        }

        /// <summary>
        /// Authenticate user 
        /// </summary>
        /// <param name="EmailAddress">Email Address </param>
        /// <param name="password">Domain Controller Password</param>
        /// <returns></returns>
        public bool AuthenticateUser(string EmailAddress, string password)
        {
            try
            {
                ADUserInfo userInfo = getUserAttributes(EmailAddress);
                if (userInfo == null)
                    return false;

                DirectoryEntry directoryEntry = new DirectoryEntry(LocalGCUri, userInfo.SamAccountName, password);
                string localFilter = string.Format(ADSearchFilter, EmailAddress);

                DirectorySearcher localSearcher = new DirectorySearcher(directoryEntry);
                localSearcher.PropertiesToLoad.Add("mail");
                // localSearcher.Filter = localFilter;

                SearchResult result = localSearcher.FindOne();


                if (result != null)
                    return true;
                else
                    return false;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get a list of all Domain Controllers IP Address and DNS Suffixes
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetLocalDomainController()
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

        /// <summary>
        /// Get All User Related Attributes from Active Directory by Quering two forests 
        /// 1. for User Related Information
        /// 2. Sip Related Information
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public ADUserInfo getUserAttributes(string mailAddress)
        {
            ADUserInfo userInfo = new ADUserInfo();



            string localFilter = string.Format(ADSearchFilter, mailAddress);
            string resourceFilter = string.Format(ADSearchFilter, mailAddress);

            localSearcher.Filter = localFilter;
            resourceSearcher.Filter = resourceFilter;

            SearchResult localForestResult = localSearcher.FindOne();
            SearchResult resourceForestResult = resourceSearcher.FindOne();

            if (localForestResult != null && resourceForestResult != null)
            {
                userInfo.Title = (string)localForestResult.Properties["title"][0];
                userInfo.FirstName = (string)localForestResult.Properties["givenName"][0];
                userInfo.LastName = (string)localForestResult.Properties["sn"][0];
                userInfo.DisplayName = (string)localForestResult.Properties["cn"][0];

                userInfo.SamAccountName = (string)localForestResult.Properties["samaccountname"][0];
                userInfo.Upn = (string)localForestResult.Properties["userprincipalname"][0];
                userInfo.EmailAddress = (string)localForestResult.Properties["mail"][0];
                userInfo.EmployeeID = (string)localForestResult.Properties["employeeid"][0];
                userInfo.Department = (string)localForestResult.Properties["department"][0];
                userInfo.BusinessPhone = (string)localForestResult.Properties["telephonenumber"][0];
                userInfo.physicalDeliveryOfficeName = (string)localForestResult.Properties["physicalDeliveryOfficeName"][0];

                userInfo.SipAccount = (string)resourceForestResult.Properties["msrtcsip-primaryuseraddress"][0];
                userInfo.Telephone = (string)resourceForestResult.Properties["msrtcsip-line"][0];
                userInfo.PrimaryHomeServerDN = ((string)resourceForestResult.Properties["msrtcsip-primaryhomeserver"][0]).Replace("CN=Lc Services,CN=Microsoft,", "");
               
            }
            return userInfo;
        }

        /// <summary>
        /// Get AD Domain NetBios Name
        /// </summary>
        /// <param name="dnsDomainName">DNS Suffix Name</param>
        /// <returns></returns>
        public string GetNetbiosDomainName(string dnsDomainName)
        {
            string netbiosDomainName = string.Empty;

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

        /// <summary>
        /// Get DNS Name from AD Netbios Name
        /// </summary>
        /// <param name="netBiosName">AD Netbios Name</param>
        /// <returns></returns>
        public string GetFqdnFromNetBiosName(string netBiosName)
        {
            string FQDN = string.Empty;

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

        public string Telephone { get; set; }
        public string SipAccount { set; get; }
        public string PrimaryHomeServerDN { get; set; }
        public string PoolName { set; get; }
        public string physicalDeliveryOfficeName { set; get; }
    }

}
