<%@ Page Title="eBill Admin | Manage Rates" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="rates.aspx.cs" Inherits="Lync_Billing.ui.admin.gateways.rates" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ManageRatesGrid}.getStore();
                store.filterBy(getRecordFilter());
            };
             
            var clearFilter = function () {
                #{CountryCodeFilter}.reset();
                
                #{ManageRatesStore}.clearFilter();
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
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="ManageRatesGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Gateways Rates">

                <Store>
                    <ext:Store
                        ID="ManageRatesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageRatesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="RateID" Type="Int" />
                                    <ext:ModelField Name="CountryCode" Type="String" />
                                    <ext:ModelField Name="CountryName" Type="String" />
                                    <ext:ModelField Name="FixedLineRate" Type="String" />
                                    <ext:ModelField Name="MobileLineRate" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>

                <Features>
                    <ext:GridFilters ID="ManageDelegatesGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="CountryCode" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

                <ColumnModel ID="ManageRatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="35" />

                        <ext:Column
                            ID="RateIDCol"
                            runat="server"
                            Text="ID"
                            Width="160"
                            DataIndex="RateID"
                            Visible="false" />

                         <ext:Column
                            ID="CountryNameCol"
                            runat="server"
                            Text="Country Name"
                            Width="210"
                            DataIndex="CountryName">
                             <HeaderItems>
                                <ext:TextField ID="CountryNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryNameFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                         </ext:Column>

                        <ext:Column
                            ID="CountryCodeCol"
                            runat="server"
                            Text="Code"
                            Width="110"
                            DataIndex="CountryCode">
                            <HeaderItems>
                                <ext:TextField ID="CountryCodeFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryCodeFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="ManageRatesGroupedColumn"
                            runat="server"
                            Text="Telephony Rates"
                            Resizable="false"
                            Sortable="false"
                            Groupable="false"
                            MenuDisabled="true"
                            Align="Center">
                            <Columns>
                                <ext:Column
                                    ID="FixedlineRateCol"
                                    runat="server"
                                    Text="Fixedline Rate"
                                    Width="155"
                                    DataIndex="FixedLineRate">
                                    <Editor>
                                        <ext:TextField
                                            ID="FixedLineRateTextbox"
                                            runat="server"
                                            DataIndex="FixedLineRate" />
                                    </Editor>
                                </ext:Column>

                                <ext:Column
                                    ID="MobileLineRateCol"
                                    runat="server"
                                    Text="Mobile Rate"
                                    Width="155"
                                    DataIndex="MobileLineRate">
                                    <Editor>
                                        <ext:TextField
                                            ID="MobileLineRateTextbox"
                                            runat="server"
                                            DataIndex="MobileLineRate" />
                                    </Editor>
                                </ext:Column>
                            </Columns>
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
                    <ext:Toolbar ID="FilterGatewaysRatesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterGatewaysBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="SiteName" 
                                ValueField="SiteID"
                                Width="200"
                                Margins="5 15 0 5"
                                FieldLabel="Site"
                                LabelSeparator=":"
                                LabelWidth="30"
                                ValidateBlank="true"
                                ValidateOnChange="true">
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
                                                {SiteName} ({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                                <DirectEvents>
                                    <Change OnEvent="GetGatewaysForSite" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:ComboBox
                                ID="FilterRatesByGateway"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="GatewayName"
                                ValueField="GatewayId"
                                FieldLabel="Gateway"
                                LabelWidth="50"
                                Width="230"
                                Margins="5 45 0 5"
                                Disabled="true">
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
                                    <Select OnEvent="GetRates" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save Changes"
                                Icon="ApplicationEdit"
                                Margins="5 10 0 5">
                                <DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent" before="return #{ManageRatesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageRatesStore}.getChangedData()" Mode="Raw" />
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
                        ID="ManageRatesPagingToolbar"
                        runat="server"
                        StoreID="ManageRatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Delegates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
