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
using System.Configuration;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Lync_Billing.ConfigurationSections;


namespace Lync_Billing.Backend
{

    public class PhoneCall
    {
        public PhoneCall() { }

        private static DBLib DBRoutines = new DBLib();

        //The private value on which we define the custom getter and setter
        private decimal _Marker_CallCost;
        public decimal Marker_CallCost
        {
            set { this._Marker_CallCost = value; }
            get { return decimal.Round(this._Marker_CallCost, 2); }
        }

        public string SessionIdTime { set; get; }
        public int SessionIdSeq { get; set; }
        public string ResponseTime { set; get; }
        public string SessionEndTime { set; get; }
        public string SourceUserUri { set; get; }
        public string ChargingParty { set; get; }
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
        public decimal Duration { set; get; }
        public string Marker_CallToCountry { set; get; }
        public string Marker_CallType { set; get; }
       
        //Users UI update Fields
        public string UI_UpdatedByUser { set; get; }
        public DateTime UI_MarkedOn { set; get; }
        public string UI_CallType { set; get; }
        public string UI_AssignedByUser { set; get; }
        public string UI_AssignedToUser { set; get; }
        public DateTime UI_AssignedOn { set; get; }

        public string AC_DisputeStatus { set; get; }
        public DateTime AC_DisputeResolvedOn { set; get; }
        public string AC_IsInvoiced { set; get; }
        public DateTime AC_InvoiceDate { set; get; }

        public string PhoneBookName { set; get; }
        
        //This is used to tell where to find and update this PhoneCall
        public string PhoneCallTableName { get; set; }
                
        //This is a container of the Billable Call Types list
        public static List<int> BillableCallTypesList = ((BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection")).BillableTypesList;

        public static List<PhoneCall> PhoneCalls = new List<PhoneCall>();


        public static List<PhoneCall> GetPhoneCalls(string sipAccount, Dictionary<string, object> wherePart = null, int limits = 0) 
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_ChargeableCalls_ForUser);
            PhoneCallsComparer linqDistinctComparer = new PhoneCallsComparer();

            //Initialize function parameters and then query the database
            List<object> functionaParams = new List<object>() { sipAccount };

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionaParams, wherePart);

            var phoneCalls = (from rw in dt.AsEnumerable()
                              select new PhoneCall()
                              {
                                    SessionIdTime = HelperFunctions.ConvertDate(Convert.ToDateTime(rw[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)])),
                                    SessionIdSeq = Convert.ToInt32(rw[Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)]),
                                    ResponseTime = HelperFunctions.ConvertDate(Convert.ToDateTime(rw[Enums.GetDescription(Enums.PhoneCalls.ResponseTime)])),
                                   
