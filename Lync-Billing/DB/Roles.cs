using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;


namespace Lync_Billing.DB
{
    public class Roles
    {
        public DBLib DBRoutines = new DBLib();

        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public List<Roles> GetRoles(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            Roles role;
            DataTable dt = new DataTable();
            List<Roles> Roles = new List<Roles>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysDetails.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                role = new Roles();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Roles.RoleID))
                        role.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Roles.RoleName))
                        role.RoleName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Roles.RoleDescription))
                        role.RoleDescription = (string)row[column.ColumnName];
                }
                Roles.Add(role);
            }

            return Roles;

        }

        public int InsertRole(Roles role)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((role.RoleName).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Roles.RoleName), role.RoleName);

            if ((role.RoleDescription).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Roles.RoleDescription), role.RoleDescription);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Roles.TableName), columnsValues);

            return rowID;
        }

        public bool UpdateRole(Roles role)
        {
            bool status = false;


            return status;
        }

        public bool DeleteRole(Roles role)
        {
            bool status = false;


            return status;
        }

    }
}