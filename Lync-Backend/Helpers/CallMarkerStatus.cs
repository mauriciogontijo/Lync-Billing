using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
{
    class CallMarkerStatus
    {
        public int MarkerId { get; set; }
        public string PhoneCallsTable { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }

        private static DBLib DBRoutines = new DBLib();
        
        /// <summary>
        /// Get related information for Call Marking status
        /// </summary>
        /// <returns>list of CallMarkingStatus Objects</returns>
        public static List<CallMarkerStatus> GetAllCallMarkerStatuses() 
        {
            DataTable dt = new DataTable();
            CallMarkerStatus callMarkerEntryStat;


            List<CallMarkerStatus> callMarkingStatus = new List<CallMarkerStatus>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.CallMarkerStatus.TableName));

            foreach (DataRow row in dt.Rows)
            {
                callMarkerEntryStat = new CallMarkerStatus();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.MarkerId) && row[column.ColumnName] != System.DBNull.Value)
                        callMarkerEntryStat.MarkerId = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable) && row[column.ColumnName] != System.DBNull.Value)
                        callMarkerEntryStat.PhoneCallsTable = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.Type) && row[column.ColumnName] != System.DBNull.Value)
                        callMarkerEntryStat.Type = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.Timestamp) && row[column.ColumnName] != System.DBNull.Value)
                        callMarkerEntryStat.Timestamp = (DateTime)row[column.ColumnName];
                }

                callMarkingStatus.Add(callMarkerEntryStat);
            }

            return callMarkingStatus;
        }

        public static CallMarkerStatus GetCallMarkerStatus(string phoneCallTable, string type)
        {
            DataTable dt = new DataTable();
            CallMarkerStatus callMarkerStatus = new CallMarkerStatus();

            List<string> columns = new List<string>();

            Dictionary<string, object> whereClause = new Dictionary<string,object>
            {
                {Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable), phoneCallTable},
                {Enums.GetDescription(Enums.CallMarkerStatus.Type), type}
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.CallMarkerStatus.TableName), columns, whereClause, 1);

            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.MarkerId) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        callMarkerStatus.MarkerId = (int)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        callMarkerStatus.PhoneCallsTable = (string)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.Type) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        callMarkerStatus.Type = (string)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.Timestamp) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        callMarkerStatus.Timestamp = (DateTime)dt.Rows[0][column.ColumnName];
                }

                return callMarkerStatus;
            }
            else
            {
                return null;
            }
        }
    }
}
