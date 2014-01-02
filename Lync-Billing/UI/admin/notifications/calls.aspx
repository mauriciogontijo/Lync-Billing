﻿<%@ Page Title="tBill Admin | Unmarekd Calls Notifications" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="calls.aspx.cs" Inherits="Lync_Billing.ui.admin.notifications.calls" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{UnmarkedCallsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{EmployeeIDFilter}.reset();
                #{SipAccountFilter}.reset();
                #{FullNameFilter}.reset();
                
                #{UnmarkedCallsStore}.clearFilter();
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
                        return filterString(#{EmployeeIDFilter}.getValue(), "EmployeeID", record);
                    }
                });
                 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{SipAccountFilter}.getValue(), "SipAccount", record);
                    }
                });

                f.push({
                    filter: function(record) {
                        return filterString(#{FullNameFilter}.getValue(), "FullName", record);
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

    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='email-unmarked-calls-alert' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="UnmarkedCallsGrid"
                Header="true"
                Title="Users With Unallocated Calls"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                ComponentCls="fix-ui-vertical-align">

                <Store>
                    <ext:Store
                        ID="UnmarkedCallsStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="UnmarkedCallsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="SipAccount" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="EmployeeID" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="FullName" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="Site" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UnmarkedCallsCost" Type="Float" />
                                    <ext:ModelField Name="UnmarkedCallsCount" Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsDuration" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="UnmarkedCallsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="EmployeeIDCol"
                            runat="server"
                            Text="Employee ID"
                            Width="90"
                            DataIndex="EmployeeID"
                            Sortable="true">
                            <HeaderItems>
                                <ext:TextField ID="EmployeeIDFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearEmployeeIDFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column
                            ID="SipAccountCol"
                            runat="server"
                            Text="Sip Account"
                            Width="165"
                            DataIndex="SipAccount"
                            Sortable="true">
                            <HeaderItems>
                                <ext:TextField ID="SipAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column
                            ID="FullNameCol"
                            runat="server"
                            Text="Full Name"
                            Width="190"
                            DataIndex="FullName"
                            Sortable="true">
                            <HeaderItems>
                                <ext:TextField ID="FullNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearFullNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="CallsInformationColum"
                            runat="server"
                            Text="Calls Information"
                            MenuDisabled="true"
                            Sortable="false"
                            Resizable="false">
                            <Columns>
                                <ext:Column
                                    ID="UnmarkedCallsDurationCol"
                                    runat="server"
                                    Text="Duration"
                                    Width="90"
                                    DataIndex="UnmarkedCallsDuration"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Renderer Fn="GetMinutes" />
                                </ext:Column>

                                <ext:Column
                                    ID="UnmarkedCallsCountCol"
                                    runat="server"
                                    Text="Number of Calls"
                                    Width="90"
                                    DataIndex="UnmarkedCallsCount"
                                    MenuDisabled="true"
                                    Sortable="false" />

                                <ext:Column
                                    ID="UnmarkedCallsCostCol"
                                    runat="server"
                                    Text="Cost"
                                    Width="80"
                                    DataIndex="UnmarkedCallsCost"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Renderer Fn="RoundCostsToTwoDecimalDigits" />
                                    </ext:Column>
                            </Columns>
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                
                <TopBar>
                    <ext:Toolbar ID="UnamrkedCalls_FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="SiteName" 
                                ValueField="SiteName"
                                Width="250"
                                Margins="5 390 0 5"
                                FieldLabel="Site"
                                LabelSeparator=":"
                                LabelWidth="25"
                                Editable="true">
                                <Store>
                                    <ext:Store
                                        ID="SitesStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="SitesModel" runat="server">
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
                                                {SiteName}&nbsp;({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetUnmarkedCallsForSite">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button 
                                ID="UnamrkedCalls_EmailAlertButton" 
                                runat="server" 
                                Text="Email Alert" 
                                Icon="EmailAdd" 
                                Margins="5 5 0 0">
                               <DirectEvents>
                                   <Click OnEvent="NotifyUsers">
                                       <EventMask ShowMask="true" />
                                       <ExtraParams>
                                           <ext:Parameter Name="Values" Value="Ext.encode(#{UnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw"/>
                                       </ExtraParams>
                                   </Click>
                                   
                               </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <SelectionModel>
                    <ext:CheckboxSelectionModel 
                        ID="UnmarkedCallsCheckboxSelectionModel"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingBottomBar"
                        runat="server"
                        StoreID="UnmarkedCallsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users with Unmarked Calls {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
