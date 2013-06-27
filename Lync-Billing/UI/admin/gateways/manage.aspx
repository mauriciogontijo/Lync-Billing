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
                                <p><a href='../gateways/edit.aspx' class="selected">Edit Gateways Data</a></p>
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
            <ext:FormPanel
                ID="ManageGateways"
                Header="true"
                Title="Edit Gateways"
                runat="server"
                Width="740"
                Height="720"
                Layout="AnchorLayout">
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
                                FieldLabel="Gateway"
                                LabelWidth="50"
                                Width="250"
                                Margins="5 5 5 5">
                            </ext:ComboBox>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <Items>
                    <ext:DateField 
                        ID="StartingDate"
                        runat="server" 
                        FieldLabel="Starting Date:"
                        LabelWidth="100"
                        EmptyText="Empty Date"
                        Width="300"
                        Margins="25 5 5 5" />

                    <ext:DateField 
                        ID="EndingDate"
                        runat="server" 
                        FieldLabel="Ending Date:"
                        LabelWidth="100"
                        EmptyText="Empty Date"
                        Width="300"
                        Margins="5 5 5 5" />

                    <ext:TextField
                        ID="ProviderName"
                        runat="server"
                        FieldLabel="Provider Name:"
                        LabelWidth="100"
                        Width="300"
                        Margins="5 5 5 5"
                        EmptyText="VODAFONE, OTE, STC, ..." />

                    <ext:TextField
                        ID="CurrencyCode"
                        runat="server"
                        FieldLabel="Currency Code:"
                        LabelWidth="100"
                        Width="300"
                        Margins="5 5 5 5"
                        EmptyText="EUR, USD, GBP, ..." />

                    <ext:TextArea
                        ID="GatewayDescription"
                        runat="server"
                        FieldLabel="Gateway Description:"
                        LabelWidth="120"
                        LabelAlign="Top"
                        Width="500"
                        Height="500"
                        Margins="25 5 5 5"
                        EmptyText="Empty Description" />

                    <ext:Button
                        ID="SubmitForm"
                        runat="server"
                        Text="Submit Form"
                        Margins="5 5 5 250" />
                </Items>
            </ext:FormPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
