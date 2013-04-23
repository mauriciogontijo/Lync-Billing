using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class PhoneCall
    {
        public int PhoneCallID { set; get; }
        public string SrcNumber { set; get; }
        public string DstNumber { set; get; }
        public string SipAccount { set; get; }
        public string DstCountry { set; get; }
        public string TypeOfService { set; get; }
        public string UpdatedBy { set; get; }
        public string ModifiedBy { set; get; }
        public string Gateway { set; get; }

        public double Duration { set; get; }
        public double rate { set; get; }
        public double Cost { set; get; }
        
        public DateTime UpdateOn{ set; get; }
        public DateTime ModifiedOn { set; get; }
        public DateTime CalledOn { set; get; }
        
        public bool IsPersonal { set; get; }
        public bool Dispute { set; get; }
        public bool Payed { set; get; }
        public bool BillIt { set; get; }

        public int INSERT()
        {
            return 0;
        }

        public List<PhoneCall> SELECT()
        {
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            return phoneCalls;
        }

        public void UPDATE()
        {

        }

        public void DELETE()
        {

        }
    }
}