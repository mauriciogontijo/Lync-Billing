<%@ Page Title="eBill User | View Telephony Rates" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="rates.aspx.cs" Inherits="Lync_Billing.ui.user.rates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div id='generate-report-block' class='block float-right wauto h100p'>

        <div class="block-body pt5">

            <ext:GridPanel
                ID="ViewRatesGrid"
                runat="server"
                Width="740"
                Height="740"
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
                            Width="220"
                            DataIndex="CountryName">
                        </ext:Column>

                        <ext:Column
                            runat="server"
                            Text="Country Code"
                            Width="150"
                            DataIndex="CountryCode">
                        </ext:Column>

                        <ext:Column
                            runat="server"
                            Text="Fixedline Rate"
                            Width="160"
                            DataIndex="FixedLineRate">
                        </ext:Column>

                        <ext:Column
                            runat="server"
                            Text="Mobile Rate"
                            Width="160"
                            DataIndex="MobileLineRate">
                        </ext:Column>

                    </Columns>
                </ColumnModel>

                <Features>
                    <ext:GridFilters ID="TelephonyRatesGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="CountryCode" />
                            <ext:StringFilter DataIndex="CountryName" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

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
                                FieldLabel="Choose Gateway"
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
                        DisplayMsg="Delegates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
