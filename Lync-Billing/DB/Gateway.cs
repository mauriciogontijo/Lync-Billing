using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Gateway
    {
        public int GatewayId { get; set; }
        public string GatewayName { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Gateway> GetGateways()
        {
            Gateway gateway;
            DataTable dt = new DataTable();
            List<Gateway> gateways = new List<Gateway>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Gateways.TableName));

            foreach (DataRow row in dt.Rows)
            {
                gateway = new Gateway();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayId) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayId = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayName) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayName = (string)row[column.ColumnName];
                }
                gateways.Add(gateway);
            }
            return gateways;
        }

    }
}