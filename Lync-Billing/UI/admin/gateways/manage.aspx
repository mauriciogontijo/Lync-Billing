<%@ Page Title="eBill Admin | Edit Gateways " Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="manage.aspx.cs" Inherits="Lync_Billing.ui.admin.gateways.manage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Edit Gateways</title>
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
                            <p><a href='../notifications/calls.aspx'>Unmarked Calls</a></p>
                            <p><a href='../notifications/bills.aspx'>Users Bills</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Gateways</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../gateways/manage.aspx' class="selected">Manage Gateways</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->

    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='edit-gateways' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Store 
                ID="GatewaysRatesStore" 
                runat="server">
                <Model>
                    <ext:Model ID="GatewaysRatesModel" runat="server">
                        <Fields>
                            <ext:ModelField Name="GatewaysRatesID" />
                            <ext:ModelField Name="GatewayID" />
                            <ext:ModelField Name="RatesTableName" />
                            <ext:ModelField Name="StartingDate" />
                            <ext:ModelField Name="EndingDate" />
                            <ext:ModelField Name="ProviderName" />
                            <ext:ModelField Name="CurrencyCode" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Panel 
                ID="ManageGatewaysForm"
                runat="server" 
                Title="Manage Gateways" 
                PaddingSummary="5px 5px 0"
                Width="740"
                Frame="true"
                Height="635"
                ButtonAlign="Center">
                <TopBar>
                    <ext:Toolbar
                        ID="EditGatewaysToolbar"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="GatewaysComboBox"
                                runat="server"
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="GatewayName" 
                                ValueField="GatewayId"
                                FieldLabel="Select Gateway"
                                LabelWidth="85"
                                Width="300"
                                Margins="5 355 5 5">
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
                                    <Change OnEvent="GetDetails" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button 
                                ID="SaveGatewayButton" 
                                runat="server" 
                                Text="Save" 
                                Icon="ApplicationFormAdd" 
                                OnDirectClick="SaveGatewayButton_DirectClick"/>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <Items>
                    <ext:Container ID="Container1" runat="server" Layout="Column" Height="15" Padding="5"></ext:Container>

                    <ext:Container ID="GatewayRatesFiels" runat="server" Layout="Column" Height="140">
                        <Items>
                            <ext:Container ID="GatewayRatesLableContainer" runat="server" Layout="FormLayout" ColumnWidth="1" Padding="5">
                                <Items>
                                    <ext:Label
                                        ID="GatewayRatesLable" 
                                        runat="server" 
                                        Text="Gateway's Rates Information:" 
                                        Cls="bold" />
                                </Items>
                            </ext:Container>

                            <ext:Container ID="FirstColumn" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:DateField 
                                        ID="StartingDate" 
                                        runat="server" 
                                        FieldLabel="Starting Date" 
                                        EmptyText="Empty Starting Date" 
                                        LabelAlign="Top"
                                        ValidateBlank="true"
                                        ValidateOnChange="true"
                                        ReadOnly="true" />
                                    
                                    <ext:TextField 
                                        ID="ProviderName" 
                                        runat="server" 
                                        FieldLabel="Provider Name" 
                                        EmptyText="VODAFONE, OTE, STC, ..." 
                                        LabelAlign="Top"  
                                        ValidateBlank="true"
                                        ValidateOnChange="true"/>
                                </Items>
                            </ext:Container>

                            <ext:Container ID="SecondColumn" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:DateField 
                                        ID="EndingDate" 
                                        runat="server" 
                                        FieldLabel="Ending Date" 
                                        EmptyText="Readonly!" 
                                        LabelAlign="Top"
                                        ReadOnly="true" />
                                    
                                    <%--<ext:TextField 
                                        ID="CurrencyCode" 
                                        runat="server" 
                                        FieldLabel="Currency Code" 
                                        EmptyText="EUR, USD, GBP, ..." 
                                        LabelAlign="Top" 
                                        ValidateBlank="true"
                                        ValidateOnChange="true"
                                        ValidateOnBlur="true" />--%>

                                    <ext:ComboBox 
                                        ID="CurrenciesCodesCombobox" 
                                        runat="server" 
                                        FieldLabel="Curreny Code" 
                                        LabelWidth="100" 
                                        LabelAlign="Top"
                                        DisplayField="CurrencyName" 
                                        ValueField="CurrencyISOName" 
                                        TriggerAction="All" 
                                        QueryMode="Local"
                                        ValidateBlank="true"
                                        ValidateOnChange="true">
                                        <Store>
                                            <ext:Store
                                                ID="CurrenciesCodesComboboxStore"
                                                runat="server">
                                                <Model>
                                                    <ext:Model ID="CurrencyModel" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="CountryName" />
                                                            <ext:ModelField Name="CurrencyName" />
                                                            <ext:ModelField Name="CurrencyISOName" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>            
                                            </ext:Store>
                                        </Store>

                                        <ListConfig>
                                            <ItemTpl ID="ItemTpl1" runat="server">
                                                <Html>
                                                    <div data-qtip="{CountryName}. {CurrencyISOName}">
                                                        {CountryName} ({CurrencyISOName})
                                                    </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>

                    <ext:Container ID="Container3" runat="server" Layout="Column" Height="10" Padding="5"></ext:Container>

                    <ext:Container ID="GatewayFieldsContainer" runat="server" Layout="Column" Height="80">
                        <Items>
                            <ext:Container ID="Container2" runat="server" Layout="FormLayout" ColumnWidth="1" Padding="5">
                                <Items>
                                    <ext:Label 
                                        ID="GatewayInformationLableContainer" 
                                        runat="server" 
                                        Text="Gateway Information:" 
                                        Cls="bold" />
                                </Items>
                            </ext:Container>

                            <ext:Container ID="Container7" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:ComboBox 
                                        ID="SitesComboBox" 
                                        runat="server" 
                                        FieldLabel="Site Name" 
                                        LabelWidth="100" 
                                        LabelAlign="Top"
                                        DisplayField="SiteName" 
                                        ValueField="SiteID" 
                                        TriggerAction="All" 
                                        QueryMode="Local"
                                        ValidateBlank="true"
                                        ValidateOnChange="true" >
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
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            
                            <ext:Container ID="Container5" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:ComboBox 
                                        ID="PoolComboBox" 
                                        runat="server" 
                                        FieldLabel="Pool FQDN" 
                                        LabelWidth="100" 
                                        LabelAlign="Top"
                                        DisplayField="PoolFQDN" 
                                        ValueField="PoolID" 
                                        TriggerAction="All" 
                                        QueryMode="Local"
                                        ValidateBlank="true"
                                        ValidateOnChange="true" >
                                        <Store>
                                            <ext:Store
                                                ID="PoolStore"
                                                runat="server">
                                                <Model>
                                                    <ext:Model ID="PoolModel" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="PoolID" />
                                                            <ext:ModelField Name="PoolFQDN" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>            
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>

                    <ext:Container ID="GatewayDescriptionContainer" runat="server" Layout="FormLayout" Height="300" Padding="5">
                        <Items>
                            <ext:TextArea
                                ID="GatewayDescription" 
                                runat="server" 
                                Height="300" 
                                FieldLabel="Gateway Description" 
                                LabelAlign="Top" 
                                ValidateBlank="true"
                                ValidateOnChange="true"/>
                        </Items>
                    </ext:Container>
                </Items>
                
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
