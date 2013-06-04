 <%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="manage_phone_calls.aspx.cs" Inherits="Lync_Billing.UI.user.manage_phone_calls" %>

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
            $('#manage-phonecalls-tab').addClass('selected');
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


        function getRowClassForIsInvoiced(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.AC_IsInvoiced == 'NO') {
                meta.style = "color: rgb(201, 20, 20);";
            }
            if (record.data.AC_IsInvoiced == 'YES') {
                meta.style = "color: rgb(46, 143, 42);";
            }
            return value
        }

        function GetMinutes(value, meta, record, rowIndex, colIndex, store) {

            var sec_num = parseInt(record.data.Duration, 10);
            var hours = Math.floor(sec_num / 3600);
            var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
            var seconds = sec_num - (hours * 3600) - (minutes * 60);

            if (hours < 10) {
                hours = "0" + hours;
            }
            if (minutes < 10) {
                minutes = "0" + minutes;
            }
            if (seconds < 10) {
                seconds = "0" + seconds;
            }

            return hours + ':' + minutes + ':' + seconds;;
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
                Height="305"
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
                            <p><a href='/UI/user/manage_phone_calls.aspx' class="selected">My Phone Calls</a></p>
                            <p><a href="/UI/user/manage_address_book.aspx">My Address Book</a></p>

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
            <ext:GridPanel
                ID="ManagePhoneCallsGrid"
                runat="server"
                Title="Manage Phone Calls"
                Width="740"
                Height="750"
                AutoScroll="true"
                Header="true"
                Scroll="Both"
                Layout="FitLayout">

                <Store>
                    <ext:Store 
                        ID="PhoneCallsStore" 
                        runat="server" 
                        IsPagingStore="true" 
                        PageSize="25"
                        OnLoad="PhoneCallsStore_Load"
                        OnSubmitData="PhoneCallsStore_SubmitData"
                        OnReadData="PhoneCallsStore_ReadData">
                        <Model>
                            <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                    <ext:ModelField Name="ResponseTime" Type="String"/>
                                    <ext:ModelField Name="SessionEndTime" Type="String"/>
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_CallType" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="160"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="myDateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="Marker_CallToCountry" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="70"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>

                        <ext:Column ID="UI_CallType"
                            runat="server"
                            Text="Type"
                            Width="80"
                            DataIndex="UI_CallType">
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column
                            ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="100"
                            DataIndex="UI_MarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" 
                        runat="server" 
                        Mode="Multi" 
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">

                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                           <ext:Label runat="server" ID="button_group_lable" Margin="5">
                                <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup ID="MarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="250"
                                Frame="false"
                                ButtonAlign="Right">
                                <Buttons>
                                    <ext:Button ID="Business" Text="Business" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignBusiness">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Personal" Text="Personal" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignPersonal">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Dispute" Text="Dispute" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>
                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 310">
                                 <Listeners>
                                    <Click Handler="submitValue(#{ManagePhoneCallsGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PhoneCallStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Phone Calls {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF MANAGE PHONE CALLS GRID *** -->
</asp:Content>
