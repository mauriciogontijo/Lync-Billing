<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="view_history.aspx.cs" Inherits="Lync_Billing.UI.user.view_history" %>

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
		});

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

		function RoundCost(value, meta, record, rowIndex, colIndex, store) {
	        return Math.round( record.data.Marker_CallCost * 100 ) / 100;
		}
               
		function getRowClassForIsPersonal(value, meta, record, rowIndex, colIndex, store) {
		    if (record.data.UI_IsPersonal == 'YES' || record.data.UI_IsPersonal == 'Yes') {
		        meta.style = "color: rgb(201, 20, 20);";
		    }
		    if (record.data.UI_IsPersonal == 'NO' || record.data.UI_IsPersonal == 'No') {
		        meta.style = "color: rgb(46, 143, 42);";
		    }
		    return value
		}
		
		function GetMinutes(value, meta, record, rowIndex, colIndex, store) {
		    var sec_num = parseInt(record.data.Duration, 10); 
		    var hours = Math.floor(sec_num / 3600);
		    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
		    var seconds = sec_num - (hours * 3600) - (minutes * 60);

		    if (hours < 10)
		    {
		        hours = "0" + hours;
		    }
		    if (minutes < 10)
		    {
		        minutes = "0" + minutes;
		    }
		    if (seconds < 10)
		    {
		        seconds = "0" + seconds;
		    }

		    return  hours + ':' + minutes + ':' + seconds;
		}
	</script>

    <ext:XScript ID="XScript1" runat="server">
        <script type="text/javascript">
            var applyFilter = function (field) {
                if(#{FilterTypeComboBox}.getValue() == "1") {
                    clearFilter();
                } else {
                    #{PhoneCallsHistoryGrid}.getStore().filterBy(getRecordFilter());
                }
            };

            var getRecordFilter = function () {
                var f = [];
                
                var FilterValue = #{FilterTypeComboBox}.getValue() || "";
                switch(FilterValue) {
                    case "2":
                        f.push({ filter: function (record) { return filterMarkedCriteria("Marked", 'UI_IsPersonal', record); }});
                        break;

                    case "3":
                        f.push({ filter: function (record) { return filterMarkedCriteria("Unmarked", 'UI_IsPersonal', record); }});
                        break;

                    case "4":
                        f.push({ filter: function (record) { return filterString('NO', 'UI_IsPersonal', record); }});
                        break;

                    case "5":
                        f.push({ filter: function (record) { return filterString('YES', 'UI_IsPersonal', record); }});
                        break;

                    case "6":
                        f.push({ filter: function (record) { return filterInvoiceCriteria("Charged", 'UI_IsInvoiced', record); }});
                        break;

                    case "7":
                        f.push({ filter: function (record) { return filterInvoiceCriteria("Uncharged", 'UI_IsInvoiced', record); }});
                        break;
                }
                
                var len = f.length;
                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            };
             
            var clearFilter = function () {
                #{FilterTypeComboBox}.reset();
                #{PhoneCallsHistoryGrid}.getStore().clearFilter();
            }
            

            /* FILTERS BY CRITERIA */
            var filterMarkedCriteria = function(value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if(value == "Marked") {
                    if (typeof val == "string" && val != 0) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if(typeof val != "string" || (typeof val == "string" && val == 0)) {
                        return true;
                    } else {
                        return false;
                    }
                }
            };

            var filterInvoiceCriteria = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if(value == "Charged") {
                    if (typeof val == "string" && val.toLowerCase().indexOf("yes") > -1) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    //if(typeof val != "string" || (typeof val == "string" && val == 0) || (typeof val == "string" && val.toLowerCase().indexOf("no") > -1)) {
                    if (typeof val == "string" && val.toLowerCase().indexOf("yes") > -1) {
                        //this returns the invesre vale of the previous identical if condition
                        return false;
                    } else {
                        return true;
                    }
                }
            };

 
            var submitValue = function (grid, hiddenFormat, format) 
            {
                    grid.submitData(false, {isUpload:true});
            };
        

            /* FILTERS BY DATA TYPE */
            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if (typeof val != "string") {
                    return value.length == 0;
                }
                
                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            };

            var filterDate = function (value, dataIndex, record) {
                var val = Ext.Date.clearTime(record.get(dataIndex), true).getTime();
 
                if (!Ext.isEmpty(value, false) && val != Ext.Date.clearTime(value, true).getTime()) {
                    return false;
                }
                return true;
            };
            
            var filterNumber = function (value, dataIndex, record) {
                var val = record.get(dataIndex);                
 
                if (!Ext.isEmpty(value, false) && val != value) {
                    return false;
                }
                
                return true;
            };
        </script>
    </ext:XScript>
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
                            <p><a href='/UI/user/manage_phone_calls.aspx'>My Phone Calls</a></p>
                            
                            <%
                                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                if (condition) {
                            %>
                                <p><a href='/UI/user/manage_delegates.aspx'>My Delegated Users</a></p>
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
                            <p><a href='/UI/user/view_history.aspx' class="selected">Phone Calls History</a></p>
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
            <% } %>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF PHONE CALLS HISTORY GRID *** -->
    <div id='phone-call-history' class='block float-right wauto h100p' style="visibility: visible;">
        <div class="block-body pt5">
            <ext:GridPanel
                ID="PhoneCallsHistoryGrid" 
                runat="server" 
                Title="Phone Calls History"
                Width="740"
                Height="740"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store
                         ID="PhoneCallStore" 
                         runat="server" 
                         OnLoad="PhoneCallStore_Load"
                         OnSubmitData="PhoneCallStore_SubmitData"
                         OnReadData="PhoneCallStore_ReadData"
                         IsPagingStore="true"  
                         PageSize="25">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_IsPersonal" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                </Fields>
                         </ext:Model>
                       </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="PhoneCallsColumnModel" runat="server">
		            <Columns>
                        <ext:Column ID="SessionIdTime" 
                            runat="server" 
                            Text="Date" 
                            Width="160" 
                            DataIndex="SessionIdTime"
                            Groupable="false">
                            <Renderer Fn="myDateRenderer" />
                        </ext:Column>

                        <ext:Column ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="Marker_CallToCountry" 
                            Groupable="true"/>

                        <ext:Column ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri"
                            Groupable="true" />

                        <ext:Column ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="70"
                            DataIndex="Duration"
                            Groupable="false" >
                            <Renderer Fn="GetMinutes"/>
                        </ext:Column>

                        <ext:Column ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost"
                            Groupable="false">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>

                        <ext:Column ID="UI_IsPersonal"
                            runat="server"
                            Text="Is Personal"
                            Width="80"
                            DataIndex="UI_IsPersonal" 
                            Groupable="false">
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="120"
                            DataIndex="UI_MarkedOn"
                             Groupable="true">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                        </ext:Column>
		            </Columns>
                </ColumnModel>
                <Features>               
                <ext:GroupingSummary 
                    ID="GroupingPhoneCallsHistory" 
                    runat="server" 
                    GroupHeaderTplString="{name}" 
                    HideGroupedHeader="true" 
                    EnableGroupingMenu="true"
                    EnableNoGroups="true"
                    ShowSummaryRow="true" />
            </Features>     
                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterTypeComboBox" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="TypeName" 
                                ValueField="TypeValue">
                                <Items>
                                    <ext:ListItem Text="Everything" Value="1"/>
                                    <ext:ListItem Text="Marked" Value="2" />
                                    <ext:ListItem Text="Unmarked" Value="3" />
                                    <ext:ListItem Text="Business" Value="4" />
                                    <ext:ListItem Text="Personal" Value="5" />
                                    <ext:ListItem Text="Charged" Value="6" />
                                    <ext:ListItem Text="Uncharged" Value="7" />
                                </Items>
                                 <Listeners>
                                    <Select Handler="applyFilter(this);" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 510">
                                 <Listeners>
                                    <Click Handler="submitValue(#{PhoneCallsHistoryGrid}, 'xls');" />
                                </Listeners>
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
        <!-- *** END OF PHONE CALLS HISTORY GRID *** -->
    </div>
</asp:Content>
