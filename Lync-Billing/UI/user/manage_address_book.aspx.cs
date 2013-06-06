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
        List<PhoneBook> AddressBookData = new List<PhoneBook>();
        List<PhoneBook> HistoryDestinationNumbers = new List<PhoneBook>();


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }

            BindDataToGrids(true);
        }

        protected void BindDataToGrids(bool GetFreshData = false)
        {
            if (GetFreshData == true)
            {
                List<PhoneBook> TempHistoryData = new List<PhoneBook>();
                Dictionary<string, PhoneBook> TempAddressBookData = new Dictionary<string, PhoneBook>();

                string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
                TempAddressBookData = PhoneBook.GetAddressBook(SipAccount);
                TempHistoryData = PhoneBook.GetDestinationNumbers(SipAccount);
                
                //Normalize the Address Book Data: Convert it from Dictionary to List.
                foreach (KeyValuePair<string, PhoneBook> entry in TempAddressBookData)
                {
                    AddressBookData.Add(entry.Value);
                }

                //Normalize the History: Remove AddressBooks entries.
                foreach (PhoneBook entry in TempHistoryData)
                {
                    if (!TempAddressBookData.ContainsKey(entry.DestinationNumber))
                    {
                        HistoryDestinationNumbers.Add(entry);
                    }
                }

                TempHistoryData.Clear();
            }

            AddressBookStore.DataSource = AddressBookData;
            ImportContactsStore.DataSource = HistoryDestinationNumbers;

            AddressBookStore.DataBind();
            ImportContactsStore.DataBind();
        }

        protected void ImportContactsFromHistory(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            List<PhoneBook> all_address_book_items = new List<PhoneBook>();
            List<PhoneBook> filtered_address_book_items = new List<PhoneBook>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            all_address_book_items = serializer.Deserialize<List<PhoneBook>>(json);

            foreach (PhoneBook entry in all_address_book_items)
            {
                if ((entry.Name != null && entry.Type != null) && (entry.Name != "" && entry.Type != ""))
                {
                    entry.SipAccount = SipAccount;
                    filtered_address_book_items.Add(entry);
                }
            }

            if (filtered_address_book_items.Count > 0)
            {
                PhoneBook.AddPhoneBookEntries(filtered_address_book_items);
                BindDataToGrids(true);
            }
        }

        protected void DeleteFromAddressBook(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            List<PhoneBook> to_be_deleted_entries = new List<PhoneBook>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            to_be_deleted_entries = serializer.Deserialize<List<PhoneBook>>(json);

            if (to_be_deleted_entries.Count > 0)
            {
                //Delete these items
                //...
                //...

                //Then, update the grid
                /*foreach (PhoneBook entry in to_be_deleted_entries)
                {
                    AddressBookGrid.GetStore().Find("DestinationNumber", entry.DestinationNumber.ToString()).Destroy();
                    AddressBookGrid.GetStore().Find("DestinationNumber", entry.DestinationNumber.ToString()).Commit();
                }*/
            }
            BindDataToGrids(true);
            AddressBookGrid.GetSelectionModel().DeselectAll();
        }

        /*
         * AddressBook Data Binding
         */
        protected void AddressBookStore_Load(object sender, EventArgs e)
        {
            BindDataToGrids();
        }

        /*
         * ImportContacts Data Binding
         */
        protected void ImportContactsStore_Load(object sender, EventArgs e)
        {
            BindDataToGrids();
        }
    }
}