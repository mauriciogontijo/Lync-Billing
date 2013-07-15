<%@ Page Title="eBill Accounting | Periodical Reports" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="periodical.aspx.cs" Inherits="Lync_Billing.ui.accounting.reports.periodical" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
  

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Hidden ID="FormatType" runat="server" />
            <ext:Panel
                ID="PeriodicalReportsTools"
                runat="server"
                Header="true"
                Title="Generate Periodical Reports"
                Width="740"
                Height="63"
                Layout="AnchorLayout">
                <TopBar>
                    <ext:Toolbar
                        ID="FilterAndSearthToolbar"
                        runat="server">
                        <Items>
                            <ext:DateField 
                                ID="StartingDate"
                                runat="server" 
                                FieldLabel="From:"
                                LabelWidth="30"
                                EmptyText="Empty Date"
                                Width="200"
                                Margins="5 10 5 5">
                            </ext:DateField>

                            <ext:DateField 
                                ID="EndingDate"
                                runat="server" 
                                FieldLabel="To:"
                                LabelWidth="20"
                                EmptyText="Empty Date"
                                Width="200"
                                Margins="0 10 5 5">
                            </ext:DateField>

                            <ext:Button
                                ID="ViewMonthlyBills"
                                runat="server"
                                Icon="UserMagnify"
                                Text="Create Report"
                                Height="22"
                                FieldLabel=""
                                OnDirectClick ="ViewMonthlyBills_DirectClick"
                                Margins="5 110 5 5">
                            </ext:Button>

                            <ext:Label
                                ID="ExportReportsButtonGroupLabel"
                                runat="server"
                                Html="Export Report:"
                                Width="80"
                                Margins="10 0 0 0" />

                            <ext:ButtonGroup
                                ID="ExportReportsButtonGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="255"
                                Frame="false"
                                ButtonAlign="Left"
                                Margins="5 0 0 0">
                                <Buttons>
                                    <ext:Button ID="ExportSummaryReportButton" 
                                        runat="server"
                                        Text="Summary" 
                                        Icon="PageExcel">
                                        <Listeners>
                                            <Click Handler="submitValue(#{PeriodicalReportsGrid}, 'xls');" />
                                        </Listeners>
                                    </ext:Button>

                                    <ext:Button ID="ExportDetailedReportButton" 
                                        runat="server"
                                        Text="Detailed" 
                                        Icon="PageExcel"
                                        OnDirectClick="ExportDetailedReportButton_DirectClick">
                                       <%-- <Listeners>
                                            <Click Handler="submitValue(#{PeriodicalReportsGrid}, 'xls');" />
                                        </Listeners>--%>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>

            <div class="h5 clear"></div>

            <ext:GridPanel
                ID="PeriodicalReportsGrid"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">
                
                <Store>
                    <ext:Store
                        ID="PeriodicalReportsStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25"
                        OnSubmitData="PeriodicalReportsStore_SubmitData">
                        <Model>
                            <ext:Model ID="PeriodicalReportsModel" runat="server" IDProperty="SipAccount">
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

                <ColumnModel ID="PeriodicalReportsColumnModel" runat="server" Flex="1">
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