<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.UI.user.dashboard" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<?xml version="1.1" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>eBill | User Homepage</title>

    <link rel="stylesheet" type="text/css" href="../resources/css/reset.css" />
    <link rel="stylesheet" type="text/css" href="../resources/css/green-layout.css" />
    <link rel="stylesheet" type="text/css" href="../resources/css/toolkit.css" />
    <script type="text/javascript" src="../resources/js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../resources/js/browserdetector.js"></script>

    <!--[if lt IE 9]>
		    <link rel="stylesheet" type="text/css" href="css/green-layout-ie-8.css" />
	    <![endif]-->

    <!--[if lt IE 8]>
		    <style type="text/css">
			    #main { padding-top: 65px !important; }
		    </style>
	    <![endif]-->

    <script type="text/javascript">
        BrowserDetect.init();

        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#home-tab').addClass('selected');
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

            return hours + ':' + minutes + ':' + seconds;
        }

        var chartsDurationFormat = function (seconds) {
            var sec_num = parseInt(seconds, 10);
            var hours = Math.floor(sec_num / 3600);
            var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
            var seconds = sec_num - (hours * 3600) - (minutes * 60);

            if (hours < 10) hours = "0" + hours;
            if (minutes < 10) minutes = "0" + minutes;
            if (seconds < 10) seconds = "0" + seconds;

            return hours + ':' + minutes + ':' + seconds;
        }

        var submitValue = function (grid, hiddenFormat, format) {
            grid.submitData(false, { isUpload: true });
        };

        var tipDuartionRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0;

            App.PhoneCallsDuartionChart.getStore().each(function (rec) {
                total += rec.get('TotalDuration');
            });

            this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalDuration') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br>' + 'Net Duration: ' + chartsDurationFormat(storeItem.get('TotalDuration')) + ' hours.' +
                '<br>' + 'Net Cost: ' + storeItem.get('TotalCost') + ' euros'
            );
        };

        var TotalDurationLableRenderer = function (storeItem, item) {
            var total = 0, business_duration = 0, personal_duration = 0, unmarked_duration = 0;

            App.PhoneCallsDuartionChart.getStore().each(function (rec) {
                total += rec.get('TotalDuration');

                if (rec.get('Name') == 'Business') {
                    business_duration = rec.get('TotalDuration');
                }
                else if (rec.get('Name') == 'Personal') {
                    personal_duration = rec.get('TotalDuration');
                }
                else if (rec.get('Name') == 'Unmarked') {
                    unmarked_duration = rec.get('TotalDuration');
                }
            });

            if (storeItem == "Business") {
                return ((business_duration / total).toFixed(4) * 100.0).toFixed(2) + '%';
                //return business_duration
            }
            else if (storeItem == "Personal") {
                return ((personal_duration / total).toFixed(4) * 100.0).toFixed(2) + '%';
                //return personal_duration;
            }
            else if (storeItem == "Unmarked") {
                return ((unmarked_duration / total).toFixed(4) * 100.0).toFixed(2) + '%';
                //return unmarked_duration;
            }
        };

        var GetHoursFromMinutes = function (value) {
            var sec_num = parseInt(value, 10);
            var hours = Math.floor(sec_num / 60);
            var minutes = Math.floor((sec_num - (hours * 60)));
            return hours + "." + minutes;
        };

        var redirect_to_manage_phonecalls = function () {
            to = "User_ManagePhoneCalls.aspx"
            window.location = to;
        };

        var redirect_to = function (destination) {
            if (typeof destination == "string" && destination != 0) {
                window.location = destination;
            }
        };

        Ext.override(Ext.chart.LegendItem, {
            createSeriesMarkers: function (config) {
                var me = this,
                    index = config.yFieldIndex,
                    series = me.series,
                    seriesType = series.type,
                    surface = me.surface,
                    z = me.zIndex;

                // Line series - display as short line with optional marker in the middle
                if (seriesType === 'line' || seriesType === 'scatter') {
                    if (seriesType === 'line') {
                        var seriesStyle = Ext.apply(series.seriesStyle, series.style);
                        me.drawLine(0.5, 0.5, 16.5, 0.5, z, seriesStyle, index);
                    };

                    if (series.showMarkers || seriesType === 'scatter') {
                        var markerConfig = Ext.apply(series.markerStyle, series.markerConfig || {}, {
                            fill: series.getLegendColor(index)
                        });
                        me.drawMarker(8.5, 0.5, z, markerConfig);
                    }
                }
                    // All other series types - display as filled box
                else {
                    me.drawFilledBox(12, 12, z, index);
                }
            },

            /**
             * @private Creates line sprite for Line series.
             */
            drawLine: function (fromX, fromY, toX, toY, z, seriesStyle, index) {
                var me = this,
                    surface = me.surface,
                    series = me.series;

                return me.add('line', surface.add({
                    type: 'path',
                    path: 'M' + fromX + ',' + fromY + 'L' + toX + ',' + toY,
                    zIndex: (z || 0) + 2,
                    "stroke-width": series.lineWidth,
                    "stroke-linejoin": "round",
                    "stroke-dasharray": series.dash,
                    stroke: seriesStyle.stroke || series.getLegendColor(index) || '#000',
                    style: {
                        cursor: 'pointer'
                    }
                }));
            }
        });
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="resourceManager" runat="server" Theme="Gray" />

        <div id="toolbar">
            <style type="text/css">
                .more-container {
                    margin: 5px -3px 5px -3px;
                }

                .ptabs {
                    width: auto;
                }
            </style>

            <div style="margin-bottom: 100px !important;" class="blackbox">
                <div class="center-piece">
                    <div id='logo' class='logo float-left'>
                        <a href='/UI/user/dashboard.aspx'>eBill</a>
                    </div>

                    <ul id="navigation-tabs" style="width: auto" class="ptabs">
                        <li id="logout-tab">
                            <a title="Logout" href="/UI/session/logout.aspx">Logout</a>
                        </li>

                        <li id="home-tab" class="selected">
                            <a title="Home" href="/UI/user/dashboard.aspx">Home</a>
                        </li>

                        <li id="manage-phonecalls-tab">
                            <a title="Manage My Phone Calls" href="/UI/user/manage_phone_calls.aspx">My Phone Calls</a>
                        </li>

                        <% 
                            bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAccountant || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                            if (condition)
                            { 
                        %>
                        <li id="accounting-tab" class="last">
                            <a title="Accounting Tools" href="#">Accounting Tools&nbsp;&nbsp;<img src="/UI/resources/images/header-ddl-icon.png"></a>
                            <div class="more-container">
                                <a href="/UI/accounting/manage_disputes.aspx">
                                    <div class="float-left ml5">Manage Disputed Calls</div>
                                </a>
                                <a href="/UI/accounting/monthly_user_reports.aspx">
                                    <div class="float-left ml5">Monthly User Report</div>
                                </a>
                                <a href="/UI/accounting/periodical_user_reports.aspx">
                                    <div class="float-left ml5">Periodical User Report</div>
                                </a>
                                <a href="/UI/accounting/monthly_site_reports.aspx">
                                    <div class="float-left ml5">Monthly Site Report</div>
                                </a>
                                <a href="/UI/accounting/periodical_site_reports.aspx">
                                    <div class="float-left ml5">Periodical Site Report</div>
                                </a>
                            </div>
                        </li>
                        <% } %>
                    </ul>
                </div>
            </div>
        </div>

        <div id='main' class='main bottom-rounded'>
            <div id='announcements' class='announcements shadow mb20 p10'>
                <div class='m10'>
                    <p class='font-18'>ANNOUNCEMENTS!</p>
                </div>
                <div class="block-body">
                    <p class='font-14'>Welcome to the new eBill, it's now more customized and personal. Please take your time going through your personal analytics and have a look at our new personal management tools.</p>
                </div>
            </div>

            <div class='clear h15'></div>

            <!-- START OF LEFT COLUMN -->
            <div style="float: left; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
                <div id='duration-report-block' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                        <ext:Panel ID="PhoneCallsDuartionChartPanel"
                            runat="server"
                            Title="Calls Duration Report (Last 3 Months)"
                            Width="465"
                            Height="360"
                            Layout="FitLayout">
                            <Items>
                                <ext:Chart
                                    ID="PhoneCallsDuartionChart"
                                    runat="server"
                                    Animate="true"
                                    Shadow="true"
                                    InsetPadding="20"
                                    Width="465"
                                    Height="350"
                                    Theme="Base:gradients">
                                    <LegendConfig Position="Right" />
                                    <Store>
                                        <ext:Store ID="PhoneCallsDuartionChartStore"
                                            OnLoad="PhoneCallsDuartionChartStore_Load"
                                            runat="server">
                                            <Model>
                                                <ext:Model ID="PhoneCallsDuartionCharModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Name" />
                                                        <ext:ModelField Name="TotalCalls" />
                                                        <ext:ModelField Name="TotalCost" />
                                                        <ext:ModelField Name="TotalDuration" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <Series>
                                        <ext:PieSeries
                                            AngleField="TotalDuration"
                                            ShowInLegend="true"
                                            Donut="30"
                                            Highlight="true"
                                            HighlightSegmentMargin="10">
                                            <Label Field="Name" Display="Rotate" Contrast="true" Font="16px Arial">
                                                <Renderer Fn="TotalDurationLableRenderer" />
                                            </Label>
                                            <Tips ID="Tips1" runat="server" TrackMouse="true" Width="200" Height="75">
                                                <Renderer Fn="tipDuartionRenderer" />
                                            </Tips>
                                            <Listeners>
                                                <ItemClick Fn="redirect_to_manage_phonecalls" />
                                            </Listeners>
                                        </ext:PieSeries>
                                    </Series>
                                </ext:Chart>
                            </Items>
                        </ext:Panel>
                    </div>
                    <!-- END OF CONTENT -->
                </div>
                <!-- END OF BLOCk -->

                <div class='clear h20'></div>

                <div id='summary-block' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                        <ext:Panel ID="UserPhoneCallsSummary"
                            runat="server"
                            Height="230"
                            Width="465"
                            Layout="AccordionLayout"
                            Title="Summary">
                            <Loader ID="SummaryLoader"
                                runat="server"
                                DirectMethod="#{DirectMethods}.GetSummaryData"
                                Mode="Component">
                                <LoadMask ShowMask="true" />
                            </Loader>
                        </ext:Panel>
                    </div>
                    <!-- END OF CONTENT -->
                </div>
                <!-- END OF BLOCk -->

                <div class='clear h20'></div>

                <div id='top-destination-countries' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                         <ext:Panel
                            ID="TopDestinationCountriesPanel"
                            runat="server"
                            Width="465"
                            Height="380"
                            Header="True"
                            Title="Top Destination Countries"
                            Layout="FitLayout">
                            <Items>
                               
                            </Items>
                        </ext:Panel>
                    </div>
                </div>
            </div>
            <!-- END OF LEFT COLUMN -->

            <!-- START OF RIGHT COLUMN -->
            <div style="float: right; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
                <div id='brief-history-block' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                        <ext:GridPanel
                            ID="PhoneCallsHistoryGrid"
                            runat="server"
                            Title="History Brief"
                            Width="465"
                            Height="210"
                            AutoScroll="true"
                            Header="true"
                            Scroll="Both"
                            Layout="FitLayout">

                            <Store>
                                <ext:Store
                                    ID="PhoneCallsHistoryStore"
                                    runat="server"
                                    IsPagingStore="true">
                                    <Model>
                                        <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                            <Fields>
                                                <ext:ModelField Name="SessionIdTime" Type="String" />
                                                <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                                <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                                <ext:ModelField Name="Duration" Type="Float" />
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
                                        Text="Country"
                                        Width="60"
                                        DataIndex="Marker_CallToCountry"
                                        Align="Center" />

                                    <ext:Column
                                        ID="DestinationNumberUri"
                                        runat="server"
                                        Text="Destination"
                                        Width="140"
                                        DataIndex="DestinationNumberUri" />

                                    <ext:Column
                                        ID="Duration"
                                        runat="server"
                                        Text="Duration"
                                        Width="100"
                                        DataIndex="Duration">
                                        <Renderer Fn="GetMinutes" />
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>

                            <TopBar>
                                <ext:Toolbar ID="CallsHistoryGridToolbar" runat="server">
                                    <Items>
                                        <ext:Button ID="HistoryGridViewMore" runat="server" Text="View More..." Icon="ApplicationGo" Margins="0 0 0 365">
                                            <Listeners>
                                                <Click Handler="redirect_to('User_ViewHistory.aspx');" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:GridPanel>
                    </div>
                    <!-- END OF CONTENT -->
                </div>
                <!-- END OF BLOCk -->

                <div class='clear h20'></div>

                <div id='duration-cost-chart-block' class='block wauto'>
                    <div class="content wauto float-left mb10">
                        <ext:Panel
                            ID="DurationCostChartPanel"
                            runat="server"
                            Width="465"
                            Height="380"
                            Header="True"
                            Title="Personal Duration/Cost Report"
                            Layout="FitLayout">
                            <Items>
                                <ext:Chart
                                    ID="DurationCostChart"
                                    runat="server"
                                    Animate="true">
                                    <Store>
                                        <ext:Store ID="DurationCostChartStore" runat="server">
                                            <Model>
                                                <ext:Model ID="DurationCostChartModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="MonthDate" />
                                                        <ext:ModelField Name="Duration" />
                                                        <ext:ModelField Name="PersonalCallsCost" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <Axes>
                                        <ext:CategoryAxis
                                            Position="Bottom"
                                            Fields="MonthDate"
                                            Title="Current Year">
                                            <Label>
                                                <Renderer Handler="return Ext.util.Format.date(value, 'M');" />
                                            </Label>
                                        </ext:CategoryAxis>

                                        <ext:NumericAxis
                                            Title="Duration in Hours"
                                            Fields="Duration"
                                            Position="Left">
                                            <LabelTitle Fill="#115fa6" />
                                            <Label Fill="#115fa6" />
                                            <Label>
                                                <Renderer Fn="GetHoursFromMinutes" />
                                            </Label>
                                        </ext:NumericAxis>

                                        <ext:NumericAxis
                                            Title="Cost in Local Currency"
                                            Fields="PersonalCallsCost"
                                            Position="Right">
                                            <LabelTitle Fill="#94ae0a" />
                                            <Label Fill="#94ae0a" />
                                        </ext:NumericAxis>
                                    </Axes>

                                    <Series>
                                        <ext:LineSeries
                                            Titles="Calls Duartion"
                                            XField="MonthDate"
                                            YField="Duration"
                                            Axis="Left"
                                            Smooth="3">
                                            <HighlightConfig Size="7" Radius="7" />
                                            <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                        </ext:LineSeries>

                                        <ext:LineSeries
                                            Titles="Calls Cost"
                                            XField="MonthDate"
                                            YField="PersonalCallsCost"
                                            Axis="Right"
                                            Smooth="3">
                                            <HighlightConfig Size="7" Radius="7" />
                                            <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                        </ext:LineSeries>
                                    </Series>

                                    <Plugins>
                                        <ext:VerticalMarker ID="VerticalMarker1" runat="server">
                                            <XLabelRenderer Handler="return Ext.util.Format.date(value, 'Y M');" />
                                            <YLabelRenderer FormatHandler="true"></YLabelRenderer>
                                        </ext:VerticalMarker>
                                    </Plugins>
                                    <LegendConfig Position="Bottom" />
                                </ext:Chart>
                            </Items>
                        </ext:Panel>
                    </div>
                    <!-- END OF CONTENT -->
                </div>
                <!-- END OF BLOCk -->

                <div class='clear h20'></div>

                <div id='TOP-Destination-Numbers-Block' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                        <ext:GridPanel
                            ID="TOPDestinationNumbersGrid"
                            runat="server"
                            Title="TOP Destination Numbers"
                            Width="465"
                            Height="210"
                            AutoScroll="true"
                            Header="true"
                            Scroll="Both"
                            Layout="FitLayout">
                            <Store>
                                <ext:Store
                                    ID="TOPDestinationNumbersStore"
                                    runat="server"
                                    IsPagingStore="true">
                                    <Model>
                                        <ext:Model ID="TOPDestinationNumbersModel" runat="server" IDProperty="SessionIdTime">
                                            <Fields>
                                                <ext:ModelField Name="PhoneNumber" Type="String" />
                                                <ext:ModelField Name="Internal" Type="String" />
                                                <ext:ModelField Name="NumberOfPhoneCalls" Type="Int" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <ColumnModel ID="TOPDestinationNumbersColumnModel" runat="server" Flex="1">
                                <Columns>
                                    <ext:Column
                                        ID="PhoneNumber"
                                        runat="server"
                                        Text="Number"
                                        Width="160"
                                        DataIndex="PhoneNumber">
                                    </ext:Column>

                                    <ext:Column
                                        ID="Internal"
                                        runat="server"
                                        Text="User"
                                        Width="160"
                                        DataIndex="Internal" />

                                    <ext:Column
                                        ID="NumberOfPhoneCalls"
                                        runat="server"
                                        Text="Number of Calls"
                                        Width="140"
                                        DataIndex="NumberOfPhoneCalls" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </div>
                </div>
            </div>
            <!-- END OF RIGHT COLUMN -->
        </div>

        <div class='clear h10'></div>

        <div id='footer' class='footer'>
        </div>
    </form>
</body>
</html>


<%--
    <%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.User_Dashboard" %>
    <asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server"></asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server"></asp:Content>
--%>
