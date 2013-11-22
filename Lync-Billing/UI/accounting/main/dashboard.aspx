<%@ Page Title="tBill Accounting | Dashboard" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.accounting.main.dashboard" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='dashboard-message' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Panel 
                ID="AccountingAnnouncementsPanel"
                runat="server" 
                Title="Announcements"
                PaddingSummary="10px 10px 10px 10px"
                Width="740"
                Height="120"
                ButtonAlign="Center">
                <Content>
                    <div class="p10 font-12">
                        <p>This is the Accounting dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>