﻿using System;
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
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = "~/UI/user/manage_address_book.aspx";
                Response.Redirect("~/UI/session/login.aspx?redirect_to=" + redirect_to);
            }

            GridsDataManager(true);
        }

        protected void GridsDataManager(bool GetFreshData = false, bool BindData = true)
        {
            if(GetFreshData == true)
            {
                List<PhoneBook> TempHistoryData = new List<PhoneBook>();
                Dictionary<string, PhoneBook> TempAddressBookData = new Dictionary<string, PhoneBook>();
                string SipAccount = string.Empty;
                
                SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount.ToString();
                TempAddressBookData = PhoneBook.GetAddressBook(SipAccount);
                TempHistoryData = PhoneBook.GetDestinationNumbers(SipAccount);

                //Always clear the contents of the data containers
                AddressBookData.Clear();
                HistoryDestinationNumbers.Clear();

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
                TempAddressBookData.Clear();
            }

            if (BindData == true)
            {
                AddressBookStore.DataSource = AddressBookData;
                AddressBookStore.DataBind();

                ImportContactsStore.DataSource = HistoryDestinationNumbers;
                ImportContactsStore.DataBind();
            }
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
                GridsDataManager(true);

                AddressBookGrid.GetStore().Reload();
                ImportContactsGrid.GetStore().Reload();
            }
        }


        /*
         * Update edited contacts in Address Book
         */
        protected void AddressBookUpdateContacts(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            List<PhoneBook> all_address_book_items = new List<PhoneBook>();
            List<PhoneBook> filtered_address_book_items = new List<PhoneBook>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            all_address_book_items = serializer.Deserialize<List<PhoneBook>>(json);

            foreach (PhoneBook entry in all_address_book_items) {
                if ((entry.Name != null && entry.Type != null) && (entry.Name != "" && entry.Type != "")) {
                    if (entry.SipAccount == null || entry.SipAccount == string.Empty) {
                        entry.SipAccount = SipAccount;
                    }

                    filtered_address_book_items.Add(entry);
                }
            }

            if (filtered_address_book_items.Count > 0)
            {
                foreach(PhoneBook entry in filtered_address_book_items) {
                    PhoneBook.UpdatePhoneBookEntry(entry);
                }

                GridsDataManager(true);

                AddressBookGrid.GetStore().Reload();
                ImportContactsGrid.GetStore().Reload();
            }
        }


        /*
         * Delete selected contacts from Address Book
         */
        protected void AddressBookDeleteContacts(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            List<PhoneBook> to_be_deleted_entries = new List<PhoneBook>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            to_be_deleted_entries = serializer.Deserialize<List<PhoneBook>>(json);

            if (to_be_deleted_entries.Count > 0)
            {
                PhoneBook.DeleteFromPhoneBook(to_be_deleted_entries);
                GridsDataManager(true);

                AddressBookGrid.GetStore().Reload();
                ImportContactsGrid.GetStore().Reload();
            }
            AddressBookGrid.GetSelectionModel().DeselectAll();
        }


        /*
         * AddressBook Data Binding
         */
        protected void AddressBookStore_Load(object sender, EventArgs e)
        {
            GridsDataManager(false);
        }

        /*
         * ImportContacts Data Binding
         */
        protected void ImportContactsStore_Load(object sender, EventArgs e)
        {
            GridsDataManager(false);
        }
    }
}