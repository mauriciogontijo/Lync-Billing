<%@ Page Title="iBill Admin | Manage Department Heads"  Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="depheads.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.users.depheads" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
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
                var store = #{ManageDepartmentHeadsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{SipAccountFilter}.reset();
                #{DepartmentFilter}.reset();
                
                #{ManageDepartmentHeadsStore}.clearFilter();
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
                        return filterString(#{DepartmentHeadFilter}.getValue(), "DepartmentHeadName", record);
                    }
                });
                
                f.push({
                    filter: function(record) {
                        return filterString(#{SiteFilter}.getValue(), "SiteName", record);
                    }
                });

                f.push({
                    filter: function(record) {
                        return filterString(#{DepartmentFilter}.getValue(), "DepartmentName", record);
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

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            
            <ext:GridPanel
                ID="ManageDepartmentHeadsGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Department Heads"
                ComponentCls="fix-ui-vertical-align">

                <Store>
                    <ext:Store ID="ManageDepartmentHeadsStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageDepartmentHeadsModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="Int" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="SiteID" Type="Int" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="DepartmentID" Type="Int" />
                                    <ext:ModelField Name="DepartmentName" Type="String" />
                                    <ext:ModelField Name="DepartmentHeadName" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageDepartmentHeadsColumnModel" runat="server" Flex="1">
                    <Columns>

                        <ext:Column ID="Column1"
                            runat="server"
                            Text="Department Head"
                            Width="240"
                            DataIndex="DepartmentHeadName"
                            Sortable="false"
                            Groupable="false">
                            <HeaderItems>
                                <ext:TextField ID="DepartmentHeadFilter" runat="server" Icon="Magnifier">
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
                            Text="Site"
                            Width="230"
                            DataIndex="SiteName"
                            Sortable="false"
                            Groupable="false">
                            
                            <HeaderItems>
                                <ext:TextField ID="SiteFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSiteFiler" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="Column5"
                            runat="server"
                            Text="Department"
                            Width="230"
                            DataIndex="DepartmentName"
                            Sortable="false"
                            Groupable="false">
                            
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
                        </ext:Column>

                        <ext:ImageCommandColumn
                            ID="DeleteButtonsColumn"
                            runat="server"
                            Width="30"
                            Align="Center"
                            Sortable="false"
                            Groupable="false">
                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Department Head" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>
                            <Listeners>
                                <Command Handler="this.up(#{ManageDepartmentHeadsGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>

                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterDepartmentsSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterDepartmentHeadsBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="Query"
                                QueryMode="Local"
                                Editable="true"
                                DisplayField="SiteName"
                                ValueField="SiteID"
                                FieldLabel="Site"
                                LabelWidth="25"
                                Margins="5 5 0 5"
                                Width="250">
                                <Store>
                                    <ext:Store ID="DepartmentHeadsSitesStore" runat="server" OnLoad="DepartmentHeadsSitesStore_Load">
                                        <Model>
                                            <ext:Model ID="DelegatesSitesStoreModel" runat="server">
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
                                    <ItemTpl ID="ItemTpl1" runat="server">
                                        <Html>
                                            <div>{SiteName}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetDepartmentHeads" />
                                </DirectEvents>
                            </ext:ComboBox>


                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New Department Head"
                                Icon="Add"
                                Margins="5 5 0 215">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddDepartmentHeadWindowPanel" />
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
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageDepartmentHeadsStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDepartmentHeadsStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>


                            <ext:Window 
                                ID="AddNewDepartmentHeadWindowPanel" 
                                runat="server" 
                                Frame="true"
                                Resizable="false"
                                Title="New Department Head Role" 
                                Hidden="true"
                                Width="410"
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
                                        ID="NewDepartmentHead_UserSipAccount"
                                        runat="server"
                                        Icon="Find"
                                        TriggerAction="Query"
                                        QueryMode="Remote"
                                        Editable="true"
                                        DisplayField="SipAccount"
                                        ValueField="SipAccount"
                                        FieldLabel="User Email:"
                                        EmptyText="Please Select a User"
                                        LabelWidth="80"
                                        Width="380">
                                        <Store>
                                            <ext:Store ID="NewDepartmentHead_UserSipAccountStore" runat="server">
                                                <Model>
                                                    <ext:Model ID="NewDepartmentHead_UserSipAccountStoreModel" runat="server">
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
                                            <BeforeQuery OnEvent="NewDepartmentHead_UserSipAccount_BeforeQuery" />
                                        </DirectEvents>

                                        <ListConfig
                                            Border="true"
                                            MinWidth="400"
                                            MaxHeight="300"
                                            EmptyText="Type User Name or Email...">
                                            <ItemTpl ID="ItemTpl2" runat="server">
                                                <Html>
                                                    <div class="search-item">
                                                        <h3><span>{DisplayName}</span>{SipAccount}</h3>
                                                    </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewDepartmentHead_SitesList"
                                        runat="server"
                                        DisplayField="SiteName"
                                        ValueField="SiteID"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        Editable="true"
                                        EmptyText="Please Select Site"
                                        FieldLabel="Site:"
                                        LabelWidth="80"
                                        Width="380">
                                        <Store>
                                            <ext:Store ID="SitesListStore" runat="server" OnLoad="NewDepartmentHead_SitesListStore_Load">
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
                                            <ItemTpl ID="NewDepartmentHeadSitesListTpl" runat="server">
                                                <Html>
                                                    <div>{SiteName}&nbsp;({CountryCode})</div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>

                                        <DirectEvents>
                                            <Select OnEvent="NewDepartmentHead_SitesList_Selected" />
                                        </DirectEvents>
                                    </ext:ComboBox>

                                    <ext:ComboBox
                                        ID="NewDepartmentHead_DepartmentsList"
                                        runat="server"
                                        DisplayField="DepartmentName"
                                        ValueField="DepartmentID"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        Editable="true"
                                        EmptyText="Please Select Department"
                                        FieldLabel="Department:"
                                        LabelWidth="80"
                                        Width="380">
                                        <Store>
                                            <ext:Store ID="DepartmentsListStore" runat="server">
                                                <Model>
                                                    <ext:Model ID="DepartmentsStoreModel" runat="server">
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
                                    </ext:ComboBox>
                                </Items>

                                <BottomBar>
                                    <ext:StatusBar
                                        ID="NewDepartmentHeadWindowBottomBar"
                                        runat="server"
                                        StatusAlign="Right"
                                        DefaultText=""
                                        Height="30">
                                        <Items>
                                            <ext:Button
                                                ID="AddNewDepartmentHeadButton"
                                                runat="server"
                                                Text="Add"
                                                Icon="ApplicationFormAdd"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="AddNewDepartmentHeadButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button
                                                ID="CancelNewDepartmentHeadButton"
                                                runat="server"
                                                Text="Cancel"
                                                Icon="Cancel"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="CancelNewDepartmentHeadButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:ToolbarSeparator
                                                ID="NewDepartmentHeadWindow_BottomBarSeparator"
                                                runat="server" />

                                            <ext:ToolbarTextItem
                                                ID="NewDepartmentHead_StatusMessage"
                                                runat="server"
                                                Height="15"
                                                Text=""
                                                Margins="0 0 0 5" />
                                        </Items>
                                    </ext:StatusBar>
                                </BottomBar>

                                <DirectEvents>
                                    <BeforeHide OnEvent="AddNewDepartmentHeadWindowPanel_BeforeHide" />
                                </DirectEvents>
                            </ext:Window>

                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDepartmentHeadsPagingToolbar"
                        runat="server"
                        StoreID="ManageDepartmentHeadsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Department Heads {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
