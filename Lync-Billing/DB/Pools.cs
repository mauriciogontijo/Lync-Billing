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
        public string PoolFQDN { set; get; }
       
        public List<Pools> GetPools(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            Pools pool;
            DataTable dt = new DataTable();
            List<Pools> pools = new List<Pools>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Pools.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                pool = new Pools();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Pools.PoolID))
                        pool.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Pools.PoolFQDN))
                        pool.PoolFQDN = (string)row[column.ColumnName];
                }
                pools.Add(pool);
            }
            return pools;
        }

        public int InsertPool(Pools pool)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();

            //Set Part
            if (pool.PoolFQDN != null)
                columnsValues.Add(Enums.GetDescription(Enums.Pools.PoolFQDN), pool.PoolFQDN);
            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Pools.TableName), columnsValues);

            return rowID;
        }

        public bool UpdatePool(Pools pool)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (pool.PoolFQDN != null)
                setPart.Add(Enums.GetDescription(Enums.Pools.PoolFQDN), pool.PoolFQDN);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Pools.TableName), 
                setPart, 
                Enums.GetDescription(Enums.Pools.PoolID),
                pool.PoolID);

            if (status == false)
            {
                //throw error message
            }
            return true;
        }

        public bool DeletePool(Pools pool)
        {
            bool status = false;
            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.Pools.TableName),
                Enums.GetDescription(Enums.Pools.PoolID),
                pool.PoolID);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }
    }
}