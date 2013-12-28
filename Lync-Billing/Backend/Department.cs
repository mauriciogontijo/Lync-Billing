using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Lync_Billing.Libs;

namespace Lync_Billing.Backend
{
    public class Department
    {
        private static DBLib DBRoutines = new DBLib();

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }


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
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if(column.ColumnName == Enums.GetDescription(Enums.Departments.DepartmentName))
                        department.DepartmentName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.Description))
                        department.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

              
            }

            return department;
        }

        public static Department GetDepartment(string departmnetName) 
        {
            DataTable dt;
            Department department = new Department();
            List<string> columns;
            Dictionary<string, object> wherePart;

            columns = new List<string>();
            wherePart = new Dictionary<string, object>
            { 
                { Enums.GetDescription(Enums.Departments.DepartmentName), departmnetName } 
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Departments.TableName), columns, wherePart, 1);

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Departments.ID))
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.DepartmentName))
                        department.DepartmentName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.Description))
                        department.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }
            }

            return department;
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
                        department.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.DepartmentName))
                        department.DepartmentName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.Departments.Description))
                        department.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                departmentsList.Add(department);
            }

            return departmentsList;
        }

        public static bool UpdateDepartment(Department existingDepartment, bool FORCE_RESET_DESCRIPTION = false)
        {
            bool status = false;

            Dictionary<string, object> columnValues = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            wherePart.Add(Enums.GetDescription(Enums.Departments.ID), existingDepartment.DepartmentID);


            //Set Part
            if (!string.IsNullOrEmpty(existingDepartment.DepartmentName))
                columnValues.Add(Enums.GetDescription(Enums.Departments.DepartmentName), existingDepartment.DepartmentName);

           if (!string.IsNullOrEmpty(existingDepartment.Description))
                columnValues.Add(Enums.GetDescription(Enums.Departments.Description), existingDepartment.Description);


            //RESET FLAG
            if (FORCE_RESET_DESCRIPTION == true)
                columnValues.Add(Enums.GetDescription(Enums.Departments.Description), null);


            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.Departments.TableName), columnValues, wherePart);

            return status;
        }

        public static int AddDepartment(Department newDepartment)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(newDepartment.DepartmentName))
            {
                columnsValues.Add(Enums.GetDescription(Enums.Departments.DepartmentName), newDepartment.DepartmentName);

                if (!string.IsNullOrEmpty(newDepartment.Description))
                    columnsValues.Add(Enums.GetDescription(Enums.Departments.Description), newDepartment.Description);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Departments.TableName), columnsValues, Enums.GetDescription(Enums.Departments.ID));
            }

            return rowID;
        }

        public static bool DeleteDepartment(Department department)
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.Departments.TableName), Enums.GetDescription(Enums.Departments.ID), department.DepartmentID);

            return status;
        }

        public static void SyncDepartments() 
        {
            List<Users> listOfUsers =  Users.GetUsers(null, null, 0);

            foreach (Users user in listOfUsers) 
            {
                Department departmnet = GetDepartment(user.Department);
                string sameer;

                if (!string.IsNullOrEmpty(departmnet.DepartmentName))
                    continue;
                else
                    AddDepartment(new Department { DepartmentName = user.Department });
                
            }
        }

    }

}