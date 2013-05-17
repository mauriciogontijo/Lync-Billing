<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Inner.aspx.cs" Inherits="Lync_Billing.UI.User_Inner" %>

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
		    value = Ext.util.Format.date(value, "d M Y h:m A");
		    return value;
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
                        f.push({
                            filter: function (record) {
                                return filterMarkedCriteria("Marked", 'UI_IsPersonal', record); 
                            }
                        });
                        break;

                    case "3":
                        f.push({
                            filter: function (record) {
                                return filterMarkedCriteria("Unmarked", 'UI_IsPersonal', record); 
                            }
                        });
                        break;

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

                    case "6":
                        f.push({
                            filter: function (record) {
                                return filterInvoiceCriteria("Charged", 'UI_IsInvoiced', record); 
                            }
                        });
                        break;

                    case "7":
                        f.push({
                            filter: function (record) {
                                return filterInvoiceCriteria("Uncharged", 'UI_IsInvoiced', record); 
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
    <div id='sidebar' class='sidebar block float-left w20p'>
	    <div class='block-header top-rounded bh-shadow'>
		    <p class='font-1-2-em bold'>User Management</p>
	    </div>
	    <div class='block-body bottom-rounded bb-shadow'>
		    <div class='wauto float-left mb15'>
			    <p class='section-header'>Manage Phone Calls</p>
			    <p class='section-item show-content-0-btn'><a href='#'>Content Block 0</a></p>
			    <p class='section-item show-content-1-btn'><a href='#'>Content Block 1</a></p>
			    <p class='section-item show-content-2-btn'><a href='#'>Content Block 2</a></p>
		    </div>
					
		    <div class='wauto float-left mb15'>
			    <p class='section-header'>Manage Statistics</p>
			    <p class='section-item show-content-0-btn'><a href='#'>Content Block 0</a></p>
			    <p class='section-item show-content-1-btn'><a href='#'>Content Block 1</a></p>
			    <p class='section-item show-content-2-btn'><a href='#'>Content Block 2</a></p>
		    </div>

		    <div class='wauto float-left mb15'>
			    <p class='section-header'>History</p>
                <p class='section-item show-phone-call-history-btn'><a href='#'>View Phone Calls History</a></p>
		    </div>

		    <div class='clear h5'></div>
	    </div>
    </div>

    <div id='phone-call-history' class='block float-right w80p h100p' style="visibility: visible;">
        <div class="block-body pt5">
            <ext:GridPanel 
                ID="PhoneCallsHistoryGrid" 
                runat="server" 
                Title="Phone Calls History"
                Width="740"
                Height="650"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                 <ext:Store 
                     ID="PhoneCallStore" 
                     runat="server" 
                     IsPagingStore="true"  
                     PageSize="25"
                   >
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
                            Width="140" 
                            DataIndex="SessionIdTime">
                            <Renderer Fn="myDateRenderer" />
                        </ext:Column>

                        <ext:Column ID="marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="DestinationNumberUri Code" />

                        <ext:Column ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="150"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="60"
                            DataIndex="Duration" />

                        <ext:Column ID="marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="marker_CallCost" />

                        <ext:Column ID="UI_IsPersonal"
                            runat="server"
                            Text="Is Personal"
                            Width="80"
                            DataIndex="UI_IsPersonal" >
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="120"
                            DataIndex="UI_MarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                        </ext:Column>
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
