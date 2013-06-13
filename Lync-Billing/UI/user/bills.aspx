<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="bills.aspx.cs" Inherits="Lync_Billing.UI.user.bills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner { font-family: tahoma, verdana; display: block; font-weight: normal; font-style: normal; color:#385F95; white-space: normal; }
        .x-grid-rowbody div { margin: 2px 5px 20px 5px !important; width: 99%; color: Gray; }
        .x-grid-row-expanded td.x-grid-cell { border-bottom-width: 0px; }
    </style>

	<script type="text/javascript">
	    BrowserDetect.init();

	    $(document).ready(function () {
	        $('#navigation-tabs>li.selected').removeClass('selected');
	        $('#user-tab').addClass('selected');
	    });

	    function RoundCost(value, meta, record, rowIndex, colIndex, store) {
	        return Math.round(record.data.PersonalCallsCost * 100) / 100;
	    }

	    //Manage-Phone-Calls Grid JavaScripts
	    var myDateRenderer = function (value) {
	        if (typeof value != undefined && value != 0) {
	            if (BrowserDetect.browser != "Explorer") {
	                value = Ext.util.Format.date(value, "M Y");
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

	                return (my_date.date.month + " " + my_date.date.year);
	            }
	        }
	    }

	    function GetMinutes(value, meta, record, rowIndex, colIndex, store) {

	        var sec_num = parseInt(record.data.PersonalCallsDuration, 10);
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
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="450"
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
                            <p><a href='../user/phonecalls.aspx'>Phone Calls</a></p>
                            <p><a href="../user/addressbook.aspx">Address Book</a></p>
                            <p><a href="#">Authorized Delegate</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/history.aspx'>Phone Calls History</a></p>
                            <p><a href='../user/bills.aspx' class="selected">Bills History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/statistics.aspx'>Phone Calls Statistics</a></p>
                        </div>
                    </div>

                    <%
                        bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                        if (condition) {
                    %>
                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Delegee Accounts</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='../user/manage_delegates.aspx'>Manage Delegee(s)</a></p>
                            </div>
                        </div>
                    <% } %>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF PHONE CALLS HISTORY GRID *** -->
    <div id='phone-call-history' class='block float-right wauto h100p' style="visibility: visible;">
        <div class="block-body pt5">
            <ext:GridPanel
                ID="BillsHistoryGrid" 
                runat="server" 
                Title="Bills History"
                Width="740"
                Height="740"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="BillsStore" 
                        runat="server" 
                        OnLoad="BillsStore_Load"
                        OnReadData="BillsStore_ReadData"
                        IsPagingStore="true"  
                        PageSize="25">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="BillsModel">
                                <Fields>
                                    <ext:ModelField Name="MonthDate" Type="Date" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="Float" />
                                    <ext:ModelField Name="PersonalCallsCount" Type="Int" />
                                    <ext:ModelField Name="PersonalCallsDuration" Type="Int" />
                                </Fields>
                         </ext:Model>
                       </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="BillsColumnModel" runat="server">
		            <Columns>
                        <ext:Column ID="BillDate" 
                            runat="server" 
                            Text="Accounting Date" 
                            Width="180" 
                            DataIndex="MonthDate"
                            Groupable="false">
                            <Renderer Fn="myDateRenderer" />
                        </ext:Column>
                        <ext:Column ID="TotalCalls"
                            runat="server"
                            Text="Number of Calls"
                            Width="180"
                            DataIndex="PersonalCallsCount"
                            Groupable="false" 
                            Align="Center"/>
                        
                        <ext:Column ID="TotalDuration"
                            runat="server"
                            Text="Duration"
                            Width="180"
                            DataIndex="PersonalCallsDuration"
                            Groupable="false"
                            Align="Center">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>
		                <ext:Column ID="TotalCost"
                            runat="server"
                            Text="Total Cost"
                            Width="180"
                            DataIndex="PersonalCallsCost"
                            Groupable="false"
                            Align="Center">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <BottomBar>
                    <ext:PagingToolbar 
                        ID="PhoneCallsPagingToolbar" 
                        runat="server" 
                        StoreID="BillsStore" 
                        DisplayInfo="true" 
                        Weight="25" 
                        DisplayMsg="Phone Calls {0} - {1} of {2}"
                         />
                </BottomBar>
            </ext:GridPanel>
        </div>
        <!-- *** END OF PHONE CALLS HISTORY GRID *** -->
    </div>
</asp:Content>
