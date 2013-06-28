﻿<%@ Page Title="eBill Admin | Dashboard" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.admin.main.dashboard" %>

<%-- ADMIN DASHBOARD --%>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Dashboard</title>
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='dashboard-message' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-16">This is the Admin dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>