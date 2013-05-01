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
    public class GatewaysDetails
    {
        public DBLib DBRoutines = new DBLib();
        
        public int GatewayID { set; get; }
        public string GatewayName { set; get; }
        public string GatewayLocation { set; get; }
        public string CountryCode { set; get; }
      

        public List<GatewaysDetails> GetGateways(List<string> columns, Dictionary<string, object> wherePart, int limits) 
        {
            GatewaysDetails gateway;
            DataTable dt = new DataTable();
            List<GatewaysDetails> gateways = new List<GatewaysDetails>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysDetails.TableName), columns, wherePart, limits);

             foreach(DataRow row in dt.Rows)
            {
                gateway = new GatewaysDetails();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayID))
                        gateway.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayName))
                        gateway.GatewayName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayLocation))
                        gateway.GatewayLocation = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.CountryCode))
                        gateway.CountryCode = (string)row[column.ColumnName];

                }
                 gateways.Add(gateway);
             }

            return gateways;
        }

        public int InsertGateway(GatewaysDetails gateway) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();;
           
            //Set Part
            if (gateway.GatewayName != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayName), gateway.GatewayName);

            if (gateway.CountryCode != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.CountryCode), gateway.CountryCode);

            if (gateway.GatewayLocation != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayLocation), gateway.GatewayLocation);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.GatewaysDetails.TableName), columnsValues);
           
            return rowID;
        }

        public bool UpdateGateway(GatewaysDetails gateway) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

           
            //Where Part
            wherePart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayID), gateway.GatewayID);

            //Set Part
            if (gateway.GatewayName != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayName), gateway.GatewayName);

            if (gateway.CountryCode != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.CountryCode), gateway.CountryCode);

            if (gateway.GatewayLocation != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayLocation), gateway.GatewayLocation);

            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.GatewaysDetails.TableName), setPart, wherePart);

            if (status == false)
            {
                //throw error message
            }
            
            return true;
        }

        public bool DeleteGateway(GatewaysDetails gateway) 
        {
            bool status = false;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            if ((gateway.GatewayID).ToString() != null)
                wherePart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayID), gateway.GatewayID);

            if (gateway.GatewayName != null)
                wherePart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayName), gateway.GatewayName);

            if (gateway.CountryCode != null)
                wherePart.Add(Enums.GetDescription(Enums.GatewaysDetails.CountryCode), gateway.CountryCode);

            if (gateway.GatewayLocation != null)
                wherePart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayLocation), gateway.GatewayLocation);

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.GatewaysDetails.TableName), wherePart);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }
    }
}