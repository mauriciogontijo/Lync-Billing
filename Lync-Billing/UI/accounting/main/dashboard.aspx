<%@ Page Title="wBill Accounting | Dashboard" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.accounting.main.dashboard" %>

<%-- ACCOUNTING DASHBOARD --%>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <title>eBill Accounting | Dashboard</title>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='dashboard-message' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-16">This is the Accounting dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>