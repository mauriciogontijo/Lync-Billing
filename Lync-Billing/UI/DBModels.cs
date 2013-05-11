using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;

namespace Lync_Billing.UI
{
    public class DBModels
    {

        public static Model PhoneCallModel()
        {
            return new Model()
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
    }
}