using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
{
    class CallMarkerStatus
    {
        public int MarkerId { get; set; }
        public string PhoneCallsTable { get; set; }
        public DateTime Timestamp { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static CallMarkerStatus GetCallsMarkerStatus(string tableName)
        {
            DataTable dt = new DataTable();
            CallMarkerStatus importStat = null;

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.CallMarkerStatus.TableName), "phoneCallsTableName", tableName);

            if (dt.Rows.Count > 0)
            {
                importStat = new CallMarkerStatus();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.MarkerId) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.MarkerId = (int)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.PhoneCallsTable = (string)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallMarkerStatus.Timestamp) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.Timestamp = (DateTime)dt.Rows[0][column.ColumnName];
                }
            }

            return importStat;
        }


    }
}
