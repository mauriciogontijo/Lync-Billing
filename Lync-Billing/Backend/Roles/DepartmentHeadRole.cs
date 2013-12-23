using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Lync_Billing.Libs;

namespace Lync_Billing.Backend.Roles
{
    public class DepartmentHeadRole
    {
        private static DBLib DBRoutines = new DBLib();

        // Logical representation only!
        // The following instance variable doesn't represent an actual column in the original table
        // It was added to make it more easy to lookup the site names without having to go to the database each time and query it.
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentHeadName { get; set; }

        //The following instance variables represent actual columns in the original table.
        public int ID { get; set; }
        public string SipAccount { get; set; }
        public int DepartmentID { get; set; }


        public static bool IsDepartmentHead(string userSipAccount)
        {
            var departments = GetDepartmentsForHead(userSipAccount);
            return (departments.Count > 0);
        }


        public static List<DepartmentHeadRole> GetDepartmentHeads(int? departmentID = null)
        {
            DataTable dt = new DataTable();
            List<string> columns;
            Dictionary<string, object> wherePart;

            Department department;
            DepartmentHeadRole departmentHead;
            List<DepartmentHeadRole> ListOfDepartmentHeads = new List<DepartmentHeadRole>();

            //Used to retrieve users full-names.
            List<Users> allUsersInfo = Users.GetUsers(null, null, 0);

            //Get the data from the database
            if (departmentID != null)
            {
                columns = new List<string>();

                wherePart = new Dictionary<string, object>
                {
                    { Enums.GetDescription(Enums.DepartmentHeadRoles.DepartmentID), Convert.ToInt32(departmentID) }
                };

                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeadRoles.TableName), columns, wherePart, 0);
            }
            else
            {
                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeadRoles.TableName));
            }


            foreach (DataRow row in dt.Rows)
            {
                department = new Department();
                departmentHead = new DepartmentHeadRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeadRoles.ID))
                        departmentHead.ID = Convert.ToInt32(row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeadRoles.SipAccount))
                    {
                        departmentHead.SipAccount = Convert.ToString(row[column.ColumnName]);
                        var userInfo = allUsersInfo.Find(user => user.SipAccount == departmentHead.SipAccount);
                        departmentHead.DepartmentHeadName = HelperFunctions.FormatUserDisplayName(userInfo.FullName, userInfo.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeadRoles.DepartmentID))
                    {
                        departmentHead.DepartmentID = Convert.ToInt32(row[column.ColumnName]);

                        department = Department.GetDepartment(departmentHead.DepartmentID);

                        departmentHead.SiteID = department.SiteID;
                        departmentHead.SiteName = department.SiteName;
                        departmentHead.DepartmentName = department.DepartmentName;
                    }
                }

                ListOfDepartmentHeads.Add(departmentHead);
            }

            return ListOfDepartmentHeads;
        }


        public static List<Department> GetDepartmentsForHead(string sipAccount)
        {
            DataTable dt;
            List<string> columns;
            Dictionary<string, object> wherePart;

            int departmentId;
            Department department;
            List<Department> departmentsList = new List<Department>();
            
            //Get the data from the database
            columns = new List<string>();
            wherePart = new Dictionary<string, object> 
            { 
                {Enums.GetDescription(Enums.DepartmentHeadRoles.SipAccount), sipAccount } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DepartmentHeadRoles.TableName), columns, wherePart, 0);

            foreach (DataRow row in dt.Rows)
            {
                department = new Department();

                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DepartmentHeadRoles.DepartmentID))
                    {
                        departmentId = Convert.ToInt32(row[column.ColumnName]);
                        department = Department.GetDepartment(departmentId);
                        departmentsList.Add(department);
                    }
                }
            }

            return departmentsList;
        }


        public static int AddDepartmentHead(DepartmentHeadRole newDepartmentHead)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            if (!string.IsNullOrEmpty(newDepartmentHead.SipAccount) && newDepartmentHead.DepartmentID > 0 && !string.IsNullOrEmpty(newDepartmentHead.DepartmentID.ToString()))
            {
                columnsValues.Add(Enums.GetDescription(Enums.DepartmentHeadRoles.SipAccount), newDepartmentHead.SipAccount);
                columnsValues.Add(Enums.GetDescription(Enums.DepartmentHeadRoles.DepartmentID), newDepartmentHead.DepartmentID);

                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.DepartmentHeadRoles.TableName), columnsValues, Enums.GetDescription(Enums.DepartmentHeadRoles.ID));
            }

            return rowID;
        }


        public static bool DeleteDepartmentHead(DepartmentHeadRole departmentHead)
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.DepartmentHeadRoles.TableName), Enums.GetDescription(Enums.DepartmentHeadRoles.ID), departmentHead.ID);

            return status;
        }

    }

}