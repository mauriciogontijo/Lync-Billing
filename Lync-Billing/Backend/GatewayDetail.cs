﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.Backend
{
    /// <remarks>
    /// TODO: Validate if gateway has been used by other tables since there is no direct relation between tables related to gateways
    /// </remarks>
    public class GatewayDetail
    {
        private static DBLib DBRoutines = new DBLib();

        public int GatewayID { set; get; }
        public int SiteID { set; get; }
        public int PoolID { set; get; }
        public string Description { set; get; }


        public static List<GatewayDetail> GetGatewaysDetails() 
        {
            GatewayDetail gateway;
            DataTable dt = new DataTable();
            List<GatewayDetail> gateways = new List<GatewayDetail>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysDetails.TableName));

            foreach (DataRow row in dt.Rows)
            {
                gateway = new GatewayDetail();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.Description) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.Description = (string)row[column.ColumnName];

                }
                gateways.Add(gateway);
            }

            return gateways;
        }

        public static List<GatewayDetail> GetGatewaysDetails(int gatewayID) 
        {
            GatewayDetail gateway;
            DataTable dt = new DataTable();
            List<GatewayDetail> gateways = new List<GatewayDetail>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysDetails.TableName), Enums.GetDescription(Enums.GatewaysDetails.GatewayID),gatewayID);

            foreach (DataRow row in dt.Rows)
            {
                gateway = new GatewayDetail();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.PoolID = (int)row[column.ColumnName];


                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.Description) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.Description = (string)row[column.ColumnName];

                }
                gateways.Add(gateway);
            }

            return gateways;
        }

        public static List<GatewayDetail> GetGatewaysDetails(List<string> columns, Dictionary<string, object> wherePart, int limits) 
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
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.Description) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.Description = (string)row[column.ColumnName];

                }
                 gateways.Add(gateway);
             }

            return gateways;
        }

        public static int InsertGatewayDetails(GatewayDetail gatewayDetails) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();;
           
            //Set Part

            if (gatewayDetails.GatewayID.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.GatewayID), gatewayDetails.GatewayID);

            if (gatewayDetails.SiteID.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.SiteID), gatewayDetails.SiteID);

            if (gatewayDetails.PoolID.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.PoolID), gatewayDetails.PoolID);

            if (gatewayDetails.Description.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysDetails.Description), gatewayDetails.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.GatewaysDetails.TableName), columnsValues, Enums.GetDescription(Enums.GatewaysDetails.GatewayID));
           
            return rowID;
        }

        public static bool UpdateGatewayDetails(GatewayDetail gatewayDetails) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (gatewayDetails.SiteID.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.SiteID), gatewayDetails.SiteID);

            if (gatewayDetails.PoolID.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.PoolID), gatewayDetails.PoolID);

            if (gatewayDetails.Description.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysDetails.Description), gatewayDetails.Description);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.GatewaysDetails.TableName), 
                setPart, 
                Enums.GetDescription(Enums.GatewaysDetails.GatewayID),
                gatewayDetails.GatewayID);

            if (status == false)
            {
                //throw error message
            }
            
            return true;
        }

        public static bool DeleteGatewayDetails(GatewayDetail gatewayDetails) 
        {
            bool status = false;
           
            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.GatewaysDetails.TableName), 
                Enums.GetDescription(Enums.GatewaysDetails.GatewayID),
                gatewayDetails.GatewayID);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }

        public static List<GatewayDetail> GetGatewaysDetails(string siteName)
        {
            GatewayDetail gateway;
            DataTable dt = new DataTable();
            List<GatewayDetail> gateways = new List<GatewayDetail>();

            var site = Backend.Site.GetSite(SiteName: siteName);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysDetails.TableName), Enums.GetDescription(Enums.GatewaysDetails.SiteID), site.SiteID);

            foreach (DataRow row in dt.Rows)
            {
                gateway = new GatewayDetail();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysDetails.Description) && row[column.ColumnName] != System.DBNull.Value)
                        gateway.Description = (string)row[column.ColumnName];

                }
                gateways.Add(gateway);
            }

            return gateways;
        }

        public static List<Site> GetSitesForGateway(int gatewayID) 
        {
            List<GatewayDetail> gatewaysDetails = new List<GatewayDetail>();
            List<Site> sites = new List<Site>();
            
            //Get GatewayDetails Where GatewayID = gatewayID
            gatewaysDetails = GatewayDetail.GetGatewaysDetails(gatewayID);

            foreach (GatewayDetail item in gatewaysDetails) 
            {
                Site site = Site.GetSite(item.SiteID);
                if(site != null)
                    sites.Add(site);
            }

            return sites;
        }


        public static List<string> GetSitesNamesForGateway(int gatewayID)
        {
            List<GatewayDetail> gatewaysDetails = new List<GatewayDetail>();
            List<string> sites = new List<string>();

            //Get GatewayDetails Where GatewayID = gatewayID
            gatewaysDetails = GatewayDetail.GetGatewaysDetails(gatewayID);

            foreach (GatewayDetail item in gatewaysDetails)
            {
                Site site = Site.GetSite(item.SiteID);
                if (site != null)
                    sites.Add(site.SiteName);
            }

            return sites;
        }
    }
}