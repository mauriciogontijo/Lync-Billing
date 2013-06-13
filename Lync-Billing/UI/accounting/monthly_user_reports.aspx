<%@ Page Title="" Language="C#" MasterPageFile="~/UI/AccountingMaster.Master" AutoEventWireup="true" CodeBehind="monthly_user_reports.aspx.cs" Inherits="Lync_Billing.UI.accounting.monthly_user_reports" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Monthly Users Report</title>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#accounting-tab').addClass('selected');
        });

        var onKeyUp = function () {
            var me = this,
                v = me.getValue(),
                field;

            if (me.startDateField) {
                field = Ext.getCmp(me.startDateField);
                field.setMaxValue(v);
                me.dateRangeMax = v;
            } else if (me.endDateField) {
                field = Ext.getCmp(me.endDateField);
                field.setMinValue(v);
                me.dateRangeMin = v;
            }

            field.validate();
        };
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
                            <p><a href='../accounting/manage_disputes.aspx'>Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/monthly_user_reports.aspx' class="selected">Monthly Users Reports</a></p>
                            <p><a href='../accounting/periodical_user_reports.aspx'>Periodical Users Reports</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/monthly_site_reports.aspx'>Monthly Sites Reports</a></p>
                            <p><a href='../accounting/periodical_site_reports.aspx'>Periodical Sites Reports</a></p>
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
                Title="Generate Monthly User Report"
                Layout="Anchor">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:TextField
                                ID="GroupNumberField"
                                runat="server" 
                                FieldLabel="User Group Number:"
                                LabelWidth="115"
                                EmptyText="Number Only"
                                Margins="0 25 0 5" />

                            <ext:DateField 
                                ID="DateField"
                                runat="server" 
                                Vtype="daterange"
                                FieldLabel="Select Month and Year:"
                                LabelWidth="115"
                                EmptyText="Empty Date"
                                Width="300"
                                Margins="0 40 0 5">
                                <CustomConfig>
                                    <ext:ConfigItem Name="DateField" Value="DateField1" Mode="Value" />
                                </CustomConfig>
                                <%--<Listeners>
                                    <KeyUp Fn="onKeyUp" />
                                </Listeners>--%>
                            </ext:DateField>
                            <ext:Button ID="Button1" runat="server" Text="Generate Report" Icon="ApplicationGo" OnDirectClick="Button1_DirectClick">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>