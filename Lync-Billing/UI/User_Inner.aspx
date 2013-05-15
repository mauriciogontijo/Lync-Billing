﻿<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Inner.aspx.cs" Inherits="Lync_Billing.UI.User_Inner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner {
            font-family : tahoma, verdana;
            display     : block;
            font-weight : normal;
            font-style  : normal;
            color       : #385F95;
            white-space : normal;
        }
        
        .x-grid-rowbody div {
            margin : 2px 5px 20px 5px !important;
            width  : 99%;
            color  : Gray;
        }
        
        .x-grid-row-expanded td.x-grid-cell{
            border-bottom-width:0px;
        }
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

		    $('.show-content-0-btn').click(function (e) {
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#content-block-0').fadeIn(150);
		            $('#content-block-0').addClass('visibility-marker');
		        });
		    });

		    $('.show-content-1-btn').click(function (e) {
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#content-block-1').fadeIn(150);
		            $('#content-block-1').addClass('visibility-marker');
		        });
		    });

		    $('.show-content-2-btn').click(function (e) {
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#content-block-2').fadeIn(150);
		            $('#content-block-2').addClass('visibility-marker');
		        });
		    });

		    $('.show-phone-call-history-btn').click(function (e) {
		        $('#phone-call-history').fadeIn(150);
		        //$('#phone-call-history').css('display', 'block');
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#phone-call-history').fadeIn(150);
		            $('#phone-call-history').addClass('visibility-marker');
		        });
		    }); 
		});
	</script>

    <ext:XScript ID="XScript1" runat="server">
        <script type="text/javascript">
            var applyFilter = function (field) {
                var store = #{PhoneCallsHistoryGrid}.getStore();

                if(#{FilterTypeComboBox}.getValue() == "1") {
                    clearFilter();
                } else {
                    store.filterBy(getRecordFilter());
                }
            };

            var getRecordFilter = function () {
                var f = [];
                
                var FilterValue = #{FilterTypeComboBox}.getValue() || "";
                switch(FilterValue) {
                    case "4":
                        f.push({
                            filter: function (record) {
                                return filterString('NO', 'UI_IsPersonal', record); 
                            }
                        });
                        break;
                    case "5":
                        f.push({
                            filter: function (record) {
                                return filterString('YES', 'UI_IsPersonal', record); 
                            }
                        });
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
    <div id='sidebar' class='sidebar block float-left w20p'>
	    <div class='block-header top-rounded bh-shadow'>
		    <p class='font-18'>SIDEBAR</p>
	    </div>
	    <div class='block-body bottom-rounded bb-shadow'>
		    <div class='block-content wauto float-left mb15'>
			    <p class='font-16 bold mb5'>Manage Phone Calls</p>
			    <p class='font-14 ml15 show-content-0-btn'><a href='#'>Content Block 0</a></p>
			    <p class='font-14 ml15 show-content-1-btn'><a href='#'>Content Block 1</a></p>
			    <p class='font-14 ml15 show-content-2-btn'><a href='#'>Content Block 2</a></p>
		    </div>
					
		    <div class='block-content wauto float-left mb15'>
			    <p class='font-16 bold mb5'>Manage Statistics</p>
			    <p class='font-14 ml15 show-content-0-btn'><a href='#'>Content Block 0</a></p>
			    <p class='font-14 ml15 show-content-1-btn'><a href='#'>Content Block 1</a></p>
			    <p class='font-14 ml15 show-content-2-btn'><a href='#'>Content Block 2</a></p>
		    </div>

		    <div class='block-content wauto float-left mb15'>
			    <p class='font-16 bold mb5'>History</p>
                <p class='font-14 ml15 show-phone-call-history-btn'><a href='#'>Phone Calls History</a></p>
		    </div>

		    <div class='clear h5'></div>
	    </div>
    </div>

    <div id='phone-call-history' class='block float-right w80p h100p'>
        <div class="block-body">
            <ext:GridPanel 
                ID="PhoneCallsHistoryGrid" 
                runat="server" 
                Title="Phone Calls History"
                Width="530"
                Height="450"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                 <ext:Store ID="PhoneCallStore" runat="server" IsPagingStore="true"  PageSize="25"
                    OnAfterRecordUpdated="PhoneCallStore_AfterRecordUpdated"
                    OnAfterStoreChanged="PhoneCallStore_AfterStoreChanged"
                    OnAfterDirectEvent="PhoneCallStore_AfterDirectEvent"
                    OnBeforeDirectEvent="PhoneCallStore_BeforeDirectEvent"
                    OnBeforeRecordUpdated="PhoneCallStore_BeforeRecordUpdated"
                    OnBeforeStoreChanged="PhoneCallStore_BeforeStoreChanged">
                    <Model>
                        <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                            <Fields>
                                <ext:ModelField Name="SessionIdTime" Type="Date" />
                                <ext:ModelField Name="marker_CallToCountry" Type="String" />
                                <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                <ext:ModelField Name="Duration" Type="Float" />
                                <ext:ModelField Name="marker_CallCost"  Type="Float" />
                                <ext:ModelField Name="UI_IsPersonal" Type="String" />
                                <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                <ext:ModelField Name="UI_IsPersonal" Type="String" />
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
                            Width="80" 
                            DataIndex="SessionIdTime" 
                            Resizable="false" 
                            MenuDisabled="true">
                       
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                        </ext:Column>

                        <ext:Column ID="marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="80"
                            DataIndex="DestinationNumberUri Code" />

                        <ext:Column ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="70"
                            DataIndex="Duration" />

                        <ext:Column ID="marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="70"
                            DataIndex="marker_CallCost" />

                        <ext:Column ID="UI_IsPersonal"
                            runat="server"
                            Text="Is Personal"
                            Width="100"
                            Visible ="false"
                            DataIndex="UI_IsPersonal" />

                        <ext:Column ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="80"
                            DataIndex="UI_MarkedOn" />

                        <ext:Column ID="UI_IsInvoiced"
                            runat="server"
                            Text="Billing Status"
                            Visible="false"
                            Width="90"
                            DataIndex="UI_IsInvoiced" />
		            </Columns>
                </ColumnModel>
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
                                    <ext:ListItem Text="Unmarked" Value="2" />
                                    <ext:ListItem Text="Marked" Value="3" />
                                    <ext:ListItem Text="Business" Value="4" />
                                    <ext:ListItem Text="Personal" Value="5" />
                                    <ext:ListItem Text="Charged" Value="6" />
                                    <ext:ListItem Text="Uncharged" Value="7" />
                                </Items>
                                 <Listeners>
                                    <Select Handler="applyFilter(this);" />
                                </Listeners>
                               <%-- <DirectEvents>
                                    <Change OnEvent="FilterTypeChange"></Change>
                                </DirectEvents>--%>
                            </ext:ComboBox>
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
                    
                <%--<Buttons>
                    <ext:Button ID="GridSubmitChanges" runat="server" Text="Save Changes">
                        <DirectEvents>
                            <Click OnEvent="GridSubmitChanges_Click">
                                <EventMask ShowMask="true" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>--%>
            </ext:GridPanel>
        </div>
    </div>
</asp:Content>
