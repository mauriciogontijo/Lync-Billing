<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_ManagePhoneCalls.aspx.cs" Inherits="Lync_Billing.UI.User_ManagePhoneCalls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }    
        .row-green  { background-color: rgb(46, 143, 42); }
        .row-red    { background-color: rgb(201, 20, 20); }
        .row-yellow { background-color: yellow; }
        /* end manage-phone-calls grid styling */
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


	    //Manage-Phone-Calls Grid JavaScripts
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


	    function getRowClassForIsInvoiced(value, meta, record, rowIndex, colIndex, store) {
	        if (record.data.UI_IsInvoiced == 'Pending' || record.data.UI_IsInvoiced == 'PENDING') {
	            meta.style = "color: rgb(201, 20, 20);";
	        }
	        if (record.data.UI_IsInvoiced == 'Charged' || record.data.UI_IsInvoiced == 'CHARGED') {
	            meta.style = "color: rgb(46, 143, 42);";
	        }
	        return value
	    }
	</script>
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
			    <p class='section-item'><a href='User_ManagePhoneCalls.aspx'>Phone Calls</a></p>
                <p class='section-item'>Delegates</p>
		    </div>

		    <div class='wauto float-left mb15'>
			    <p class='section-header'>History and Statistics</p>
                <p class='section-item'><a href='User_ViewHistory.aspx'>View Phone Calls History</a></p>
                <p class='section-item'>View Phone Calls Statistics</p>
		    </div>

		    <div class='clear h5'></div>
	    </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->

    <!-- *** START OF MANAGE PHONE CALLS GRID *** -->
    <div id='manage-phone-calls-block' class='block float-right w80p h100p'>
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
                    <ext:Store ID="PhoneCallsStore" runat="server" IsPagingStore="true"  PageSize="25">
                        <Model>
                            <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                <Fields> 
                                    <ext:ModelField Name="SessionIdTime" Type="String"/>  <%--RenderMilliseconds="true" DateWriteFormat="d M Y G:i"--%>
                                    <ext:ModelField Name="SessionIdSeq" Type="Int"/>
                                    <ext:ModelField Name="ResponseTime" Type="Date" RenderMilliseconds="true"/>
                                    <ext:ModelField Name="SessionEndTime" Type="Date" RenderMilliseconds="true"/>
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

                <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
		            <Columns>
                        <ext:Column
                            ID="Column1"
                            runat="server"
                            Text="Date"
                            Width="160"
                            DataIndex="SessionIdTime">
                                <Renderer Fn="myDateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Column2"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="DestinationNumberUri Code"
                            Align="Center" />

                        <ext:Column
                            ID="Column3"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Column4"
                            runat="server"
                            Text="Duration"
                            Width="70"
                            DataIndex="Duration" />

                        <ext:Column
                            ID="Column5"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="marker_CallCost" />

                        <ext:Column ID="Column6"
                            runat="server"
                            Text="Is Personal"
                            Width="80"
                            DataIndex="UI_IsPersonal">
                                <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column
                            ID="Column7"
                            runat="server"
                            Text="Updated On"
                            Width="100"
                            DataIndex="UI_MarkedOn">
                                <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                        </ext:Column>
		            </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi"></ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Label runat="server" ID="button_group_lable" Margin="5">
                                <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup ID="MarkingBottonsGroup" runat="server" Layout="TableLayout" Width="250" Frame="false" ButtonAlign="Right">
                                <Buttons>
                                    <ext:Button ID="Business" Text="Business" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignBusiness">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <%--<ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />--%>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                            </DirectEvents>
                                        </ext:Button>

                                        <ext:Button ID="Personal" Text="Personal" runat="server">
                                            <DirectEvents>
                                                <Click OnEvent="AssignPersonal">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Dispute" Text="Dispute" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                                </ext:ButtonGroup>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" StoreID="PhoneCallStore" DisplayInfo="true" Weight="25" DisplayMsg="Phone Calls {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF MANAGE PHONE CALLS GRID *** -->
</asp:Content>
