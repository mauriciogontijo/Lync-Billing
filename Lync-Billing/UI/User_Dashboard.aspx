<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.User_Dashboard" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<?xml version="1.0" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>eBill | User Homepage</title>

    <link rel="stylesheet" type="text/css" href="css/reset.css" />
    <link rel="stylesheet" type="text/css" href="css/green-layout.css" />
    <link rel="stylesheet" type="text/css" href="css/toolkit.css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="js/browserdetector.js"></script>
    <script type="text/javascript" src="js/toolkit.js"></script>

    <!--[if lt IE 9]>
		    <link rel="stylesheet" type="text/css" href="css/green-layout-ie-8.css" />
	    <![endif]-->

    <!--[if lt IE 8]>
		    <style type="text/css">
			    #main { padding-top: 65px !important; }
		    </style>
	    <![endif]-->

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
            value = Ext.util.Format.date(value, "d M Y h:i A");
            return value;
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

        var tipCostRenderer = function (storeItem, item) {
            var total = 0;

            App.PhoneCallsCostChart.getStore().each(function (rec) {
                total += rec.get('TotalCost');
            });

            this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalCost') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br>' + 'Net Cost: ' + storeItem.get('TotalCost') + ' euros'
            );
        };

        var tipDuartionRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0;

            App.PhoneCallsCostChart.getStore().each(function (rec) {
                total += rec.get('TotalDuration');
            });

            this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalDuration') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br>' + 'Net Duration: ' + chartsDurationFormat(storeItem.get('TotalDuration')) + ' hours.'
            );
        };

        var TotalDurationLableRenderer = function (storeItem, item) {
            var total = 0, business_duration = 0, personal_duration = 0, unmarked_duration = 0;

            App.PhoneCallsCostChart.getStore().each(function (rec) {
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


        var TotalCostLableRenderer = function (storeItem, item) {
            var total = 0, b_total = 0, p_total = 0, u_total = 0;

            App.PhoneCallsCostChart.getStore().each(function (rec) {
                total += rec.get('TotalCost');

                if (rec.get('Name') == 'Business') {
                    b_total = rec.get('TotalCost');
                }
                else if (rec.get('Name') == 'Personal') {
                    p_total = rec.get('TotalCost');
                }
                else if (rec.get('Name') == 'Unmarked') {
                    u_total = rec.get('TotalCost');
                }
            });

            if (storeItem == "Business") {
                return ((b_total / total).toFixed(4) * 100.0).toFixed(2) + '%';
            }
            else if (storeItem == "Personal") {
                return ((p_total / total).toFixed(4) * 100.0).toFixed(2) + '%';
            }
            else if (storeItem == "Unmarked") {
                return ((u_total / total).toFixed(4) * 100.0).toFixed(2) + '%';
            }
        };

        var redirect = function () {
            window.location = "User_ManagePhoneCalls.aspx";
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

        <div id='header-container'>
            <div id='nav-toolbar' class='nav-toolbar text-center'>
                <div id='logo' class='logo float-left'>
                    <p><a href='User_Dashboard.aspx'>LOGO</a></p>
                </div>

                <% if (false)
                   { %>
                <ul id='nav-buttons'>
                    <li class='nav-button'>
                        <!--<a href='#' class='nav-button-item'><img src='images/settings.png' width='32' /></a>-->
                        <a id='settings-menu-button' href="javascript:void(0)">
                            <img alt='Settings Menu' src='images/settings.png' width='32' /></a>
                        <div id="settings-more-list-container" style="display: block;">
                            <!---<ul id="settings-more-list">
						                <li><a class='settings-menu-text' title="jQuery Cheat Sheet" href="/jquery">Example</a></li>
						                <li><a class='settings-menu-text' title="NodeJS Cheat Sheet" href="/nodejs">Example</a></li>
						                <li><a class='settings-menu-text' title="PHP Cheat Sheet" href="/php">Example</a></li>
						                <li><a class='settings-menu-text' title="Java Cheat Sheet" href="/java">Example</a></li>
						                <li><a class='settings-menu-text' href="/#more">Even More ></a></li>
						            </ul>---->
                            <a href="#">One 123123</a>
                            <a href="#">Two 123123</a>
                            <a href="#">Three 123123</a>
                        </div>
                    </li>
                    <li class='nav-button'><a href='user-inner-page.html' class='nav-button-item'>
                        <img alt='Manage My Bills' src='images/mybills.png' width='32' /></a></li>
                    <li class='nav-button'><a href='user-inner-page.html' class='nav-button-item'>
                        <img alt='Manage My Phone Calls' src='images/phonecalls.png' width='40' /></a></li>
                </ul>
                <% } %>
            </div>
        </div>

        <div id='main' class='main bottom-rounded'>
            <%--<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">--%>
            <div id='announcements' class='announcements shadow mb20 p10'>
                <div class='m10'>
                    <p class='font-18'>ANNOUNCEMENTS!</p>
                </div>
                <div class="block-body">
                    <p class='font-14'>Welcome to the new eBill, it's now more customized and personal. Please take your time going through your personal analytics and have a look at our new personal management tools.</p>
                </div>
            </div>

            <div class='clear h15'></div>
            
            <div id='duration-cost-chart-block' class='block float-left w49p'>
                <div class="content wauto float-left mb10">
                    <ext:Panel
                        ID="DurationCostChartPanel"
                        runat="server"
                        Width="460"
                        Height="350"
                        Header="True"
                        Title="Personal Duration & Cost Report"
                        Layout="FitLayout">
                        <Items>
                            <ext:Chart 
                                ID="DurationCostChart" 
                                runat="server" 
                                Animate="true">
                                <Store>
                                    <ext:Store ID="DurationCostChartStore" runat="server" >
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
                                        Title="Duration in Munites"
                                        Fields="Duration" 
                                        Position="Left">
                                            <LabelTitle Fill="#115fa6" />
                                            <Label Fill="#115fa6" />
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
                                    </ext:VerticalMarker>
                                </Plugins>
                                <LegendConfig Position="Bottom" />
                            </ext:Chart>
                        </Items>
                    </ext:Panel>
                </div><!-- END OF CONTENT -->
            </div><!-- END OF BLOCk -->

            <div id='duration-report-block' class='block float-right w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:Panel ID="PhoneCallsDuartionChartPanel"
                        runat="server"
                        Title="Duration Report (Last 3 Months)"
                        Width="465"
                        Height="350"
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
                                        <Tips ID="Tips1" runat="server" TrackMouse="true" Width="200" Height="55">
                                            <Renderer Fn="tipDuartionRenderer" />
                                        </Tips>
                                        <Listeners>
                                            <ItemClick Fn="redirect" />
                                        </Listeners>
                                    </ext:PieSeries>
                                </Series>
                            </ext:Chart>
                        </Items>
                    </ext:Panel>
                </div><!-- END OF CONTENT -->
            </div><!-- END OF BLOCk -->

            <div class='clear h15'></div>

            <div id='brief-history-block' class='block float-left w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:GridPanel
                        ID="PhoneCallsHistoryGrid"
                        runat="server"
                        Title="History Brief"
                        Width="465"
                        Height="240"
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
                    </ext:GridPanel>
                </div><!-- END OF CONTENT -->
				<div class='clear h5'></div>
				<div class='more-button wauto float-right mb5'>
					<a href='User_ViewHistory.aspx' class='font-10'>view more >></a>
				</div>
            </div><!-- END OF BLOCk -->

            <div id='summary-block' class='block float-right w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:Panel ID="UserPhoneCallsSummary"
                        runat="server"
                        Height="240"
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
                </div><!-- END OF CONTENT -->
            </div><!-- END OF BLOCk -->

            <div class='clear h15'></div>

            <div id='cost-report-block' class='block float-left w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:Panel ID="PhoneCallsCostChartPanel"
                        runat="server"
                        Title="Cost Report (Last 3 Months)"
                        Width="465"
                        Height="350"
                        Layout="FitLayout">
                        <Items>
                            <ext:Chart
                                ID="PhoneCallsCostChart"
                                runat="server"
                                Animate="true"
                                Shadow="true"
                                InsetPadding="20"
                                Width="465"
                                Height="350"
                                Theme="Base:gradients">
                                <LegendConfig Position="Right" />
                                <Store>
                                    <ext:Store ID="PhoneCallsCostChartStore"
                                        OnLoad="PhoneCallsCostChartStore_Load"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="PhoneCallsCostChartModel" runat="server">
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
                                        AngleField="TotalCost"
                                        ShowInLegend="true"
                                        Donut="30"
                                        Highlight="true"
                                        HighlightSegmentMargin="10">
                                        <Label Field="Name" Display="Rotate" Contrast="true" Font="16px Arial">
                                            <Renderer Fn="TotalCostLableRenderer" />
                                        </Label>
                                        <Tips runat="server" TrackMouse="true" Width="200" Height="55">
                                            <Renderer Fn="tipCostRenderer" />
                                        </Tips>
                                        <Listeners>
                                            <ItemClick Fn="redirect" />
                                        </Listeners>
                                    </ext:PieSeries>
                                </Series>

                            </ext:Chart>
                        </Items>
                    </ext:Panel>
                </div><!-- END OF CONTENT -->
            </div><!-- END OF BLOCk -->

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
