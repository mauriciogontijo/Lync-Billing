using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Delegates
    {
        public string SipAccount { get; set; }
        public string DelegateAccount { get; set; }

        public string GetSipAccount(string delegateAccount) 
        {
            return null;
        }

        public string getDelegateAccount(string sipAccount) 
        {
            return null;
        }

        public bool UpadeDelegate(string sipAccount) 
        {
            return false;
        }

        public bool DeleteDelegate(string sipAccount) 
        {
            return false;
        }

        public bool AddDelegate(string sipAccount) 
        {
            return false;
        }
    }

    
}