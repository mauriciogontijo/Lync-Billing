using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Rates
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

        public List<Rates> GetRates(List<string> columns, Dictionary<string, object> wherePart, int limits, string RatesTableName)
        {
            List<Rates> rates = new List<Rates>();
            DataTable dt = new DataTable();
            Rates rate;
            

            dt = DBRoutines.SELECT(RatesTableName, columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                rate = new Rates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.RateID))
                        rate.RateID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.CountryCode))
                        rate.CountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.FixedLineRate))
                        rate.FixedLineRate = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.MobileLineRate))
                        rate.MobileLineRate = (string)row[column.ColumnName];
                }
                rates.Add(rate);
            }

            return rates;
            
        }

        public  int InsertRate(Rates rate)
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

        public bool UpdateRate(Rates rates)
        {
            bool status = false;


            return status;
        }
        
    }
}