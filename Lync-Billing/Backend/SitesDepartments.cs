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
        public int ID { get; set; }
        public int SiteID { get; set; }
        public int DepartmentID { get; set; }

        private static DBLib DBRoutines = new DBLib();

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

    }
}