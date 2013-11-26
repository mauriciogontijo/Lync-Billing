using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class DelegeeAccountInfo
    {
        public int DelegeeTypeID { get; set; }

        //Site Delegate Role related
        public Site DelegeeSiteAccount { get; set; }

        //Departent Delegate Role related
        public Department DelegeeDepartmentAccount { get; set; }

        //User Delegate Role related
        public Users DelegeeUserAccount { get; set; }
        public string DelegeeUserPhonecallsPerPage { set; get; }
        public List<PhoneCall> DelegeeUserPhonecalls { get; set; }
        public List<PhoneCall> DelegeeUserPhonecallsHistory { get; set; }
        public Dictionary<string, PhoneBook> DelegeeUserAddressbook { set; get; }
    }
}