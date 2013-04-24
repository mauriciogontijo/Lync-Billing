using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class PhoneCall
    {
        public DBLib DBRoutines = new DBLib();
       
        public long PhoneCallID { set; get; }
        public string SrcNumber { set; get; }
        public string DstNumber { set; get; }
        public string SipAccount { set; get; }
        public string DstCountry { set; get; }
        public string TypeOfService { set; get; }
        public string UpdatedBy { set; get; }
        public string ModifiedBy { set; get; }
        public string Gateway { set; get; }

        public double Duration { set; get; }
        public double Rate { set; get; }
        public double Cost { set; get; }
        
        public DateTime UpdateOn{ set; get; }
        public DateTime ModifiedOn { set; get; }
        public DateTime DateOfCall { set; get; }
        

        public bool IsPersonal { set; get; }
        public bool Dispute { set; get; }
        public bool Payed { set; get; }
        public bool BillIt { set; get; }


        public List<PhoneCall> GetPhoneCalls(List<string> columns,Dictionary<string,object> wherePart,bool allFields,int limits)
        {
            DataTable dt = new DataTable();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCalls.TableName), columns, wherePart, limits, allFields);
           
            foreach(DataRow row in dt.Rows)
            {
                PhoneCall phoneCall = new PhoneCall();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.BillIt))
                        phoneCall.BillIt = (bool)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Cost))
                        phoneCall.Cost = (double)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DateOfCall))
                        phoneCall.DateOfCall = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Dispute))
                        phoneCall.Dispute = (bool)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DstCountry))
                        phoneCall.DstCountry = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DstNumber))
                        phoneCall.DstNumber = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Duration))
                        phoneCall.Duration = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Gateway))
                        phoneCall.Gateway = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.IsPersonal))
                        phoneCall.IsPersonal = (bool)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ModifiedBy))
                        phoneCall.ModifiedBy = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ModifiedOn))
                        phoneCall.ModifiedOn = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Payed))
                        phoneCall.Payed = (bool)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.PhoneCallID))
                        phoneCall.PhoneCallID = (long)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Rate))
                        phoneCall.Rate = (double)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SipAccount))
                        phoneCall.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SrcNumber))
                        phoneCall.SrcNumber = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.TypeOfService))
                        phoneCall.TypeOfService = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UpdatedBy))
                        phoneCall.UpdatedBy = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UpdateOn))
                        phoneCall.UpdateOn = (DateTime)row[column.ColumnName];
                    
                }
                phoneCalls.Add(phoneCall);
            }
            return phoneCalls;
        }

        public bool UpdateCalls(List<PhoneCall> phoneCalls) 
        {
            bool status = false;

            DataTable dt = new DataTable();
            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

          
            foreach (PhoneCall phoneCall in phoneCalls)
            {
                //Where Part
                wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.PhoneCallID), phoneCall.PhoneCallID);

                //Set Part
                if (phoneCall.BillIt != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.BillIt), phoneCall.BillIt);

                if (phoneCall.Cost != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.Cost), phoneCall.Cost);

                if (phoneCall.DateOfCall != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.DateOfCall), phoneCall.DateOfCall);

                if (phoneCall.Dispute != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.Dispute), phoneCall.Dispute);

                if (phoneCall.DstCountry != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.DstCountry), phoneCall.DstCountry);

                if (phoneCall.DstNumber != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.DstNumber), phoneCall.DstNumber);

                if (phoneCall.Duration != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.Duration), phoneCall.Duration);

                if (phoneCall.Gateway != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.Gateway), phoneCall.Gateway);

                if (phoneCall.IsPersonal != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.IsPersonal), phoneCall.IsPersonal);

                if (phoneCall.ModifiedBy != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.ModifiedBy), phoneCall.ModifiedBy);

                if (phoneCall.ModifiedOn != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.ModifiedOn), phoneCall.ModifiedOn);

                if (phoneCall.Payed != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.Payed), phoneCall.Payed);

                if (phoneCall.Rate != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.Rate), phoneCall.Rate);

                if (phoneCall.SipAccount != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.SipAccount), phoneCall.SipAccount);

                if (phoneCall.SrcNumber != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.SrcNumber), phoneCall.SrcNumber);

                if (phoneCall.TypeOfService != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.TypeOfService), phoneCall.TypeOfService);

                if (phoneCall.UpdatedBy != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UpdatedBy), phoneCall.UpdatedBy);

                if (phoneCall.UpdateOn != null)
                    setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UpdateOn), phoneCall.UpdateOn);

                //Execute Update
                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.PhoneCalls.TableName), setPart, wherePart);

                if (status == false)
                {
                    //throw error message
                }
            }

            //throw success message
            return status;
        }
    }
}