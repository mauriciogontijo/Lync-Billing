using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Lync_Billing.DB;

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
            Store store = new Store()
            {
                ID = "PhonceCallsStore",
                DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, limits),
                Model = { PhoneCallModel(columns) }
            };
            return store;
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
                    new object[] { "Marked As Dispute", 4 },
                    new object[] { "Charged Calls", 4 }
                },
                Model = { PhoneCallMarkingModel() }
            };
            return store;
        }

      
    }
}