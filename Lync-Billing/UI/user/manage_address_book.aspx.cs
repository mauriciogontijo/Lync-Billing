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
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }
        }

        protected void ImportContactsFromHistory(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = ImportContactsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneBook> all_address_book_items = new List<PhoneBook>();
            List<PhoneBook> filtered_address_book_items = new List<PhoneBook>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            all_address_book_items = serializer.Deserialize<List<PhoneBook>>(json);

            foreach (PhoneBook entry in all_address_book_items) {
                if((entry.Name != null && entry.Type != null) || (entry.Name != "" && entry.Type != "")) {
                    filtered_address_book_items.Add(entry);
                }
            }

            if (filtered_address_book_items.Count > 0) {

                ImportContactsGrid.GetStore().Find("DestinationNumber", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ImportContactsGrid.GetStore().Find("DestinationNumber", phoneCall.SessionIdTime.ToString()).Commit();
            }

            //ManagePhoneCallsGrid.GetStore().CommitChanges();
            ImportContactsGrid.GetSelectionModel().DeselectAll();
        }

        /*
         * AddressBook Data Binding
         */
        protected void AddressBookStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            AddressBookStore.DataSource = PhoneBook.GetAddressBook(SipAccount);
            AddressBookStore.DataBind();
        }

        /*
         * AddressBook Data Binding
         */
        protected void AddressBookStore_Load(object sender, EventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            AddressBookStore.DataSource = PhoneBook.GetAddressBook(SipAccount);
            AddressBookStore.DataBind();
        }

        /*
         * ImportContacts Data Binding
         */
        protected void ImportContactsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            ImportContactsStore.DataSource = PhoneBook.GetDestinationNumbers(SipAccount);
            ImportContactsStore.DataBind();
        }

        /*
         * ImportContacts Data Binding
         */
        protected void ImportContactsStore_Load(object sender, EventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            ImportContactsStore.DataSource = PhoneBook.GetDestinationNumbers(SipAccount);
            ImportContactsStore.DataBind();
        }
    }
}