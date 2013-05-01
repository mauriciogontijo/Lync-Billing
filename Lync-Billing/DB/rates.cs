using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Rates
    {
        public int RateID { get; set; }
        public string FixedLineRate { get; set; }
        public string MobileLineRate { get; set; }

        public DBLib DBRoutines = new DBLib();

        public string GetRatesTableName(string gatewayName,DateTime startingDate) 
        {
            StringBuilder RatesTableName = new StringBuilder();
            RatesTableName.Append("Rates_" + gatewayName + "_" + startingDate.Date.ToString("yyyymmdd"));
           
            return RatesTableName.ToString();
        }

        public List<Users> GetRates(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            List<Users> users = new List<Users>();

            return users;
            
        }

        public  int InsertRate(Rates rate)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
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