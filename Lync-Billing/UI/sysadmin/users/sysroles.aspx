<%@ Page Title="tBill Admin | Manage Users Roles" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="sysroles.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.users.sysroles" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <style>
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        /* end manage-phone-calls grid styling */

        /* start users search query result styling */
        .search-item {
            font          : normal 11px tahoma, arial, helvetica, sans-serif;
            padding       : 3px 10px 3px 10px;
            border        : 1px solid #fff;
            border-bottom : 1px solid #eeeeee;
            white-space   : normal;
            color         : #555;
        }
        
        .search-item h3 {
            display     : block;
            font        : inherit;
            font-weight : bold;
            color       : #222;
            margin      : 0px;
        }

        .search-item h3 span {
            float       : right;
            font-weight : normal;
            margin      : 0 0 5px 5px;
            width       : 185px;
            display     : block;
            clear       : none;
        } 
        
        p { width: 650px; }
        
        .ext-ie .x-form-text { position : static !important; }
        /* end users search query result styling */
    </style>

    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ManageSystemRolesGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{RoleOwnerNameFilter}.reset();
                #{RoleDescriptionFilter}.reset();
                #{SiteNameFilter}.reset();
                
                #{ManageSystemRolesStore}.clearFilter();
            }
 
            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if (typeof val != "string") {
                    return value.length == 0;
                }
                
                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            };
 
            var getRecordFilter = function () {
                var f = [];
 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{RoleOwnerNameFilter}.getValue(), "RoleOwnerName", record);
                    }
                });
                
                f.push({
                    filter: function(record) {
                        return filterString(#{RoleDescriptionFilter}.getValue(), "RoleDescription", record);
                    }
                });

                f.push({
                    filter: function(record) {
                        return filterString(#{SiteNameFilter}.getValue(), "SiteName", record);
                    }
                });

                var len = f.length;
                 
                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            };
        </script>
    </ext:XScript>
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="ManageSystemRolesGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Users Roles"
                ComponentCls="fix-ui-vertical-align">

                <Store>
                    <ext:Store
                        ID="ManageSystemRolesStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageSystemRolesStoreModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="Int" />
                                    <ext:ModelField Name="RoleID" Type="Int" />
                                    <ext:ModelField Name="RoleDescription" Type="String" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="RoleOwnerName" Type="String" />
                                    <ext:ModelField Name="SiteID" Type="Int" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageSystemRolesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="Column1"
                            runat="server"
                            Text="Role Owner"
                            Width="250"
                            DataIndex="RoleOwnerName"
                            Sortable="false"
                            Groupable="false">
                            <HeaderItems>
                                <ext:TextField ID="RoleOwnerNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="Column2"
                            runat="server"
                            Text="Role Description"
                            Width="250"
                            DataIndex="RoleDescription"
                            Sortable="false"
                            Groupable="false">
                            <HeaderItems>
                                <ext:TextField ID="RoleDescriptionFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearButton1" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="Column3"
                            runat="server"
                            Text="Site"
                            Width="200"
                            DataIndex="SiteName"
                            Sortable="false"
                            Groupable="false">
                            <HeaderItems>
                                <ext:TextField ID="SiteNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearButton2" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:ImageCommandColumn
                            ID="DeleteButtonsColumn"
                            runat="server"
                            Width="30"
                            Align="Center"
                            Sortable="false"
                            Groupable="false">

                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Delegate" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>

                            <Listeners>
                                <Command Handler="this.up(#{ManageSystemRolesGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>

                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                </SelectionModel>
                
                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterSystemRolesBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="SiteName"
                                ValueField="SiteID"
                                FieldLabel="Site"
                                LabelWidth="25"
                                Margins="5 5 0 5"
                                Width="250">
                                <Store>
                                    <ext:Store
                                        ID="FilterSystemRolesBySiteStore"
                                        runat="server"
                                        OnLoad="FilterSystemRolesBySiteStore_Load">
                                        <Model>
                                            <ext:Model ID="SiteModel" runat="server">
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
                                            <div>{SiteName}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Change OnEvent="GetSystemRolesPerSite" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New System Role"
                                Icon="Add"
                                Margins="5 5 0 240">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddSystemRoleWindowPanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator
                                ID="ToolbarSeparaator"
                                runat="server" />

                            <ext:Button
                                ID="SaveChangesButton"
                                runat="server"
                                Text="Save Changes"
                                Icon="DatabaseSave"
                                Margins="5 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageSystemRolesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageSystemRolesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Window 
                                ID="AddNewSystemRoleWindowPanel" 
                                runat="server" 
                                Frame="true"
                                Resizable="false"
                                Title="New System Role" 
                                Hidden="true"
                                Width="380"
                                Icon="Add" 
                                X="100"
                                Y="100"
                                BodyStyle="background-color: #f9f9f9;"
                                ComponentCls="fix-ui-vertical-align">
                                
                                <Defaults>
                                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                                </Defaults>

                                <Items>
                                    <ext:ComboBox
                                        ID="NewSystemRole_RoleTypeCombobox"
                                        runat="server"
                                        DisplayField="TypeName"
                                        ValueField="TypeValue"
                                        Width="350"
                                        FieldLabel="Role Type"
                                        LabelWidth="125">
                                        <Items>
                                            <ext:ListItem Text="Site Administrator" Value="30" />
                                            <ext:ListItem Text="Site Accountant" Value="40" />
                                        </Items>

                                        <SelectedItems>
                                            <ext:ListItem Text="Site Administrator" Value="30" />
                                        </SelectedItems>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewSystemRole_UserSipAccount"
                                        runat="server"
                                        Icon="Find"
                                        TriggerAction="Query"
                                        QueryMode="Remote"
                                        Editable="true"
                                        DisplayField="SipAccount"
                                        ValueField="SipAccount"
                                        FieldLabel="User Email:"
                                        EmptyText="Please Select a User"
                                        LabelWidth="125"
                                        Width="350">
                                        <Store>
                                            <ext:Store 
                                                ID="Store1"
                                                runat="server">
                                                <Model>
                                                    <ext:Model 
                                                        ID="Model2"
                                                        runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="EmployeeID" />
                                                            <ext:ModelField Name="SipAccount" />
                                                            <ext:ModelField Name="SiteName" />
                                                            <ext:ModelField Name="FullName" />
                                                            <ext:ModelField Name="DisplayName" />
                                                            <ext:ModelField Name="Department" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        
                                        <DirectEvents>
                                            <BeforeQuery OnEvent="NewSystemRole_UserSipAccount_BeforeQuery" />
                                        </DirectEvents>

                                        <ListConfig
                                            Border="true"
                                            MinWidth="400"
                                            MaxHeight="300"
                                            EmptyText="Type User Name or Email...">
                                            <ItemTpl ID="ItemTpl1" runat="server">
                                                <Html>
                                                    <div class="search-item">
                                                        <h3><span>{DisplayName}</span>{SipAccount}</h3>
                                                    </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewSystemRole_SitesList"
                                        runat="server"
                                        DisplayField="SiteName"
                                        ValueField="SiteID"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        Editable="true"
                                        EmptyText="Please Select Site"
                                        FieldLabel="Site:"
                                        LabelWidth="125"
                                        Width="350">
                                        <Store>
                                            <ext:Store ID="SitesListStore" runat="server" OnLoad="SitesListStore_Load">
                                                <Model>
                                                    <ext:Model ID="SitesListStoreModel" runat="server">
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
                                            <ItemTpl ID="NewSystemRoleSitesListTpl" runat="server">
                                                <Html>
                                                    <div>{SiteName}&nbsp;({CountryCode})</div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>
                                </Items>

                                <BottomBar>
                                    <ext:StatusBar
                                        ID="NewSystemRoleWindowBottomBar"
                                        runat="server"
                                        StatusAlign="Right"
                                        DefaultText=""
                                        Height="30">
                                        <Items>
                                            <ext:Button
                                                ID="AddNewSystemRoleButton"
                                                runat="server"
                                                Text="Add"
                                                Icon="ApplicationFormAdd"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="AddNewSystemRoleButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button
                                                ID="CancelNewSystemRoleButton"
                                                runat="server"
                                                Text="Cancel"
                                                Icon="Cancel"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="CancelNewSystemRoleButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:ToolbarSeparator
                                                ID="AddNewSystemRoleWindow_BottomBarSeparator"
                                                runat="server" />

                                            <ext:ToolbarTextItem
                                                ID="NewSystemRole_StatusMessage"
                                                runat="server"
                                                Height="15"
                                                Text=""
                                                Margins="0 0 0 5" />
                                        </Items>
                                    </ext:StatusBar>
                                </BottomBar>

                                <DirectEvents>
                                    <BeforeHide OnEvent="AddNewSystemRoleWindowPanel_BeforeHide" />
                                </DirectEvents>
                            </ext:Window>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageSystemRolesPagingToolbar"
                        runat="server"
                        StoreID="ManageSystemRolesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="System Roles {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>
    </div>
</asp:Content>
