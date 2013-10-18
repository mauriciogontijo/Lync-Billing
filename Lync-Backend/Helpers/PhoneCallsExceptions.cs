using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;
using Lync_Backend.Libs;


namespace Lync_Backend
{
    class PhoneCallsExceptions
    {
        public int ID { get; set; }
        public string UserUri { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }

        private static DBLib DBRoutines = new DBLib();


        /***
         * Return the exceptions (UserUris only)
         */
        public static List<string> GetUsersUris()
        {
            DataTable dt = new DataTable();
            string userUri;
            List<string> UserUris = new List<string>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCallsExceptions.TableName));

            foreach (DataRow row in dt.Rows)
            {
                userUri = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCallsExceptions.UserUri) && row[column.ColumnName] != System.DBNull.Value)
                        userUri = row[column.ColumnName].ToString();
                }

                if(!string.IsNullOrEmpty(userUri))
                    UserUris.Add(userUri);
            }

            return UserUris;
        }


        /***
         * Return the exceptions (UserNumbers only)
         */
        public static List<string> GetUsersNumbers()
        {
            DataTable dt = new DataTable();
            string number;
            List<string> UserNumbers = new List<string>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.PhoneCallsExceptions.TableName));

            foreach (DataRow row in dt.Rows)
            {
                number = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.PhoneCallsExceptions.Number) && row[column.ColumnName] != System.DBNull.Value)
                        number = row[column.ColumnName].ToString();
                }

                if (!string.IsNullOrEmpty(number))
                    UserNumbers.Add(number);
            }

            return UserNumbers;
        }


    }
}
