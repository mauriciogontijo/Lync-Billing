<%@ Page Title="tBill Admin | Manage Delegates" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="delegates.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.users.delegates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                var store = #{ManageDelegatesGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{SipAccountFilter}.reset();
                #{DelegeeAccountFilter}.reset();
                #{SiteFilter}.reset();
                #{DepartmentFilter}.reset();
                #{DescriptionFilter}.reset();
                
                #{ManageDelegatesStore}.clearFilter();
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
                        return filterString(#{SipAccountFilter}.getValue(), "SipAccount", record);
                    }
                });
                 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{DelegeeAccountFilter}.getValue(), "DelegeeAccount", record);
                    }
                });

                f.push({
                    filter: function(record) {
                        return filterString(#{SiteFilter}.getValue(), "SiteID", record);
                    }
                });

                f.push({
                    filter: function(record) {
                        return filterString(#{DepartmentFilter}.getValue(), "DepartmentID", record);
                    }
                });

                f.push({
                    filter: function(record) {
                        return filterString(#{DescriptionFilter}.getValue(), "Description", record);
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

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="ManageDelegatesGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Delegates">

                <Store>
                    <ext:Store
                        ID="ManageDelegatesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageDelegatesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="Int" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="SiteID" Type="Int" />
                                    <ext:ModelField Name="DelegeeSite" Type="String" />
                                    <ext:ModelField Name="DepartmentID" Type="Int" />
                                    <ext:ModelField Name="DelegeeDepartment" Type="String" />
                                    <ext:ModelField Name="DelegeeType" Type="Int" />
                                    <ext:ModelField Name="DelegeeAccount" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageDelegatesColumnModel" runat="server" Flex="1">
                    <Columns>

                        <ext:Column ID="Column1"
                            runat="server"
                            Text="User"
                            Width="160"
                            DataIndex="SipAccount"
                            Sortable="true"
                            Groupable="true">
                            <HeaderItems>
                                <ext:TextField ID="SipAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="Column4"
                            runat="server"
                            Text="Site"
                            Width="130"
                            DataIndex="SiteID"
                            Sortable="true"
                            Groupable="true">

                            <HeaderItems>
                                <ext:TextField ID="SiteFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSiteFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                        </ext:Column>


                        <ext:Column ID="Column5"
                            runat="server"
                            Text="Department"
                            Width="140"
                            DataIndex="DepartmentID"
                            Sortable="true"
                            Groupable="true">
                            
                            <HeaderItems>
                                <ext:TextField ID="DepartmentFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDepartmentFiler" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <%--<Renderer Fn="" />--%>
                        </ext:Column>

                        <ext:Column ID="Column2"
                            runat="server"
                            Text="Delegee User"
                            Width="150"
                            DataIndex="DelegeeAccount"
                            Sortable="true"
                            Groupable="true">
                            <HeaderItems>
                                <ext:TextField ID="DelegeeAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDelegeeAccountFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="Column3"
                            runat="server"
                            Text="Description"
                            Width="110"
                            DataIndex="Description"
                            Sortable="true"
                            Groupable="true">
                            <HeaderItems>
                                <ext:TextField ID="DescriptionFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDescFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:ImageCommandColumn
                            ID="DeleteButtonsColumn"
                            runat="server"
                            Width="30"
                            Sortable="false"
                            Align="Center">
                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Delegate" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>
                            <Listeners>
                                <Command Handler="this.up(#{ManageDelegatesGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>

                    </Columns>
                </ColumnModel>

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

                                <DirectEvents>
                                    <Change OnEvent="GetDelegates" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="Add New Delegee"
                                Icon="Add"
                                Margins="5 10 0 350">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddDelegeePanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:Window 
                                ID="AddNewDelegeeWindowPanel" 
                                runat="server" 
                                Frame="true"
                                Resizable="false"
                                Title="Add New Delegee" 
                                Hidden="true"
                                Width="380"
                                Icon="Add" 
                                X="100"
                                Y="100"
                                BodyStyle="background-color: #f9f9f9;">
                                
                                <Defaults>
                                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                                </Defaults>

                                <Items>
                                    <ext:ComboBox
                                        ID="NewDelegee_DelegeeTypeCombobox"
                                        runat="server"
                                        DisplayField="TypeName"
                                        ValueField="TypeValue"
                                        SelectOnFocus="true"
                                        SelectOnTab="true"
                                        Selectable="true"
                                        Width="350"
                                        FieldLabel="Delegee Type"
                                        LabelWidth="125">
                                        <Items>
                                            <ext:ListItem Text="User" Value="1" />
                                            <ext:ListItem Text="Department" Value="2" />
                                            <ext:ListItem Text="Site" Value="3" />
                                        </Items>

                                        <SelectedItems>
                                            <ext:ListItem Text="User" Value="1" />
                                        </SelectedItems>

                                        <DirectEvents>
                                            <Select OnEvent="DelegeeTypeMenu_Selected" />
                                        </DirectEvents>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewDelegee_UserSipAccount"
                                        runat="server"
                                        Icon="Find"
                                        TriggerAction="Query"
                                        QueryMode="Remote"
                                        DisplayField="SipAccount"
                                        ValueField="SipAccount"
                                        FieldLabel="User SipAccount:"
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
                                            <BeforeQuery OnEvent="NewDelegee_UserSipAccount_BeforeQuery" />
                                        </DirectEvents>

                                        <ListConfig
                                            Border="true"
                                            MinWidth="400"
                                            MaxHeight="300"
                                            EmptyText="Type User Name or SipAccount...">
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
                                        ID="NewDelegee_DelegeeSipAccount"
                                        runat="server"
                                        Icon="Find"
                                        TriggerAction="Query"
                                        QueryMode="Remote"
                                        DisplayField="SipAccount"
                                        ValueField="SipAccount"
                                        FieldLabel="Delegee SipAccount:"
                                        EmptyText="Please Select a Delegee"
                                        LabelWidth="125"
                                        Width="350">
                                        <Store>
                                            <ext:Store 
                                                ID="FilterUsersByDepartmentStore"
                                                runat="server">
                                                <Model>
                                                    <ext:Model 
                                                        ID="FilterUsersByDepartmentModel"
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
                                            <BeforeQuery OnEvent="NewDelegee_DelegeeSipAccount_BeforeQuery" />
                                        </DirectEvents>

                                        <ListConfig
                                            Border="true"
                                            MinWidth="400"
                                            MaxHeight="300"
                                            EmptyText="Type User Name or SipAccount...">
                                            <ItemTpl ID="ItemTpl3" runat="server">
                                                <Html>
                                                    <div class="search-item">
                                                        <h3><span>{DisplayName}</span>{SipAccount}</h3>
                                                    </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewDelegee_SitesList"
                                        runat="server"
                                        DisplayField="SiteName"
                                        ValueField="SiteID"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        EmptyText="Please Select Site"
                                        SelectOnFocus="true"
                                        SelectOnTab="true"
                                        Selectable="true"
                                        Editable="true"
                                        FieldLabel="Site:"
                                        LabelWidth="125"
                                        Width="350"
                                        Hidden="true">
                                        <Store>
                                            <ext:Store 
                                                ID="SitesListStore"
                                                runat="server">
                                                <Model>
                                                    <ext:Model 
                                                        ID="Model1"
                                                        runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="SiteID" />
                                                            <ext:ModelField Name="SiteName" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewDelegee_DepartmentsList"
                                        runat="server"
                                        DisplayField="DepartmentName"
                                        ValueField="DepartmentID"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        EmptyText="Please Select Department"
                                        SelectOnFocus="true"
                                        SelectOnTab="true"
                                        Selectable="true"
                                        Editable="true"
                                        FieldLabel="Department:"
                                        LabelWidth="125"
                                        Width="350"
                                        Hidden="true">
                                        <Store>
                                            <ext:Store 
                                                ID="DepartmentsListStore"
                                                runat="server">
                                                <Model>
                                                    <ext:Model 
                                                        ID="DepartmentHeadsStoreModel"
                                                        runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="SiteID" />
                                                            <ext:ModelField Name="SiteName" />
                                                            <ext:ModelField Name="DepartmentID" />
                                                            <ext:ModelField Name="DepartmentName" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>

                                        <ListConfig
                                            Border="true"
                                            MinWidth="150"
                                            MaxHeight="300"
                                            EmptyText="Type department name...">
                                            <ItemTpl ID="ItemTpl2" runat="server">
                                                <Html>
                                                    <div>{SiteName}&nbsp;({DepartmentName})</div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>
                                </Items>

                                <BottomBar>
                                    <ext:Toolbar
                                        ID="NewDelegeeWindowBottomBar"
                                        runat="server"
                                        Height="30">
                                        <Items>
                                            <ext:Button
                                                ID="AddNewDelegeeButton"
                                                runat="server"
                                                Text="Add"
                                                Icon="ApplicationFormAdd"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="AddNewDelegeeButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button
                                                ID="CancelNewDelegeeButton"
                                                runat="server"
                                                Text="Cancel"
                                                Icon="Cancel"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="CancelNewDelegeeButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </BottomBar>

                                <DirectEvents>
                                    <BeforeHide OnEvent="AddNewDelegeeWindowPanel_BeforeHide" />
                                </DirectEvents>
                            </ext:Window>

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
