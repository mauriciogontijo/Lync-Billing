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

                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Gateways</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='../gateways/manage.aspx' class="selected">Manage Gateways</a></p>
                            </div>
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
            <ext:Panel 
                ID="ManageGatewaysForm"
                runat="server" 
                Title="ManageGateways" 
                PaddingSummary="5px 5px 0"
                Width="740"
                Height="625"
                ButtonAlign="Center">
                <TopBar>
                    <ext:Toolbar
                        ID="EditGatewaysToolbar"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="SelectGatewayMenu"
                                runat="server"
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="TypeName" 
                                ValueField="TypeValue"
                                FieldLabel="Select Gateway"
                                LabelWidth="85"
                                Width="300"
                                Margins="5 5 5 5">
                            </ext:ComboBox>

                            <ext:Button ID="Button2" runat="server" Text="Save" />
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <Items>
                    <ext:Container ID="Container1" runat="server" Layout="Column" Height="15" Padding="5"></ext:Container>

                    <ext:Container ID="GatewayRatesFiels" runat="server" Layout="Column" Height="140">
                        <Items>
                            <ext:Container ID="GatewayRatesLableContainer" runat="server" Layout="FormLayout" ColumnWidth="1" Padding="5">
                                <Items>
                                    <ext:Label ID="GatewayRatesLable" runat="server" Text="Gateway's Rates Information:" Cls="bold" />
                                </Items>
                            </ext:Container>

                            <ext:Container ID="FirstColumn" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:DateField ID="StartingDate" runat="server" FieldLabel="Starting Date" EmptyText="Empty Starting Date" LabelAlign="Top" />
                                    <ext:TextField ID="ProviderName" runat="server" FieldLabel="ProviderName" EmptyText="VODAFONE, OTE, STC, ..." LabelAlign="Top"  />
                                </Items>
                            </ext:Container>

                            <ext:Container ID="SecondColumn" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:DateField ID="EndingDate" runat="server" FieldLabel="Ending Date" EmptyText="Empty Ending Date" LabelAlign="Top" />
                                    <ext:TextField ID="CurrencyCode" runat="server" FieldLabel="CurrencyCode" EmptyText="EUR, USD, GBP, ..." LabelAlign="Top"  />
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>

                    <ext:Container ID="Container3" runat="server" Layout="Column" Height="10" Padding="5"></ext:Container>

                    <ext:Container ID="GatewayFieldsContainer" runat="server" Layout="Column" Height="80">
                        <Items>
                            <ext:Container ID="Container2" runat="server" Layout="FormLayout" ColumnWidth="1" Padding="5">
                                <Items>
                                    <ext:Label ID="GatewayInformationLableContainer" runat="server" Text="Gateway Information:" Cls="bold" />
                                </Items>
                            </ext:Container>

                            <ext:Container ID="Container7" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:ComboBox ID="AssociatedSiteList" runat="server" FieldLabel="Associated Site" LabelWidth="100" LabelAlign="Top" />
                                </Items>
                            </ext:Container>
                            
                            <ext:Container ID="Container5" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:ComboBox ID="AssociatedPoolList" runat="server" FieldLabel="Associated Pool" LabelWidth="100" LabelAlign="Top" />
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>

                    <ext:Container ID="GatewayDescriptionContainer" runat="server" Layout="FormLayout" Height="300" Padding="5">
                        <Items>
                            <ext:HtmlEditor ID="GatewayDescription" runat="server" Height="300" FieldLabel="Gateway Description" LabelAlign="Top" />
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
