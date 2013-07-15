<%@ Page Title="eBill Accounting | Monthly Reports" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="monthly.aspx.cs" Inherits="Lync_Billing.ui.accounting.reports.monthly" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{MonthlyReportsGrids}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{EmployeeIDFilter}.reset();
                #{SipAccountFilter}.reset();
                #{FullNameFilter}.reset();
                
                #{MonthlyReportsStore}.clearFilter();
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
                Height="65"
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
                                Margins="5 10 5 5">
                            </ext:DateField>

                            <ext:Button
                                ID="ViewMonthlyBills"
                                runat="server"
                                Icon="UserMagnify"
                                Text="Create Report"
                                Width="100"
                                Height="22"
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
                                            <Click Handler="submitValue(#{MonthlyReportsGrids}, 'xls');" />
                                        </Listeners>
                                    </ext:Button>

                                    <ext:Button ID="ExportDetailedReportButton" 
                                        runat="server"
                                        Text="Detailed" 
                                        Icon="PageExcel"
                                        OnDirectClick="ExportDetailedReportButton_DirectClick">
                                       <%-- <Listeners>
                                            <Click Handler="submitValue(#{MonthlyReportsGrids}, 'xls');" />
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
                        <Model>
                            <ext:Model ID="MonthlyReportsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="EmployeeID" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="String" />
                                    <ext:ModelField Name="BusinessCallsCost" Type="String" />
                                    <ext:ModelField Name="UnmarkedCallsCost" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <Features>
                    <ext:GridFilters ID="MonthlyReportsGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="EmployeeID" />
                            <ext:StringFilter DataIndex="SipAccount" />
                            <ext:StringFilter DataIndex="FullName" />
                        </Filters>
                    </ext:GridFilters>
                </Features>
                
                <ColumnModel ID="MonthlyReportsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:Column
                            ID="EmployeeID"
                            runat="server"
                            Text="Employee ID"
                            Width="90"
                            DataIndex="EmployeeID">
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
                            ID="SipAccount"
                            runat="server"
                            Text="Sip Account"
                            Width="160"
                            DataIndex="SipAccount">
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
                            ID="FullName"
                            runat="server"
                            Text="Full Name"
                            Width="180"
                            DataIndex="FullName">
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

                        <ext:Column
                            ID="GrouopedCostsColumns"
                            runat="server"
                            MenuDisabled="false"
                            Sortable="false"
                            Groupable="false"
                            Resizable="false"
                            Text="Calls Costs">
                            <Columns>
                                <ext:Column
                                    ID="PersonalCallsCostCol"
                                    runat="server"
                                    Text="Personal"
                                    Width="80"
                                    DataIndex="PersonalCallsCost" />

                                <ext:Column
                                    ID="BusinessCallsCostCol"
                                    runat="server"
                                    Text="Business"
                                    Width="80"
                                    DataIndex="BusinessCallsCost" />

                                <ext:Column
                                    ID="UnmarkedCallsCostCol"
                                    runat="server"
                                    Text="Unallocated"
                                    Width="80"
                                    DataIndex="UnmarkedCallsCost" />
                            </Columns>
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <%--<Features>
                    <ext:GridFilters>
                        <Filters>
                            <ext:StringFilter DataIndex="EmployeeID" />
                            <ext:StringFilter DataIndex="SipAccount" />
                            <ext:StringFilter DataIndex="FullName" />
                            <ext:NumericFilter DataIndex="Month" />
                            <ext:NumericFilter DataIndex="Year" />
                        </Filters>
                    </ext:GridFilters>
                </Features>--%>

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