using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class PhoneBook
    {
        public int ID { get; set; }
        public string SipAccount { get; set; }
        public string DestinationNumber { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public static Dictionary<string, PhoneBook> GetAddressBook(string sipAccount) 
        {
            Dictionary<string, PhoneBook> phoneBookEntries = new Dictionary<string, PhoneBook>();


            return phoneBookEntries;
        }

        public static void SetPhoneBook(string sipAccount, List<PhoneBook> phoneBookEntries) 
        {

        }
    }
}