<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.admin.main.dashboard" %>
<%-- ADMIN DASHBOARD --%>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Admin Dashboard</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
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
                            <p><a href='../gateways/manage.aspx'>Edit Gateways</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='dashboard-message' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-16">This is the Admin dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>