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
        public static Model PhoneCallModel(List<string> columns)
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

      
    }
}