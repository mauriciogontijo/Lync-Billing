<%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.user.statistics" %>

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

	    //Pie Chart Data-Lable Renderer for Personal Calls
	    var TotalDuration_LableRenderer = function (storeItem, item) {
	        var total = 0,
                business_duration = 0,
                personal_duration = 0,
                unmarked_duration = 0,
                dispute_duration = 0,
                chart_element_id = "main_content_place_holder_" + "PhoneCallsDuartionChart";

	        //App.PhoneCallsDuartionChart
	        App[chart_element_id].getStore().each(function (rec) {
	            total += rec.get('TotalDuration');

	            if (rec.get('Name') == 'Business') {
	                business_duration = rec.get('TotalDuration');
	            }
	            else if (rec.get('Name') == 'Personal') {
	                personal_duration = rec.get('TotalDuration');
	            }
	            else if (rec.get('Name') == 'Dispute') {
	                dispute_duration = rec.get('TotalDuration');
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
	        else if (storeItem == "Dispute") {
	            return ((dispute_duration / total).toFixed(4) * 100.0).toFixed(2) + '%';
	            //return unmarked_duration;
	        }
	    };


	    //Pie Chart Data-Tip Renderer for Personal Calls
	    var TotalDuration_TipRenderer = function (storeItem, item) {
	        var total = 0,
                chart_element_id = "main_content_place_holder_" + "PhoneCallsDuartionChart";

	        //App.PhoneCallsDuartionChart
	        App[chart_element_id].getStore().each(function (rec) {
	            total += rec.get('TotalDuration');
	        });

	        this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalDuration') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br>' + 'Total Duration: ' + chartsDurationFormat(storeItem.get('TotalDuration')) + ' hours.'
            );
	        //'<br>' + 'Total Cost: ' + storeItem.get('TotalCost') + ' euros'
	    };


	    //Pie Chart Data-Lable Renderer for Personal Calls
	    var TotalCost_LableRenderer = function (storeItem, item) {
	        var total = 0,
                business_cost = 0,
                personal_cost = 0,
                unmarked_cost = 0,
                dispute_cost = 0,
                chart_element_id = "main_content_place_holder_" + "PhoneCallsCostChart";

	        //App.PhoneCallsDuartionChart
	        App[chart_element_id].getStore().each(function (rec) {
	            total += rec.get('TotalCost');

	            if (rec.get('Name') == 'Business') {
	                business_cost = rec.get('TotalCost');
	            }
	            else if (rec.get('Name') == 'Personal') {
	                personal_cost = rec.get('TotalCost');
	            }
	            else if (rec.get('Name') == 'Dispute') {
	                dispute_cost = rec.get('TotalCost');
	            }
	            else if (rec.get('Name') == 'Unmarked') {
	                unmarked_cost = rec.get('TotalCost');
	            }
	        });

	        if (storeItem == "Business") {
	            return ((business_cost / total).toFixed(4) * 100.0).toFixed(2) + '%';
	        }
	        else if (storeItem == "Personal") {
	            return ((personal_cost / total).toFixed(4) * 100.0).toFixed(2) + '%';
	        }
	        else if (storeItem == "Unmarked") {
	            return ((unmarked_cost / total).toFixed(4) * 100.0).toFixed(2) + '%';
	        }
	        else if (storeItem == "Dispute") {
	            return ((dispute_cost / total).toFixed(4) * 100.0).toFixed(2) + '%';
	        }
	    };


	    //Pie Chart Data-Tip Renderer for Personal Calls
	    var TotalCost_TipRenderer = function (storeItem, item) {
	        var total = 0,
                chart_element_id = "main_content_place_holder_" + "PhoneCallsCostChart";

	        //App.PhoneCallsDuartionChart
	        App[chart_element_id].getStore().each(function (rec) {
	            total += rec.get('TotalCost');
	        });

	        this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalCost') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br>' + 'Total Cost: ' + storeItem.get('TotalCost') + ' euros'
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

	    var redirect_to_manage_phonecalls = function () {
	        to = "../user/phonecalls.aspx"
	        window.location = to;
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
                            <p><a href='../user/bills.aspx'>Bills History</a></p>
                            <p><a href='../user/history.aspx'>Phone Calls History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/statistics.aspx' class="selected">Phone Calls Statistics</a></p>
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
                                <p><a href='../user/delegees.aspx'>Manage Delegee(s)</a></p>
                            </div>
                        </div>
                    <% } %>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF STATISTICS CHARTS CONTAINER *** -->
    <div class="block float-right w80p h100p">
        <div id='personal-duration-cost-chart' class='block float-right wauto w100p'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="DurationCostChartPanel"
                    runat="server"
                    Height="420"
                    Width="740"
                    Header="True"
                    Title="Personal Duration/Cost Reports"
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
        </div>

        <div class="clear h5"></div>

        <div id='personal-calls-duration-pie-chart' class='block float-right w49p hauto'>
            <div class="block-body">
                <ext:Panel ID="PhoneCallsDuartionChartPanel"
                    runat="server"
                    Width="364"
                    Height="320"
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
                                        <Renderer Fn="TotalDuration_LableRenderer" />
                                    </Label>
                                    <Tips ID="Tips1" runat="server" TrackMouse="true" Width="200" Height="75">
                                        <Renderer Fn="TotalDuration_TipRenderer" />
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
        </div><!-- END OF DURATION PIE CHART -->

        <div id='total-cost-chart' class='block float-right w49p hauto'>
            <div class="block-body">
                <ext:Panel ID="PhoneCallsCostChartPanel"
                    runat="server"
                    Width="364"
                    Height="320"
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
                                        <Renderer Fn="TotalCost_LableRenderer" />
                                    </Label>
                                    <Tips ID="Tips2" runat="server" TrackMouse="true" Width="200" Height="75">
                                        <Renderer Fn="TotalCost_TipRenderer" />
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
        </div><!-- END OF COST PIE CHART -->
    </div>
</asp:Content>