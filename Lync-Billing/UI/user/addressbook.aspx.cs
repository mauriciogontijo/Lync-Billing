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
using Lync_Billing.Backend;
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

            //Get the sip account of this user
            sipAccount = session.GetEffectiveSipAccount();

            //Get the data
            GridsDataManager(true);
        }
        
        
        private void UpdateSessionRelatedInformation(PhoneBook phoneBookObj = null)
        {
            List<PhoneCall> phoneCalls;
            Dictionary<string, PhoneBook> addressBook;

            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.GetEffectiveSipAccount();

            //Get user addressbook
            addressBook = PhoneBook.GetAddressBook(sipAccount);

            //Get userphonecalls
            phoneCalls = session.GetUserSessionPhoneCalls();

            if (phoneBookObj != null)
            {
                if (phoneCalls.Find(item => item.DestinationNumberUri == phoneBookObj.DestinationNumber) != null)
                {
                    //Update user phonecalls
                    foreach (var phoneCall in phoneCalls)
                    {
                        if (addressBook.ContainsKey(phoneCall.DestinationNumberUri))
                        {
                            phoneCall.PhoneBookName = ((PhoneBook)addressBook[phoneCall.DestinationNumberUri]).Name;
                        }
                        else
                        {
                            phoneCall.PhoneBookName = string.Empty;
                        }
                    }
                }
            }
            else
            {
                foreach (var phoneCall in phoneCalls)
                {
                    if (addressBook.ContainsKey(phoneCall.DestinationNumberUri))
                    {
                        phoneCall.PhoneBookName = ((PhoneBook)addressBook[phoneCall.DestinationNumberUri]).Name;
                    }
                    else
                    {
                        phoneCall.PhoneBookName = string.Empty;
                    }
                }
            }


            //Allocate the phonecalls and addressbook to the session
            //Handle normal user mode, and user delegee mode
            session.AssignSessionPhonecallsAndAddressbookData(phoneCalls, addressBook, null);
            
        }


        protected void GridsDataManager(bool GetFreshData = false, bool BindData = true)
        {
            sipAccount = session.GetEffectiveSipAccount();

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

        protected void RejectAddressBookChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            AddressBookGrid.GetStore().RejectChanges();
        }

        protected void RejectImportChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ImportContactsGrid.GetStore().RejectChanges();
        }

        protected void ImportContactsFromHistory(object sender, DirectEventArgs e)
        {
            sipAccount = session.GetEffectiveSipAccount();

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

                //Update the session's phonebook dictionary and phonecalls list.
                UpdateSessionRelatedInformation();
            }
        }

        protected void UpdateAddressBook_DirectEvent(object sender, DirectEventArgs e)
        {
            sipAccount = session.GetEffectiveSipAccount();

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

            //Update the session's phonebook dictionary and phonecalls list.
            UpdateSessionRelatedInformation();
        }


        protected void AddNewAddressBookContact_Click(object sender, DirectEventArgs e)
        {
            AddNewContactWindowPanel.Show();
        }


        protected void CancelNewContactButton_Click(object sender, DirectEventArgs e)
        {
            AddNewContactWindowPanel.Hide();
        }


        protected void AddNewContactWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewContact_ContactName.Text = null;
            NewContact_ContactNumber.Text = null;
            NewContact_ContactType.Select(0);
            NewContact_Country.Value = null;
        }

        protected void NewContact_CountryStore_Load(object sender, EventArgs e)
        {
            var countries = Country.GetAllCountries();
            NewContact_Country.GetStore().DataSource = countries;
            NewContact_Country.GetStore().LoadData(countries);
        }

        protected void AddNewContactButton_Click(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            PhoneBook NewContact;
            string statusMessage = string.Empty;

            if (!string.IsNullOrEmpty(NewContact_ContactNumber.Text) && NewContact_ContactType.SelectedItem.Index > -1)
            {
                NewContact = new PhoneBook();

                NewContact.DestinationNumber = NewContact_ContactNumber.Text;
                NewContact.SipAccount = sipAccount;
                NewContact.Type = Convert.ToString(NewContact_ContactType.SelectedItem.Value);
                NewContact.Name = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(NewContact_ContactName.Text));
                NewContact.DestinationCountry = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(NewContact_Country.SelectedItem.Value));

                if (session.Addressbook.ContainsKey(NewContact.DestinationNumber))
                {
                    statusMessage = "Cannot add duplicate contacts.";
                }
                else
                {
                    PhoneBook.AddPhoneBookEntry(NewContact);

                    GridsDataManager(true);

                    //Update the session's phonebook dictionary and phonecalls list.
                    UpdateSessionRelatedInformation(NewContact);

                    AddNewContactWindowPanel.Hide();
                }
            }
            else
            {
                statusMessage = "Please provide all the information.";
            }

            NewContact_StatusMessage.Text = statusMessage;
        }
    }

}