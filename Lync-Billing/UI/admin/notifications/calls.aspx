<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="calls.aspx.cs" Inherits="Lync_Billing.ui.admin.notifications.calls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Calls Notifications</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='accountint-dashboard-sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="AdminToolsSidebar"
                runat="server"
                Height="230"
                Width="180"
                Title="Admin Tools"
                Collapsed="false"
                Collapsible="true">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Email Notifications</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../notifications/calls.aspx' class="selected">Unmarked Calls</a></p>
                            <p><a href='../notifications/bills.aspx'>Users Bills</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->

    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='email-unmarked-calls-alert' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="UnmarkedCallsGrid"
                Header="true"
                Title="Users With Unmarked Calls"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">
                <Store>
                    <ext:Store
                        ID="UnmarkedCallsStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="false">
                        <Model>
                            <ext:Model ID="UnmarkedCallsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="EmployeeID" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="Site" Type="String" />
                                    <ext:ModelField Name="UnmarkedCallsCost" Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsCount" Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsDuration" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="UnmarkedCallsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:Column
                            ID="EmployeeID"
                            runat="server"
                            Text="Employee ID"
                            Width="80"
                            DataIndex="EmployeeID" />

                        <ext:Column
                            ID="SipAccount"
                            runat="server"
                            Text="Sip Account"
                            Width="160"
                            DataIndex="SipAccount" />

                        <ext:Column
                            ID="FullName"
                            runat="server"
                            Text="Full Name"
                            Width="180"
                            DataIndex="FullName" />

                        <ext:Column
                            ID="UnmarkedCallsDuration"
                            runat="server"
                            Text="Duration"
                            Width="110"
                            DataIndex="UnmarkedCallsDuration" />

                        <ext:Column
                            ID="UnmarkedCallsCount"
                            runat="server"
                            Text="Count"
                            Width="70"
                            DataIndex="UnmarkedCallsCount" />

                        <ext:Column
                            ID="UnmarkedCallsCost"
                            runat="server"
                            Text="Cost"
                            Width="70"
                            DataIndex="UnmarkedCallsCost" />
                    </Columns>
                </ColumnModel>
                
                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="TypeName" 
                                ValueField="TypeValue"
                                Margins="5 5 5 5">
                                <%--<Store>
                                    <ext:Store
                                        ID="FilterUsersBySiteStore"
                                        runat="server"
                                        IsPagingStore="false">
                                    </ext:Store>
                                </Store>
                                 <DirectEvents>
                                     <Select OnEvent="PhoneCallsHistoryFilter" />
                                 </DirectEvents>--%>
                            </ext:ComboBox>

                            <ext:Button ID="EmailAlertButton" runat="server" Text="Email Alert" Icon="EmailAdd" Margins="5 5 5 485">
                                 <Listeners>
                                    <Click Handler="submitValue(#{PhoneCallsHistoryGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <Features>
                    <ext:GridFilters>
                        <Filters>
                            <ext:StringFilter DataIndex="EmployeeID" />
                            <ext:StringFilter DataIndex="SipAccount" />
                            <ext:StringFilter DataIndex="FullName" />
                            <ext:StringFilter DataIndex="Site" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                
                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PhoneCallStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Phone Calls {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
