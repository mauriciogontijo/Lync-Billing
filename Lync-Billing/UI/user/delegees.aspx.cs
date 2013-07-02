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
using Lync_Billing.DB;
using Lync_Billing.Libs;
using Newtonsoft.Json;

namespace Lync_Billing.ui.user
{
    public partial class delegees : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();
        List<UsersDelegates> delegates = new List<UsersDelegates>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/ui/session/login.aspx");
            }
            ////but if the user is actually logged in we only need to check if he is marked as a delegate user
            //else
            //{
            //    UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //    if ((session.IsDelegate == true || session.IsDeveloper == true) && (session.PrimarySipAccount == session.EffectiveSipAccount))
            //    {
            //        if ( ! string.IsNullOrEmpty(Request.QueryString["identity"]) )
            //        {
            //            //Switch to a delegee account
            //            if (session.ListOfDelegees.Keys.Contains(Request.QueryString["identity"]))
            //            {
            //                //Switch identity
            //                session.EffectiveSipAccount = Request.QueryString["identity"];
            //                session.EffectiveDisplayName = session.ListOfDelegees[session.EffectiveSipAccount];

            //                //Initialize the PhoneBook in the session
            //                session.PhoneBook = new Dictionary<string, PhoneBook>();
            //                session.PhoneBook = PhoneBook.GetAddressBook(session.EffectiveSipAccount);

            //                //Clear the PhoneCalls containers in the session
            //                session.PhoneCalls = new List<PhoneCall>();
            //                session.PhoneCallsPerPage = string.Empty;

            //                //Redirect to Uer Dashboard
            //                Response.Redirect("~/ui/user/dashboard.aspx");
            //            }
            //        }
            //    }
            //    //Swtich back to original user account
            //    else if ( ! string.IsNullOrEmpty(Request.QueryString["identity"]) && session.PrimarySipAccount == Request.QueryString["identity"] )
            //    {
            //        //Switch back to original identity
            //        session.EffectiveSipAccount = session.PrimarySipAccount;
            //        session.EffectiveDisplayName = session.PrimaryDisplayName;

            //        //Initialize the PhoneBook in the session
            //        session.PhoneBook = new Dictionary<string, PhoneBook>();
            //        session.PhoneBook = PhoneBook.GetAddressBook(session.EffectiveSipAccount);

            //        //Clear the PhoneCalls containers in the session
            //        session.PhoneCalls = new List<PhoneCall>();
            //        session.PhoneCallsPerPage = string.Empty;

            //        //Redirect to user dashboard
            //        Response.Redirect("~/ui/user/dashboard.aspx");
            //    }
            //    else
            //    {
            //        //We redirect the users to the User Dashboard page if they have requested the Manage Delegates page without being marked as delegates themselves
            //        Response.Redirect("~/ui/user/dashboard.aspx");
            //    }
            //}
        }
    }
}