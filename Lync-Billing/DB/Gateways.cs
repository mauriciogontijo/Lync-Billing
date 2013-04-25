using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    /// <remarks>
    /// TODO: Validate if gateway has been used by other tables since there is no direct relation between tables related to gateways
    /// </remarks>
    public class Gateways
    {
        public DBLib DBRoutines = new DBLib();
        public int GatewayID { set; get; }
        public string GatewayFQDN { set; get; }
        public string GatewayLocation { set; get; }
        public string GatewayCountry { set; get; }
        public string GatewayUPN { set; get; }
        public int PoolID { set; get; }
        public int RatesTableID { set; get; }

        public List<Gateways> GetGateways(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits) 
        {
            Gateways gateway;
            DataTable dt = new DataTable();
            List<Gateways> gateways = new List<Gateways>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Gateways.TableName), columns, wherePart, limits, allFields);

             foreach(DataRow row in dt.Rows)
            {
                gateway = new Gateways();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayID))
                        gateway.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayFQDN))
                        gateway.GatewayFQDN = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayCountry))
                        gateway.GatewayCountry = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.GatewayLocation))
                        gateway.GatewayCountry = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Gateways.PoolID))
                        gateway.PoolID = (int)row[column.ColumnName];
                }
                 gateways.Add(gateway);
             }

            return gateways;
        }

        public int InsertGateways(List<Gateways> gateways) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();;

            foreach (Gateways gateway in gateways)
            {
                 
                //Set Part
                if (gateway.GatewayFQDN != null)
                    columnsValues.Add(Enums.GetDescription(Enums.Gateways.GatewayFQDN), gateway.GatewayFQDN);

                if (gateway.GatewayCountry != null)
                    columnsValues.Add(Enums.GetDescription(Enums.Gateways.GatewayCountry), gateway.GatewayCountry);

                if (gateway.GatewayLocation != null)
                    columnsValues.Add(Enums.GetDescription(Enums.Gateways.GatewayLocation), gateway.GatewayLocation);

                if (gateway.RatesTableID.ToString() != null)
                    columnsValues.Add(Enums.GetDescription(Enums.Gateways.PoolID), gateway.PoolID);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Gateways.TableName), columnsValues);
            }
            return rowID;
        }

        public bool UpdateGateways(List<Gateways> gateways) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            foreach (Gateways gateway in gateways)
            {
                //Where Part
                wherePart.Add(Enums.GetDescription(Enums.Gateways.GatewayID), gateway.GatewayID);

                //Set Part
                if (gateway.GatewayFQDN != null)
                    setPart.Add(Enums.GetDescription(Enums.Gateways.GatewayFQDN), gateway.GatewayFQDN);

                if (gateway.GatewayCountry != null)
                    setPart.Add(Enums.GetDescription(Enums.Gateways.GatewayCountry), gateway.GatewayCountry);

                if (gateway.GatewayLocation != null)
                    setPart.Add(Enums.GetDescription(Enums.Gateways.GatewayLocation), gateway.GatewayLocation);

                if (gateway.PoolID.ToString() != null)
                    setPart.Add(Enums.GetDescription(Enums.Gateways.PoolID), gateway.PoolID);

                //Execute Update
                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.Gateways.TableName), setPart, wherePart);

                 if (status == false)
                {
                    //throw error message
                }
            }
            return true;
        }

        public bool DeleteGateways(List<Gateways> gateways) 
        {
            bool status = false;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            
            foreach (Gateways gateway in gateways) 
            {

                if ((gateway.GatewayID).ToString() != null)
                    wherePart.Add(Enums.GetDescription(Enums.Gateways.GatewayID), gateway.GatewayID);

                if (gateway.GatewayFQDN != null)
                    wherePart.Add(Enums.GetDescription(Enums.Gateways.GatewayFQDN), gateway.GatewayFQDN);

                if (gateway.GatewayCountry != null)
                    wherePart.Add(Enums.GetDescription(Enums.Gateways.GatewayCountry), gateway.GatewayCountry);

                if (gateway.GatewayLocation != null)
                    wherePart.Add(Enums.GetDescription(Enums.Gateways.GatewayLocation), gateway.GatewayLocation);

                if (gateway.PoolID.ToString() != null)
                    wherePart.Add(Enums.GetDescription(Enums.Gateways.PoolID), gateway.PoolID);

                status = DBRoutines.DELETE(Enums.GetDescription(Enums.Gateways.TableName), wherePart);

                if (status == false)
                {
                    //throw error message
                }
            }
            return status;
        }
    }
}