<%@ Page Title="" Language="C#" MasterPageFile="~/ui/AccountingMaster.Master" AutoEventWireup="true" CodeBehind="monthly.aspx.cs" Inherits="Lync_Billing.ui.accounting.reports.monthly" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Monthly Users Report</title>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#accounting-tab').addClass('selected');
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='Div1' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="AccountingToolsSidebar"
                runat="server"
                Height="230"
                Width="180"
                Title="Accounting Tools"
                Collapsed="false"
                Collapsible="true">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Disputes</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../main/disputes.aspx'>Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../reports/monthly.aspx' class="selected">Monthly Reports</a></p>
                            <p><a href='../reports/periodical.aspx'>Periodical Reports</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->

    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Panel
                ID="MonthlyReportsTools"
                runat="server"
                Header="true"
                Title="Generate Monthly Reports"
                Width="740"
                Height="59"
                Layout="AnchorLayout">
                <TopBar>
                    <ext:Toolbar
                        ID="FilterAndSearthToolbar"
                        runat="server">
                        <Items>
                            <ext:TextField
                                ID="UserSearch"
                                runat="server"
                                Icon="UserMagnify"
                                TriggerAction="All"
                                QueryMode="Local"
                                EmptyText="User's Sip Account"
                                Width="210"
                                Height="22"
                                FieldLabel="Search User:"
                                LabelWidth="70"
                                Margins="5 25 0 5" />

                            <ext:DateField 
                                ID="reportDateField"
                                runat="server" 
                                Vtype="daterange"
                                FieldLabel="Filter By Date:"
                                LabelWidth="80"
                                EmptyText="Empty Date"
                                Width="220"
                                Margins="5 170 0 5"
                                Format="Y-m">
                            </ext:DateField>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>

            <div class="h5 clear"></div>

            <ext:GridPanel
                ID="MonthlyReportsGrids"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">
                 <asp:ObjectDataSource
                    ID="MonthlyReportsDataSource"
                    runat="server"
                    OnSelecting="MonthlyReportsDataSource_Selecting"
                    OnSelected="MonthlyReportsDataSource_Selected"
                    SelectMethod="GetMonthlyReportsFilter"
                    TypeName="Lync_Billing.ui.accounting.reports.monthly">
                    <SelectParameters>
                        <asp:Parameter Name="start" Type="Int32" />
                        <asp:Parameter Name="limit" Type="Int32" />
                        <asp:Parameter Name="sort" Type="Object" />
                        <asp:Parameter Name="count" Direction="Output" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <Store>
                    <ext:Store
                        ID="MonthlyReportsStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Proxy>
                            <ext:PageProxy CacheString="" />
                        </Proxy>
                        <Model>
                            <ext:Model ID="MonthlyReportsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="EmployeeID" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="Cost" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel ID="MonthlyReportsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:Column
                            ID="UserID"
                            runat="server"
                            Text="Employee ID"
                            Width="160"
                            DataIndex="UserID" />

                        <ext:Column
                            ID="SipAccount"
                            runat="server"
                            Text="Sip Account"
                            Width="160"
                            DataIndex="SipAccount" />

                        <ext:Column
                            ID="UserName"
                            runat="server"
                            Text="Full Name"
                            Width="200"
                            DataIndex="UserName" />

                        <ext:Column
                            ID="Cost"
                            runat="server"
                            Text="Cost"
                            Width="160"
                            DataIndex="Cost" />
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <%--<ext:CheckboxSelectionModel ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>--%>
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
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>