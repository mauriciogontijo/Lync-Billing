﻿<%@ Page Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="manage_disputes.aspx.cs" Inherits="Lync_Billing.UI.accounting.manage_disputes" %>

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
                Height="305"
                Width="180"
                Title="User Tools"
                Collapsed="true"
                Collapsible="true">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Manage</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/user/manage_phone_calls.aspx'>Phone Calls</a></p>
                            
                            <%
                                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                if (condition) {
                            %>
                                <p><a href='/UI/user/manage_delegates.aspx'>Delegates</a></p>
                            <% } %>

                            <% if(false) { %>
                                <p><a href='#'>Address Book</a></p>
                            <% } %>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/user/view_history.aspx'>Phone Calls History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/user/view_statistics.aspx'>Phone Calls Statistics</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
            
            <div class="clear h20"></div>

            <ext:Panel ID="AccountingToolsSidebar"
                runat="server"
                Height="330"
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
                            <p><a href='/UI/accounting/manage_disputes.aspx' class="selected">Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/accounting/monthly_user_reports.aspx'>Monthly Users Reports</a></p>
                            <p><a href='/UI/accounting/periodical_user_reports.aspx'>Periodical Users Reports</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/accounting/monthly_site_reports.aspx'>Monthly Sites Reports</a></p>
                            <p><a href='/UI/accounting/periodical_site_reports.aspx'>Periodical Sites Reports</a></p>
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