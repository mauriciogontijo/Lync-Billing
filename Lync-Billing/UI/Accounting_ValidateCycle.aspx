<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Accounting_ValidateCycle.aspx.cs" Inherits="Lync_Billing.UI.Accounting_ValidateCycle" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Accounting Mainpage</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class='block-header top-rounded bh-shadow'>
            <p class='font-12 bold'>Accounting Tools</p>
        </div>
        <div class='block-body bottom-rounded bb-shadow pt15'>
            <div class='wauto float-left mb15'>
                <p class='section-header'><a href='Accounting_Dashboard.aspx'>Dashboard</a></p>
            </div>
            <div class='clear h5'></div>
            <div class='wauto float-left mb15'>
                <p class='section-header'>Reports</p>
                <p class='section-item'><a href='Accounting_ValidateCycle.aspx'>Validate Accounting Cycle</a></p>
                <p class='section-item'><a href='Accounting_GenerateReport.aspx'>Generate Report</a></p>
            </div>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='manage-phone-calls-block' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-18">Validate Accounting Cycle!</p>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>