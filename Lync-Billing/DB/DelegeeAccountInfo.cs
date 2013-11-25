using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class DelegeeAccountInfo
    {
        public int DelegeeTypeID { get; set; }

        public Site DelegeeSiteAccount { get; set; }

        public Department DelegeeDepartmentAccount { get; set; }

        public Users DelegeeUserAccount { get; set; }
        public string DelegeeUserPhonecallsPerPage { set; get; }
        public List<PhoneCall> DelegeeUserPhonecalls { get; set; }
        public Dictionary<string, PhoneBook> DelegeeUserAddressbook { set; get; }
    }
}