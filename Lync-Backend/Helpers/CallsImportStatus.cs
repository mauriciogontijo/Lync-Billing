using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Libs;
using System.Data;

namespace Lync_Backend.Helpers
{
    class CallsImportStatus
    {
        public int ImportId { get; set; }
        public string PhoneCallsTableName { get; set; }
        public DateTime Timestamp { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static CallsImportStatus GetCallsImportStatus(string tableName)
        {
            DataTable dt = new DataTable();            
            CallsImportStatus importStat = null;

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.CallsImportStatus.TableName), "phoneCallsTableName", tableName);

            if (dt.Rows.Count > 0)
            {
                importStat = new CallsImportStatus();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.CallsImportStatus.ImportID) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.ImportId = (int)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallsImportStatus.PhoneCallsTableName) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.PhoneCallsTableName = (string)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallsImportStatus.Timestamp) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.Timestamp = (DateTime)dt.Rows[0][column.ColumnName];
                }
            }

            return importStat;
        }
    }
}
