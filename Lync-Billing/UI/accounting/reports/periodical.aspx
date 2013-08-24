<%@ Page Title="eBill Accounting | Periodical Reports" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="periodical.aspx.cs" Inherits="Lync_Billing.ui.accounting.reports.periodical" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{PeriodicalReportsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{EmployeeIDFilter}.reset();
                #{SipAccountFilter}.reset();
                #{FullNameFilter}.reset();
                
                #{PeriodicalReportsStore}.clearFilter();
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
                ID="PeriodicalReportsTools"
                runat="server"
                Header="true"
                Title="Generate Periodical Reports"
                Width="740"
                Height="61"
                Layout="AnchorLayout">
                <TopBar>
                    <ext:Toolbar
                        ID="FilterAndSearthToolbar"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterReportsBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="SiteName"
                                ValueField="SiteName"
                                FieldLabel="Site:"
                                LabelWidth="25"
                                Width="230"
                                Margins="5 15 0 5">
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

                                <DirectEvents>
                                    <Select OnEvent="FilterReportsBySite_Selecting">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <ListConfig>
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div data-qtip="{SiteName}. {CountryCode}">
                                                {SiteName} ({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                            </ext:ComboBox>

                            <ext:DateField 
                                ID="StartingDate"
                                runat="server" 
                                FieldLabel="From:"
                                LabelWidth="30"
                                EmptyText="Empty Date"
                                Width="130"
                                Margins="5 10 5 5"
                                Disabled="true">
                                <DirectEvents>
                                    <Select OnEvent="StartingDate_Selection">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>
                            </ext:DateField>

                            <ext:DateField 
                                ID="EndingDate"
                                runat="server" 
                                FieldLabel="To:"
                                LabelWidth="20"
                                EmptyText="Empty Date"
                                Width="130"
                                Margins="5 40 5 5"
                                Disabled="true">
                                <DirectEvents>
                                    <Select OnEvent="EndingDate_Selection">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>
                            </ext:DateField>

                            <ext:Button
                                ID="ExportPDFReport"
                                runat="server"
                                Text="To PDF"
                                Disabled="true"
                                Icon="PageSave">
                                <Menu>
                                    <ext:Menu ID="Menu1" runat="server">
                                        <Items>
                                            <ext:MenuItem ID="ExportPDFSummaryRreport" runat="server" Text="Summary Report" Icon="PageSave">
                                                <Listeners>
                                                    <Click Handler="submitValue(#{PeriodicalReportsGrid}, #{FormatType}, 'pdf');" />
                                                </Listeners>
                                            </ext:MenuItem>

                                            <ext:MenuItem ID="ExportPDFDetailedRreport" runat="server" Text="Detailed Report" Icon="PageSave">
                                                <Listeners>
                                                    <Click Handler="submitValue(#{PeriodicalReportsGrid}, #{FormatType}, 'pdf-d');" />
                                                </Listeners>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>

                            <ext:Button
                                ID="ExportExcelReport"
                                runat="server"
                                Text="To Excel"
                                Disabled="true"
                                Icon="PageExcel">
                                <Menu>
                                    <ext:Menu ID="ReportExportOptionsMenu" runat="server">
                                        <Items>
                                            <ext:MenuItem ID="ExportExcelSummaryReport" runat="server" Text="Summary Report" Icon="PageExcel">
                                                <Listeners>
                                                    <Click Handler="submitValue(#{PeriodicalReportsGrid}, #{FormatType}, 'xls');" />
                                                </Listeners>
                                            </ext:MenuItem>

                                            <%--<ext:MenuItem ID="ExportExcelDetailedReport" runat="server" Text="Detailed Report" Icon="PageExcel">
                                            </ext:MenuItem>--%>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>

            <div class="h5 clear"></div>

            <ext:GridPanel
                ID="PeriodicalReportsGrid"
                runat="server"
                Width="740"
                Height="710"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">
                
                <Store>
                    <ext:Store
                        ID="PeriodicalReportsStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25"
                        OnSubmitData="PeriodicalReportsStore_SubmitData">
                        <Model>
                            <ext:Model ID="PeriodicalReportsModel" runat="server" IDProperty="SipAccount">
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

                <ColumnModel ID="PeriodicalReportsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="EmployeeIDCol"
                            runat="server"
                            Text="Employee ID"
                            Width="100"
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
                            ID="SipAccountCol"
                            runat="server"
                            Text="Sip Account"
                            Width="175"
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
                            ID="FullNameCol"
                            runat="server"
                            Text="Full Name"
                            Width="200"
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
                                    Width="85"
                                    DataIndex="PersonalCallsCost" />

                                <ext:Column
                                    ID="BusinessCallsCostCol"
                                    runat="server"
                                    Text="Business"
                                    Width="85"
                                    DataIndex="BusinessCallsCost" />

                                <ext:Column
                                    ID="UnmarkedCallsCostCol"
                                    runat="server"
                                    Text="Unallocated"
                                    Width="85"
                                    DataIndex="UnmarkedCallsCost" />
                            </Columns>
                        </ext:Column>
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

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PeriodicalReportsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>