<%@ Page Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="manage_disputes.aspx.cs" Inherits="Lync_Billing.UI.accounting.manage_disputes" %>

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

    <div id='Div1' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="330"
                Width="180"
                Title="User Tools">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Manage</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='User_ManagePhoneCalls.aspx'>Phone Calls</a></p>
                            <%
                                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                if (condition) {
                            %>
                                <p><a href='User_ManageDelegates.aspx'>Delegates</a></p>
                            <% } %>
                            <p><a href='#'>Address Book</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='User_ViewHistory.aspx'>Phone Calls History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='#'>Phone Calls Statistics</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
            
            <div class="clear h20"></div>

            <ext:Panel ID="AccountingToolsSidebar"
                runat="server"
                Height="330"
                Width="180"
                Title="Accounting Tools">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Disputes</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_ManageDisputes.aspx' class="selected">Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_MonthlyUserReport.aspx'>Monthly Users Report</a></p>
                            <p><a href='Accounting_PeriodicalUserReport.aspx'>Periodical Users Report</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_MonthlySiteReport.aspx'>Monthly Sites Report</a></p>
                            <p><a href='Accounting_PeriodicalSiteReport.aspx'>Periodical Sites Report</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='Div2' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <p class="font-18">Accounting Dashboard!</p>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>