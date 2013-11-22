using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Department
    {
        private static DBLib DBRoutines = new DBLib();

        public int SiteID { get; set; }
        public int DepartmentID { get; set; }
        public string SiteName { get; set; }
        public string DepartmentName { get; set; }

        public static string GetDepartmentName(int departmentID)
        {
            DataTable dt;
            List<string> columns;
            Dictionary<string, object> wherePart;

            string departmentName = string.Empty;
            string columnName = string.Empty;

            columns = new List<string>() { Enums.GetDescription(Enums.Departments.DepartmentName) };
            wherePart = new Dictionary<string, object>
            { 
                { Enums.GetDescription(Enums.Departments.ID), departmentID } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Departments.TableName), columns, wherePart, 1);

            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.Departments.DepartmentName);

                if (dt.Columns.Contains(columnName))
                    departmentName = Convert.ToString(row[columnName]);
            }

            return departmentName;
        }

        public static Department GetDepartment(int departmentID)
        {
            DataTable dt;
            Department department = new Department();
            List<string> columns;
            Dictionary<string, object> wherePart;

            columns = new List<string>();
            wherePart = new Dictionary<string, object>
            { 
                { Enums.GetDescription(Enums.Departments.ID), departmentID } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Departments.TableName), columns, wherePart, 1);

            foreach(DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Departments.ID))
                        department.DepartmentID = Convert.ToInt32(row[column.ColumnName]);

                    else if(column.ColumnName == Enums.GetDescription(Enums.Departments.DepartmentName))
                        department.DepartmentName = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.SiteID))
                    {
                        department.SiteID = Convert.ToInt32(row[column.ColumnName]);
                        department.SiteName = Site.GetSiteName(department.SiteID);
                    }
                }
            }

            return department;
        }


        public static List<Department> GetDepartmentsInSite(int siteID)
        {
            DataTable dt;
            List<string> columns;
            Dictionary<string, object> wherePart;

            Department department;
            List<Department> departmentsList = new List<Department>();

            columns = new List<string>();
            wherePart = new Dictionary<string, object>
            { 
                { Enums.GetDescription(Enums.Departments.SiteID), siteID } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Departments.TableName), columns, wherePart, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new Department();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Departments.ID))
                        department.DepartmentID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.DepartmentName))
                        department.DepartmentName = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.SiteID))
                    {
                        department.SiteID = Convert.ToInt32(row[column.ColumnName]);
                        department.SiteName = Site.GetSiteName(department.SiteID);
                    }
                }

                departmentsList.Add(department);
            }

            return departmentsList;
        }

        public static List<Department> GetAllDepartments()
        {
            DataTable dt;
            List<string> columns;
            Dictionary<string, object> wherePart;

            Department department;
            List<Department> departmentsList = new List<Department>();

            columns = new List<string>();
            wherePart = new Dictionary<string, object>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Departments.TableName), columns, wherePart, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new Department();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Departments.ID))
                        department.DepartmentID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.DepartmentName))
                        department.DepartmentName = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.SiteID))
                    {
                        department.SiteID = Convert.ToInt32(row[column.ColumnName]);
                        department.SiteName = Site.GetSiteName(department.SiteID);
                    }
                }

                departmentsList.Add(department);
            }

            return departmentsList;
        }

    }
}