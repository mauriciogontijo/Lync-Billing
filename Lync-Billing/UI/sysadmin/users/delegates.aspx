<%@ Page Title="tBill Admin | Manage Delegates" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="delegates.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.users.delegates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ManageDelegatesColumnModel" runat="server" Flex="1">
                    <Columns>

                        <ext:Column ID="Column1"
                            runat="server"
                            Text="User"
                            Width="150"
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
                            <Editor>
                                <ext:TextField
                                    ID="SipAccountTextbox"
                                    runat="server"
                                    DataIndex="SipAccount" />
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="Column4"
                            runat="server"
                            Text="Site"
                            Width="120"
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

                            <Editor>
                                <ext:ComboBox
                                    ID="SitesList"
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
                                    Width="130">
                                    <Store>
                                        <ext:Store 
                                            ID="Store1"
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
                            </Editor>
                        </ext:Column>


                        <ext:Column ID="Column5"
                            runat="server"
                            Text="Department"
                            Width="130"
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
                            
                            <Editor>
                                <ext:ComboBox
                                    ID="DepartmentsList"
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
                                    Width="130">
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
                            </Editor>

                            <%--<Renderer Fn="" />--%>
                        </ext:Column>

                        <ext:Column ID="Column2"
                            runat="server"
                            Text="Delegee User"
                            Width="140"
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
                            <Editor>
                                <ext:TextField
                                    ID="DelegeeAccountTextbox"
                                    runat="server"
                                    DataIndex="DelegeeAccount" />
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="Column3"
                            runat="server"
                            Text="Description"
                            Width="100"
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
                            <Editor>
                                <ext:TextField
                                    ID="DescriptionTextbox"
                                    runat="server"
                                    DataIndex="Description" />
                            </Editor>
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

                         <ext:CommandColumn ID="RejectChange" runat="server" Width="65">
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
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save Changes"
                                Icon="ApplicationEdit"
                                Margins="5 10 0 250">
                                <DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent" before="return #{ManageDelegatesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDelegatesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelChangesButton"
                                Text="Cancel Changes"
                                Icon="Cancel"
                                runat="server"
                                Margins="5 0 0 0">
                                <DirectEvents>
                                    <Click OnEvent="RejectChanges_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
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
