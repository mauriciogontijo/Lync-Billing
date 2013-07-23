using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;
using Ext.Net;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Xml.Serialization;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


namespace Lync_Billing.DB
{
    [XmlRoot("Document")]
    //[XmlInclude(typeof(PhoneCall))]
    public class PhoneCall
    {
        public PhoneCall() { }
        private static DBLib DBRoutines = new DBLib();

        public string SessionIdTime { set; get; }
        public int SessionIdSeq { get; set; }
        public string ResponseTime { set; get; }
        public string SessionEndTime { set; get; }
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

        public decimal Duration { set; get; }
        public decimal Marker_CallCost { set; get; }
       
        //User UI update Fields
        public string UI_UpdatedByUser { set; get; }
        public DateTime UI_MarkedOn { set; get; }
        public string UI_CallType { set; get; }
        public string AC_DisputeStatus { set; get; }
        public DateTime AC_DisputeResolvedOn { set; get; }
        public string AC_IsInvoiced { set; get; }
        public DateTime AC_InvoiceDate { set; get; }
        public string PhoneBookName { set; get; }

        public static List<PhoneCall> PhoneCalls = new List<PhoneCall>();
        
        public static List<PhoneCall> GetPhoneCalls(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            PhoneCall phoneCall;
            DataTable dt = new DataTable();

            
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCalls.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                phoneCall = new PhoneCall();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SessionIdTime = Convert.ToDateTime( row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SessionIdSeq = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ResponseTime) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ResponseTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionEndTime) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SessionEndTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SourceUserUri = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SourceNumberUri = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.DestinationNumberUri = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromMediationServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.FromMediationServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToMediationServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ToMediationServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromGateway) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.FromGateway = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToGateway) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ToGateway = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SourceUserEdgeServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.DestinationUserEdgeServer = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ServerFQDN) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ServerFQDN = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.PoolFQDN) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.PoolFQDN = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Duration) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.Duration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.Marker_CallToCountry = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.Marker_CallCost = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_MarkedOn = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_UpdatedByUser = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_CallType) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_CallType = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_DisputeStatus = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_DisputeResolvedOn) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_DisputeResolvedOn = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_IsInvoiced = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_InvoiceDate) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_InvoiceDate = (DateTime)row[column.ColumnName];
                    
                }
                
                phoneCalls.Add(phoneCall);
            }
            return phoneCalls;
        }

        public static XmlNode GetPhoneCallsXML(List<string> columns, Dictionary<string, object> wherePart, int limits) 
        {
            DataTable dt = new DataTable();
            XmlDocument xml = new XmlDocument();
            string xmldoc = string.Empty;
            StringBuilder sb = new StringBuilder();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCalls.TableName), columns, wherePart, limits);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            xmldoc = ds.GetXml();
            

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmldoc);
            
            return  doc.DocumentElement;
        }

        public static bool UpdatePhoneCall(PhoneCall phoneCall) 
        {
            bool status = false;

            DataTable dt = new DataTable();
            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            
            //Where Part
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime);
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq), phoneCall.SessionIdSeq);

            //Set Part
            if (phoneCall.UI_MarkedOn != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn), phoneCall.UI_MarkedOn);

            if (phoneCall.UI_UpdatedByUser != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser), phoneCall.UI_UpdatedByUser);

            if (phoneCall.UI_CallType != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_CallType), phoneCall.UI_CallType);

            if (phoneCall.AC_DisputeStatus != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus), phoneCall.AC_DisputeStatus);

            if (phoneCall.AC_DisputeResolvedOn != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_DisputeResolvedOn), phoneCall.AC_DisputeResolvedOn);

            if (phoneCall.AC_IsInvoiced != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced), phoneCall.AC_IsInvoiced);

            if (phoneCall.AC_InvoiceDate != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_InvoiceDate), phoneCall.AC_InvoiceDate);

            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.PhoneCalls.TableName), setPart, wherePart);

            if (status == false)
            {
                //throw error message
            }
           
            //throw success message
            return status;
        }

        
        public static List<PhoneCall> GetPhoneCallsFilter(List<string> columns, Dictionary<string, object> wherePart, int start, int limit, out int count)
        {
            List<PhoneCall> phoneCalls = GetPhoneCalls(columns, wherePart, 0);

            count = phoneCalls.Count();

            return phoneCalls.Skip(start).Take(limit).ToList();
            
        }

        public static void ExportPhoneCalls(List<string> columns, Dictionary<string, object> wherePart, int limits, HttpResponse response,out Document document) 
        {
            DataTable dt = new DataTable();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCalls.TableName), columns, wherePart, limits);

            document = PDFLib.CreatePDF(dt, response);
        }
        
        public string GetPhoneCallsXML(List<PhoneCall> phonecalls) 
        {
            PhoneCallsWrapper phonecallsWrapper = new PhoneCallsWrapper();

            phonecallsWrapper.Records = phonecalls;

            return Misc.SerializeObject<PhoneCallsWrapper>(phonecallsWrapper);
        }
    
    }
}