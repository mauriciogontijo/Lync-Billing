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
                Frame="true"
                PaddingSummary="5px 5px 0"
                Width="740"
                Height="720"
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
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <Items>
                    <ext:Container ID="Container1" runat="server" Layout="Column" Height="20" Padding="5"></ext:Container>

                    <ext:Container ID="AssociatedSiteContainer" runat="server" Layout="Column" Height="50" Padding="5">
                        <Items>
                            <ext:ComboBox
                                ID="AssociatedSite"
                                runat="server"
                                TriggerAction="All" 
                                QueryMode="Local" 
                                FieldLabel="Associated Site"
                                LabelWidth="100"
                                LabelAlign="Top"
                                Width="300"
                                Margins="5 5 5 5">
                            </ext:ComboBox>
                        </Items>
                    </ext:Container>

                    <ext:Container ID="GatewayRatesFiels" runat="server" Layout="Column" Height="130">
                        <Items>
                            <ext:Container ID="Container2" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:TextField ID="TextField1" runat="server" FieldLabel="First Name" LabelAlign="Top" />
                                    <ext:TextField ID="TextField2" runat="server" FieldLabel="Company" LabelAlign="Top"  />
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container3" runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                <Items>
                                    <ext:TextField ID="TextField3" runat="server" FieldLabel="Last Name" LabelAlign="Top" />
                                    <ext:TextField ID="TextField4" runat="server" FieldLabel="Email" LabelAlign="Top" />
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>

                    <ext:Container ID="Container4" runat="server" Layout="FormLayout" Height="300" Padding="5">
                        <Items>
                            <ext:HtmlEditor ID="HtmlEditor1" runat="server" Height="200" FieldLabel="Biography" LabelAlign="Top" />
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button ID="Button1" runat="server" Text="Save" />
                    <ext:Button ID="Button2" runat="server" Text="Cancel" />
                </Buttons>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
