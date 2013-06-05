using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using Ext.Net;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using System.Xml;
using System.Xml.Xsl;

namespace Lync_Billing.UI.user
{
    public partial class manage_address_book : System.Web.UI.Page
    {
        Dictionary<string, PhoneBook> AddressBookData = new Dictionary<string, PhoneBook>();
        List<PhoneBook> HistoryDestinationNumbers = new List<PhoneBook>();


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }

            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            AddressBookData = PhoneBook.GetAddressBook(SipAccount);
            HistoryDestinationNumbers = PhoneBook.GetDestinationNumbers(SipAccount);
        }

        protected void ImportContactsFromHistory(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            List<PhoneBook> all_address_book_items = new List<PhoneBook>();
            List<PhoneBook> filtered_address_book_items = new List<PhoneBook>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            all_address_book_items = serializer.Deserialize<List<PhoneBook>>(json);

            foreach (PhoneBook entry in all_address_book_items) {
                if ((entry.Name != null && entry.Type != null) && (entry.Name != "" && entry.Type != "")) {
                    entry.SipAccount = SipAccount;
                    filtered_address_book_items.Add(entry);
                }
            }

            if (filtered_address_book_items.Count > 0) {
                PhoneBook.AddPhoneBookEntries(filtered_address_book_items);

                ImportContactsStore.DataSource = PhoneBook.GetDestinationNumbers(SipAccount);
                ImportContactsStore.DataBind();
            }
        }

        /*
         * AddressBook Data Binding
         */
        protected void AddressBookStore_Load(object sender, EventArgs e)
        {
            AddressBookStore.DataSource = AddressBookData;
            AddressBookStore.DataBind();
        }

        /*
         * ImportContacts Data Binding
         */
        protected void ImportContactsStore_Load(object sender, EventArgs e)
        {
            ImportContactsStore.DataSource = HistoryDestinationNumbers;
            ImportContactsStore.DataBind();
        }
    }
}