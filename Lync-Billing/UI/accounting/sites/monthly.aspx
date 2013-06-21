﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ui/AccountingMaster.Master" AutoEventWireup="true" CodeBehind="monthly.aspx.cs" Inherits="Lync_Billing.ui.accounting.sites.monthly" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Monthly Sites Report</title>

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
            <ext:Panel ID="AccountingToolsSidebar"
                runat="server"
                Height="340"
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
                            <p><a href='../accounting/sites/monthly.aspx' class="selected">Monthly Sites Reports</a></p>
                            <p><a href='../accounting/sites/periodical.aspx'>Periodical Sites Reports</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->

    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Panel
                ID="GenerateReportPanel" 
                runat="server" 
                Width="750"
                Height="53"  
                Header="true"
                Title="Generate Monthly Sites Report"
                Layout="Anchor">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:DateField 
                                ID="DateField"
                                runat="server" 
                                Vtype="daterange"
                                FieldLabel="Select Month and Year:"
                                LabelWidth="120"
                                EmptyText="Empty Date"
                                Width="300"
                                Margins="0 320 0 5"
                                EnableKeyEvents="true">    
                                <CustomConfig>
                                    <ext:ConfigItem Name="DateField" Value="DateField1" Mode="Value" />
                                </CustomConfig>
                                <Listeners>
                                    <KeyUp Fn="onKeyUp" />
                                </Listeners>
                            </ext:DateField>

                            <ext:Button ID="Button1" runat="server" Text="Generate Report" Icon="ApplicationGo" Margins="">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>