using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Libs;
using System.Data;

namespace Lync_Backend.Helpers
{
    class MonitoringServersInfo
    {
        public int Id { get; set; }
        public string InstanceHostName { get; set; }
        public string InstanceName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneCallsTable { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<MonitoringServersInfo> GetMonitoringServersInfo() 
        {
            DataTable dt = new DataTable();
            MonitoringServersInfo monInfo;

            List<MonitoringServersInfo> monInfos = new List<MonitoringServersInfo>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.MonitoringServersInfo.TableName));


            foreach (DataRow row in dt.Rows)
            {
                monInfo = new MonitoringServersInfo();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.Id) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.Id = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.InstanceHostName) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.InstanceHostName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.InstanceName) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.InstanceName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.Userame) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.Username = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.Password) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.Password = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.PhoneCallsTable) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.PhoneCallsTable = (string)row[column.ColumnName];
                }
                monInfos.Add(monInfo);
            }

            return monInfos;
        }

    }
}
