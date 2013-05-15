<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Inner.aspx.cs" Inherits="Lync_Billing.UI.User_Inner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

	<link rel="stylesheet" type="text/css" href="css/reset.css" />
	<link rel="stylesheet" type="text/css" href="css/green-layout.css" />
	<link rel="stylesheet" type="text/css" href="css/toolkit.css" />

	<!--[if lt IE 9]>
		<link rel="stylesheet" type="text/css" href="css/green-layout-ie-8.css" />
	<![endif]-->

	<!--[if lt IE 8]>
		<style type="text/css">
			#main { padding-top: 65px !important; }
		</style>
	<![endif]-->

	<!--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>-->
	<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>

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
                Width="745"
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
                                <ext:ModelField Name="ui_IsPersonal" Type="Boolean" />
                                <ext:ModelField Name="ui_MarkedOn" Type="Date" />
                                <ext:ModelField Name="ui_IsInvoiced" Type="Boolean" />
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

                        <ext:Column ID="ui_IsPersonal"
                            runat="server"
                            Text="Type"
                            Width="100"
                            DataIndex="ui_IsPersonal" />

                        <ext:Column ID="ui_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="80"
                            DataIndex="ui_MarkedOn" />

                        <ext:Column ID="ui_IsInvoiced"
                            runat="server"
                            Text="Billing Status"
                            Width="90"
                            DataIndex="ui_IsInvoiced" />
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
                                    <ext:ListItem Text="Unmarked" Value="1" />
                                    <ext:ListItem Text="Marked" Value="2" />
                                    <ext:ListItem Text="Business" Value="3" />
                                    <ext:ListItem Text="Personal" Value="4" />
                                    <ext:ListItem Text="Charged" Value="5" />
                                    <ext:ListItem Text="Uncharged" Value="6" />
                                </Items>
                                <DirectEvents>
                                    <Change OnEvent="FilterTypeChange"></Change>
                                </DirectEvents>
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
                    <ext:CheckboxSelectionModel ID="PhoneCallsCheckBoxColumn" runat="server" Mode="Multi" />
                </SelectionModel>            
                    
                <Buttons>
                    <ext:Button ID="GridSubmitChanges" runat="server" Text="Save Changes">
                        <DirectEvents>
                            <Click OnEvent="GridSubmitChanges_Click">
                                <EventMask ShowMask="true" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:GridPanel>
        </div>
    </div>
</asp:Content>
