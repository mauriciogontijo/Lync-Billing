<%@ Page Title="" Language="C#" MasterPageFile="~/ui/AccountingMaster.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.accounting.dashboard" %>

<%-- ACCOUNTING DASHBOARD --%>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Accounting Mainpage</title>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#accounting-tab').addClass('selected');
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='accountint-dashboard-sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="AccountingToolsSidebar"
                runat="server"
                Height="415"
                Width="180"
                Title="Accounting Tools"
                Collapsed="false"
                Collapsible="true">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Disputes</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/disputes.aspx'>Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/users/monthly.aspx'>Monthly Users Reports</a></p>
                            <p><a href='../accounting/users/periodical.aspx'>Periodical Users Reports</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/sites/monthly.aspx'>Monthly Sites Reports</a></p>
                            <p><a href='../accounting/sites/periodical.aspx'>Periodical Sites Reports</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='Div2' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-16">This is the accounting dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>