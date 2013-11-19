using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;

namespace Lync_Billing.ui.user
{
    public partial class addressbook : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        List<PhoneBook> AddressBookData = new List<PhoneBook>();
        List<PhoneBook> HistoryDestinationNumbers = new List<PhoneBook>();


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/addressbook.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            GridsDataManager(true);
            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
        }

        protected void GridsDataManager(bool GetFreshData = false, bool BindData = true)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = userSession.EffectiveSipAccount;

            if (GetFreshData == true)
            {
                List<PhoneBook> TempHistoryData = new List<PhoneBook>();
                Dictionary<string, PhoneBook> TempAddressBookData = new Dictionary<string, PhoneBook>();

                TempAddressBookData = PhoneBook.GetAddressBook(sipAccount);
                TempHistoryData = PhoneBook.GetDestinationNumbers(sipAccount);

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
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = userSession.EffectiveSipAccount;

            string json = e.ExtraParams["Values"];
            
            List<PhoneBook> all_address_book_items = new List<PhoneBook>();
            List<PhoneBook> filtered_address_book_items = new List<PhoneBook>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            all_address_book_items = serializer.Deserialize<List<PhoneBook>>(json);

            foreach (PhoneBook entry in all_address_book_items)
            {
                if (!string.IsNullOrEmpty(entry.Type) && (entry.Type == "Personal" || entry.Type == "Business"))
                {
                    entry.SipAccount = sipAccount;
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

            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).PhoneBook = PhoneBook.GetAddressBook(sipAccount);
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

        protected void UpdateAddressBook_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;
            string json = e.ExtraParams["Values"];

            List<PhoneBook> recordsToUpate = new List<PhoneBook>();
            List<PhoneBook> filteredItemsForUpdate = new List<PhoneBook>();
            ChangeRecords<PhoneBook> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<PhoneBook>();

            if (toBeUpdated.Updated.Count > 0)
            {
                foreach (PhoneBook entry in toBeUpdated.Updated)
                {
                    if (!string.IsNullOrEmpty(entry.Type) && (entry.Type == "Personal" || entry.Type == "Business"))
                    {
                        if (!string.IsNullOrEmpty(entry.SipAccount))
                        {
                            entry.SipAccount = sipAccount;
                        }

                        filteredItemsForUpdate.Add(entry);
                    }
                }

                if (filteredItemsForUpdate.Count > 0)
                {
                    foreach (PhoneBook entry in filteredItemsForUpdate) 
                    {
                        PhoneBook.UpdatePhoneBookEntry(entry);
                    }

                    GridsDataManager(true);

                    AddressBookGrid.GetStore().Reload();
                    ImportContactsGrid.GetStore().Reload();
                }
            }

            if (toBeUpdated.Deleted.Count > 0)
            {
                PhoneBook.DeleteFromPhoneBook(toBeUpdated.Deleted);
                GridsDataManager(true);

                AddressBookGrid.GetStore().Reload();
                ImportContactsGrid.GetStore().Reload();
            }

            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).PhoneBook = PhoneBook.GetAddressBook(sipAccount);
        }

        protected void RejectAddressBookChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            AddressBookGrid.GetStore().RejectChanges();
        }

        protected void RejectImportChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ImportContactsGrid.GetStore().RejectChanges();
        }
    }
}