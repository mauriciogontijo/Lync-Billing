using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Libs;
using System.Data;

namespace Lync_Backend.Helpers
{
    public class CallsTypes
    {
        public int id { get; set; }
        public string CallType { get; set; }

        private static DBLib DBRoutines = new DBLib();

        /// <summary>
        /// Get the list of all calls Types 
        /// </summary>
        /// <returns>List of CallTypes Objects</returns>
        public static List<CallsTypes> GetCallTypes() 
        {
            DataTable dt;

            List<CallsTypes> callsTypes = new List<CallsTypes>();

            CallsTypes callType;

            dt = DBRoutines.SELECT("CallTypes");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    callType = new CallsTypes();

                    callType.id = Convert.ToInt32(row["id"]);
                    callType.CallType = row["CallType"].ToString();

                    callsTypes.Add(callType);
                }
            }
            
            return callsTypes;
        }

    }
}
