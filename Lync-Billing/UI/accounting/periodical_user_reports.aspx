<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="periodical_user_reports.aspx.cs" Inherits="Lync_Billing.UI.accounting.periodical_user_reports" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Periodical Users Report</title>

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
                            <p><a href='/UI/accounting/manage_disputes.aspx'>Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/accounting/monthly_user_reports.aspx'>Monthly Users Reports</a></p>
                            <p><a href='/UI/accounting/periodical_user_reports.aspx' class="selected">Periodical Users Reports</a></p>
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
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Panel
                ID="GenerateReportPanel" 
                runat="server" 
                Width="750"
                Height="53"  
                Header="true"
                Title="Generate Periodical Site Report"
                Layout="Anchor">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:TextField
                                ID="GroupNumberField"
                                runat="server" 
                                Vtype="text"
                                FieldLabel="User Group Number:"
                                LabelWidth="110"
                                EmptyText="Number Only"
                                Margins="0 10 0 5"
                                Width="195"
                                EnableKeyEvents="true" />

                            <ext:DateField 
                                ID="StartDateField"
                                runat="server" 
                                Vtype="daterange"
                                FieldLabel="Starting Date:"
                                LabelWidth="70"
                                EmptyText="Empty Date"
                                Margins="0 10 0 5"
                                Width="190"
                                EnableKeyEvents="true">
                                <CustomConfig>
                                    <ext:ConfigItem Name="StartDateField" Value="DateField1" Mode="Value" />
                                </CustomConfig>
                                <Listeners>
                                    <KeyUp Fn="onKeyUp" />
                                </Listeners>
                            </ext:DateField>

                            <ext:DateField 
                                ID="EndDateField"
                                runat="server" 
                                Vtype="daterange"
                                FieldLabel="Ending Date:"
                                LabelWidth="65"
                                EmptyText="Empty Date"
                                Margins="0 15 0 5"
                                Width="190"
                                EnableKeyEvents="true">
                                <CustomConfig>
                                    <ext:ConfigItem Name="EndDateField" Value="DateField2" Mode="Value" />
                                </CustomConfig>
                                <Listeners>
                                    <KeyUp Fn="onKeyUp" />
                                </Listeners>
                            </ext:DateField>

                            <ext:Button ID="Button1" runat="server" Text="Generate Report" Icon="ApplicationGo">
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