<%@ Page Title="iBill Admin | Manage Sites" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="sites.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.manage.sites" %>

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
                var store = #{ManageSitesGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{SiteNameFilter}.reset();
                #{CountryNameFilter}.reset();
                #{DescriptionFilter}.reset();
                
                #{ManageSitesGridStore}.clearFilter();
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
                        return filterString(#{SiteNameFilter}.getValue(), "SiteName", record);
                    }
                });
                
                f.push({
                    filter: function(record) {
                        return filterString(#{CountryNameFilter}.getValue(), "CountryName", record);
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
                ID="ManageSitesGrid"
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
                        ID="ManageSitesGridStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25"
                        OnLoad="ManageSitesGridStore_Load">
                        <Model>
                            <ext:Model ID="ManageSitesGridStoreModel" runat="server" IDProperty="SiteID">
                                <Fields>
                                    <ext:ModelField Name="SiteID" Type="Int" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="CountryCode" Type="String" />
                                    <ext:ModelField Name="CountryName" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:RowEditing ID="ManageSites_RowEditing" runat="server" ClicksToEdit="2">
                        <%--<DirectEvents>
                            <BeforeEdit OnEvent="ManageSites_RowEditing_BeforeEdit" />
                        </DirectEvents>--%>
                    </ext:RowEditing>
                </Plugins>

                <ColumnModel ID="ManageSitesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="SiteNameColumn"
                            runat="server"
                            Text="Site Name"
                            Width="150"
                            DataIndex="SiteName"
                            Sortable="false"
                            Groupable="false">

                            <HeaderItems>
                                <ext:TextField ID="SiteNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSiteFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <%--<Editor>
                                <ext:TextField ID="Editor_SiteNameTextField" runat="server" />
                            </Editor>--%>
                        </ext:Column>

                        <ext:Column ID="CountryNameColumn"
                            runat="server"
                            Text="Country Name"
                            Width="280"
                            DataIndex="CountryName"
                            Sortable="false"
                            Groupable="false">
                            
                            <HeaderItems>
                                <ext:TextField ID="CountryNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryNameFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Editor>
                                <ext:ComboBox
                                    ID="Editor_CountryNameCombobox"
                                    runat="server"
                                    TriggerAction="All"
                                    Editable="false"
                                    DisplayField="CountryName"
                                    ValueField="CountryName"
                                    EmptyText="Please Select Country">
                                    <Store>
                                        <ext:Store
                                            ID="Editor_CountryNameComboboxStore"
                                            runat="server"
                                            OnLoad="Editor_CountryNameComboboxStore_Load">
                                            <Model>
                                                <ext:Model ID="Editor_CountryNameComboboxStoreModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="CountryName" Type="String" />
                                                        <ext:ModelField Name="CountryCodeISO2" Type="String" />
                                                        <ext:ModelField Name="CountryCodeISO3" Type="String" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>

                                            <Sorters>
                                                <ext:DataSorter Property="CountryName" Direction="ASC" />
                                            </Sorters>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="DescriptionColumn"
                            runat="server"
                            Text="Description"
                            Width="220"
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
                                <ext:TextField ID="Editor_DescriptionTextField" runat="server" />
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
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <%--<ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New Site"
                                Icon="Add"
                                Margins="5 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddSitePanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator
                                ID="ToolbarSeparaator"
                                runat="server" />--%>

                            <ext:Button
                                ID="SaveChangesButton"
                                runat="server"
                                Text="Save Changes"
                                Icon="DatabaseSave"
                                Margins="5 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageSitesGridStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageSitesGridStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <%--<ext:Window 
                                ID="AddNewSiteWindowPanel" 
                                runat="server" 
                                Frame="true"
                                Resizable="false"
                                Title="New Site" 
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
                                    <ext:TextField
                                        ID="NewSite_SiteName"
                                        runat="server"
                                        AllowBlank="false"
                                        AllowOnlyWhitespace="false"
                                        EmptyText="Empty Site Name"
                                        FieldLabel="Site Name"
                                        LabelSeparator=":"
                                        LabelWidth="70"
                                        Width="380" />

                                    <ext:ComboBox
                                        ID="NewSite_CountryList"
                                        runat="server"
                                        TriggerAction="Query"
                                        QueryMode="Local"
                                        Editable="true"
                                        DisplayField="CountryName"
                                        ValueField="CountryName"
                                        EmptyText="Please Select Country"
                                        FieldLabel="Country"
                                        LabelSeparator=":"
                                        LabelWidth="70"
                                        Width="380"
                                        AllowBlank="false"
                                        AllowOnlyWhitespace="false">
                                        <Store>
                                            <ext:Store ID="NewSite_CountryListStore" runat="server" OnLoad="NewSite_CountryListStore_Load">
                                                <Model>
                                                    <ext:Model ID="NewSite_CountryListStoreModel" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="CountryName" Type="String" />
                                                            <ext:ModelField Name="CountryCodeISO2" Type="String" />
                                                            <ext:ModelField Name="CountryCodeISO3" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>

                                    <ext:TextField
                                        ID="NewSite_Description"
                                        runat="server"
                                        EmptyText="Empty Description"
                                        FieldLabel="Description"
                                        LabelSeparator=":"
                                        LabelWidth="70"
                                        Width="380" />
                                </Items>

                                <BottomBar>
                                    <ext:StatusBar
                                        ID="NewSiteWindowBottomBar"
                                        runat="server"
                                        StatusAlign="Right"
                                        DefaultText=""
                                        Height="30">
                                        <Items>
                                            <ext:Button
                                                ID="AddNewSiteButton"
                                                runat="server"
                                                Text="Add"
                                                Icon="ApplicationFormAdd"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="AddNewSiteButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button
                                                ID="CancelNewSiteButton"
                                                runat="server"
                                                Text="Cancel"
                                                Icon="Cancel"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="CancelNewSiteButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:ToolbarSeparator
                                                ID="NewSiteWindow_BottomBarSeparator"
                                                runat="server" />

                                            <ext:ToolbarTextItem
                                                ID="NewSite_StatusMessage"
                                                runat="server"
                                                Height="15"
                                                Text=""
                                                Margins="0 0 0 5" />
                                        </Items>
                                    </ext:StatusBar>
                                </BottomBar>

                                <DirectEvents>
                                    <BeforeHide OnEvent="AddNewSiteWindowPanel_BeforeHide" />
                                </DirectEvents>
                            </ext:Window>--%>

                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageSitesPagingToolbar"
                        runat="server"
                        StoreID="ManageSitesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Sites {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
