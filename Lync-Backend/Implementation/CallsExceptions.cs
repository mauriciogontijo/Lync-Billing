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

        public static bool ApplyMOAExceptions(ref PhoneCalls phoneCall,string siteName) 
        {
            
            if (siteName == "MOA" && !string.IsNullOrEmpty(phoneCall.DestinationNumberUri) && phoneCall.DestinationNumberUri.StartsWith("+30"))
            {
                //Check if call has been made from moa to greek land line
                if( phoneCall.DestinationNumberUri.StartsWith("+302") == true)
                {
                    phoneCall.Marker_CallCost = Convert.ToDecimal(0);
                    return true;
                }

                //Check if mobile from vodafone subscribers
                if (phoneCall.DestinationNumberUri.StartsWith("+306")) 
                {
                    OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

                    string sqlValidationQuery = string.Format("SELECT Number FROM PhoneCallsExceptions WHERE Number='{0}'", phoneCall.DestinationNumberUri);

                    OleDbCommand comm = new OleDbCommand(sqlValidationQuery, sourceDBConnector);

                    try
                    {
                        sourceDBConnector.Open();

                        object result = comm.ExecuteScalar();

                        if (result != null)
                        {
                            phoneCall.Marker_CallCost = Convert.ToDecimal(0);
                            return true;
                        }
                        
                    }
                    catch (Exception ex) { }

                    finally
                    {
                        sourceDBConnector.Close();
                    }

                }

            }
            return false;
        }

    }
}
