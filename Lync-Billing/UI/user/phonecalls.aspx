<%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="phonecalls.aspx.cs" Inherits="Lync_Billing.ui.user.phonecalls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell {
            height: 25px !important;
        }

        .row-green {
            background-color: rgb(46, 143, 42);
        }

        .row-red {
            background-color: rgb(201, 20, 20);
        }

        .row-yellow {
            background-color: yellow;
        }
        /* end manage-phone-calls grid styling */
    </style>

    <script type="text/javascript">
        toolTipData = { "name": {}, "sip_account": "" };
        window.toolTipData = toolTipData;

        BrowserDetect.init();

        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#user-tab').addClass('selected');
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

            if (record.data != null) {
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

        var onShow = function (toolTip, grid) {
            var view = grid.getView(),
                store = grid.getStore(),
                record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
                column = view.getHeaderByCell(toolTip.triggerElement),
                data = record.get(column.dataIndex);

            if (column.id == "main_content_place_holder_DestinationNumberUri") {
                data = record.get("PhoneBookName");
            }

            toolTip.update(data);
        };

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="420"
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
                            <p><a href='../user/phonecalls.aspx' class="selected">Phone Calls</a></p>
                            <p><a href="../user/addressbook.aspx">Address Book</a></p>
                            <!--<p><a href="#">Authorized Delegate</a></p>-->
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/history.aspx'>Phone Calls History</a></p>
                            <p><a href='../user/bills.aspx'>Bills History</a></p>
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
                        bool is_delegate = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                        if (is_delegate)
                        {
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


    <!-- *** START OF MANAGE PHONE CALLS GRID *** -->
    <div id='manage-phone-calls-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">

            <asp:ObjectDataSource
                ID="MarkedPhoneCallsDataSource"
                runat="server"
                OnSelecting="MarkedPhoneCallsDataSource_Selecting"
                OnSelected="MarkedPhoneCallsDataSource_Selected"
                SelectMethod="GetMarkedPhoneCallsFilter"
                TypeName="Lync_Billing.ui.user.phonecalls">
                <SelectParameters>
                    <asp:Parameter Name="start" Type="Int32" />
                    <asp:Parameter Name="limit" Type="Int32" />
                    <asp:Parameter Name="sort" Type="Object" />
                    <asp:Parameter Name="count" Direction="Output" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

             <asp:ObjectDataSource
                ID="UnMarkedPhoneCallsDataSource"
                runat="server"
                OnSelecting="UnmarkedPhoneCallsDataSource_Selecting"
                OnSelected="UnmarkedPhoneCallsDataSource_Selected"
                SelectMethod="GetUnmarkedPhoneCallsFilter"
                TypeName="Lync_Billing.ui.user.phonecalls">
                <SelectParameters>
                    <asp:Parameter Name="start" Type="Int32" />
                    <asp:Parameter Name="limit" Type="Int32" />
                    <asp:Parameter Name="sort" Type="Object" />
                    <asp:Parameter Name="count" Direction="Output" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <ext:TabPanel ID="ManagePhonCallsTabpanel"
                runat="server"
                Width="740"
                Height="760"
                Margins="0 0 20 0"
                Frame="true">
                <Defaults>
                    <ext:Parameter Name="autoScroll" Value="true" Mode="Raw" />
                </Defaults>
                <Items>
                    <ext:GridPanel
                        ID="ManageUnmarkedCallsGrid"
                        runat="server"
                        Width="740"
                        Height="760"
                        Title="Unmarked Calls"
                        AutoScroll="true"
                        Scroll="Both"
                        Layout="FitLayout">
                        <Store>
                            <ext:Store
                                ID="ManageUnmarkedCallsStore"
                                runat="server"
                                RemoteSort="true"
                                PageSize="25"
                                DataSourceID="UnmarkedPhoneCallsDataSource"
                                OnReadData="ManageUnmarkedCallsStore_ReadData">
                                <Proxy>
                                    <ext:PageProxy CacheString="" />
                                </Proxy>
                                <Sorters>
                                    <ext:DataSorter Property="SessionIdTime" Direction="DESC" />
                                </Sorters>
                                <Model>
                                    <ext:Model ID="Model1" Name="PhoneCallsDataModel" runat="server" IDProperty="SessionIdTime">
                                        <Fields>
                                            <ext:ModelField Name="SessionIdTime" Type="String" />
                                            <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                            <ext:ModelField Name="ResponseTime" Type="String" />
                                            <ext:ModelField Name="SessionEndTime" Type="String" />
                                            <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                            <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                            <ext:ModelField Name="Duration" Type="Float" />
                                            <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                            <ext:ModelField Name="UI_CallType" Type="String" />
                                            <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                            <ext:ModelField Name="PhoneBookName" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />
                                <ext:Column
                                    ID="SessionIdTime"
                                    runat="server"
                                    Text="Date"
                                    Width="140"
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
                                    <Renderer Fn="RoundCost" />
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
                                                    <Click OnEvent="AssignAllBusiness">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageUnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="unmarked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button ID="Personal" Text="Personal" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAllPersonal">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageUnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="unmarked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button ID="Dispute" Text="Dispute" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignDispute">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageUnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="unmarked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Buttons>
                                    </ext:ButtonGroup>

                                    <ext:ButtonGroup ID="ButtonGroup1" runat="server" Frame="false" ButtonAlign="Center" Width="250">
                                        <Buttons>
                                            <ext:Button ID="AssignMarkedAlwaysBusiness" Text=" Always Business" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAlwaysBusiness">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageUnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="unmarked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button ID="AssignMarkedAlwaysPersonal" Text=" Always Personal" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAlwaysPersonal">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageUnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="unmarked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Buttons>
                                    </ext:ButtonGroup>

                                    <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 50">
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
                  
                    <ext:GridPanel
                        ID="ManageMarkedCallsGrid"
                        runat="server"
                        Width="740"
                        Height="760"
                        Title="Marked Calls"
                        AutoScroll="true"
                        Scroll="Both"
                        Layout="FitLayout">
                        <Store>
                            <ext:Store
                                ID="ManageMarkedCallsStore"
                                runat="server"
                                RemoteSort="true"
                                PageSize="25"
                                DataSourceID="MarkedPhoneCallsDataSource"
                                OnReadData="ManageMarkedCallsStore_ReadData">
                                <Proxy>
                                    <ext:PageProxy CacheString="" />
                                </Proxy>
                                <Sorters>
                                    <ext:DataSorter Property="SessionIdTime" Direction="DESC" />
                                </Sorters>
                                <Model>
                                    <ext:Model ID="PhoneCallsDataModel" Name="PhoneCallsDataModel" runat="server" IDProperty="SessionIdTime">
                                        <Fields>
                                            <ext:ModelField Name="SessionIdTime" Type="String" />
                                            <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                            <ext:ModelField Name="ResponseTime" Type="String" />
                                            <ext:ModelField Name="SessionEndTime" Type="String" />
                                            <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                            <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                            <ext:ModelField Name="Duration" Type="Float" />
                                            <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                            <ext:ModelField Name="UI_CallType" Type="String" />
                                            <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                            <ext:ModelField Name="PhoneBookName" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ManageMarkedCallsColumnModel" runat="server" Flex="1">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Width="25" />
                                <ext:Column
                                    ID="SessionIdTimeCol"
                                    runat="server"
                                    Text="Date"
                                    Width="140"
                                    DataIndex="SessionIdTime">
                                    <Renderer Fn="myDateRenderer" />
                                </ext:Column>

                                <ext:Column
                                    ID="Marker_CallToCountryCol"
                                    runat="server"
                                    Text="Country Code"
                                    Width="90"
                                    DataIndex="Marker_CallToCountry" />

                                <ext:Column
                                    ID="DestinationNumberUriCol"
                                    runat="server"
                                    Text="Destination"
                                    Width="130"
                                    DataIndex="DestinationNumberUri" />

                                <ext:Column
                                    ID="DurationCol"
                                    runat="server"
                                    Text="Duration"
                                    Width="70"
                                    DataIndex="Duration">
                                    <Renderer Fn="GetMinutes" />
                                </ext:Column>

                                <ext:Column
                                    ID="Marker_CallCostCol"
                                    runat="server"
                                    Text="Cost"
                                    Width="60"
                                    DataIndex="Marker_CallCost">
                                    <Renderer Fn="RoundCost" />
                                </ext:Column>

                                <ext:Column ID="UI_CallTypeCol"
                                    runat="server"
                                    Text="Type"
                                    Width="80"
                                    DataIndex="UI_CallType">
                                    <Renderer Fn="getRowClassForIsPersonal" />
                                </ext:Column>

                                <ext:Column
                                    ID="UI_MarkedOnCol"
                                    runat="server"
                                    Text="Updated On"
                                    Width="100"
                                    DataIndex="UI_MarkedOn">
                                    <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>

                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2"
                                runat="server"
                                Mode="Multi"
                                AllowDeselect="true"
                                IgnoreRightMouseSelection="true"
                                CheckOnly="true">
                            </ext:CheckboxSelectionModel>
                        </SelectionModel>

                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:Label runat="server" ID="Label1" Margin="5">
                                        <Content>Mark Selected As:</Content>
                                    </ext:Label>

                                    <ext:ButtonGroup ID="ButtonGroup2"
                                        runat="server"
                                        Layout="TableLayout"
                                        Width="250"
                                        Frame="false"
                                        ButtonAlign="Right">
                                        <Buttons>
                                            <ext:Button ID="Button1" Text="Business" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAllBusiness">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageMarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="marked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button ID="Button2" Text="Personal" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAllPersonal">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageMarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="marked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button ID="Button3" Text="Dispute" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignDispute">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageMarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="marked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Buttons>
                                    </ext:ButtonGroup>

                                    <ext:ButtonGroup ID="ButtonGroup3" runat="server" Frame="false" ButtonAlign="Center" Width="250">
                                        <Buttons>
                                            <ext:Button ID="Button4" Text=" Always Business" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAlwaysBusiness">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageMarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="marked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button ID="Button5" Text=" Always Personal" runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="AssignAlwaysPersonal">
                                                        <EventMask ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{ManageMarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                            <ext:Parameter Name="store" Value="marked"/>
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Buttons>
                                    </ext:ButtonGroup>

                                    <ext:Button ID="Button6" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 50">
                                        <Listeners>
                                            <Click Handler="submitValue(#{ManagePhoneCallsGrid}, 'xls');" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <BottomBar>
                            <ext:PagingToolbar
                                ID="PagingToolbar2"
                                runat="server"
                                StoreID="PhoneCallStore"
                                DisplayInfo="true"
                                Weight="25"
                                DisplayMsg="Phone Calls {0} - {1} of {2}" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:TabPanel>
            <%-- <ext:ToolTip ID="ToolTip1"
                runat="server"
                Target="={#{ManagePhoneCallsGrid}.getView().el}"
                Delegate=".x-grid-cell"
                TrackMouse="true">
                <Listeners>
                    <Show Handler="onShow(this, #{ManagePhoneCallsGrid});" />
                </Listeners>
            </ext:ToolTip>--%>
        </div>
    </div>
    <!-- *** END OF MANAGE PHONE CALLS GRID *** -->
</asp:Content>
