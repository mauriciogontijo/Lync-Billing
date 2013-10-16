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
        public string ImportedTableName { get; set; }
        public DateTime Timestamp { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static CallsImportStatus GetCallsImportStatus(string className)
        {
            DataTable dt = new DataTable();            
            CallsImportStatus importStat = null;

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.CallsImportStatus.TableName), "importedTableName", className);
            
            if (dt.Rows.Count > 0)
            {
                importStat = new CallsImportStatus();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.CallsImportStatus.ImportID) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.ImportId = (int)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallsImportStatus.ImportedTableName) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.ImportedTableName = (string)dt.Rows[0][column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.CallsImportStatus.Timestamp) && dt.Rows[0][column.ColumnName] != System.DBNull.Value)
                        importStat.Timestamp = (DateTime)dt.Rows[0][column.ColumnName];
                }
            }

            return importStat;
        }


        public static void SetCallsImportStatus(string className, string timestamp)
        {
            DataTable dt = new DataTable();

            Dictionary<string, object> wherePart = new Dictionary<string, object>()
            {
                {Enums.GetDescription(Enums.CallsImportStatus.ImportedTableName), className}
            };

            Dictionary<string, object> importStatusRecord = new Dictionary<string, object>()
            {
                {Enums.GetDescription(Enums.CallsImportStatus.ImportedTableName), className},
                {Enums.GetDescription(Enums.CallsImportStatus.Timestamp), timestamp}
            };

            //Check if the class name has already a CallsImportStatus record.
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.CallsImportStatus.TableName), "importedTableName", className);

            //Update it if it has a record, otherwise insert a new one
            if (dt.Rows.Count > 0)
            {
                DBRoutines.UPDATE(Enums.GetDescription(Enums.CallsImportStatus.TableName), importStatusRecord, wherePart);
            }
            else
            {
                DBRoutines.INSERT(Enums.GetDescription(Enums.CallsImportStatus.TableName), importStatusRecord);
            }
        }
    }
}
