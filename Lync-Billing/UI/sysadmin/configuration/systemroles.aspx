<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="systemroles.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.configuration.systemroles" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">

            <ext:GridPanel
                ID="ManageUsersRolesGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Users Roles">

                <Store>
                    <ext:Store
                        ID="ManageUsersRolesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageDelegatesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="UsersRolesID" Type="Int" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="RoleID" Type="Int" />
                                    <ext:ModelField Name="SiteID" Type="Int" />
                                    <ext:ModelField Name="PoolID" Type="Int" />
                                    <ext:ModelField Name="GatewayID" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageDelegatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="Column1"
                            runat="server"
                            Text="User Email"
                            Width="200"
                            DataIndex="SipAccount"
                            Sortable="true"
                            Groupable="true">
                        </ext:Column>

                        <ext:Column ID="Column2"
                            runat="server"
                            Text="Role"
                            Width="150"
                            DataIndex="RoleID"
                            Sortable="true"
                            Groupable="true">
                        </ext:Column>

                        <ext:Column ID="Column4"
                            runat="server"
                            Text="Pool"
                            Width="180"
                            DataIndex="PoolID"
                            Sortable="true"
                            Groupable="true">
                        </ext:Column>

                        <ext:Column ID="Column5"
                            runat="server"
                            Text="Gateway"
                            Width="200"
                            DataIndex="GatewayID"
                            Sortable="true"
                            Groupable="true">
                        </ext:Column>

                    </Columns>
                </ColumnModel>
                
                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDelegatesPagingToolbar"
                        runat="server"
                        StoreID="ManageDelegatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users Delegates {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>
    </div>
</asp:Content>