using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
{
    class Gateways
    {
        public int GatewayId { get; set; }
        public string GatewayName { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Gateways> GetGateways() 
        {
            DataTable dt = new DataTable();
            Gateways gateway;


            List<Gateways> listOfGateways = new List<Gateways>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Gateways.TableName));

            foreach (DataRow row in dt.Rows)
            {
                gateway = new Gateways();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayId) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayId = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayName) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayName = (string)row[column.ColumnName];
                }

                listOfGateways.Add(gateway);
            }

            return listOfGateways;
        }
    }
}
