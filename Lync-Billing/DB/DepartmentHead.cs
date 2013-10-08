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

        private int ID { get; set; }
        public string SipAccount { get; set; }
        public string Department { get; set; }

        public static List<DepartmentHead> GetDepartmentHeads(string departmentName)
        {
            DataTable dt = new DataTable();
            DepartmentHead head;
            List<DepartmentHead> departmentHeads = new List<DepartmentHead>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeads.TableName), "Department", departmentName.ToString());

            foreach (DataRow row in dt.Rows)
            {
                head = new DepartmentHead();

                foreach (DataColumn column in dt.Columns)
                {
                    if(column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.ID))
                        head.ID = (int)(row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.SipAccount))
                        head.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeads.Department))
                        head.Department = (string)row[column.ColumnName];
                }
                
                departmentHeads.Add(head);
            }

            return departmentHeads;
        }

        public static List<Department> GetDepartmentsForHead(string sipAccount)
        {
            DataTable dt = new DataTable();
            Department department;
            List<Department> departmentsList = new List<Department>();
            
            List<string> columns = new List<string>();
            columns.Add(Enums.GetDescription(Enums.DepartmentHeads.Department));

            Dictionary<string, object> whereClause = new Dictionary<string, object> 
            { 
                {"SipAccount", sipAccount } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeads.TableName), columns, whereClause, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new Department();

                if (row[Enums.GetDescription(Enums.DepartmentHeads.Department)] != System.DBNull.Value)
                {
                    department.DepartmentName = (row[Enums.GetDescription(Enums.DepartmentHeads.Department)]).ToString();
                    departmentsList.Add(department);
                }
            }

            return departmentsList;
        }
    }


    public class Department
    {
        public string DepartmentName { get; set; }
    }
}