using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Lync_Billing.Libs;

namespace Lync_Billing.Backend
{
    public class DID
    {
        public int ID { get; set; }
        public string DIDPattern { get; set; }
        public string Description { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<DID> GetAllDIDs()
        {
            DID DIDObject;
            DataTable dt = new DataTable();
            List<DID> DIDsList = new List<DID>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DIDs.TableName));

            foreach (DataRow row in dt.Rows)
            {
                DIDObject = new DID();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DIDs.ID))
                        DIDObject.ID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DIDs.DIDPattern))
                        DIDObject.DIDPattern = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DIDs.Description))
                        DIDObject.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                DIDsList.Add(DIDObject);
            }

            return DIDsList;
        }


        public static int AddDID(DID newDIDObject)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            if (!string.IsNullOrEmpty(newDIDObject.DIDPattern))
            {
                columnsValues.Add(Enums.GetDescription(Enums.DIDs.DIDPattern), newDIDObject.DIDPattern);

                if (!string.IsNullOrEmpty(newDIDObject.Description))
                    columnsValues.Add(Enums.GetDescription(Enums.DIDs.Description), newDIDObject.Description);

                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.DIDs.TableName), columnsValues, Enums.GetDescription(Enums.DIDs.ID));
            }

            return rowID;
        }


        public static bool UpdateDID(DID existingDIDObject, bool FORCE_RESET_DESCRIPTION = false)
        {
            bool status = false;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            wherePart.Add(Enums.GetDescription(Enums.DIDs.ID), existingDIDObject.ID);

            if (!string.IsNullOrEmpty(existingDIDObject.DIDPattern) && existingDIDObject.ID > 0)
            {
                columnsValues.Add(Enums.GetDescription(Enums.DIDs.DIDPattern), existingDIDObject.DIDPattern);

                if (!string.IsNullOrEmpty(existingDIDObject.Description))
                    columnsValues.Add(Enums.GetDescription(Enums.DIDs.Description), existingDIDObject.Description);

                else if (FORCE_RESET_DESCRIPTION == true)
                    columnsValues.Add(Enums.GetDescription(Enums.DIDs.Description), null);

                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.DIDs.TableName), columnsValues, wherePart);
            }

            return status;
        }


        public static bool DeleteDID(DID existingDIDObject)
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.DIDs.TableName), Enums.GetDescription(Enums.DIDs.ID), existingDIDObject.ID);

            return status;
        }
    }
}