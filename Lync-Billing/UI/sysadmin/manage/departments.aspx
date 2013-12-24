<%@ Page Title="tBill Admin | Manage Departments" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="departments.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.manage.departments" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var ViewUpperCase = function (sourceString) {
            if (typeof sourceString !== undefined) {
                return sourceString.toString().toUpperCase();
            } else {
                return "";
            }
        }
    </script>

    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ManageDepartmentsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{DepartmentNameFilter}.reset();
                #{DescriptionFilter}.reset();
                
                #{ManageDepartmentsGridStore}.clearFilter();
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
                        return filterString(#{DepartmentNameFilter}.getValue(), "DepartmentName", record);
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

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            
            <ext:GridPanel
                ID="ManageDepartmentsGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Delegates"
                ComponentCls="fix-ui-vertical-align">

                <Store>
                    <ext:Store
                        ID="ManageDepartmentsGridStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageDepartmentsGridStoreModel" runat="server" IDProperty="DepartmentID">
                                <Fields>
                                    <ext:ModelField Name="SiteID" Type="String" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="DepartmentID" Type="Int" />
                                    <ext:ModelField Name="DepartmentName" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:RowEditing ID="ManageDepartments_RowEditing" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ManageDepartmentsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="DepartmentNameColumn"
                            runat="server"
                            Text="Department Name"
                            Width="280"
                            DataIndex="DepartmentName"
                            Sortable="false"
                            Groupable="false">

                            <HeaderItems>
                                <ext:TextField ID="DepartmentNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDepartmentFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Editor>
                                <ext:TextField
                                    ID="Editor_DepartmentNameTextField"
                                    runat="server"
                                    DataIndex="DepartmentName" />
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="DescriptionColumn"
                            runat="server"
                            Text="Description"
                            Width="380"
                            DataIndex="Description"
                            Sortable="false"
                            Groupable="false">

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

                            <Editor>
                                <ext:TextField
                                    ID="Editor_DescriptionTextField"
                                    runat="server"
                                    DataIndex="Description" />
                            </Editor>
                        </ext:Column>

                        <ext:CommandColumn ID="RejectChange" runat="server" Width="70">
                            <Commands>
                                <ext:GridCommand Text="Reject" ToolTip-Text="Reject row changes" CommandName="reject" Icon="ArrowUndo" />
                            </Commands>
                            <PrepareToolbar Handler="toolbar.items.get(0).setVisible(record.dirty);" />
                            <Listeners>
                                <Command Handler="record.reject();" />
                            </Listeners>
                        </ext:CommandColumn>
                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesDepartmentsToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterDepartmentsBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="SiteName"
                                ValueField="SiteID"
                                FieldLabel="Site"
                                LabelWidth="25"
                                Margins="5 5 0 5"
                                Width="250">
                                <Store>
                                    <ext:Store
                                        ID="FilterDepartmentsBySiteStore"
                                        runat="server"
                                        OnLoad="FilterDepartmentsBySiteStore_Load">
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
                                    <Select OnEvent="GetDepartmentsPerSite" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New Department"
                                Icon="Add"
                                Margins="5 5 0 240">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddNewDepartmentWindowPanel" />
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
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageDepartmentsGridStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDepartmentsGridStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Window 
                                ID="AddNewDepartmentWindowPanel" 
                                runat="server" 
                                Frame="true"
                                Resizable="false"
                                Title="New Department" 
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
                                        ID="NewDepartment_SitesList"
                                        runat="server"
                                        DisplayField="SiteName"
                                        ValueField="SiteID"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        Editable="true"
                                        EmptyText="Please Select Site"
                                        FieldLabel="Site"
                                        LabelSeparator=":"
                                        LabelWidth="115"
                                        Width="350"
                                        AllowBlank="false">
                                        <Store>
                                            <ext:Store ID="NewDepartment_SitesListStore" runat="server" OnLoad="NewDepartment_SitesListStore_Load">
                                                <Model>
                                                    <ext:Model ID="NewDepartment_SitesListStoreModel" runat="server">
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
                                            <ItemTpl ID="NewDepartment_SitesListStoreTpl" runat="server">
                                                <Html>
                                                    <div>{SiteName}&nbsp;({CountryCode})</div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>

                                    <ext:TextField
                                        ID="NewDepartment_DepartmentName"
                                        runat="server"
                                        EmptyText="Empty Department Name"
                                        Width="350"
                                        FieldLabel="Department Name"
                                        LabelSeparator=":"
                                        LabelWidth="115"
                                        AllowBlank="false" />

                                    <ext:TextField
                                        ID="NewDepartment_Description"
                                        runat="server"
                                        EmptyText="Empty Description"
                                        Width="350"
                                        FieldLabel="Description"
                                        LabelSeparator=":"
                                        LabelWidth="115" />
                                </Items>

                                <BottomBar>
                                    <ext:StatusBar
                                        ID="NewDepartmentWindowBottomBar"
                                        runat="server"
                                        StatusAlign="Right"
                                        DefaultText=""
                                        Height="30">
                                        <Items>
                                            <ext:Button
                                                ID="AddNewDepartmentButton"
                                                runat="server"
                                                Text="Add"
                                                Icon="ApplicationFormAdd"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="AddNewDepartmentButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button
                                                ID="CancelNewDepartmentButton"
                                                runat="server"
                                                Text="Cancel"
                                                Icon="Cancel"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="CancelNewDepartmentButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:ToolbarSeparator
                                                ID="NewDepartmentWindow_BottomBarSeparator"
                                                runat="server" />

                                            <ext:ToolbarTextItem
                                                ID="NewDepartment_StatusMessage"
                                                runat="server"
                                                Height="15"
                                                Text=""
                                                Margins="0 0 0 5" />
                                        </Items>
                                    </ext:StatusBar>
                                </BottomBar>

                                <DirectEvents>
                                    <BeforeHide OnEvent="AddNewDepartmentWindowPanel_BeforeHide" />
                                </DirectEvents>
                            </ext:Window>

                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDepartmentsPagingToolbar"
                        runat="server"
                        StoreID="ManageDepartmentsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Departments {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
