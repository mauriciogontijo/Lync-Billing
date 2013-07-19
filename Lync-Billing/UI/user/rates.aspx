<%@ Page Title="eBill User | View Telephony Rates" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="rates.aspx.cs" Inherits="Lync_Billing.ui.user.rates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ViewRatesGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{CountryCodeFilter}.reset();
                #{CountryNameFilter}.reset();
                
                #{ViewRatesStore}.clearFilter();
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
                        return filterString(#{CountryCodeFilter}.getValue(), "CountryCode", record);
                    }
                });
                 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{CountryNameFilter}.getValue(), "CountryName", record);
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
    <div id='generate-report-block' class='block float-right wauto h100p'>

        <div class="block-body pt5">

            <ext:GridPanel
                ID="ViewRatesGrid"
                runat="server"
                Width="740"
                Height="760"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="View Telephony Rates">

                <Store>
                    <ext:Store
                        ID="ViewRatesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ViewRatesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="RateID" Type="Int" />
                                     <ext:ModelField Name="CountryName" Type="String" />
                                    <ext:ModelField Name="CountryCode" Type="String" />
                                    <ext:ModelField Name="FixedLineRate" Type="String" />
                                    <ext:ModelField Name="MobileLineRate" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Features>
                    <ext:GridFilters ID="TelephonyRatesGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="CountryCode" />
                            <ext:StringFilter DataIndex="CountryName" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

                <ColumnModel ID="ViewRatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn
                            ID="RowNumbererColumn2"
                            runat="server"
                            Width="45" />

                        <ext:Column
                            runat="server"
                            Text="ID"
                            Width="160"
                            DataIndex="RateID"
                            Visible="false" />

                         <ext:Column 
                            runat="server"
                            Text="Country"
                            Width="230"
                            DataIndex="CountryName">
                            <HeaderItems>
                                <ext:TextField ID="CountryNameFilter"
                                    runat="server"
                                    Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column
                            runat="server"
                            Text="Country Code"
                            Width="140"
                            DataIndex="CountryCode">
                            <HeaderItems>
                                <ext:TextField ID="CountryCodeFilter"
                                    runat="server"
                                    Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryCodeFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="RatesColumn"
                            runat="server"
                            Text="Rates"
                            Align="Center"
                            MenuDisabled="true"
                            Resizable="false"
                            Groupable="false"
                            Sortable="false">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    Text="Fixedline Rate"
                                    Width="160"
                                    DataIndex="FixedLineRate" />

                                <ext:Column
                                    runat="server"
                                    Text="Mobile Rate"
                                    Width="160"
                                    DataIndex="MobileLineRate" />
                            </Columns>
                        </ext:Column>

                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterRatesByGateway"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="GatewayName"
                                ValueField="GatewayId"
                                FieldLabel="Choose Provider"
                                LabelWidth="90"
                                Width="300"
                                Margins="5 200 0 5">
                                <Store>
                                    <ext:Store
                                        ID="GatewaysStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="GatewaysModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="GatewayId" />
                                                    <ext:ModelField Name="GatewayName" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Change OnEvent="GetRates" />
                                </DirectEvents>
                            </ext:ComboBox>

                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ViewRatesPagingToolbar"
                        runat="server"
                        StoreID="ViewRatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Telephony Rates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
