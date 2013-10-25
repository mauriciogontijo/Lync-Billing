using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Libs;
using System.Data;

namespace Lync_Backend.Helpers
{
    public class DIDs
    {
        public int id { get; set; }
        public string did { get; set; }
        public string description { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<DIDs> GetDIDs() 
        {
            DataTable dt;
            List<DIDs> dids = new List<DIDs>();
            DIDs didInfo ;

            dt = DBRoutines.SELECT("DIDs");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    didInfo = new DIDs();

                    didInfo.id = Convert.ToInt32(row["id"]);
                    didInfo.did = row["did"].ToString();
                    didInfo.description = row["description"].ToString();

                    dids.Add(didInfo);
                }
            }
            return dids;
        } 

    }
}
