using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;

namespace Lync_Billing.Backend.Roles
{
    public class DelegateRole
    {
        private static DBLib DBRoutines = new DBLib();
        
        public int ID { set; get; }
        public int DelegeeType { get; set; }
        public int SiteID { get; set; }
        public int DepartmentID { get; set; }
        public string SipAccount { get; set; }
        public string DelegeeSipAccount { get; set; }
        public string Description { get; set; }

        //These are logical representation of data, they don't belong to the table
        public string DelegeeDisplayName { get; set; }
        public User DelegeeUser { get; set; }
        public Site DelegeeSite { get; set; }
        public Department DelegeeDepartment { get; set; }

        //The following are also logical representation of data, they are used to quickly lookup names from the Ext.NET views
        public string DelegeeDepartmentName { get; set; }
        public string DelegeeSiteName { get; set; }

        //These are for lookup use only in the application
        public static int UserDelegeeTypeID { get { return Convert.ToInt32(Enums.GetDescription(Enums.DelegateTypes.UserDelegeeType)); } }
        public static int DepartmentDelegeeTypeID { get { return Convert.ToInt32(Enums.GetDescription(Enums.DelegateTypes.DepartemntDelegeeType)); ; } }
        public static int SiteDelegeeTypeID { get { return Convert.ToInt32(Enums.GetDescription(Enums.DelegateTypes.SiteDelegeeType)); ; } }


        public static bool IsUserDelegate(string delegateAccount) 
        {
            List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.UserDelegeeTypeID);
            return (delegatedAccounts.Count > 0 ? true : false);
        }