                                    SessionEndTime = HelperFunctions.ConvertDate(Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.SessionEndTime)]))),
                                    
                                    SourceUserUri = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.SourceUserUri)])),
                                    ChargingParty = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.ChargingParty)])),
                                    SourceNumberUri = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri)])),
                                    DestinationNumberUri = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri)])),
                                    FromMediationServer = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.FromMediationServer)])),
                                    ToMediationServer = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.ToMediationServer)])),
                                    FromGateway = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.FromGateway)])),
                                    ToGateway = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.FromGateway)])),
                                    SourceUserEdgeServer = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer)])),
                                    DestinationUserEdgeServer = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer)])),
                                    ServerFQDN = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.ServerFQDN)])),
                                    PoolFQDN = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.PoolFQDN)])),
                                    Duration = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.Duration)])),
                                    
                                    Marker_CallToCountry = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry)])),
                                    Marker_CallType = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.Marker_CallType)])),
                                    Marker_CallCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost)])),
                                    
                                    UI_UpdatedByUser = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser)])),
                                    UI_MarkedOn = Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn)])),
                                    UI_CallType = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.UI_CallType)])),
                                    UI_AssignedByUser = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.UI_AssignedByUser)])),
                                    UI_AssignedToUser = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.UI_AssignedToUser)])),
                                    UI_AssignedOn = Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.UI_AssignedOn)])),
                                    
                                    AC_DisputeStatus = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw["AC_DisputeStatus"])),
                                    AC_DisputeResolvedOn = Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.AC_DisputeResolvedOn)])),
                                    AC_IsInvoiced = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced)])),
                                    AC_InvoiceDate = Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(rw[Enums.GetDescription(Enums.PhoneCalls.AC_InvoiceDate)])),
                                    
                                    PhoneCallTableName = Convert.ToString(rw[Enums.GetDescription(Enums.PhoneCalls.PhoneCallsTableName)])
                              }).ToList<PhoneCall>();

            phoneCalls = phoneCalls.Distinct<PhoneCall>(linqDistinctComparer).ToList();

            if (limits > 0)
                return phoneCalls.GetRange(0, limits);
            else
                return phoneCalls;
        }

        /***
         * GetPhoneCalls
         * This is used to return the list of phonecalls for a given user.
         * Used in the Disputed page.
         */
        [Obsolete]
        public static List<PhoneCall> GetPhoneCallsOld(string sipAccount, Dictionary<string, object> wherePart = null, int limits = 0)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_ChargeableCalls_ForUser);

            PhoneCall phoneCall;
            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            PhoneCallsComparer linqDistinctComparer = new PhoneCallsComparer();

            //Initialize function parameters and then query the database
            List<object> functionaParams = new List<object>() { sipAccount };
            
            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionaParams, wherePart);


            foreach (DataRow row in dt.Rows)
            {
                phoneCall = new PhoneCall();
               
                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SessionIdTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SessionIdSeq = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ResponseTime) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ResponseTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionEndTime) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SessionEndTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SourceUserUri = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SourceNumberUri = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.DestinationNumberUri = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromMediationServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.FromMediationServer = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToMediationServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ToMediationServer = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromGateway) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.FromGateway = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToGateway) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ToGateway = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.SourceUserEdgeServer = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.DestinationUserEdgeServer = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ServerFQDN) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ServerFQDN = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.PoolFQDN) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.PoolFQDN = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Duration) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.Duration = Convert.ToDecimal(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.Marker_CallToCountry = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall._Marker_CallCost = Convert.ToDecimal(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ChargingParty) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.ChargingParty = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_MarkedOn = Convert.ToDateTime(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_UpdatedByUser = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_AssignedByUser) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_AssignedByUser = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_AssignedToUser) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_AssignedToUser = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_AssignedOn) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_AssignedOn = Convert.ToDateTime(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_CallType) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.UI_CallType = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_DisputeStatus = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_DisputeResolvedOn) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_DisputeResolvedOn = Convert.ToDateTime(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_IsInvoiced = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_InvoiceDate) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.AC_InvoiceDate = Convert.ToDateTime(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.PhoneCallsTableName) && row[column.ColumnName] != System.DBNull.Value)
                        phoneCall.PhoneCallTableName = Convert.ToString(row[column.ColumnName]);
                }

                phoneCalls.Add(phoneCall);
            }

            //Return a list of unique phonecalls.
            phoneCalls = phoneCalls.Distinct<PhoneCall>(linqDistinctComparer).ToList();

            if (limits > 0)
                return phoneCalls.GetRange(0, limits);
            else
                return phoneCalls;
        }


        /***
         * GetDisputedPhoneCalls
         * This is used to return the list of disputed calls per user.
         * Used in the Disputed page.
         */
        public static List<PhoneCall> GetDisputedPhoneCalls(List<string> sipAccountsList, Dictionary<string, object> wherePart, int limits)
        {
            List<PhoneCall> temporaryPhoneCallsStorage;
            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            PhoneCallsComparer linqDistinctComparer = new PhoneCallsComparer();

            if(!wherePart.Keys.Contains(Enums.GetDescription(Enums.PhoneCalls.UI_CallType)))
                wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_CallType), "Disputed");

            foreach (string sipAccount in sipAccountsList)
            {
                temporaryPhoneCallsStorage = GetPhoneCalls(sipAccount, wherePart, limits);
                
                phoneCalls.AddRange(
                    (from call in temporaryPhoneCallsStorage where !phoneCalls.Contains(call, linqDistinctComparer) select call).ToList<PhoneCall>()
                );
            }
            
            if (limits > 0)
                return phoneCalls.GetRange(0, limits);
            else
                return phoneCalls;
        }


        /***
         * GetPhoneCalls
         * This is an overload method for another GetPhoneCalls function, this returns a list of phonecalls from a specified table.
         */
        public static List<PhoneCall> GetPhoneCalls(string tableName, List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            PhoneCall phoneCall;
            DataTable dt = new DataTable();
            PhoneCallsComparer linqDistinctComparer = new PhoneCallsComparer();

            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            //For each phonecalls table get the phonecalls from it
            if(!string.IsNullOrEmpty(tableName))
            {
                dt = DBRoutines.SELECT(tableName, columns, wherePart, limits);

                foreach (DataRow row in dt.Rows)
                {
                    phoneCall = new PhoneCall();
                    phoneCall.PhoneCallTableName = tableName;

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.SessionIdTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.SessionIdSeq = (int)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ResponseTime) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.ResponseTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SessionEndTime) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.SessionEndTime = Convert.ToDateTime(row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserUri) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.SourceUserUri = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.SourceNumberUri = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.DestinationNumberUri = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromMediationServer) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.FromMediationServer = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToMediationServer) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.ToMediationServer = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.FromGateway) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.FromGateway = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ToGateway) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.ToGateway = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.SourceUserEdgeServer = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.DestinationUserEdgeServer = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ServerFQDN) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.ServerFQDN = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.PoolFQDN) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.PoolFQDN = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Duration) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.Duration = (decimal)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.Marker_CallToCountry = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall._Marker_CallCost = (decimal)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.ChargingParty) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.ChargingParty = Convert.ToString(row[column.ColumnName]);

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.UI_MarkedOn = (DateTime)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.UI_UpdatedByUser = Convert.ToString(row[column.ColumnName]);

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_CallType) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.UI_CallType = Convert.ToString(row[column.ColumnName]);

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_AssignedByUser) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.UI_AssignedByUser = Convert.ToString(row[column.ColumnName]);

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.UI_AssignedOn) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.UI_AssignedOn = Convert.ToDateTime(row[column.ColumnName]);

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.AC_DisputeStatus = (string)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_DisputeResolvedOn) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.AC_DisputeResolvedOn = (DateTime)row[column.ColumnName];

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.AC_IsInvoiced = Convert.ToString(row[column.ColumnName]);

                        else if (column.ColumnName == Enums.GetDescription(Enums.PhoneCalls.AC_InvoiceDate) && row[column.ColumnName] != System.DBNull.Value)
                            phoneCall.AC_InvoiceDate = (DateTime)row[column.ColumnName];

                    }

                    phoneCalls.Add(phoneCall);
                }
            }

            //Return a list of unique phonecalls.
            return phoneCalls.Distinct<PhoneCall>(linqDistinctComparer).ToList();
        }


        /***
         * UpdatePhoneCall
         * Updates a phone call record in the database, given a phonecall object.
         */
        public static bool UpdatePhoneCall(PhoneCall phoneCall, bool FORCE_RESET_UI_CallType = false) 
        {
            bool status = false;

            DataTable dt = new DataTable();
            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            
            //Where Part
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime);
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq), phoneCall.SessionIdSeq);

            //Set Part
            if(!string.IsNullOrEmpty(phoneCall.ChargingParty))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.ChargingParty), phoneCall.ChargingParty);

            if (phoneCall.UI_MarkedOn != DateTime.MinValue)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_MarkedOn), phoneCall.UI_MarkedOn);

            if (!string.IsNullOrEmpty(phoneCall.UI_UpdatedByUser))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_UpdatedByUser), phoneCall.UI_UpdatedByUser);

            if (!string.IsNullOrEmpty(phoneCall.UI_CallType))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_CallType), phoneCall.UI_CallType);

            if (!string.IsNullOrEmpty(phoneCall.UI_AssignedByUser))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_AssignedByUser), phoneCall.UI_AssignedByUser);

            if (!string.IsNullOrEmpty(phoneCall.UI_AssignedToUser))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_AssignedToUser), phoneCall.UI_AssignedToUser);

            if (phoneCall.UI_AssignedOn != DateTime.MinValue)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_AssignedOn), phoneCall.UI_AssignedOn);

            if (!string.IsNullOrEmpty(phoneCall.AC_DisputeStatus))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_DisputeStatus), phoneCall.AC_DisputeStatus);

            if (phoneCall.AC_DisputeResolvedOn != DateTime.MinValue)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_DisputeResolvedOn), phoneCall.AC_DisputeResolvedOn);

            if (!string.IsNullOrEmpty(phoneCall.AC_IsInvoiced))
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_IsInvoiced), phoneCall.AC_IsInvoiced);

            if (phoneCall.AC_InvoiceDate != DateTime.MinValue)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.AC_InvoiceDate), phoneCall.AC_InvoiceDate);

            if (FORCE_RESET_UI_CallType == true)
                setPart.Add(Enums.GetDescription(Enums.PhoneCalls.UI_CallType), null);

            //Execute Update
            status = DBRoutines.UPDATE(phoneCall.PhoneCallTableName, setPart, wherePart);

            //throw success message
            return status;
        }


        //public static void ExportUserPhoneCalls(string sipAccount, HttpResponse response, out Document document, Dictionary<string, string> headers)
        //{
        //    //THE PDF REPORT PROPERTIES
        //    PDFReportsPropertiesSection section = ((PDFReportsPropertiesSection)ConfigurationManager.GetSection(PDFReportsPropertiesSection.ConfigurationSectionName));
        //    PDFReportPropertiesElement pdfReportProperties = section.GetReportProperties("UserPhoneCalls");

        //    DataTable dt = new DataTable();
        //    Dictionary<string, object> wherePart = new Dictionary<string, object>();
        //    List<string> columns = new List<string>();

        //    int[] pdfColumnWidths = { };
        //    Dictionary<string, object> totals;


        //    //Initialize all the database-related data collections
        //    wherePart.Add("SourceUserUri", sipAccount);
        //    wherePart.Add("marker_CallTypeID", 1);
        //    wherePart.Add("Exclude", false);

        //    if (pdfReportProperties != null)
        //    {
        //        columns = pdfReportProperties.ColumnsNames();
        //        pdfColumnWidths = pdfReportProperties.ColumnsWidths();
        //    }


        //    //Get the user phonecalls from the database
        //    dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Phonecalls.TableName), columns, wherePart, 0);


        //    //Try to compute totals, if an error occurs which is the case of an empty "dt", set the totals dictionary to zeros
        //    try
        //    {

        //        totals = new Dictionary<string, object>()
        //        {
        //            {"Duration", HelperFunctions.ConvertSecondsToReadable(Convert.ToInt32(dt.Compute("Sum(Duration)", "Duration > 0 and ui_CallType='Personal'")))},
        //            {"_Marker_CallCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(_Marker_CallCost)", "_Marker_CallCost > 0 and ui_CallType='Personal'")), 2)},
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        totals = new Dictionary<string, object>()
        //        {
        //            {"Duration", 0},
        //            {"_Marker_CallCost", 0.00},
        //        };
        //    }

        //    document = PDFLib.InitializePDFDocument(response);
        //    PdfPTable pdfContentsTable = PDFLib.InitializePDFTable(dt.Columns.Count, pdfColumnWidths);
        //    PDFLib.AddPDFHeader(ref document, headers);
        //    PDFLib.AddPDFTableContents(ref document, ref pdfContentsTable, dt);
        //    PDFLib.AddPDFTableTotalsRow(ref document, totals, dt, pdfColumnWidths);
        //    PDFLib.ClosePDFDocument(ref document);
        //}
    
    }


    //The phonecalls version of the IEqualityComparer, used with LINQ's Distinct function
    class PhoneCallsComparer : IEqualityComparer<PhoneCall>
    {
        public bool Equals(PhoneCall firstCall, PhoneCall secondCall)
        {
            return (
                firstCall.SourceUserUri == secondCall.SourceUserUri &&
                firstCall.SessionIdTime == secondCall.SessionIdTime && 
                firstCall.SessionIdSeq == secondCall.SessionIdSeq
            );
        }

        public int GetHashCode(PhoneCall call)
        {
            string hashcode = call.SourceUserUri.ToString() + call.SessionIdTime + call.SessionIdSeq;
            return hashcode.GetHashCode();
        }
    }
}