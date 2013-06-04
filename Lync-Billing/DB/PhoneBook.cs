using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class PhoneBook
    {
        public int ID { get; set; }
        public string SipAccount { get; set; }
        public string DestinationNumber { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static Dictionary<string, PhoneBook> GetAddressBook(string sipAccount)
        {
            Dictionary<string, PhoneBook> phoneBookEntries = new Dictionary<string, PhoneBook>();
            DataTable dt = new DataTable();
            PhoneBook phoneBookEntry;

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneBook.TableName), "SipAccount", sipAccount);

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
                }

                phoneBookEntries.Add(phoneBookEntry.DestinationNumber,phoneBookEntry);
            }

            return phoneBookEntries;
        }

        public static void SetPhoneBook(string sipAccount, List<PhoneBook> phoneBookEntries) 
        {

        }
    }
}