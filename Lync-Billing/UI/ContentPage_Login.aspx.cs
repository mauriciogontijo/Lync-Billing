using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace Lync_Billing.UI
{
    public partial class ContentPage_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string machineName = System.Environment.MachineName;
            //IPHostEntry hostEntity = System.Net.Dns.GetHostEntry(machineName);
            //IPAddress[] ip = hostEntity.AddressList;
            //local_ip_address.Value = ip[1].ToString();
            local_ip_address.Value = Request.Headers.GetValues("Host")[0];
            string userAgent = Request.Headers.GetValues("User-Agent")[0];

        }
    }
}