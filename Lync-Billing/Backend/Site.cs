using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.Backend
{
    public class Site
    {
        private static DBLib DBRoutines = new DBLib();
        
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }

        //This is a logical representation of data, it doesn't belong to the table.
        public string CountryName { get; set; }


        public static string GetSiteName(int ID)
        {
            DataTable dt;
            string siteName = string.Empty;

            string columnName = string.Empty;
            List<string> columns;
            Dictionary<string, object> wherePart;

            columns = new List<string>()
            { 
                Enums.GetDescription(Enums.Sites.SiteName)
            };

            wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.Sites.SiteID), ID }
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName), columns, wherePart, 1);

            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.Sites.SiteName);
                if (dt.Columns.Contains(columnName))
                    siteName = Convert.ToString(row[columnName]);
            }

            return siteName;
        }

        public static Site GetSite(string siteName) 
        {
            DataTable dt;
          
            List<string> columns;
            Dictionary<string, object> wherePart;

            Site site = new Site();

            columns = new List<string>()
            { 
                Enums.GetDescription(Enums.Sites.SiteName)
            };

            wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.Sites.SiteName), siteName }
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName), columns, wherePart, 1);

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                   
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID))
                        site.SiteID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName))
                        site.SiteName = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode))
                        site.CountryCode = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.Description))
                        site.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }
            }
            return site;
        }

        public static Site GetSite(int? SiteID = null, string SiteName = null)
        {
            Site site = new Site();
            DataTable dt = new DataTable();
            List<string> columns = new List<string>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            if (SiteID != null && string.IsNullOrEmpty(SiteName))
            {
                wherePart.Add(Enums.GetDescription(Enums.Sites.SiteID), Convert.ToInt32(SiteID));
            }
            else if (SiteID == null && !string.IsNullOrEmpty(SiteName))
            {
                wherePart.Add(Enums.GetDescription(Enums.Sites.SiteName), Convert.ToString(SiteName));
            }
            else 
            {
                //both of the parameters are null
                return null;
            }

            dt = DBRoutines.SELECT( Enums.GetDescription(Enums.Sites.TableName), columns, wherePart, 1);

            foreach (DataRow row in dt.Rows)
            {
                site = new Site();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID))
                        site.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName))
                        site.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode))
                    {
                        site.CountryCode = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                        if (!string.IsNullOrEmpty(site.CountryCode))
                        {
                            if (site.CountryCode.Length == 2) site.CountryName = NumberingPlan.GetCountryName(TwoCharactersCountryCode: site.CountryCode.ToUpper());
                            if (site.CountryCode.Length == 3) site.CountryName = NumberingPlan.GetCountryName(ThreeCharactersCountryCode: site.CountryCode.ToUpper());
                        }
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.Description))
                        site.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }
            }

            return site;
        }

        
        public static List<Site> GetAllSites(List<string> columns = null, Dictionary<string, object> wherePart = null, int limits = 0)
        {
            Site site;
            DataTable dt = new DataTable();
            List<Site> sites = new List<Site>();
          
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                site = new Site();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID))
                        site.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName))
                        site.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode))
                    {
                        site.CountryCode = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                        if (!string.IsNullOrEmpty(site.CountryCode))
                        {
                            if (site.CountryCode.Length == 2) site.CountryName = NumberingPlan.GetCountryName(TwoCharactersCountryCode: site.CountryCode.ToUpper());
                            if (site.CountryCode.Length == 3) site.CountryName = NumberingPlan.GetCountryName(ThreeCharactersCountryCode: site.CountryCode.ToUpper());
                        }
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.Sites.Description))
                        site.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                sites.Add(site);
            }

            return sites;
        }


        /// <summary>
        /// Given a list of user roles for a specific user, his SipAccount and his granted user-roles, return a list of the AllSites on which he was granted elevated access.
        /// </summary>
        /// <param name="userRoles">A list of the user's roles, taken from the session.</param>
        /// <param name="sipAccount">This is the user's SipAccount, taken from the session.</param>
        /// <param name="enumValidRole">This is parameter of the type Backend.Enum.ValidRoles.</param>
        /// <returns>The list of AllSites on which the user was granted an elevated-access, such as: SiteAdmin, SiteAccountant. Developer is a universal access-role.</returns>
        public static List<Site> GetUserRoleSites(List<SystemRole> userRoles, string enumValidRole)
        {
            List<Site> sites = new List<Site>();
            List<int> tmpUserSites = new List<int>();

            SystemRole DeveloperRole = userRoles.Find(role => role.IsDeveloper() == true);
            if (DeveloperRole != null && DeveloperRole.IsDeveloper())
            {
                return Backend.Site.GetAllSites();
            }

            else if (Enums.GetDescription(Enums.ValidRoles.IsSystemAdmin) == enumValidRole)
            {
                tmpUserSites = userRoles.Where(item => item.IsSystemAdmin()).Select(item => item.SiteID).ToList();

                foreach (int site in tmpUserSites)
                {
                    sites.Add(Backend.Site.GetSite(site));
                }

                return sites;
            }

            else if (Enums.GetDescription(Enums.ValidRoles.IsSiteAdmin) == enumValidRole)
            {
                tmpUserSites = userRoles.Where(item => item.IsSiteAdmin()).Select(item => item.SiteID).ToList();

                foreach (int site in tmpUserSites)
                {
                    sites.Add(Backend.Site.GetSite(site));
                }

                return sites;
            }

            else if (Enums.GetDescription(Enums.ValidRoles.IsSiteAccountant) == enumValidRole)
            {
                tmpUserSites = userRoles.Where(item => item.IsSiteAccountant()).Select(item => item.SiteID).ToList();

                foreach (int site in tmpUserSites)
                {
                    sites.Add(Backend.Site.GetSite(site));
                }

                return sites;
            }

            //else return an empty list of AllSites
            return (new List<Site>());
        }


        public static int AddSite(Site site)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            if (!string.IsNullOrEmpty(site.SiteName))
            {
                columnsValues.Add(Enums.GetDescription(Enums.Sites.SiteName), site.SiteName);

                if (!string.IsNullOrEmpty(site.CountryCode))
                    columnsValues.Add(Enums.GetDescription(Enums.Sites.CountryCode), site.CountryCode);

                if (!string.IsNullOrEmpty(site.Description))
                    columnsValues.Add(Enums.GetDescription(Enums.Sites.Description), site.Description);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Sites.TableName), columnsValues, Enums.GetDescription(Enums.Sites.SiteID));
            }

            return rowID;
        }


        public static bool UpdateSite(Site site, bool FORCE_RESET_DESCRIPTION = false)
        {
            bool status = false;

            Dictionary<string, object> columnValues = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            wherePart.Add(Enums.GetDescription(Enums.Sites.SiteID), site.SiteID);


            //Set Part
            if (!string.IsNullOrEmpty(site.SiteName))
                columnValues.Add(Enums.GetDescription(Enums.Sites.SiteName), site.SiteName);

            if (!string.IsNullOrEmpty(site.CountryCode))
                columnValues.Add(Enums.GetDescription(Enums.Sites.CountryCode), site.CountryCode);

            if (!string.IsNullOrEmpty(site.Description))
                columnValues.Add(Enums.GetDescription(Enums.Sites.Description), site.Description);


            //RESET FLAG
            if (FORCE_RESET_DESCRIPTION == true)
                columnValues.Add(Enums.GetDescription(Enums.Sites.Description), null);


            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.Sites.TableName), columnValues, wherePart);

            return status;
        }


        public static void SyncSites() 
        {
            List<Users> listOfUsers = Users.GetUsers(null, null, 0);

            foreach (Users user in listOfUsers)
            {
                Site site = GetSite(user.SiteName);

                if (!string.IsNullOrEmpty(site.SiteName))
                    continue;
                else
                    AddSite(new Site { SiteName = user.SiteName , CountryCode="N/A" , Description = "N/A"} );
            }
        }

        public static bool DeleteSite(Site site)
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.Sites.TableName), Enums.GetDescription(Enums.Sites.SiteID), site.SiteID);

            return status;
        }

    }

}