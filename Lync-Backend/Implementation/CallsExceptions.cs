using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;
using Lync_Backend.Libs;
using System.Data.OleDb;
using System.Configuration;

namespace Lync_Backend.Implementation
{
    public class CallsExceptions
    {
        private DBLib DBRoutines = new DBLib();

        public static void ApplyMOAExceptions(ref PhoneCalls phoneCall,string siteName,out bool status) 
        {
            status = false;

            if (siteName == "MOA")
            {
                //Check if call has been made from moa to greek land line
                if(phoneCall.DestinationNumberUri.StartsWith("+302"))
                {
                    phoneCall.Marker_CallCost = Convert.ToDecimal(0);
                    status = true;
                }

                OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString
                
                string sqlValidationQuery = string.Format("SELECT Number FROM PhoneCallsExceptions WHERE Number='{0}'", phoneCall.DestinationNumberUri);

                OleDbCommand comm = new OleDbCommand(sqlValidationQuery, sourceDBConnector);

                try
                {
                    sourceDBConnector.Open();

                    string result = comm.ExecuteScalar().ToString();

                    if (result == phoneCall.DestinationNumberUri) 
                    {
                        phoneCall.Marker_CallCost = Convert.ToDecimal(0);
                        status = true;
                    }

                }
                catch (Exception ex){ }
                
                finally
                {
                    sourceDBConnector.Close();
                }

            }
        }

    }
}
