﻿<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="manage_address_book.aspx.cs" Inherits="Lync_Billing.UI.user.manage_address_book" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        .row-green { background-color: rgb(46, 143, 42); }
        .row-red { background-color: rgb(201, 20, 20); }
        .row-yellow { background-color: yellow; }
        /* end manage-phone-calls grid styling */
    </style>

    <script type="text/javascript">
        BrowserDetect.init();

        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#user-tab').addClass('selected');
        });

        function RoundCost(value, meta, record, rowIndex, colIndex, store) {
            return Math.round(record.data.Marker_CallCost * 100) / 100;
        }

        //Manage-Phone-Calls Grid JavaScripts
        var myDateRenderer = function (value) {
            if (typeof value != undefined && value != 0) {
                if (BrowserDetect.browser != "Explorer") {
                    value = Ext.util.Format.date(value, "d M Y h:i A");
                    return value;
                } else {
                    var my_date = {};
                    var value_array = value.split(' ');
                    var months = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

                    my_date["date"] = value_array[0];
                    my_date["time"] = value_array[1];

                    var date_parts = my_date["date"].split('-');
                    my_date["date"] = {
                        year: date_parts[0],
                        month: months[parseInt(date_parts[1])],
                        day: date_parts[2]
                    }

                    var time_parts = my_date["time"].split(':');
                    my_date["time"] = {
                        hours: time_parts[0],
                        minutes: time_parts[1],
                        period: (time_parts[0] < 12 ? 'AM' : 'PM')
                    }

                    //var date_format = Date(my_date["date"].year, my_date["date"].month, my_date["date"].day, my_date["time"].hours, my_date["time"].minutes);
                    return (
                        my_date.date.day + " " + my_date.date.month + " " + my_date.date.year + " " +
                        my_date.time.hours + ":" + my_date.time.minutes + " " + my_date.time.period
                    );
                }//END ELSE
            }//END OUTER IF
        }

        function getRowClassForIsPersonal(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.UI_CallType == 'Personal') {
                meta.style = "color: rgb(201, 20, 20);";
            }
            if (record.data.UI_CallType == 'Business') {
                meta.style = "color: rgb(46, 143, 42);";
            }
            if (record.data.UI_CallType == 'Dispute') {
                meta.style = "color: rgb(31, 115, 164);";
            }

            return value
        }

        var submitValue = function (grid, hiddenFormat, format) {
            grid.submitData(false, { isUpload: true });
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="330"
                Width="180"
                Title="User Tools"
                Collapsed="false"
                Collapsible="true">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Manage</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/user/manage_phone_calls.aspx'>My Phone Calls</a></p>
                            <p><a href="/UI/user/manage_address_book.aspx" class="selected">My Address Book</a></p>

                            <%
                                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                if (condition) {
                            %>
                                <p><a href='/UI/user/manage_delegates.aspx'>My Delegated Users</a></p>
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

            <% 
                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAccountant || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                if ( condition )
                {
            %>
                <ext:Panel ID="AccountingToolsSidebar"
                    runat="server"
                    Height="330"
                    Width="180"
                    Title="Accounting Tools"
                    Collapsed="true"
                    Collapsible="true">
                    <Content>
                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Disputes</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='/UI/accounting/manage_disputes.aspx'>Manage Disputed Calls</a></p>
                            </div>
                        </div>

                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Generate User Reports</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='/UI/accounting/monthly_user_reports.aspx'>Monthly Users Reports</a></p>
                                <p><a href='/UI/accounting/periodical_user_reports.aspx'>Periodical Users Reports</a></p>
                            </div>
                        </div>

                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Generate Site Reports</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='/UI/accounting/monthly_site_reports.aspx'>Monthly Sites Reports</a></p>
                                <p><a href='/UI/accounting/periodical_site_reports.aspx'>Periodical Sites Reports</a></p>
                            </div>
                        </div>
                    </Content>
                </ext:Panel>
            <% } %>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF MANAGE PHONE CALLS GRID *** -->
    <div id='manage-phone-calls-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:TabPanel ID="AddressBookTabPanel" 
                runat="server" 
                Width="750"
                Height="600"
                Margins="0 0 20 0"
                Frame="true">
                <Defaults>
                    <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                    <ext:Parameter Name="autoScroll" Value="true" Mode="Raw" />
                </Defaults>

                <Items>
                    <ext:Panel ID="AddressBook"
                        runat="server"
                        Title="My Address Book"
                        Html="My Address Book Grid......"
                        AutoDataBind="true">
                        <Items></Items>
                    </ext:Panel>

                    <ext:Panel ID="ImportFromHistoryTab"
                        runat="server" 
                        Title="Import Contacts from History" 
                        Html="Import Contacts from History Grid......" 
                        AutoDataBind="true">
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
        </div>
    </div>
    <!-- *** END OF MANAGE PHONE CALLS GRID *** -->
</asp:Content>
