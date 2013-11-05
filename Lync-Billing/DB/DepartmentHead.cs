using Lync_Billing.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class DepartmentHead
    {
        private static DBLib DBRoutines = new DBLib();

        // Logical representation only!
        // The following instance variable doesn't represent an actual column in the original table
        // It was added to make it more easy to lookup the site names without having to go to the database each time and query it.
        public string SiteName { get; set; }

        //The following instance variables represent actual columns in the original table.
        private int ID { get; set; }
        public string SipAccount { get; set; }
        public string Department { get; set; }
        public int SiteID { get; set; }


        public static List<DepartmentHead> GetDepartmentHeads(string departmentName, int siteID)
        {
            DataTable dt = new DataTable();
            DepartmentHead head;
            List<DepartmentHead> ListOfDepartmentHeads = new List<DepartmentHead>();

            if (string.IsNullOrEmpty(departmentName))
                return ListOfDepartmentHeads;

            Dictionary<string, object> whereClause = new Dictionary<string, object>
            {
                { "Department",  departmentName.ToString() },
                { "SiteID", siteID }
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeads.TableName), (new List<string>()), whereClause, 0);

            foreach (DataRow row in dt.Rows)
            {
                head = new DepartmentHead();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.ID))
                        head.ID = (int)(row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.SipAccount))
                        head.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.Department))
                        head.Department = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.SiteID))
                        head.SiteID = Convert.ToInt32(row[column.ColumnName]);
                }

                ListOfDepartmentHeads.Add(head);
            }

            return ListOfDepartmentHeads;
        }


        public static List<Department> GetDepartmentsForHead(string sipAccount)
        {
            DataTable dt = new DataTable();
            Department department;
            List<Department> departmentsList = new List<Department>();
            
            List<string> columns = new List<string>();
            columns.Add(Enums.GetDescription(Enums.DepartmentHeads.Department));
            columns.Add(Enums.GetDescription(Enums.DepartmentHeads.SiteID));

            Dictionary<string, object> whereClause = new Dictionary<string, object> 
            { 
                {"SipAccount", sipAccount } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeads.TableName), columns, whereClause, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new Department();

                department.DepartmentName = (row[Enums.GetDescription(Enums.DepartmentHeads.Department)]).ToString();
                department.SiteID = Convert.ToInt32(row[Enums.GetDescription(Enums.DepartmentHeads.SiteID)]);
                department.SiteName = DB.Site.getSite(department.SiteID).SiteName ?? string.Empty;
                departmentsList.Add(department);
            }

            return departmentsList;
        }
    }


    public class Department
    {
        public string DepartmentName { get; set; }
        public string SiteName { get; set; }
        public int SiteID { get; set; }
    }
}