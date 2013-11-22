using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;
using Lync_Billing.DB.Roles;

namespace Lync_Billing.DB
{
    public class Site
    {
        private static DBLib DBRoutines = new DBLib();
        
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string CountryCode { get; set; }


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

        public static Site GetSite(int ID)
        {
            Site site = new Site();
            DataTable dt = new DataTable();

            List<string> columns = new List<string>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.Sites.SiteID), ID }
            };

            dt = DBRoutines.SELECT( Enums.GetDescription(Enums.Sites.TableName), columns, wherePart, 1);

            foreach (DataRow row in dt.Rows)
            {
                site = new Site();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID))
                        site.SiteID = Convert.ToInt32(Misc.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName))
                        site.SiteName = Convert.ToString(Misc.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode))
                        site.CountryCode = Convert.ToString(Misc.ReturnEmptyIfNull(row[column.ColumnName]));
                }
            }

            return site;
        }

        public static List<Site> GetAllSites() 
        {
            Site site;
            DataTable dt = new DataTable();
            List<Site> sites = new List<Site>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName));

            foreach (DataRow row in dt.Rows)
            {
                site = new Site();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        site.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName) && row[column.ColumnName] != System.DBNull.Value)
                        site.SiteName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        site.CountryCode = (string)row[column.ColumnName];
                }
                sites.Add(site);
            }

            return sites;
        }

        public static List<Site> GetAllSites(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            Site site;
            DataTable dt = new DataTable();
            List<Site> sites = new List<Site>();
          
            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Sites.TableName),
                columns,
                wherePart,
                limits);

            foreach (DataRow row in dt.Rows)
            {
                site = new Site();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        site.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName) && row[column.ColumnName] != System.DBNull.Value)
                        site.SiteName = (string)row[column.ColumnName];
                   
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        site.CountryCode = (string)row[column.ColumnName];
                }
                sites.Add(site);
            }

            return sites;

        }

        public static int InsertSite(Site site)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((site.SiteName).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Sites.SiteName), site.SiteName);
          
            if ((site.CountryCode).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Sites.CountryCode), site.CountryCode);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Sites.TableName), columnsValues, Enums.GetDescription(Enums.Sites.SiteID));

            return rowID;
        }

        public static bool UpdateSite(Site site)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((site.SiteName).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Sites.SiteName), site.SiteName);

            if (site.CountryCode != null)
                setPart.Add(Enums.GetDescription(Enums.Sites.CountryCode), site.CountryCode);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Sites.TableName),
                setPart,
                Enums.GetDescription(Enums.Sites.SiteID),
                site.SiteID);

            return status;
        }

        public static bool DeleteSite(Site site)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.Sites.TableName), 
                Enums.GetDescription(Enums.Sites.SiteID),site.SiteID );

            return status;
        }

        /// <summary>
        /// Given a list of user roles for a specific user, his SipAccount and his granted user-roles, return a list of the sites on which he was granted elevated access.
        /// </summary>
        /// <param name="userRoles">A list of the user's roles, taken from the session.</param>
        /// <param name="sipAccount">This is the user's SipAccount, taken from the session.</param>
        /// <param name="enumValidRole">This is parameter of the type DB.Enum.ValidRoles.</param>
        /// <returns>The list of sites on which the user was granted an elevated-access, such as: SiteAdmin, SiteAccountant. Developer is a universal access-role.</returns>
        public static List<Site> GetUserRoleSites(List<SystemRole> userRoles, string enumValidRole)
        {
            List<Site> sites = new List<Site>();
            List<int> tmpUserSites = new List<int>();

            SystemRole DeveloperRole = userRoles.Find(role => role.IsDeveloper() == true);
            if (DeveloperRole != null && DeveloperRole.IsDeveloper())
            {
                return DB.Site.GetAllSites();
            }

            else if (Enums.GetDescription(Enums.ValidRoles.IsSystemAdmin) == enumValidRole)
            {
                tmpUserSites = userRoles.Where(item => item.IsSystemAdmin()).Select(item => item.SiteID).ToList();

                foreach (int site in tmpUserSites)
                {
                    sites.Add(DB.Site.GetSite(site));
                }

                return sites;
            }

            else if (Enums.GetDescription(Enums.ValidRoles.IsSiteAdmin) == enumValidRole)
            {
                tmpUserSites = userRoles.Where(item => item.IsSiteAdmin()).Select(item => item.SiteID).ToList();

                foreach (int site in tmpUserSites)
                {
                    sites.Add(DB.Site.GetSite(site));
                }

                return sites;
            }

            else if (Enums.GetDescription(Enums.ValidRoles.IsSiteAccountant) == enumValidRole)
            {
                tmpUserSites = userRoles.Where(item => item.IsSiteAccountant()).Select(item => item.SiteID).ToList();

                foreach (int site in tmpUserSites)
                {
                    sites.Add(DB.Site.GetSite(site));
                }

                return sites;
            }

            //else return an empty list of sites
            return (new List<Site>());
        }

    }
}