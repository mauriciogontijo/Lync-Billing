using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.Backend
{
    public class PhoneBook
    {
        public int ID { get; set; }
        public string SipAccount { get; set; }
        public string DestinationNumber { get; set; }
        public string DestinationCountry { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static Dictionary<string, PhoneBook> GetAddressBook(string sipAccount)
        {
            Dictionary<string, PhoneBook> phoneBookEntries = new Dictionary<string, PhoneBook>();
            DataTable dt = new DataTable();
            PhoneBook phoneBookEntry;

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneBook.TableName), Enums.GetDescription(Enums.PhoneBook.SipAccount), sipAccount);

            foreach (DataRow row in dt.Rows)
            {
                phoneBookEntry = new PhoneBook();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneBook.ID) && row[column.ColumnName] != System.DBNull.Value)
                        phoneBookEntry.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneBook.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        phoneBookEntry.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneBook.DestinationNumber) && row[column.ColumnName] != System.DBNull.Value)
                        phoneBookEntry.DestinationNumber = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneBook.Type) && row[column.ColumnName] != System.DBNull.Value)
                        phoneBookEntry.Type = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneBook.Name) && row[column.ColumnName] != System.DBNull.Value)
                        phoneBookEntry.Name = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneBook.DestinationCountry) && row[column.ColumnName] != System.DBNull.Value)
                        phoneBookEntry.DestinationCountry = (string)row[column.ColumnName];
                }

                if(!phoneBookEntries.ContainsKey(phoneBookEntry.DestinationNumber)) { 
                    phoneBookEntries.Add(phoneBookEntry.DestinationNumber, phoneBookEntry); 
                }
            }

            return phoneBookEntries;
        }


        public static List<PhoneBook> GetDestinationNumbers(string sipAccount)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_DestinationsNumbers_ForUser);

            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            List<object> functionparameters = new List<object>();

            PhoneBook phoneBookEntry;
            List<PhoneBook> phoneBookEntries = new List<PhoneBook>();
            PhoneBookContactComparer linqDistinctComparer = new PhoneBookContactComparer();
            DateTime fromDate, toDate;

            //Get the top 200 Destination in the last 6 years
            int contactNumbersLimit = 200;
            fromDate = new DateTime(DateTime.Now.Year - 5, 1, 1);
            toDate = fromDate.AddYears(6).AddDays(-1);

            //Initialize function parameters and then query the database
            functionparameters.Add(sipAccount);
            functionparameters.Add(fromDate);
            functionparameters.Add(toDate);
            functionparameters.Add(contactNumbersLimit);

            //Get all the destinations for this user where the phonecall is of a billable CallTypeID
            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionparameters, wherePart);


            foreach (DataRow row in dt.Rows)
            {
                phoneBookEntry = new PhoneBook();

                columnName = Enums.GetDescription(Enums.TopDestinationNumbers.PhoneNumber);
                phoneBookEntry.DestinationNumber = (string)HelperFunctions.ReturnEmptyIfNull(row[columnName]);

                columnName = Enums.GetDescription(Enums.TopDestinationNumbers.Country);
                phoneBookEntry.DestinationCountry = (string)HelperFunctions.ReturnEmptyIfNull(row[columnName]);

                phoneBookEntries.Add(phoneBookEntry);
            }

            //Filter only the distinct values.
            phoneBookEntries = phoneBookEntries.Distinct(linqDistinctComparer).ToList();
            return phoneBookEntries;
        }


        public static void AddPhoneBookEntries(List<PhoneBook> phoneBookEntries)
        {
            Dictionary<string, object> ColumnValues;

            foreach (PhoneBook phoneBookEntry in phoneBookEntries)
            {
                ColumnValues = new Dictionary<string, object>();
                //Set Part
                if (phoneBookEntry.SipAccount != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.SipAccount), phoneBookEntry.SipAccount);

                if (phoneBookEntry.DestinationNumber != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.DestinationNumber), phoneBookEntry.DestinationNumber);

                if (phoneBookEntry.Type != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.Type), phoneBookEntry.Type);

                if (phoneBookEntry.Name != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.Name), phoneBookEntry.Name);

                if (phoneBookEntry.DestinationCountry != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.DestinationCountry), phoneBookEntry.DestinationCountry);

                DBRoutines.INSERT(Enums.GetDescription(Enums.PhoneBook.TableName), ColumnValues, Enums.GetDescription(Enums.PhoneBook.ID));
            }
        }
        
        
        public static void UpdatePhoneBookEntry(PhoneBook phoneBookEntry) 
        {
            DataTable dt = new DataTable();
            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            //Where Part
            wherePart.Add(Enums.GetDescription(Enums.PhoneBook.SipAccount), phoneBookEntry.SipAccount);
            wherePart.Add(Enums.GetDescription(Enums.PhoneBook.DestinationNumber), phoneBookEntry.DestinationNumber);

            //Set Part
            if (phoneBookEntry.Type != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneBook.Type), phoneBookEntry.Type);
           
            if (phoneBookEntry.Name != null)
                setPart.Add(Enums.GetDescription(Enums.PhoneBook.Name), phoneBookEntry.Name);

            DBRoutines.UPDATE(Enums.GetDescription(Enums.PhoneBook.TableName), setPart, wherePart);
        }


        public static void AddOrUpdatePhoneBookEntries(string sipAccount, List<PhoneBook> phoneBookEntries)
        {
            Dictionary<string, PhoneBook> existingContacts = PhoneBook.GetAddressBook(sipAccount);
            
            Dictionary<string, object> ColumnValues;
            Dictionary<string, object> WherePart;

            foreach (PhoneBook phoneBookEntry in phoneBookEntries)
            {
                //Initialize the WherePart which is used for the update statement
                WherePart = new Dictionary<string,object>
                {
                    { Enums.GetDescription(Enums.PhoneBook.Name), phoneBookEntry.Name },
                    { Enums.GetDescription(Enums.PhoneBook.Type), phoneBookEntry.Type },
                };


                //Initialize the ColumnValues which is used in both the Insert and Update
                ColumnValues = new Dictionary<string, object>();

                if (phoneBookEntry.SipAccount != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.SipAccount), phoneBookEntry.SipAccount);

                if (phoneBookEntry.DestinationNumber != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.DestinationNumber), phoneBookEntry.DestinationNumber);

                if (phoneBookEntry.Type != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.Type), phoneBookEntry.Type);

                if (phoneBookEntry.Name != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.Name), phoneBookEntry.Name);

                if (phoneBookEntry.DestinationCountry != null)
                    ColumnValues.Add(Enums.GetDescription(Enums.PhoneBook.DestinationCountry), phoneBookEntry.DestinationCountry);


                //Either update or insert to the database
                if(existingContacts.ContainsKey(phoneBookEntry.DestinationNumber))
                    DBRoutines.UPDATE(Enums.GetDescription(Enums.PhoneBook.TableName), ColumnValues, WherePart);
                else
                    DBRoutines.INSERT(Enums.GetDescription(Enums.PhoneBook.TableName), ColumnValues, Enums.GetDescription(Enums.PhoneBook.ID));
            }
        }


        public static void DeleteFromPhoneBook(List<PhoneBook> phoneBookEntries) 
        {
            foreach (PhoneBook phoneBookEntry in phoneBookEntries) 
            {
                DBRoutines.DELETE(Enums.GetDescription(Enums.PhoneBook.TableName),"ID", phoneBookEntry.ID);
            }
        }

    }


    //This is used with LINQ Distinct method to compare if two contacts are the same before adding them to the "List of Contacts from Calls History"
    class PhoneBookContactComparer : IEqualityComparer<PhoneBook>
    {
        public bool Equals(PhoneBook firstContact, PhoneBook secondContact)
        {
            return (firstContact.DestinationNumber == secondContact.DestinationNumber && firstContact.DestinationCountry == secondContact.DestinationCountry);
        }

        public int GetHashCode(PhoneBook contact)
        {
            return (contact.DestinationNumber.ToString() + contact.DestinationCountry.ToString()).GetHashCode();
        }
    }
}