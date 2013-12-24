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

using Lync_Billing.Libs;
using Lync_Billing.Backend;


namespace Lync_Billing.ui.sysadmin.manage
{
    public partial class dids : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        List<DID> allDIDs = new List<DID>();


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/sysadmin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=sysadmin");
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;

            allDIDs = DID.GetAllDIDs();
        }


        protected void ManageDIDsGridStore_Load(object sender, EventArgs e)
        {
            ManageDIDsGrid.GetStore().DataSource = allDIDs;
            ManageDIDsGrid.GetStore().LoadData(allDIDs);
        }


        protected void ShowAddNewDIDWindowPanel(object sender, DirectEventArgs e)
        {
            this.AddNewDIDWindowPanel.Show();
        }


        protected void CancelNewDIDButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDIDWindowPanel.Hide();
        }
        

        protected void AddNewDIDWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewDID_DIDPattern.Text = null;
            NewDID_Description.Text = null;
            NewDID_StatusMessage.Text = null;
        }

        
        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            bool statusFlag = false;
            bool resetDescriptionFlag = false;
            string messageType = "error";
            string notificationMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<DID> storeChangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeChangedData = new StoreDataHandler(json).BatchObjectData<DID>();

                //DELETE EXISTING DIDS
                if(storeChangedData.Deleted.Count > 0)
                {
                    foreach (DID storeDIDObject in storeChangedData.Deleted)
                    {
                        statusFlag = Backend.DID.DeleteDID(storeDIDObject);

                        if (statusFlag == false)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("The DID(s) were NOT deleted successfully. An error has occured. Please try again.");

                            break;
                        }

                        messageType = "success";
                        notificationMessage = "DID(s) were updated successfully, changes were saved.";
                    }

                    ManageDIDsGrid.GetStore().Reload();

                    HelperFunctions.Message("Update Sites", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                }


                //RESET STATUS FLAG
                statusFlag = false;


                //Update Existing DIDs
                if (storeChangedData.Updated.Count > 0)
                {
                    foreach (DID storeDIDObject in storeChangedData.Updated)
                    {
                        var originalDIDObject = (DID)allDIDs.Find(DID => DID.ID == storeDIDObject.ID);

                        //Check for duplicate site name
                        //If the DID Pattern was changed
                        if (storeDIDObject.DIDPattern != originalDIDObject.DIDPattern)
                        {
                            //If the changed (submitted) DID Pattern alread exists in the system, exit and display error message
                            if (allDIDs.Find(DID => DID.DIDPattern == storeDIDObject.DIDPattern) != null)
                            {
                                messageType = "error";
                                notificationMessage = String.Format("DID was not changed to due to a duplicate DID Pattern. Please enter a valide DID Pattern.");

                                break;
                            }
                        }

                        //Update the DID Record
                        if (string.IsNullOrEmpty(storeDIDObject.Description) && storeDIDObject.Description != originalDIDObject.Description)
                            resetDescriptionFlag = true;

                        statusFlag = Backend.DID.UpdateDID(storeDIDObject, FORCE_RESET_DESCRIPTION: resetDescriptionFlag);


                        //If an error has occured during the Database Update, display error message
                        if (statusFlag == false)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("The DID(s) were NOT updated successfully. An error has occured. Please try again.");
                        }
                        else
                        {
                            messageType = "success";
                            notificationMessage = "DID(s) were updated successfully, changes were saved.";

                            ManageDIDsGrid.GetStore().GetById(storeDIDObject.ID).Commit();
                            ManageDIDsGrid.GetStore().Reload();
                        }
                    }

                    HelperFunctions.Message("Update Sites", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                }//End if
            }//End if
        }


        protected void AddNewDIDButton_Click(object sender, DirectEventArgs e)
        {
            DID NewDID;

            string DIDPattern = string.Empty;
            string Description = string.Empty;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (!string.IsNullOrEmpty(NewDID_DIDPattern.Text) && !string.IsNullOrEmpty(NewDID_Description.Text))
            {
                NewDID = new DID();

                DIDPattern = NewDID_DIDPattern.Text.ToString();
                Description = NewDID_Description.Text.ToString();
                
                //Check for duplicates
                if (allDIDs.Find(DID => DID.DIDPattern == DIDPattern) != null)
                {
                    statusMessage = "Cannot add duplicate DIDs!";
                }
                //This Site record doesn't exist, add it.
                else
                {
                    NewDID.DIDPattern = DIDPattern;
                    NewDID.Description = Description;

                    //Insert the New Site to the database
                    Backend.DID.AddDID(NewDID);

                    //Close the window
                    this.AddNewDIDWindowPanel.Hide();

                    //Add the New Site record to the store and apply the filter
                    ManageDIDsGrid.GetStore().Add(NewDID);
                    ManageDIDsGrid.GetStore().Reload();

                    successStatusMessage = String.Format("The DID was added successfully.");
                }
            }
            else
            {
                statusMessage = "Please provide all the required information!";
            }

            this.NewDID_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                HelperFunctions.Message("New DID", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }

    }

}