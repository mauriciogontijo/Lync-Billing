<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Accounting_MonthlyUserReport.aspx.cs" Inherits="Lync_Billing.UI.Accounting_MonthlyUserReport" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Accounting Mainpage</title>

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
                            <p>Generate User Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_MonthlyUserReport.aspx' class='selected'>Monthly Users Report</a></p>
                            <p><a href='Accounting_PeriodicalUserReport.aspx'>Periodical Users Report</a></p>
                            <p><a href='Accounting_ValidateCycle.aspx'>Validate Accounting Cycle</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            
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
            <p class="font-18">Generate Monthly User Reports!</p>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->

    <%--<!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='manage-phone-calls-block' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="AccountingReportGrid" 
                runat="server" 
                Width="740"
                Height="740"  
                AutoScroll="true"
                Scroll="Both" 
                Header="true"
                Title="Generate Accounting Report"
                Layout="FitLayout">
                <Store>
                    <ext:Store
                        ID="AccountingReportStore" 
                        runat="server"
                        IsPagingStore="true"  
                        PageSize="25">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                                <Fields>
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="SourceUserUri" Type="String" />
                                    <ext:ModelField Name="Cost" Type="Float" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="AccountingReportColumnModel" runat="server">
		            <Columns>
                        <ext:Column ID="SipAccount" 
                            runat="server" 
                            Text="Sip Account" 
                            Width="160"
                            DataIndex="SipAccount"
                            Groupable="True" />

                        <ext:Column ID="FullName"
                            runat="server"
                            Text="Full Name"
                            Width="160"
                            DataIndex="FullName" 
                            Groupable="True" />

                        <ext:Column ID="SourceUserUri"
                            runat="server"
                            Text="Email Address"
                            Width="160"
                            DataIndex="SourceUserUri"
                            Groupable="True" />

                        <ext:Column ID="Cost"
                            runat="server"
                            Text="Cost"
                            Width="160"
                            DataIndex="Cost"
                            Groupable="True" />
		            </Columns>
                </ColumnModel>
                        
                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
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

                            <ext:Button ID="Button1" runat="server" Text="Generate" Icon="Application" Margins="0 0 0 145">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar 
                        ID="PhoneCallsPagingToolbar" 
                        runat="server" 
                        StoreID="PhoneCallStore" 
                        DisplayInfo="true" 
                        Weight="25" 
                        DisplayMsg="Phone Calls {0} - {1} of {2}"
                            />
                </BottomBar>
                    
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="PhoneCallsCheckBoxColumn" runat="server" Mode="Multi"  Visible="false"/>
                </SelectionModel>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->--%>
</asp:Content>