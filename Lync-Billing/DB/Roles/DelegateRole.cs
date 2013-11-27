using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;

namespace Lync_Billing.DB.Roles
{
    public class DelegateRole
    {
        private static DBLib DBRoutines = new DBLib();
        
        public int ID { set; get; }
        public int DelegeeType { get; set; }
        public int SiteID { get; set; }
        public int DepartmentID { get; set; }
        public string SipAccount { get; set; }
        public string DelegeeAccount { get; set; }
        public string Description { get; set; }

        //These are logical representation of data, they don't belong to the table
        public string DelegeeDisplayName { get; set; }
        public Users DelegeeUser { get; set; }
        public Site DelegeeSite { get; set; }
        public Department DelegeeDepartment { get; set; }

        //These are for lookup use only in the application
        public static int UserDelegeeTypeID { get { return 1; } }
        public static int DepartmentDelegeeTypeID { get { return 2; } }
        public static int SiteDelegeeTypeID { get { return 3; } }

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

        public static List<DelegateRole> GetDelegees(string delegateAccount) 
        {
            DataTable dt = new DataTable();

            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName), Enums.GetDescription(Enums.DelegateRoles.Delegee), delegateAccount);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID))
                        delegatedAccount.ID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee))
                        delegatedAccount.DelegeeAccount = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType))
                        delegatedAccount.DelegeeType = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SiteID))
                        delegatedAccount.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DepartmentID))
                        delegatedAccount.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount))
                        delegatedAccount.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description))
                        delegatedAccount.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<DelegateRole> GetDelegees(string degateAccount, int delegeeType) 
        {
            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            DataTable dt = new DataTable();

            Dictionary<string, object> wherePart = new Dictionary<string, object>
            {
                {Enums.GetDescription(Enums.DelegateRoles.DelegeeType),delegeeType},
                {Enums.GetDescription(Enums.DelegateRoles.Delegee) ,degateAccount}
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName),null,wherePart,0);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID))
                        delegatedAccount.ID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee))
                        delegatedAccount.DelegeeAccount = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType))
                        delegatedAccount.DelegeeType = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SiteID))
                        delegatedAccount.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DepartmentID))
                        delegatedAccount.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount))
                        delegatedAccount.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description))
                        delegatedAccount.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                if (delegatedAccount.DelegeeType == DelegateRole.UserDelegeeTypeID)
                {
                    delegatedAccount.DelegeeUser = Users.GetUser(delegatedAccount.SipAccount);
                    delegatedAccount.DelegeeUser.DisplayName = HelperFunctions.FormatUserDisplayName(delegatedAccount.DelegeeUser.FullName, delegatedAccount.DelegeeUser.SipAccount, returnNameIfExists: true);
                    delegatedAccount.DelegeeDisplayName = delegatedAccount.DelegeeUser.DisplayName;
                }

                else if (delegatedAccount.DelegeeType == DelegateRole.DepartmentDelegeeTypeID)
                {
                    delegatedAccount.DelegeeDepartment = Department.GetDepartment(delegatedAccount.DepartmentID);

                    var sipAccountInfo = Users.GetUser(delegatedAccount.SipAccount);

                    if (sipAccountInfo == null)
                        delegatedAccount.DelegeeDisplayName = delegatedAccount.SipAccount;
                    else
                        delegatedAccount.DelegeeDisplayName = HelperFunctions.FormatUserDisplayName(sipAccountInfo.FullName, sipAccountInfo.SipAccount, returnAddressPartIfExists: true);
                }

                else if (delegatedAccount.DelegeeType == DelegateRole.SiteDelegeeTypeID)
                {
                    delegatedAccount.DelegeeSite = Site.GetSite(delegatedAccount.SiteID);

                    var sipAccountInfo = Users.GetUser(delegatedAccount.SipAccount.TrimEnd());

                    if (sipAccountInfo == null)
                        delegatedAccount.DelegeeDisplayName = delegatedAccount.SipAccount;
                    else
                        delegatedAccount.DelegeeDisplayName = HelperFunctions.FormatUserDisplayName(sipAccountInfo.FullName, sipAccountInfo.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                }

                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<DelegateRole> GetDelegees() 
        {
            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName));

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID))
                        delegatedAccount.ID = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee))
                        delegatedAccount.DelegeeAccount = Convert.ToString(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType))
                        delegatedAccount.DelegeeType = Convert.ToInt32(row[column.ColumnName]);

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SiteID))
                        delegatedAccount.SiteID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DepartmentID))
                        delegatedAccount.DepartmentID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount))
                        delegatedAccount.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description))
                        delegatedAccount.Description = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
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
                    userInfo = Users.GetUserInfo(delegateAccount.SipAccount);
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
    
        public static bool UpadeDelegate(DelegateRole delegee) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount), delegee.SipAccount);

            if (delegee.SiteID.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.SiteID), delegee.SiteID);
            
            if (delegee.DepartmentID.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.DepartmentID), delegee.DepartmentID);

            if (delegee.DelegeeAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeAccount);

            if (delegee.DelegeeType.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.DelegeeType), delegee.DelegeeType);

            if (delegee.Description.ToString() != null)
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

        public static int AddDelegate(DelegateRole delegee) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount), delegee.SipAccount);

            if (delegee.SiteID.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.SiteID), delegee.SiteID);

            if (delegee.DepartmentID.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.DepartmentID), delegee.DepartmentID);

            if (delegee.DelegeeAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeAccount);

            if (delegee.DelegeeType.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeType);

            if (delegee.Description.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Description), delegee.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.DelegateRoles.TableName), columnsValues, Enums.GetDescription(Enums.DelegateRoles.ID));

            return rowID;
        }
    }

    
}