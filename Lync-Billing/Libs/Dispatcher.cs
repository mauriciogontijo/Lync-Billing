using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using Lync_Billing.DB;

namespace Lync_Billing.Libs
{
    public class Dispatcher
    {
        public string URL { get; set; }
        public static string RedirectTo { get; set; }
        public static readonly Dictionary<string, Dictionary<string, string>> Pages = new Dictionary<string, Dictionary<string, string>>();

        public Dispatcher()
        {
            if (Pages.Count == 0)
            {
                Dictionary<string, string> session_pages = new Dictionary<string, string>();
                Dictionary<string, string> user_pages = new Dictionary<string, string>();
                Dictionary<string, string> accounting_pages = new Dictionary<string, string>();

                session_pages.Add("login", "~/UI/session/login.aspx");
                session_pages.Add("logout", "~/UI/session/logout.aspx");

                user_pages.Add("dashboard", "~/UI/user/dashboard.aspx");
                user_pages.Add("phonecalls", "~/UI/user/phonecalls.aspx");
                user_pages.Add("addressbook", "~/UI/user/addressbook.aspx");
                user_pages.Add("manage_delegates", "~/UI/user/manage_delegates.aspx");
                user_pages.Add("history", "~/UI/user/history.aspx");
                user_pages.Add("statistics", "~/UI/user/statistics.aspx");

                accounting_pages.Add("dashboard", "~/UI/accounting/dashboard.aspx");
                accounting_pages.Add("manage_disputes", "~/UI/accounting/manage_disputes.aspx");
                accounting_pages.Add("monthly_user_reports", "~/UI/accounting/monthly_user_reports.aspx");
                accounting_pages.Add("periodical_user_reports", "~/UI/accounting/periodical_user_reports.aspx");
                accounting_pages.Add("monthly_site_reports", "~/UI/accounting/monthly_site_reports.aspx");
                accounting_pages.Add("periodical_site_reports", "~/UI/accounting/periodical_site_reports.aspx");

                Pages.Add("session", session_pages);
                Pages.Add("user", user_pages);
                Pages.Add("accounting", accounting_pages);
            }
        }

        public string DispatchRequestedURL(UserSession session, string page_context, string page_name)
        {
            URL = string.Empty;
            RedirectTo = string.Empty;

            if (session != null && session.SipAccount != null && session.SipAccount != string.Empty)
            {
                if (Pages.Keys.Contains(page_context) && Pages[page_context].Keys.Contains(page_name))
                {
                    if (page_context == "accounting" && session.IsAccountant == false)
                    {
                        URL = Pages["user"]["dashboard"];
                    }
                    else if (page_context == "user" && page_name == "manage_delegates" && session.IsDelegate == false)
                    {
                        URL = Pages["user"]["dashboard"];
                    }
                    else
                    {
                        URL = Pages[page_context][page_name];
                    }
                }
                else
                {
                    URL = Pages["user"]["dashboard"];
                }
            }
            else
            {
                //the @part of the following IF STATEMENT prevents looping inside the Login page.
                //@part: page_context != "session"
                if (Pages.Keys.Contains(page_context) && Pages[page_context].Keys.Contains(page_name) && page_context != "session")
                {
                    RedirectTo = Pages[page_context][page_name];
                }
                URL = Pages["session"]["login"];
            }

            return URL;
        }
    }
}