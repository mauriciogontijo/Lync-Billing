<%@ Page Title="eBill Accounting | Monthly Reports" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="monthly.aspx.cs" Inherits="Lync_Billing.ui.accounting.reports.monthly" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill Accounting | Monthly Reports</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Hidden ID="FormatType" runat="server" />
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
                            <ext:DateField 
                                ID="reportDateField"
                                runat="server" 
                                FieldLabel="Choose Date:"
                                LabelWidth="80"
                                EmptyText="Empty Date"
                                Width="250"
                                Margins="0 10 0 5">
                            </ext:DateField>

                            <ext:Button
                                ID="ViewMonthlyBills"
                                runat="server"
                                Icon="UserMagnify"
                                Text="Create Report"
                                Width="120"
                                Height="22"
                                FieldLabel=""
                                LabelWidth="70"
                                OnDirectClick ="ViewMonthlyBills_DirectClick"
                                Margins="0 25 0 5">
                            </ext:Button>
                            
                            <ext:Button 
                                runat="server"
                                Text="To Excel" 
                                Icon="PageExcel"
                                Margins="0 0 5 230">
                                <Listeners>
                                    <Click Handler="submitValue(#{MonthlyReportsGrids}, 'xls');" />
                                </Listeners>
                            </ext:Button>
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
                
                <Store>
                    <ext:Store
                        ID="MonthlyReportsStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25"
                        OnSubmitData="MonthlyReportsStore_SubmitData">
                        <Proxy>
                            <ext:PageProxy CacheString="" />
                        </Proxy>
                        <Model>
                            <ext:Model ID="MonthlyReportsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="EmployeeID" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="String" />
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
                            ID="EmployeeID"
                            runat="server"
                            Text="Employee ID"
                            Width="160"
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
                            Width="200"
                            DataIndex="FullName" />

                        <ext:Column
                            ID="PersonalCallsCost"
                            runat="server"
                            Text="Cost"
                            Width="160"
                            DataIndex="PersonalCallsCost" />
                    </Columns>
                </ColumnModel>
                <Features>
                    <ext:GridFilters>
                        <Filters>
                            <ext:StringFilter DataIndex="EmployeeID" />
                            <ext:StringFilter DataIndex="SipAccount" />
                            <ext:StringFilter DataIndex="FullName" />
                            <ext:NumericFilter DataIndex="Month" />
                            <ext:NumericFilter DataIndex="Year" />
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
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>