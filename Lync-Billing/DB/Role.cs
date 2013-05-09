using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;


namespace Lync_Billing.DB
{
    public class Role
    {
        private static DBLib DBRoutines = new DBLib();

        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public static List<Role> GetRoles(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            Role role;
            DataTable dt = new DataTable();
            List<Role> Roles = new List<Role>();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.GatewaysDetails.TableName), 
                columns,
                wherePart, 
                limits);

            foreach (DataRow row in dt.Rows)
            {
                role = new Role();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Roles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        role.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Roles.RoleName) && row[column.ColumnName] != System.DBNull.Value)
                        role.RoleName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Roles.RoleDescription) && row[column.ColumnName] != System.DBNull.Value)
                        role.RoleDescription = (string)row[column.ColumnName];
                }
                Roles.Add(role);
            }

            return Roles;

        }

        public static int InsertRole(Role role)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((role.RoleName).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Roles.RoleName), role.RoleName);

            if ((role.RoleDescription).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Roles.RoleDescription), role.RoleDescription);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Roles.TableName), columnsValues, Enums.GetDescription(Enums.Roles.RoleID));

            return rowID;
        }

        public static bool UpdateRole(Role role)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((role.RoleName).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Roles.RoleName), role.RoleName);

            if (role.RoleDescription != null)
                setPart.Add(Enums.GetDescription(Enums.Roles.RoleDescription), role.RoleDescription);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Roles.TableName), 
                setPart,
                Enums.GetDescription(Enums.Roles.RoleID),
                role.RoleID);

            if (status == false)
            {
                //throw error message
            }

            return status;
        }

        public static bool DeleteRole(Role role)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.Roles.TableName), 
                Enums.GetDescription(Enums.Roles.RoleID), role.RoleID);

            return status;
        }

    }
}