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
    public class GatewayDetail
    {
        public DBLib DBRoutines = new DBLib();

        public int GatewaysDetailsID { set; get; }
        public string GatewayName { set; get; }
        public string GatewayLocation { set; get; }
        public string CountryCode { set; get; }
      


        public List<GatewayDetail> GetGatewaysDetails(List<string> columns, Dictionary<string, object> wherePart, int limits) 
        {
            GatewayDetail gateway;
            DataTable dt = new DataTable();
            List<GatewayDetail> gateways = new List<GatewayDetail>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysDetails.TableName), columns, wherePart, limits);

             foreach(DataRow row in dt.Rows)
            {
                gateway = new GatewayDetail();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewaysDetailsID))
                        gateway.GatewaysDetailsID = (int)row[column.ColumnName];

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

        public int InsertGatewayDetails(GatewayDetail gatewayDetails) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();;
           
            //Set Part
            if (gatewayDetails.GatewayName != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayName), gatewayDetails.GatewayName);

            if (gatewayDetails.CountryCode != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.CountryCode), gatewayDetails.CountryCode);

            if (gatewayDetails.GatewayLocation != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayLocation), gatewayDetails.GatewayLocation);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.GatewaysDetails.TableName), columnsValues);
           
            return rowID;
        }

        public bool UpdateGatewayDetails(GatewayDetail gatewayDetails) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (gatewayDetails.GatewayName != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayName), gatewayDetails.GatewayName);

            if (gatewayDetails.CountryCode != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.CountryCode), gatewayDetails.CountryCode);

            if (gatewayDetails.GatewayLocation != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayLocation), gatewayDetails.GatewayLocation);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.GatewaysDetails.TableName), 
                setPart, 
                Enums.GetDescription(Enums.GatewaysDetails.GatewaysDetailsID),
                gatewayDetails.GatewaysDetailsID);

            if (status == false)
            {
                //throw error message
            }
            
            return true;
        }

        public bool DeleteGatewayDetails(GatewayDetail gatewayDetails) 
        {
            bool status = false;
           
            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.GatewaysDetails.TableName), 
                Enums.GetDescription(Enums.GatewaysDetails.GatewaysDetailsID),
                gatewayDetails.GatewaysDetailsID);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }
    }
}