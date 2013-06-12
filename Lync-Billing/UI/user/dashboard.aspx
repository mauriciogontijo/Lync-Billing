<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.UI.user.dashboard" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<?xml version="1.1" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=9" />

    <title>eBill | User Homepage</title>

    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/reset.css" />
    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/layouts.css" />
    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/toolkit.css" />
    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/global.css" />
    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/green-layout.css" />
    <!--<link rel="stylesheet" type="text/css" media="all" href="../resources/css/dropdown-menu.css" />-->
    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/dropdown-menu-white.css" />

    <script type="text/javascript" src="../resources/js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../resources/js/browserdetector.js"></script>

    <!--[if lte IE 9]>
	    <link rel="stylesheet" type="text/css" href="../resources/css/green-layout-ie-hacks.css" />
	<![endif]-->

    <!--[if lt IE 8]>
        <style type='text/css'>
            .info-block p.message, .warning-block p.message { line-height: 35px; height: 35px; font-size: 14px; float: left; display: inline-block; }
            .info-block, .warning-block { height: 35px; background-position-x: 10px; background-position-y: 15px; }
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


        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopCountries_LableRenderer = function (storeItem, item) {
            var total = 0, all_countries_data = {};
            App.TopDestinationCountriesChart.getStore().each(function (rec) {
                total += rec.get('TotalDuration');

                var country_name = rec.get('CountryName');
                if (country_name != 0 && all_countries_data[country_name] == undefined) {
                    all_countries_data[country_name] = rec.get('TotalDuration');
                }
            });

            if (all_countries_data[storeItem] != undefined) {
                return ((all_countries_data[storeItem] * 1.0 / total).toFixed(4) * 100.0).toFixed(2) + '%';
            }
        };


        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopCountries_TipRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0;

            App.TopDestinationCountriesChart.getStore().each(function (rec) {
                total += rec.get('TotalDuration');
            });

            this.setTitle(
                storeItem.get('CountryName') + ': ' +
                ((storeItem.get('TotalDuration') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Net Duration: ' + chartsDurationFormat(storeItem.get('TotalDuration')) + ' hours.' +
                '<br>' + 'Net Cost: ' + storeItem.get('TotalCost') + ' euros'
            );
        };


        var GetHoursFromMinutes = function (value) {
            var sec_num = parseInt(value, 10);
            var hours = Math.floor(sec_num / 60);
            var minutes = Math.floor((sec_num - (hours * 60)));
            return hours + "." + minutes;
        };

        var redirect_to_manage_phonecalls = function () {
            //window.location = 'dashboard.aspx';
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
       
        <div class="smart-block tool-dark-bg liquid toolbar" >
            <div class="hwrapper rtl ">
                <div class="col size2of3">
                    <div class="ie7flot-fix" ><!--ie7 fix-->
                        <ul id="navigation-tabs" class="vertical-navigation">
                            <li id="home-tab" class="selected">
                                <a title="Home" href="../user/dashboard.aspx">Home</a>
                            </li>

                            <li id="user-tab" class="">
                                <a href="#"><%= ((Lync_Billing.DB.UserSession)HttpContext.Current.Session.Contents["UserData"]).DisplayName %><span class="drop"></span></a>
                                <ul id="user-dropdown">
                                    <li class="first-child"><a title="Manage My Phone Calls" href="../user/phonecalls.aspx">Phone Calls</a></li>
                                    <li class="separator-bottom"><a title="Address Book" href="../user/addressbook.aspx">Address Book</a></li>
                                    <%
                                        bool is_delegate = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                        if (is_delegate) {
                                    %>
                                        <li class="separator-bottom"><a href="../user/manage_delegates.aspx">Delegated Users</a></li>
                                    <% } %>
                                    <li class=""><a title="Call History" href="../user/history.aspx">Calls History</a></li>
                                    <li class="separator-bottom"><a title="Call Statistics" href="../user/statistics.aspx">Calls Statistics</a></li>
                                    <li class="last-child"><a title="Logout" href="../session/logout.aspx">Logout</a></li>
                                </ul>
                            </li>

                            <%
                                bool higher_access = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAccountant || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAdmin;
                                if (higher_access) {
                            %>
                                <li id="switch-roles" class="">
                                    <a href="#">Elevate Access<span class="drop"></span></a>
                                    <ul id="roles-dropdown">
                                        <% if(((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAccountant || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper) { %>
                                            <li class="first-child last-child"><a title="Elevate Access to Accounting Role" href="../accounting/dashboard.aspx">Accounting Role</a></li>
                                        <% } if (((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAdmin || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper) { %>
                                            <li class="first-child last-child"><a title="Elevate Access to Administrator Role" href="#">Administrator Role</a></li>
                                        <% } %>
                                    </ul>
                                </li>
                            <% } %>
                        </ul>
                    </div>
                </div>
                <!--end toolbar right nav-->
                <div class="col size1of3 lastcol">
                    <a class="logo fl" href='../user/dashboard.aspx'>eBill</a>
                </div>
            </div>
            <!--end toolbar wrapper--> 
        </div>
        <!-- toolbar block-->

        <div id='main' class='main-container bottom-rounded'>
            <% if (false) { %>
                <div id='announcements' class='announce-block shadow mb20 p10'>
                    <div class='m10'>
                        <p class='font-18'>ANNOUNCEMENTS!</p>
                    </div>
                    <div class="block-body">
                        <p class='font-14'>Welcome to the new eBill, it's now more customized and personal. Please take your time going through your personal analytics and have a look at our new personal management tools.</p>
                    </div>
                </div>
                
                <div class='clear h20'></div>
            <% } %>

            <% if(unmarked_calls_count != null) { %>
                <% if(unmarked_calls_count > 0) { %>
                    <div id='warning-block' class='warning-block shadow'>
                        <p class="message"><%= String.Format("You have a total of <span class='bold'>{0}&nbsp;unmarked</span> calls, please click <a class='link bold' href='../user/phonecalls.aspx'>here</a> to mark them.", unmarked_calls_count) %></p>
                    </div>
                <% } else { %>
                    <div id='information-block' class='info-block shadow'>
                        <p class="message">All of your phone calls are marked. Thank you for keeping your phone calls updated!</p>
                    </div>
                <% } %>
                
                <div class='clear h20'></div>
            <% } %>

            <!-- START OF LEFT COLUMN -->
            <div style="float: left; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
                <div id='top-destination-countries' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                         <ext:Panel
                            ID="TopDestinationCountriesPanel"
                            runat="server"
                            Width="465"
                            Height="360"
                            Header="True"
                            Title="Top Destination Countries"
                            Layout="FitLayout">
                            <Items>
                                <ext:Chart
                                    ID="TopDestinationCountriesChart"
                                    runat="server"
                                    Animate="true"
                                    Shadow="true"
                                    InsetPadding="20"
                                    Width="465"
                                    Height="350"
                                    Theme="Base:gradients">
                                    <LegendConfig Position="Right" />
                                    <Store>
                                        <ext:Store ID="TopDestinationCountriesStore"
                                            OnLoad="TopDestinationCountriesStore_Load"
                                            runat="server">
                                            <Model>
                                                <ext:Model ID="TopDestinationCountriesModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="CountryName" />
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
                                            <Label Field="CountryName" Display="Rotate" Contrast="true" Font="16px Arial">
                                                <Renderer Fn="TopCountries_LableRenderer" />
                                            </Label>
                                            <Tips ID="Tips2" runat="server" TrackMouse="true" Width="200" Height="75">
                                                <Renderer Fn="TopCountries_TipRenderer" />
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
            </div>
            <!-- END OF LEFT COLUMN -->

            <!-- START OF RIGHT COLUMN -->
            <div style="float: right; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
                <div id='TOP-Destination-Numbers-Block' class='block wauto'>
                    <div class='content wauto float-left mb10'>
                        <ext:GridPanel
                            ID="TOPDestinationNumbersGrid"
                            runat="server"
                            Title="Top Destination Numbers"
                            Width="465"
                            Height="180"
                            AutoScroll="true"
                            Header="true"
                            Scroll="Both"
                            Layout="FitLayout">
                            <Store>
                                <ext:Store
                                    ID="TopDestinationNumbersStore"
                                    runat="server"
                                    OnLoad="TopDestinationNumbersStore_Load">
                                    <Model>
                                        <ext:Model ID="TopDestinationNumbersModel" runat="server" IDProperty="SessionIdTime">
                                            <Fields>
                                                <ext:ModelField Name="PhoneNumber" Type="String" />
                                                <ext:ModelField Name="UserName" Type="String" />
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
                                        Text="Number / Sip"
                                        Width="160"
                                        DataIndex="PhoneNumber">
                                    </ext:Column>

                                    <ext:Column
                                        ID="UserName"
                                        runat="server"
                                        Text="User"
                                        Width="180"
                                        DataIndex="UserName" />

                                    <ext:Column
                                        ID="NumberOfPhoneCalls"
                                        runat="server"
                                        Text="Number of Calls"
                                        Width="120"
                                        DataIndex="NumberOfPhoneCalls" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </div>
                </div>
                <!-- END OF BLOCk -->

                <div class='clear h20'></div>

                <div id='duration-cost-chart-block' class='block wauto'>
                    <div class="content wauto float-left mb10">
                        <ext:Panel
                            ID="DurationCostChartPanel"
                            runat="server"
                            Width="465"
                            Height="410"
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
            </div>
            <!-- END OF RIGHT COLUMN -->
        </div>

        <div class='clear h10'></div>

        <div id='footer' class='footer-container'>
        </div>
    </form>
</body>
</html>


<%--
    <%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.User_Dashboard" %>
    <asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server"></asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server"></asp:Content>
--%>
