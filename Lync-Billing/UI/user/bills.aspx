<%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="bills.aspx.cs" Inherits="Lync_Billing.ui.user.bills" %>

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
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="340"
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
                            <!--<p><a href="#">Authorized Delegate</a></p>-->
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/bills.aspx' class="selected">Bills History</a></p>
                            <p><a href='../user/history.aspx'>Phone Calls History</a></p>
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
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="SpecialDateRenderer" />
                        </ext:Column>

                        <ext:Column ID="TotalCalls"
                            runat="server"
                            Text="Number of Calls"
                            Width="180"
                            DataIndex="PersonalCallsCount"
                            Groupable="false" 
                            Align="Left"/>
                        
                        <ext:Column ID="TotalDuration"
                            runat="server"
                            Text="Duration"
                            Width="180"
                            DataIndex="PersonalCallsDuration"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

		                <ext:Column ID="TotalCost"
                            runat="server"
                            Text="Total Cost"
                            Width="180"
                            DataIndex="PersonalCallsCost"
                            Groupable="false"
                            Align="Left">
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
