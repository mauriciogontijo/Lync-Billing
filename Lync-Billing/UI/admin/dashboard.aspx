<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.admin.dashboard" %>
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
                            <p>Notifications</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../admin/notify.aspx'>Email Unmarked-Calls Notification</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='Div2' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-16">This is the Admin dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>