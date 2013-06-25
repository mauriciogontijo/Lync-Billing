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
                /*
                 * We don't apply the logic of redirecting back to this page because this is an intermediary stage and user will be confused if s/he were  
                 * redirected to the login page and after that being redirected to a delegee account!
                 * so we just redirect them back to the login page and from there they will access their dashboard.
                 */
                
                // string redirect_to = @"~/ui/user/delegees.aspx";
                // string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                // Response.Redirect(url);

                Response.Redirect("~/ui/session/login.aspx");
            }
            //but if the user is actually logged in we only need to check if he is marked as a delegate user
            else
            {
                /*
                 * First get the session in a UserSession format.
                 * Then make the 3-level check:
                 * 1> Check access-roles and permissions, alongside the Primary and Effective SipAccounts in the session.
                 * 2> Check the existence of the "identity" variable in the Request handler (uri).
                 * 3> Check if the passed SipAccount through the "identity" variable actually belongs to the ListOfDelegees of the current user.
                 *
                 **/

                UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if ((session.IsDelegate == true || session.IsDeveloper == true) && (session.PrimarySipAccount == session.EffectiveSipAccount))
                {
                    if (Request.QueryString["identity"] != null && Request.QueryString["identity"] != string.Empty)
                    {
                        //Switch to a delegee account
                        if (session.ListOfDelegees.Contains(Request.QueryString["identity"]))
                        {
                            //Switch identity
                            session.EffectiveSipAccount = Request.QueryString["identity"];

                            //Initialize the PhoneBook in the session
                            session.PhoneBook.Clear();
                            session.PhoneBook = PhoneBook.GetAddressBook(session.EffectiveSipAccount);

                            //Clear the PhoneCalls containers in the session
                            session.PhoneCalls.Clear(); //or = null;
                            session.PhoneCallsPerPage = string.Empty; //or = null;

                            //Redirect to Uer Dashboard
                            Response.Redirect("~/ui/user/dashboard.aspx");
                        }
                    }
                }
                //Swtich back to original user account
                else if (!string.IsNullOrEmpty(Request.QueryString["identity"]) && session.PrimarySipAccount == Request.QueryString["identity"])
                {
                    //Switch back to original identity
                    session.EffectiveSipAccount = session.PrimarySipAccount;

                    //Initialize the PhoneBook in the session
                    session.PhoneBook.Clear();
                    session.PhoneBook = PhoneBook.GetAddressBook(session.EffectiveSipAccount);

                    //Clear the PhoneCalls containers in the session
                    session.PhoneCalls.Clear(); //or = null;
                    session.PhoneCallsPerPage = string.Empty; //or = null;

                    //Redirect to user dashboard
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
                else
                {
                    //We redirect the users to the User Dashboard page if they have requested the Manage Delegates page without being marked as delegates themselves
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }
        }
    }
}