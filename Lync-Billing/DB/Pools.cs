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

        public int INSERT()
        {
            return 0;
        }

        public List<Pools> GetPools(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            DataTable dt = new DataTable();
            List<Pools> pools = new List<Pools>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Gateways.TableName), columns, wherePart, limits, allFields);

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

        public void UPDATE()
        {

        }

        public void DELETE()
        {
           
        }
    }
}