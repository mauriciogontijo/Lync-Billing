using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class TmpUsers
    {
        int ID { set; get; }
        string SipAccount { set; get; }

        private static DBLib DBRoutines = new DBLib();
        private static AdLib ADRoutine = new AdLib();

        private static List<string> GetAllUsers() 
        {
            List<string> users = new List<string>();
            DataTable dt = new DataTable();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.TmpUsers.TableName));

            TmpUsers user;

            foreach (DataRow row in dt.Rows)
            {
                user = new TmpUsers();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TmpUsers.ID) && row[column.ColumnName] != System.DBNull.Value)
                        user.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TmpUsers.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        user.SipAccount = (string)row[column.ColumnName];

                }
                users.Add(user.SipAccount);
            }
            return users;
        }

        public static void InsertUsers()
        {
            List<string> users = GetAllUsers();

            ADUserInfo userInfo;
            Users dbUser;
            foreach (string user in users) 
            {
                userInfo = new ADUserInfo();
                dbUser = new Users();

                userInfo = ADRoutine.GetUserAttributes(user);

                if (userInfo == null)
                    continue;

                dbUser.SipAccount = user;

                if (userInfo.FirstName != null)
                    dbUser.FullName = userInfo.FirstName + " " + userInfo.LastName;
                else if (userInfo.DisplayName != null)
                    dbUser.FullName = userInfo.DisplayName;
                else
                    dbUser.FullName = string.Empty;
                try
                {
                    dbUser.EmployeeID = Convert.ToInt32((userInfo.EmployeeID));
                }
                catch (Exception ex) 
                {
                    dbUser.EmployeeID = 0;
                }
                
                dbUser.SiteName = userInfo.physicalDeliveryOfficeName;
                dbUser.Department = userInfo.department;

                //get the actual data from ADUser table to match if exists and needs to be updated or inserted 
                Users adUser = Users.GetUser(dbUser.SipAccount);

                if (adUser == null)
                    Users.InsertUser(dbUser);
                else
                    Users.UpdateUser(dbUser);

            }

        }


    }
}