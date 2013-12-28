<%@ Page Title="tBill Admin | Sync with AD" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="syncusers.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.activedirectory.syncusers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
         
            <ext:Button
                ID="SyncWithADButton"
                runat="server"
                Text="Sync Users with Active Directory">
                <DirectEvents>
                    <Click OnEvent="SyncWithADButton_Click">
                        <EventMask ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>

        </div>
    </div>
</asp:Content>
