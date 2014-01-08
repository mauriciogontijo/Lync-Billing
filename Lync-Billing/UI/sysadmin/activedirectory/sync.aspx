<%@ Page Title="iBill Admin | Sync with AD" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="sync.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.activedirectory.sync" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
         
            <ext:Panel
                ID="SyncDataWithActiveDirectory"
                runat="server"
                Height="140"
                Width="740"
                Header="true"
                Title="Sync System Data with Active Directory">
                <Items>
                    <ext:Button
                        ID="SyncUsersButton"
                        runat="server"
                        Text="Sync Users Data"
                        Margin="15">
                        <DirectEvents>
                            <Click OnEvent="SyncUsersButton_Click">
                                <EventMask ShowMask="true" Msg="Syncronizing Users Data with Active Directory" MinDelay="5000" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>

                    <ext:Label
                        ID="SyncUsersButtonLabel"
                        runat="server"
                        Text="This will add the new users to the database and will update the existing ones."
                        Margin="15" />

                    <%--<ext:ToolbarSeparator ID="PanelSeparator1" runat="server" />

                    <ext:Button
                        ID="SyncDepartmentsButton"
                        runat="server"
                        Text="Sync Departments Data"
                        Margin="15">
                        <DirectEvents>
                            <Click OnEvent="SyncDepartmentsButton_Click">
                                <EventMask ShowMask="true" Msg="Syncronizing Departments with Active Directory" MinDelay="5000" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>

                    <ext:Label
                        ID="SyncDepartmentsButtonLabel"
                        runat="server"
                        Text="This will add the new departments to the database and will update the existing ones."
                        Margin="15" />

                    <ext:ToolbarSeparator ID="PanelSeparator2" runat="server" />

                    <ext:Button
                        ID="SyncSitesButton"
                        runat="server"
                        Text="Sync Sites Data"
                        Margin="15">
                        <DirectEvents>
                            <Click OnEvent="SyncSitesButton_Click">
                                <EventMask ShowMask="true" Msg="Syncronizing Sites with Active Directory" MinDelay="5000" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>

                    <ext:Label
                        ID="SyncSitesButtonLabel"
                        runat="server"
                        Text="This will add the new sites to the database and will update the existing ones."
                        Margin="15" />--%>

                    <ext:ToolbarSeparator ID="PanelSeparator3" runat="server" />

                    <ext:Button
                        ID="Button1"
                        runat="server"
                        Text="Sync Sites And Departments"
                        Margin="15">
                        <DirectEvents>
                            <Click OnEvent="SyncSitesDepartmentsButton_Click">
                                <EventMask ShowMask="true" Msg="Associating Sites and Departments" MinDelay="5000" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>

                    <ext:Label
                        ID="Label1"
                        runat="server"
                        Text="This will associate the departments to their respective sites."
                        Margin="15" />
                </Items>
            </ext:Panel>
        </div>
    </div>
</asp:Content>
