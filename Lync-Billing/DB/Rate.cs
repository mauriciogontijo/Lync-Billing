﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Rate
    {
        public int RateID { get; set; }
        public string CountryCode { get; set; }
        public string FixedLineRate { get; set; }
        public string MobileLineRate { get; set; }

        public DBLib DBRoutines = new DBLib();

        public string GetRatesTableName(string gatewayName,DateTime startingDate) 
        {
            StringBuilder RatesTableName = new StringBuilder();
            RatesTableName.Append("Rates_" + gatewayName + "_" + startingDate.Date.ToString("yyyymmdd"));
           
            return RatesTableName.ToString();
        }

        public List<Rate> GetRates(List<string> columns, Dictionary<string, object> wherePart, int limits, string ratesTableName)
        {
            List<Rate> rates = new List<Rate>();
            DataTable dt = new DataTable();
            Rate rate;
            

            dt = DBRoutines.SELECT(ratesTableName, columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                rate = new Rate();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.CountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.FixedLineRate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.FixedLineRate = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.MobileLineRate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.MobileLineRate = (string)row[column.ColumnName];
                }
                rates.Add(rate);
            }

            return rates;
            
        }

        public  int InsertRate(Rate rate)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((rate.CountryCode).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.CountryCode), rate.CountryCode);

            if ((rate.FixedLineRate).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.FixedLineRate), rate.FixedLineRate);

            if ((rate.MobileLineRate).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.MobileLineRate), rate.MobileLineRate);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.UsersRoles.TableName), columnsValues);

            return rowID;
        }

        public bool UpdatetRate(Rate rate, string ratesTableName)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((rate.CountryCode).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.CountryCode), rate.CountryCode);

            if ((rate.FixedLineRate).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.FixedLineRate), rate.FixedLineRate);

            if (rate.MobileLineRate != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.MobileLineRate), rate.MobileLineRate);

            //Execute Update
            status = DBRoutines.UPDATE(
                ratesTableName,
                setPart,
                Enums.GetDescription(Enums.Rates.RateID),
                rate.RateID);

            return status;
        }

        public bool DeleteRate(Rate rate,string ratesTableName)
        {
             bool status = false;

            status = DBRoutines.DELETE(
                ratesTableName, 
                Enums.GetDescription(Enums.Rates.RateID),
                rate.RateID);

            return status;
        }
        
    }
}