<%@ Page Title="eBill Admin | Manage Users Roles" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.users.roles" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
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

                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                </SelectionModel>
                
                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterDelegatesBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="SiteName"
                                ValueField="SiteName"
                                FieldLabel="Site"
                                LabelWidth="25"
                                Margins="5 5 0 5"
                                Width="250">
                                <Store>
                                    <ext:Store
                                        ID="DelegatesSitesStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="DelegatesSitesModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SiteID" />
                                                    <ext:ModelField Name="SiteName" />
                                                    <ext:ModelField Name="CountryCode" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <ListConfig>
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div data-qtip="{SiteName}. {CountryCode}">
                                                {SiteName} ({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <%--<DirectEvents>
                                    <Change OnEvent="" />
                                </DirectEvents>--%>
                            </ext:ComboBox>

                            <ext:Button
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save Changes"
                                Icon="ApplicationEdit"
                                Margins="5 10 0 250">
                                <%--<DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent" before="return #{ManageDelegatesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDelegatesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>--%>
                            </ext:Button>

                            <ext:Button
                                ID="CancelChangesButton"
                                Text="Cancel Changes"
                                Icon="Cancel"
                                runat="server"
                                Margins="5 0 0 0">
                                <%--<DirectEvents>
                                    <Click OnEvent="RejectChanges_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>--%>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

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
