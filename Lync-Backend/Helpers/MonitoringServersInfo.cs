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
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TelephonySolutionName { get; set; }
        public string PhoneCallsTable { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 


        private static DBLib DBRoutines = new DBLib();

        public static Dictionary<string, MonitoringServersInfo> GetMonitoringServersInfo() 
        {
            DataTable dt = new DataTable();
            MonitoringServersInfo monInfo;

            Dictionary<string, MonitoringServersInfo> monInfos = new Dictionary<string, MonitoringServersInfo>();

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

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.DatabaseName) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.DatabaseName = (string)row[column.ColumnName];
                    
                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.Userame) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.Username = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.Password) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.Password = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.TelephonySolutionName) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.TelephonySolutionName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.PhoneCallsTable) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.PhoneCallsTable = (string)row[column.ColumnName];
                    
                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.Description) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.Description = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MonitoringServersInfo.CreatedAt) && row[column.ColumnName] != System.DBNull.Value)
                        monInfo.CreatedAt = (DateTime)row[column.ColumnName];
                 
                }
                monInfos.Add(monInfo.PhoneCallsTable ,monInfo);
            }

            return monInfos;
        }

        public static string CreateConnectionString(MonitoringServersInfo monInfo) 
        {
            string ConnectionString = null;

            if (monInfo.InstanceName != null)
            {
                ConnectionString = String.Format("Provider=SQLOLEDB.1;Data Source={0}\\{1};Persist Security Info=True;User ID={2};Password='{3}';Initial Catalog={4}",
                    monInfo.InstanceHostName,
                    monInfo.InstanceName,
                    monInfo.Username,
                    monInfo.Password,
                    monInfo.DatabaseName);
            }
            else
            {
                ConnectionString = String.Format("Provider=SQLOLEDB.1;Data Source={0};Persist Security Info=True;User ID={1};Password='{2}';Initial Catalog={3}",
                    monInfo.InstanceHostName,
                    monInfo.Username,
                    monInfo.Password,
                    monInfo.DatabaseName);
            }
            
            return ConnectionString;
        }

    }
}