        public static bool IsDepartmentDelegate(string delegateAccount)
        {
            List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.DepartmentDelegeeTypeID);
            return (delegatedAccounts.Count > 0 ? true : false);
        }


        public static bool IsSiteDelegate(string delegateAccount)
        {
            List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.SiteDelegeeTypeID);
            return (delegatedAccounts.Count > 0 ? true : false);
        }


        public static List<DelegateRole> GetDelegees(string delegateSipAccount = null, int? delegeeType = null) 
        {
            DataTable dt = new DataTable();

            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            Dictionary<string, object> wherePart;

            //Used to Lookup all the names of departments and sites
            List<Department> allDepartments = Department.GetAllDepartments();
            List<Site> allSites = Site.GetAllSites();


            if(string.IsNullOrEmpty(delegateSipAccount) && delegeeType == null)
            {
                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName));
            }
            else if (!string.IsNullOrEmpty(delegateSipAccount) && delegeeType == null)
            {
                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName), Enums.GetDescription(Enums.DelegateRoles.Delegee), delegateSipAccount);
            }
            else
            {
                wherePart = new Dictionary<string, object>
                {
                    {Enums.GetDescription(Enums.DelegateRoles.DelegeeType),delegeeType},
                    {Enums.GetDescription(Enums.DelegateRoles.Delegee), delegateSipAccount}
                };

                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName), null, wherePart, 0);
            }


            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID))
                        delegatedAccount.ID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee))
                        delegatedAccount.DelegeeSipAccount = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType))
                        delegatedAccount.DelegeeType = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SiteID))
                    {
                        delegatedAccount.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                        
                        if (delegatedAccount.SiteID > 0)
                            delegatedAccount.DelegeeSiteName = ((Site)allSites.Find(site => site.SiteID == delegatedAccount.SiteID)).SiteName;
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DepartmentID))
                    {
                        delegatedAccount.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                        if (delegatedAccount.DepartmentID > 0)
                            delegatedAccount.DelegeeDepartmentName = ((Department)allDepartments.Find(department => department.DepartmentID == delegatedAccount.DepartmentID)).DepartmentName;
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount))
                        delegatedAccount.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description))
                        delegatedAccount.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                if (delegatedAccount.DelegeeType == DelegateRole.UserDelegeeTypeID)
                {
                    delegatedAccount.DelegeeUser = User.GetUser(delegatedAccount.SipAccount);
                    delegatedAccount.DelegeeUser.DisplayName = HelperFunctions.FormatUserDisplayName(delegatedAccount.DelegeeUser.FullName, delegatedAccount.DelegeeUser.SipAccount);
                    delegatedAccount.DelegeeDisplayName = HelperFunctions.FormatUserDisplayName(delegatedAccount.DelegeeUser.FullName, delegatedAccount.DelegeeUser.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true); ;
                }

                else if (delegatedAccount.DelegeeType == DelegateRole.DepartmentDelegeeTypeID)
                {
                    delegatedAccount.DelegeeDepartment = Department.GetDepartment(delegatedAccount.DepartmentID);

                    var sipAccountInfo = User.GetUser(delegatedAccount.SipAccount);

                    if (sipAccountInfo == null)
                        delegatedAccount.DelegeeDisplayName = delegatedAccount.SipAccount;
                    else
                        delegatedAccount.DelegeeDisplayName = HelperFunctions.FormatUserDisplayName(sipAccountInfo.FullName, sipAccountInfo.SipAccount, returnAddressPartIfExists: true);
                }

                else if (delegatedAccount.DelegeeType == DelegateRole.SiteDelegeeTypeID)
                {
                    delegatedAccount.DelegeeSite = Site.GetSite(delegatedAccount.SiteID);

                    var sipAccountInfo = User.GetUser(delegatedAccount.SipAccount.TrimEnd());

                    if (sipAccountInfo == null)
                        delegatedAccount.DelegeeDisplayName = delegatedAccount.SipAccount;
                    else
                        delegatedAccount.DelegeeDisplayName = HelperFunctions.FormatUserDisplayName(sipAccountInfo.FullName, sipAccountInfo.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                }

                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }
        

        /*
         * This function returns a dictionary of the delegees sip-accounts and names {sip => name}, if they exist!
         **/
        public static Dictionary<string, string> GetDelegeesNames(string delegeeAddress, int delegateTypeID)
        {
            ADUserInfo userInfo;
            string delegateName;
            List<DelegateRole> delegatesList;
            Dictionary<string, string> DelegatedAccounts = new Dictionary<string, string>();

            delegatesList = GetDelegees(delegeeAddress, delegateTypeID);

            foreach (var delegateAccount in delegatesList)
            {
                delegateName = string.Empty;

                //If the delegate account is not a user's account
                if (delegateTypeID == DelegateRole.UserDelegeeTypeID)
                {
                    //Try to get the user from the system by the associated delegateAccount.SipAccount
                    userInfo = User.GetUserInfo(delegateAccount.SipAccount);
                    delegateName = (userInfo == null) ? delegateAccount.SipAccount : HelperFunctions.ReturnEmptyIfNull(userInfo.FirstName) + " " + HelperFunctions.ReturnEmptyIfNull(userInfo.LastName);

                    DelegatedAccounts.Add(delegateAccount.SipAccount, delegateName);
                }

                else if(delegateTypeID == DelegateRole.DepartmentDelegeeTypeID)
                {
                    var department = Department.GetDepartment(delegateAccount.DepartmentID);
                    delegateName = String.Format("{0}-{1}", department.SiteName, department.DepartmentName);

                    DelegatedAccounts.Add(delegateName, delegateName);
                }

                else if (delegateTypeID == DelegateRole.SiteDelegeeTypeID)
                {
                    var site = Site.GetSite(delegateAccount.SiteID);
                    delegateName = site.SiteName.ToString();

                    DelegatedAccounts.Add(delegateName, delegateName);
                }
            }

            return DelegatedAccounts;
        }


        public static int AddDelegate(DelegateRole delegee)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (!string.IsNullOrEmpty(delegee.SipAccount) && !string.IsNullOrEmpty(delegee.DelegeeSipAccount) && delegee.DelegeeType != 0)
            {
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.DelegeeType), delegee.DelegeeType);

                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount), delegee.SipAccount);

                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeSipAccount);

                if (delegee.SiteID != 0 && !string.IsNullOrEmpty(delegee.SiteID.ToString()))
                    columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.SiteID), delegee.SiteID);

                if (delegee.DepartmentID != 0 && !string.IsNullOrEmpty(delegee.DepartmentID.ToString()))
                    columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.DepartmentID), delegee.DepartmentID);

                if (!string.IsNullOrEmpty(delegee.Description))
                    columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Description), delegee.Description);

                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.DelegateRoles.TableName), columnsValues, Enums.GetDescription(Enums.DelegateRoles.ID));
            }

            return rowID;
        }


        public static bool UpadeDelegate(DelegateRole delegee) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (!string.IsNullOrEmpty(delegee.SipAccount))
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount), delegee.SipAccount);

            if (delegee.SiteID != 0 && !string.IsNullOrEmpty(delegee.SiteID.ToString()))
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.SiteID), delegee.SiteID);

            if (delegee.DepartmentID != 0 && !string.IsNullOrEmpty(delegee.DepartmentID.ToString()))
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.DepartmentID), delegee.DepartmentID);

            if (!string.IsNullOrEmpty(delegee.DelegeeSipAccount))
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeSipAccount);

            if (delegee.DelegeeType != 0 && !string.IsNullOrEmpty(delegee.DelegeeType.ToString()))
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.DelegeeType), delegee.DelegeeType);

            if (!string.IsNullOrEmpty(delegee.Description))
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.Description), delegee.Description);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.DelegateRoles.TableName),
                setPart,
                Enums.GetDescription(Enums.DelegateRoles.ID),
                delegee.ID);

            
            return status;
        }


        public static bool DeleteDelegate(DelegateRole delegee) 
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.DelegateRoles.TableName), Enums.GetDescription(Enums.DelegateRoles.ID), delegee.ID);

            return status;
        }

    }

    
}