<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.user.dashboard" %>
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
    <script type="text/javascript" src="../resources/js/ext-js-specific.js"></script>

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
                                <a href="#"><%= current_session.EffectiveDisplayName %><span class="drop"></span></a>
                                <ul id="user-dropdown">
                                    <li class="first-child"><a title="Manage My Phone Calls" href="../user/phonecalls.aspx">Phone Calls</a></li>
                                    <li class="separator-bottom"><a title="Address Book" href="../user/addressbook.aspx">Address Book</a></li>
                                    <li class=""><a title="Bills History" href="../user/bills.aspx">Bills History</a></li>
                                    <li class=""><a title="Calls History" href="../user/history.aspx">Calls History</a></li>
                                    <% if(current_session.PrimarySipAccount == current_session.EffectiveSipAccount) { %>
                                        <li class="separator-bottom"><a title="Calls Statistics" href="../user/statistics.aspx">Calls Statistics</a></li>
                                        <li class="last-child"><a title="Logout" href="../session/logout.aspx">Logout</a></li>
                                    <% } else { %>
                                        <li class="last-child"><a title="Calls Statistics" href="../user/statistics.aspx">Calls Statistics</a></li>
                                    <% } %>
                                </ul>
                            </li>

                            <%
                                //The first part of the condition checks if the user has an actual permission of access-elevation; the second part is to ensure that delegated user don't abuse the current user's permissions of access elevation
                                bool is_delegate = (current_session.IsDelegate || current_session.IsDeveloper) && (current_session.PrimarySipAccount == current_session.EffectiveSipAccount) && current_session.ListOfDelegees.Count > 0;
                                if (is_delegate)
                                {
                            %>
                                <li id="switch-to-delegee" class="">
                                    <a href="#">Switch to User<span class="drop"></span></a>
                                    <ul id="delegees-dropdown">
                                    <% foreach(KeyValuePair<string, string> delegeeRecord in current_session.ListOfDelegees) { %>
                                        <li class="last-child">
                                            <a href="../user/delegees.aspx?identity=<%= delegeeRecord.Key %>">
                                                <%= delegeeRecord.Value %>
                                            </a>
                                        </li>
                                    <% } %>
                                    </ul>
                                </li>
                            <% } %>

                            <%
                                //The first part of the condition checks if the user has an actual permission of access-elevation; the second part is to ensure that delegated user don't abuse the current user's permissions of access elevation
                                bool higher_access = (current_session.IsAccountant || current_session.IsDeveloper || current_session.IsAdmin) && (current_session.PrimarySipAccount == current_session.EffectiveSipAccount);
                                if (higher_access) {
                            %>
                                <li id="switch-roles" class="">
                                    <a href="#">Elevate Access<span class="drop"></span></a>
                                    <ul id="roles-dropdown">
                                        <% if(current_session.IsAccountant || current_session.IsDeveloper) { %>
                                            <li class="first-child"><a title="Elevate Access to Accounting Role" href="../accounting/main/dashboard.aspx">Accounting Role</a></li>
                                        <% } if (current_session.IsAdmin || current_session.IsDeveloper) { %>
                                            <li class="first-child last-child"><a title="Elevate Access to Administrator Role" href="../admin/main/dashboard.aspx">Administrator Role</a></li>
                                        <% } %>
                                    </ul>
                                </li>
                            <% } %>

                            <% if(current_session.PrimarySipAccount != current_session.EffectiveSipAccount) { %>
                                <li id="access-tab">
                                    <a title="Drop delegee access" href="../user/delegees.aspx?identity=<%= current_session.PrimarySipAccount %>">Drop Delegee Access<span class="shutdown"></span></a>
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
    <%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.ui.User_Dashboard" %>
    <asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server"></asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server"></asp:Content>
--%>
