using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Pools
    {
        public DBLib DBRoutines = new DBLib();
        public int PoolID { set; get; }
        public string PoolName { set; get; }

        public int InsertPool(List<Pools> pools)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues;

            foreach (Pools pool in pools)
            {
                columnsValues = new Dictionary<string, object>();
                //Set Part
                if (pool.PoolName != null)
                    columnsValues.Add(Enums.GetDescription(Enums.Pools.PoolName), pool.PoolName);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Pools.TableName), columnsValues);
            }
            return rowID;
        }

        public List<Pools> GetPools(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            DataTable dt = new DataTable();
            List<Pools> pools = new List<Pools>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Pools.TableName), columns, wherePart, limits, allFields);

            foreach (DataRow row in dt.Rows)
            {
                Pools pool = new Pools();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Pools.PoolID))
                        pool.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Pools.PoolName))
                        pool.PoolName = (string)row[column.ColumnName];
                }
                pools.Add(pool);
            }
            return pools;
        }

        public bool UpdatePools(List<Pools> pools)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            foreach (Pools pool in pools)
            {
                //Where Part
                wherePart.Add(Enums.GetDescription(Enums.Pools.PoolID), pool.PoolID);

                //Set Part
                if (pool.PoolName != null)
                    setPart.Add(Enums.GetDescription(Enums.Pools.PoolName), pool.PoolName);

                //Execute Update
                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.Pools.TableName), setPart, wherePart);

                if (status == false)
                {
                    //throw error message
                }
            }
            return true;
        }

        public bool DeletePools(List<Pools> pools)
        {
            bool status = false;
            Dictionary<string, object> wherePart;

            foreach (Pools pool in pools)
            {
                wherePart = new Dictionary<string, object>();


                if ((pool.PoolID).ToString() != null)
                    wherePart.Add(Enums.GetDescription(Enums.Pools.PoolID), pool.PoolID);

                if (pool.PoolName != null)
                    wherePart.Add(Enums.GetDescription(Enums.Pools.PoolName), pool.PoolName);

                status = DBRoutines.DELETE(Enums.GetDescription(Enums.Pools.TableName), wherePart);

                if (status == false)
                {
                    //throw error message
                }
            }
            return status;
        }
    }
}