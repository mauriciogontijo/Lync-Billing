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
using Microsoft.Build.Tasks;
using Ext.Net;
using Lync_Billing.Backend;
using Lync_Billing.Libs;
using System.Linq.Expressions;
using Newtonsoft.Json;
using iTextSharp.text;


namespace Lync_Billing.ui.user
{
    public partial class history : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        
        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();
        private StoreReadDataEventArgs e = new StoreReadDataEventArgs();
        private string pageData = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/history.aspx";
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

            sipAccount = session.GetEffectiveSipAccount();
        }


        protected void PhoneCallStore_Load(object sender, EventArgs e)
        {
            var phoneCallsHistory = PhoneCall.GetPhoneCalls(sipAccount).Where(phoneCall => phoneCall.AC_InvoiceDate != DateTime.MinValue && (!string.IsNullOrEmpty(phoneCall.AC_IsInvoiced) && phoneCall.AC_IsInvoiced == "YES")).ToList();

            PhoneCallsHistoryGrid.GetStore().DataSource = phoneCallsHistory;
            PhoneCallsHistoryGrid.GetStore().DataBind();
        }
        
        
        [DirectMethod]
        protected void PhoneCallsHistoryFilter(object sender, DirectEventArgs e) 
        {
            List<PhoneCall> phoneCallsHistory = new List<PhoneCall>();

            if (FilterTypeComboBox.SelectedItem.Index > -1)
            {
                int filterType = Convert.ToInt32(FilterTypeComboBox.SelectedItem.Value);
                phoneCallsHistory = PhoneCall.GetPhoneCalls(sipAccount).Where(phoneCall => phoneCall.AC_InvoiceDate != DateTime.MinValue && (!string.IsNullOrEmpty(phoneCall.AC_IsInvoiced) && phoneCall.AC_IsInvoiced == "YES")).ToList();
                
                //Business filter
                if (filterType == 2)
                    phoneCallsHistory = phoneCallsHistory.Where(phonecall => phonecall.UI_CallType == "Business").ToList();
                //Personal filter
                else if(filterType == 3)
                    phoneCallsHistory = phoneCallsHistory.Where(phonecall => phonecall.UI_CallType == "Personal").ToList();
            }

            PhoneCallsHistoryGrid.GetStore().LoadData(phoneCallsHistory);
        }

    }

}