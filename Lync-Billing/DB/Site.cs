using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Site
    {
        public DBLib DBRoutines = new DBLib();
        
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string CountryCode { get; set; }
        public string SiteLocation { get; set; }

        public Site getSite(int ID)
        {
            Site site = new Site() ;
            DataTable dt = new DataTable();
            List<Site> sites = new List<Site>();
            
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add(Enums.GetDescription(Enums.Sites.SiteID), ID);

            dt = DBRoutines.SELECT( Enums.GetDescription(Enums.Sites.TableName), null, wherePart, 0);

            return site;
        }

        public List<Site> GetSites(List<string> columns, Dictionary<string, object> wherePart, int limits)
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
                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteID))
                        site.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteName))
                        site.SiteName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.SiteLocation))
                        site.SiteLocation = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Sites.CountryCode))
                        site.CountryCode = (string)row[column.ColumnName];
                }
                sites.Add(site);
            }

            return sites;

        }

        public int InsertSite(Site site)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((site.SiteName).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Sites.SiteName), site.SiteName);

            if ((site.SiteLocation).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Sites.SiteLocation), site.SiteLocation);

            if ((site.CountryCode).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Sites.CountryCode), site.CountryCode);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Sites.TableName), columnsValues);

            return rowID;
        }

        public bool UpdateSite(Site site)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((site.SiteName).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Sites.SiteName), site.SiteName);

            if (site.SiteLocation != null)
                setPart.Add(Enums.GetDescription(Enums.Sites.SiteLocation), site.SiteLocation);

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

        public bool DeleteSite(Site site)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.Sites.TableName), 
                Enums.GetDescription(Enums.Sites.SiteID),site.SiteID );

            return status;
        }

    }
}