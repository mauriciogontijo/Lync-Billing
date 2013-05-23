<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Accounting_PeriodicalUserReport.aspx.cs" Inherits="Lync_Billing.UI.Accounting_PeriodicalUserReport" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Periodical Users Report</title>

    <script type="text/javascript">
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
            <ext:Panel ID="SidebarPanel"
                runat="server"
                Height="380"
                Title="User Tools">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Pages</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_Dashboard.aspx' class='selected'>Accounting Dashboard</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_MonthlyUserReport.aspx'>Monthly Users Report</a></p>
                            <p><a href='Accounting_PeriodicalUserReport.aspx' class='selected'>Periodical Users Report</a></p>
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
    <div id='manage-phone-calls-block' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-18">Generate Periodical Users Reports!</p>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>