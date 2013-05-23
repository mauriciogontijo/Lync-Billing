<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Accounting_MonthlyUserReport.aspx.cs" Inherits="Lync_Billing.UI.Accounting_MonthlyUserReport" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Monthly Users Report</title>

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
                            <p><a href='Accounting_Dashboard.aspx'>Accounting Dashboard</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Users Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_MonthlyUserReport.aspx' class='selected'>Monthly Users Report</a></p>
                            <p><a href='Accounting_PeriodicalUserReport.aspx'>Periodical Users Report</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Sites Reports</p>
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
    <div id='generate-report-block' class='block float-right w80p h100p'>
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
                                Vtype="text"
                                FieldLabel="User Group Number:"
                                Margins="0 25 0 5"
                                EnableKeyEvents="true" />

                            <ext:DateField 
                                ID="DateField"
                                runat="server" 
                                Vtype="daterange"
                                FieldLabel="Month & Year:"
                                Margins="0 25 0 5"
                                EnableKeyEvents="true">
                                <CustomConfig>
                                    <ext:ConfigItem Name="DateField" Value="DateField1" Mode="Value" />
                                </CustomConfig>
                                <Listeners>
                                    <KeyUp Fn="onKeyUp" />
                                </Listeners>
                            </ext:DateField>

                            <ext:Button ID="Button1" runat="server" Text="Generate" Icon="ApplicationGo" Margins="0 0 0 125">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <%--<Content>
                    <div class="block-body wauto p5">
                        <ext:DateField 
                            ID="StartDateField"
                            runat="server" 
                            Vtype="daterange"
                            FieldLabel="From"
                            Margins="0 25 0 5"
                            EnableKeyEvents="true">    
                            <CustomConfig>
                                <ext:ConfigItem Name="startDateField" Value="DateField1" Mode="Value" />
                            </CustomConfig>
                            <Listeners>
                                <KeyUp Fn="onKeyUp" />
                            </Listeners>
                        </ext:DateField>

                        <ext:DateField 
                            ID="EndDateField" 
                            runat="server"
                            Vtype="daterange"
                            FieldLabel="To"
                            EnableKeyEvents="true">  
                            <CustomConfig>
                                <ext:ConfigItem Name="endDateField" Value="DateField2" Mode="Value" />
                            </CustomConfig>
                            <Listeners>
                                <KeyUp Fn="onKeyUp" />
                            </Listeners>
                        </ext:DateField>

                        <ext:Button ID="Button1" runat="server" Text="Generate" Icon="Application" Margins="50 0 0 0">
                        </ext:Button>
                    </div>
                </Content>--%>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>