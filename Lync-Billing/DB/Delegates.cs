using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Delegates
    {
        public string SipAccount { get; set; }
        public string DelegateAccount { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public string GetSipAccount(string delegateAccount) 
        {
            Dictionary<string,object> whereStatemnet = new Dictionary<string,object>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "DelegateAccount", delegateAccount);

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