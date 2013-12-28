using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.Backend
{
    public class SitesDepartments
    {
        private static DBLib DBRoutines = new DBLib();

        public int ID { get; set; }
        public int SiteID { get; set; }
        public int DepartmentID { get; set; }

        public static List<Department> AllDepartments = Department.GetAllDepartments();
        public static List<Site> AllSites = Backend.Site.GetAllSites();

        public Site Site
        {
            get { return Site; }
            set { Site = AllSites.FirstOrDefault(item => item.SiteID == SiteID); }
        }

        public Department Department
        {
            get { return Department; }
            set { Department = AllDepartments.FirstOrDefault(item => item.DepartmentID == DepartmentID); }
        }

        public string SiteName
        {
            get { return SiteName; }
            set { SiteName = (Site != null ? Site.SiteName : null); }
        }
        
        public string DepartmentName
        {
            get { return DepartmentName; }
            set { DepartmentName = (Department != null ? Department.DepartmentName : null); }
        }



        public static Department GetDepartment(int departmentID)
        {
            return AllDepartments.FirstOrDefault(item => item.DepartmentID == departmentID);
        }

        public static Site GetSite(int SiteID)
        {
            return AllSites.FirstOrDefault(item => item.SiteID == SiteID);
        }

        public static List<SitesDepartments> GetDepartmentsInSite(int siteID)
        {
            DataTable dt;
            List<string> columns;
            Dictionary<string, object> wherePart;

            SitesDepartments department;

            List<SitesDepartments> departmentsList = new List<SitesDepartments>();

            columns = new List<string>();
            wherePart = new Dictionary<string, object>
            { 
                { Enums.GetDescription(Enums.SitesDepartmnets.SiteID), siteID } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SitesDepartmnets.TableName), columns, wherePart, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new SitesDepartments();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.ID))
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.SiteID))
                    {
                        department.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.DepartmentID))
                    {
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                       
                    }
                }

                departmentsList.Add(department);
            }

            return departmentsList;
        }
        
        public static List<SitesDepartments> GetAllSitesDepartments() 
        {
            List<SitesDepartments> sitesDepartmnets = new List<SitesDepartments>();
            SitesDepartments department;

            DataTable dt;
            List<string> columns;
        
            columns = new List<string>();
        
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SitesDepartmnets.TableName), columns, null, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new SitesDepartments();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.ID))
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.SiteID))
                    {
                        department.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.DepartmentID))
                    {
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    }
                }

                sitesDepartmnets.Add(department);
            }

            return sitesDepartmnets;
        }


        public static SitesDepartments GetSiteDepartment(int siteDepartmentID)
        {
            SitesDepartments department;

            DataTable dt;
            List<string> columns;

            columns = new List<string>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SitesDepartmnets.TableName), Enums.GetDescription(Enums.SitesDepartmnets.ID), siteDepartmentID);

            if(dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                department = new SitesDepartments();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.ID))
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.SiteID))
                    {
                        department.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.SitesDepartmnets.DepartmentID))
                    {
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                    }
                }

                return department;
            }

            return null;
        }

    }
}