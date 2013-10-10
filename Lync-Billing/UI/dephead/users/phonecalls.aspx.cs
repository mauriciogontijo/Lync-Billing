﻿using Ext.Net;
using Lync_Billing.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.ui.dephead.users
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart;
        private List<string> columns;
        private string sipAccount = string.Empty;
        private UserSession session;
        private List<Department> UserDepartments;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/dephead/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != "dephead")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=dephead");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            BindDepartmentsForThisUser();
        }


        private void BindDepartmentsForThisUser(bool alwaysFireSelect = false)
        {
            UserDepartments = DepartmentHead.GetDepartmentsForHead(sipAccount);

            //By default the filter combobox is not read only
            FilterDepartments.ReadOnly = false;

            if (UserDepartments.Count > 0)
            {
                //Handle the FireSelect event
                if (alwaysFireSelect == true)
                {
                    FilterDepartments.SetValueAndFireSelect(UserDepartments.First().DepartmentName);

                    //Handle the ReadOnly Property
                    if (UserDepartments.Count == 1)
                    {
                        FilterDepartments.ReadOnly = true;
                    }
                }

                //Bind all the Data and return to the view
                FilterDepartments.GetStore().DataSource = UserDepartments;
                FilterDepartments.GetStore().DataBind();
            }
            //in case there are no longer any departments for this user to monitor.
            else
            {
                FilterDepartments.Disabled = true;
            }
        }


        private List<Users> GetUsers(string departmentName)
        {
            //List<Users> users = new List<Users>();
            List<string> columns = new List<string>();
            Dictionary<string, object> whereClause = new Dictionary<string, object>() 
            { 
                { "AD_Department", departmentName } 
            };

            return Users.GetUsers(columns, whereClause, 0);
        }


        protected void GetUsersPerDepartment(object sender, DirectEventArgs e)
        {
            //Clear the list of user
            FilterUsersByDepartment.Clear();
            FilterUsersByDepartment.ReadOnly = false;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                List<Users> users = GetUsers(FilterDepartments.SelectedItem.Value);

                FilterUsersByDepartment.Disabled = false;
                FilterUsersByDepartment.GetStore().DataSource = users;
                FilterUsersByDepartment.GetStore().DataBind();

                if (users.Count == 1)
                {
                    FilterUsersByDepartment.SetValueAndFireSelect(users.First().SipAccount);
                    FilterUsersByDepartment.ReadOnly = true;
                }
                else
                {
                    FilterUsersByDepartment.ReadOnly = false;
                }
            }
        }


        protected void GetPhoneCallsForUser(object sender, DirectEventArgs e)
        {
            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                if(FilterUsersByDepartment.SelectedItem != null && !string.IsNullOrEmpty(FilterUsersByDepartment.SelectedItem.Value))
                {
                    List<PhoneCall> phoneCalls = GetUserPhoneCalls(FilterUsersByDepartment.SelectedItem.Value);

                    ViewPhoneCallsGrid.GetStore().DataSource = phoneCalls;
                    ViewPhoneCallsGrid.GetStore().DataBind();
                }
            }
        }

        private List<PhoneCall> GetUserPhoneCalls(string userSipAccount)
        {
            List<PhoneCall> ListOfPhoneCalls;
            wherePart = new Dictionary<string, object>();
            columns = new List<string>();

            wherePart.Add("SourceUserUri", userSipAccount);
            //wherePart.Add("ac_IsInvoiced", "NO");
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("Exclude", false);

            columns.Add("SourceUserUri");
            columns.Add("SessionIdTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_CallType");
            
            ListOfPhoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0)
                                .Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null)
                                .ToList();

            List<PhoneCall> businessCalls = ListOfPhoneCalls.Where(call => call.UI_CallType == "Business").ToList();

            List<PhoneCall> otherCalls = (from phonecall in ListOfPhoneCalls 
                              where phonecall.UI_CallType != "Business" 
                              select phonecall
                              ).Select<PhoneCall, PhoneCall>(e => new PhoneCall{
                                  SourceUserUri = e.SourceUserUri,
                                  SessionIdTime = e.SessionIdTime,
                                  Marker_CallToCountry = e.Marker_CallToCountry,
                                  DestinationNumberUri = "***************",
                                  Duration = e.Duration,
                                  Marker_CallCost = e.Marker_CallCost,
                                  UI_CallType = e.UI_CallType
                              }).ToList();

            return businessCalls.Concat(otherCalls).ToList<PhoneCall>();
        }

    }
}