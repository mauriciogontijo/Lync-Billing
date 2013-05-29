using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Announcements
    {
        int ID { get; set; }
        string Announcement { get; set; }
        string Role { get; set; }
        DateTime AnnouncementDate { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static string GetAnnouncemnet(string role)
        {
            Announcements announcement = new Announcements();
            DataTable dt = new DataTable();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Announcements.TableName),"Role",role);

            foreach (DataRow row in dt.Rows)
            {
                announcement = new Announcements();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Announcements.ID) && row[column.ColumnName] != System.DBNull.Value)
                        announcement.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Announcements.Role) && row[column.ColumnName] != System.DBNull.Value)
                        announcement.Role = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Announcements.Announcement) && row[column.ColumnName] != System.DBNull.Value)
                        announcement.Announcement = (string)row[column.ColumnName];

                }
                //announcement.Add(gateway);
            }

            return announcement.Announcement;
        }
    
    }

    
}