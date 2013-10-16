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
        public DateTime Timestamp { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<CallMarkerStatus> GetCallMarkerStatus() 
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

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.Timestamp) && row[column.ColumnName] != System.DBNull.Value)
                        callMarkerEntryStat.Timestamp = (DateTime)row[column.ColumnName];
                }

                callMarkingStatus.Add(callMarkerEntryStat);
            }

            return callMarkingStatus;
        }
    
    }
}
