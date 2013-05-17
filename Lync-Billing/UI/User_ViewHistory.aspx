<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_ViewHistory.aspx.cs" Inherits="Lync_Billing.UI.User_ViewHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner { font-family: tahoma, verdana; display: block; font-weight: normal; font-style: normal; color:#385F95; white-space: normal; }
        .x-grid-rowbody div { margin: 2px 5px 20px 5px !important; width: 99%; color: Gray; }
        .x-grid-row-expanded td.x-grid-cell { border-bottom-width: 0px; }
    </style>

	<script type="text/javascript">
		$(document).ready(function () {
		    $('settings-menu-button').click(function (e) {
		        e.preventDefault();

		        if ($('#settings-more-list-container').css('display') == 'none') {
		            $('#settings-more-list-container').fadeIn();
		            $('#settings-more-list-container').css('display', 'block');
		        } else {
		            $('#settings-more-list-container').fadeOut();
		            $('#settings-more-list-container').css('display', 'none');
		        }

		        return false;
		    });

		    $('#nav-more').click(function (e) {
		        e.preventDefault();
		        var top = $(this).offset().top;
		        var right = $(this).offset().right;

		        $('#more-list-container').css({ right: right - 1, top: top + 4 }).fadeIn('fast');
		        return false;
		    });

		    $('body').click(function (e) {
		        $('#more-list-container').fadeOut('fast');
		    });
		});

		var myDateRenderer = function (value) {
		    value = Ext.util.Format.date(value, "d M Y h:m A");
		    return value;
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

		var myDateRenderer = function (value) {
		    value = Ext.util.Format.date(value, "d M Y h:i A");
		    return value;
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

		    return  hours + ':' + minutes + ':' + seconds;;
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
        <div class='block-header top-rounded bh-shadow'>
            <p class='font-1-2-em bold'>User Management</p>
        </div>
        <div class='block-body bottom-rounded bb-shadow'>
            <div class='wauto float-left mb15'>
                <p class='section-header'>Manage</p>
                <p class='section-item'><a href='User_ManagePhoneCalls.aspx'>Business Personal</a></p>
                <p class='section-item'><a href='#'>Delegates</a></p>
                 <p class='section-item'><a href='#'>Address Book</a></p>
                
            </div>

            <div class='wauto float-left mb15'>
                <p class='section-header'>History</p>
                <p class='section-item'><a href='User_ViewHistory.aspx'>Phone Calls History</a></p>
            </div>

            <div class='wauto float-left mb15'>
                <p class='section-header'>Statistics</p>
                <p class='section-item'><a href='#'>Phone Calls Statistics</a></p>
            </div>

            <div class='clear h5'></div>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF PHONE CALLS HISTORY GRID *** -->
    <div id='phone-call-history' class='block float-right w80p h100p' style="visibility: visible;">
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
                         OnSubmitData="PhoneCallStore_SubmitData"
                         OnReadData="PhoneCallStore_ReadData"
                         IsPagingStore="true"  
                         PageSize="25">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="marker_CallCost"  Type="Float" />
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

                        <ext:Column ID="marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="DestinationNumberUri Code" 
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

                        <ext:Column ID="marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="marker_CallCost"
                            Groupable="false" />

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
                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 500">
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
