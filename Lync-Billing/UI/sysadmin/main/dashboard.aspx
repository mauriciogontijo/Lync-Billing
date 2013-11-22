<%@ Page Title="eBill Admin | Dashboard" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.main.dashboard" %>

<%-- ADMIN DASHBOARD --%>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='dashboard-message' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Panel 
                ID="AdminAnnouncementsPanel"
                runat="server" 
                Title="Announcements"
                PaddingSummary="5px 5px 0"
                Width="740"
                Height="120"
                ButtonAlign="Center">
                <Content>
                    <div class="p10 font-12">
                        <p>This is the System Admin dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>