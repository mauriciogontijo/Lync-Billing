using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Lync_Billing.DB;
using System.Web.Script.Serialization;

namespace Lync_Billing.UI
{
    public class DBStores
    {
        private static Model PhoneCallModel(List<string> columns)
        {
            Model model;

            if (columns != null)
            {
                model = new Model()
                {
                    ID = "phoneCallModel",
                    IDProperty = "SessionIdTime",
                };

                foreach (string column in columns)
                {
                    model.Fields.Add(new ModelField(column));
                }
            }
            else
            {
                model = new Model()
                {
                    ID = "phoneCallModel",
                    IDProperty = "SessionIdTime",
                    Fields = 
                    {
                        new ModelField("SessionIdTime"),
                        new ModelField("SessionIdSeq"),
                        new ModelField("ResponseTime"),
                        new ModelField("SessionEndTime"),
                        new ModelField("SourceUserUri"),
                        new ModelField("SourceNumberUri"),
                        new ModelField("DestinationNumberUri"),
                        new ModelField("ServerFQDN"),
                        new ModelField("PoolFQDN"),
                        new ModelField("Marker_CallToCountry"),
                        new ModelField("marker_CallType"),
                        new ModelField("Duration"),
                        new ModelField("Marker_CallCost"),
                        new ModelField("UI_UpdatedByUser"),
                        new ModelField("UI_MarkedOn"),
                        new ModelField("UI_IsPersonal"),
                        new ModelField("UI_Dispute"),
                        new ModelField("UI_IsInvoiced")
                    }
                };
            }
            return model;
        }

        private static Model PhoneCallMarkingModel() 
        {
            Model model;

            model = new Model()
            {
                ID = "PhoneCallMarkingModel",
                IDProperty = "SessionIdTime",
                Fields = 
                    {
                        new ModelField("MarkingStatus"),
                        new ModelField("MarkingValue")
                    }
            };

            return model;
        }

        public static Store PhoneCallsStore(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            Model model = PhoneCallModel(columns);
            Store store = new Store()
            {
                ID = "PhonceCallsStore",
                DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, limits),
                Model = { model },
                IsPagingStore = true,
                PageSize = 25
            };
            store.AfterRecordInserted += AfterPhoneCallRecordInserted;
            store.AfterRecordUpdated  += AfterPhoneCallRecordUpdated;
            store.AfterRecordDeleted  += AfterPhoneCallRecordDeleted;
            store.AfterStoreChanged   += store_AfterStoreChanged;
            store.BeforeDirectEvent   += store_BeforeDirectEvent;
            store.BeforeRecordUpdated +=store_BeforeRecordUpdated;

            return store;
        }

        private static void store_BeforeRecordUpdated(object sender, BeforeRecordUpdatedEventArgs e)
        {
            PhoneCall phonecall = e.Object<PhoneCall>();
        }

        private static void store_BeforeDirectEvent(object sender, BeforeDirectEventArgs e)
        {
            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            phoneCalls = serializer.Deserialize<List<PhoneCall>>(e.Data.JsonData);
        }
        private static void store_AfterStoreChanged(object sender, AfterStoreChangedEventArgs e)
        {
            if (e.Action == StoreAction.Update) 
            {
                ResponseRecordsList recordlist = e.ResponseRecords;
            }
         
        }

        private static void AfterPhoneCallRecordDeleted(object sender, AfterRecordDeletedEventArgs e)
        {
            PhoneCall phonecall = e.Object<PhoneCall>();
         
        }

        private static void AfterPhoneCallRecordUpdated(object sender, AfterRecordUpdatedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void AfterPhoneCallRecordInserted(object sender, AfterRecordInsertedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static Store PhoneCallMarkingStore() 
        {
            Store store = new Store()
            {
                ID = "PhoneCallMarkingStore",
                DataSource = new Object[]
                {
                    new object[] { "Unmarked" , 1 },
                    new object[] { "Marked" , 2 },
                    new object[] { "Marked As Business", 3 },
                    new object[] { "Marked As Personal", 4 },
                    new object[] { "Marked As Dispute", 5 },
                    new object[] { "Charged Calls", 6 }
                },
                Model = { PhoneCallMarkingModel() }
            };
            return store;
        }
    }
}