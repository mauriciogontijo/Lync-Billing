<%@ Page Title="tBill User | Calls Statistics" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.user.statistics" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner { font-family: tahoma, verdana; display: block; font-weight: normal; font-style: normal; color:#385F95; white-space: normal; }
        .x-grid-rowbody div { margin: 2px 5px 20px 5px !important; width: 99%; color: Gray; }
        .x-grid-row-expanded td.x-grid-cell { border-bottom-width: 0px; }
    </style>

	<script type="text/javascript">
	    //Pie Chart Data-Lable Renderer for Personal Calls
	    var TotalDuration_LableRenderer = function (storeItem, item) {
	        var total = 0,
                percentage = 0,
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
	            else if (rec.get('Name') == 'Disputed') {
	                dispute_duration = rec.get('TotalDuration');
	            }
	            else if (rec.get('Name') == 'Unmarked') {
	                unmarked_duration = rec.get('TotalDuration');
	            }
	        });

	        if (storeItem == "Business") {
	            percentage = ((business_duration / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
	        }
	        else if (storeItem == "Personal") {
	            percentage = ((personal_duration / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
	        }
	        else if (storeItem == "Unmarked") {
	            percentage = ((unmarked_duration / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
	        }
	        else if (storeItem == "Disputed") {
	            percentage = ((dispute_duration / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
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
                percentage = 0,
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
	            else if (rec.get('Name') == 'Disputed') {
	                dispute_cost = rec.get('TotalCost');
	            }
	            else if (rec.get('Name') == 'Unmarked') {
	                unmarked_cost = rec.get('TotalCost');
	            }
	        });

	        if (storeItem == "Business") {
	            percentage = ((business_cost / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
	        }
	        else if (storeItem == "Personal") {
	            percentage = ((personal_cost / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
	        }
	        else if (storeItem == "Unmarked") {
	            percentage = ((unmarked_cost / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
	        }
	        else if (storeItem == "Disputed") {
	            percentage = ((dispute_cost / total).toFixed(4) * 100.0).toFixed(2);
	            return ((percentage < 3.0) ? '' : percentage + '%');
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

	</script>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF STATISTICS CHARTS CONTAINER *** -->
    <div class="block float-right w80p h100p">
        <div id='personal-duration-cost-chart' class='block float-right w100p'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="CustomizeStatisticsPanel"
                    runat="server"
                    Height="61"
                    Width="740"
                    Header="True"
                    Title="Customize Statistics"
                    Layout="FitLayout"
                    ComponentCls="fix-ui-vertical-align">
                    <TopBar>
                        <ext:Toolbar
                            ID="CustomizeStatisticsToolbar"
                            runat="server">
                            <Items>
                                <ext:ComboBox
                                    ID="CustomizeStats_Years"
                                    runat="server"
                                    Editable="false"
                                    DisplayField="YearName"
                                    ValueField="YearName"
                                    Width="250"
                                    LabelWidth="70"
                                    LabelSeparator=":"
                                    Margins="5 15 5 5"
                                    FieldLabel="Filter Period">
                                    <Store>
                                        <ext:Store ID="CustomizeStats_YearStore" runat="server" OnLoad="CustomizeStats_YearStore_Load">
                                            <Model>
                                                <ext:Model ID="CustomizeStats_YearStoreModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="YearName" Type="String" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <DirectEvents>
                                        <Select OnEvent="CustomizeStats_Years_Select" />
                                    </DirectEvents>
                                </ext:ComboBox>

                                <ext:ComboBox
                                    ID="CustomizeStats_Quarters"
                                    runat="server"
                                    DisplayField="TypeName"
                                    ValueField="TypeValue"
                                    Editable="false"
                                    FieldLabel="Quarter"
                                    Width="250"
                                    LabelWidth="45"
                                    LabelSeparator=":"
                                    Margins="5 15 5 5">
                                    <Items>
                                        <ext:ListItem Text="1st Quarter" Value="1" />
                                        <ext:ListItem Text="2nd Quarter" Value="2" />
                                        <ext:ListItem Text="3rd Quarter" Value="3" />
                                        <ext:ListItem Text="4th Quarter" Value="4" />
                                        <ext:ListItem Text="All Quarters" Value="5" />
                                    </Items>
                                    <SelectedItems>
                                        <ext:ListItem Text="All Quarters" Value="5" />
                                    </SelectedItems>
                                </ext:ComboBox>

                                <ext:Button
                                    ID="SubmitCustomizeStatisticsBtn"
                                    runat="server"
                                    Text="Apply"
                                    Icon="ApplicationGo"
                                    Height="25"
                                    Margins="5 5 0 125">
                                    <DirectEvents>
                                        <Click OnEvent="SubmitCustomizeStatisticsBtn_Click">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>


                <div class="h25 clear"></div>


                <ext:Panel
                    ID="DurationCostChartPanel"
                    runat="server"
                    Height="420"
                    Width="740"
                    Header="True"
                    Title="Business/Personal Calls"
                    Layout="FitLayout">
                    <Items>
                        <ext:Chart
                            ID="DurationCostChart"
                            runat="server"
                            Animate="true">
                            <Store>
                                <ext:Store ID="DurationCostChartStore" runat="server" OnLoad="DurationCostChartStore_Load">
                                    <Model>
                                        <ext:Model ID="DurationCostChartModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="Date" />
                                                <ext:ModelField Name="PersonalCallsDuration" />
                                                <ext:ModelField Name="PersonalCallsCost" />
                                                <ext:ModelField Name="BusinessCallsDuration" />
                                                <ext:ModelField Name="BusinessCallsCost" />
                                                
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <Axes>
                                <ext:CategoryAxis
                                    Position="Bottom"
                                    Fields="Date"
                                    Title="Current Year">
                                    <Label>
                                        <Renderer Handler="return Ext.util.Format.date(value, 'M');" />
                                    </Label>
                                </ext:CategoryAxis>

                                <ext:NumericAxis
                                    Title="Duration in Seconds"
                                    Fields="PersonalCallsDuration,BusinessCallsDuration"
                                    Position="Left">
                                    <LabelTitle Fill="#115fa6" />
                                    <Label Fill="#115fa6" />
                                    <Label>
                                        <Renderer Fn="GetHoursFromMinutes" />
                                    </Label>
                                </ext:NumericAxis>

                                <ext:NumericAxis
                                    Title="Cost in Local Currency"
                                    Fields="PersonalCallsCost,BusinessCallsCost"
                                    Position="Right">
                                    <LabelTitle Fill="#94ae0a" />
                                    <Label Fill="#94ae0a" />
                                </ext:NumericAxis>
                            </Axes>

                            <Series>
                                <ext:LineSeries
                                    Titles="Personal Duartion"
                                    XField="Date"
                                    YField="PersonalCallsDuration"
                                    Axis="Left"
                                    Smooth="3">
                                    <HighlightConfig Size="7" Radius="7" />
                                    <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                </ext:LineSeries>

                                <ext:LineSeries
                                    Titles="Personal Cost"
                                    XField="Date"
                                    YField="PersonalCallsCost"
                                    Axis="Right"
                                    Smooth="3">
                                    <HighlightConfig Size="7" Radius="7" />
                                    <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                </ext:LineSeries>

                                 <ext:LineSeries
                                    Titles="Business Duartion"
                                    XField="Date"
                                    YField="BusinessCallsDuration"
                                    Axis="Left"
                                    Smooth="3">
                                    <HighlightConfig Size="7" Radius="7" />
                                    <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                </ext:LineSeries>

                                <ext:LineSeries
                                    Titles="Business Cost"
                                    XField="Date"
                                    YField="BusinessCallsCost"
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


        <div class="clear h15"></div>


        <div id='personal-calls-duration-pie-chart' class='block float-right w49p hauto'>
            <div class="block-body">
                <ext:Panel ID="PhoneCallsDuartionChartPanel"
                    runat="server"
                    Width="364"
                    Height="320"
                    Layout="FitLayout"
                    Header="true"
                    Title="Calls Duration">
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
                                    <%--<Listeners>
                                        <ItemClick Fn="redirect_to_manage_phonecalls" />
                                    </Listeners>--%>
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
                    Layout="FitLayout"
                    Header="true"
                    Title="Calls Costs">
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
                                    <%--<Listeners>
                                        <ItemClick Fn="redirect_to_manage_phonecalls" />
                                    </Listeners>--%>
                                </ext:PieSeries>
                            </Series>
                        </ext:Chart>
                    </Items>
                </ext:Panel>
            </div>
        </div><!-- END OF COST PIE CHART -->
    </div>
</asp:Content>