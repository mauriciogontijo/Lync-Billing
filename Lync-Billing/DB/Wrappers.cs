using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lync_Billing.DB
{

    [XmlRoot("Document")]
    public class PhoneCallsWrapper 
    {
        [XmlArray("records"), XmlArrayItem(typeof(PhoneCall), ElementName = "record")]
        public List<PhoneCall> Records { get; set; }
    }
}