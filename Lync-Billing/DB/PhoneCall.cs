﻿using System;
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

        public DateTime SessionIdTime { set; get; }
        public int SessionIdSeq { get; set; }
        public DateTime ResponseTime { set; get; }
        public DateTime SessionEndTime { set; get; }
        public string SourceUserUri { set; get; }
        public string SourceNumberUri { set; get; }
        public string DestinationNumberUri { set; get; }
        public string FromMediationServer { set; get; }
        public string ToMediationServer { set; get; }
        public string FromGateway { set; get; }
        public string ToGateway { set; get; }
        public string SourceUserEdgeServer { set; get; }
        public string DestinationUserEdgeServer { set; get; }
        public string ServerFQDN { set; get; }
        public string PoolFQDN { set; get; }
        public string Marker_CallToCountry { set; get; }
        public string marker_CallType { set; get; }

        public int Duration { set; get; }
        public double Marker_CallCost { set; get; }
       
        //User UI update Fields
        public string UI_UpdatedByUser { set; get; }
        public DateTime UI_MarkedOn { set; get; }
        public bool UI_IsPersonal { set; get; }
        public bool UI_Dispute { set; get; }
        public bool UI_IsInvoiced { set; get; }



        public List<PhoneCall> GetPhoneCalls(List<string> columns,Dictionary<string,object> wherePart, int limits)
        {
            PhoneCall phoneCall;
            DataTable dt = new DataTable();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCalls.TableName), columns, wherePart, limits);
           
            foreach(DataRow row in dt.Rows)
            {
                phoneCall = new PhoneCall();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdTime))
                        phoneCall.SessionIdTime = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq))
                        phoneCall.SessionIdSeq = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ResponseTime))
                        phoneCall.ResponseTime = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionEndTime))
                        phoneCall.SessionEndTime = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserUri))
                        phoneCall.SourceUserUri = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri))
                        phoneCall.SourceNumberUri = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri))
                        phoneCall.DestinationNumberUri = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromMediationServer))
                        phoneCall.FromMediationServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToMediationServer))
                        phoneCall.ToMediationServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromGateway))
                        phoneCall.FromGateway = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToGateway))
                        phoneCall.ToGateway = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer))
                        phoneCall.SourceUserEdgeServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer))
                        phoneCall.DestinationUserEdgeServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ServerFQDN))
                        phoneCall.ServerFQDN = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.PoolFQDN))
                        phoneCall.PoolFQDN = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Duration))
                        phoneCall.Duration = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry))
                        phoneCall.Marker_CallToCountry = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost))
                        phoneCall.Marker_CallCost = (double)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn))
                        phoneCall.UI_MarkedOn = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser))
                        phoneCall.UI_UpdatedByUser = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_IsPersonal))
                        phoneCall.UI_IsPersonal = (bool)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_Dispute))
                        phoneCall.UI_Dispute = (bool)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_IsInvoiced))
                        phoneCall.UI_IsInvoiced = (bool)row[column.ColumnName];
                   
                    
                }
                phoneCalls.Add(phoneCall);
            }
            return phoneCalls;
        }

        public bool UpdateCall(PhoneCall phoneCall) 
        {
            bool status = false;

            DataTable dt = new DataTable();
            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            
            //Where Part
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime);
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq), phoneCall.SessionIdSeq);

            //Set Part
            if ((phoneCall.UI_MarkedOn).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn), phoneCall.UI_MarkedOn);

            if ((phoneCall.UI_UpdatedByUser).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser), phoneCall.UI_UpdatedByUser);

            if ((phoneCall.UI_IsPersonal).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_IsPersonal), phoneCall.UI_IsPersonal);

            if ((phoneCall.UI_Dispute).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_Dispute), phoneCall.UI_Dispute);

            if ((phoneCall.UI_IsInvoiced).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_IsInvoiced), phoneCall.UI_IsInvoiced);
               

            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.PhoneCalls.TableName), setPart, wherePart);

            if (status == false)
            {
                //throw error message
            }
           
            //throw success message
            return status;
        }
    }
}