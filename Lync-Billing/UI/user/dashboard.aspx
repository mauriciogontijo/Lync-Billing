<%@ Page Title="eBill User | Dashboard" Language="C#" AutoEventWireup="true" MasterPageFile="~/ui/MasterPage.Master" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.user.dashboard" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopDestinationCountries_LableRenderer = function (storeItem, item) {
            var total = 0,
                all_countries_data = {},
                component_name = "main_content_place_holder_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('CallsDuration');

                var country_name = rec.get('CountryName');
                if (country_name != 0 && all_countries_data[country_name] == undefined) {
                    all_countries_data[country_name] = rec.get('CallsDuration');
                }
            });

            if (all_countries_data[storeItem] != undefined) {
                var percentage = ((all_countries_data[storeItem] * 1.0 / total).toFixed(4) * 100.0).toFixed(2);
                return ((percentage < 3.5) ? '' : (percentage + '%'));
            }
        };


        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopDestinationCountries_TipRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0,
                component_name = "main_content_place_holder_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('CallsDuration');
            });

            this.setTitle(
                storeItem.get('CountryName') + ': ' +
                ((storeItem.get('CallsDuration') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br>' + 'Net Duration: ' + chartsDurationFormat(storeItem.get('CallsDuration')) + ' hours.' +
                '<br>' + 'Net Cost: ' + RoundCostsToTwoDecimalDigits(storeItem.get('CallsCost')) + ' euros'
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

    <% if(unmarkedCallsCount != null) { %>
        <% if(unmarkedCallsCount > 0) { %>
            <div id='warning-block' class='warning-block shadow'>
                <p class="message"><%= String.Format("You have a total of <span class='bold'>{0}&nbsp;unmarked</span> calls, please click <a class='link bold' href='../user/phonecalls.aspx'>here</a> to mark them.", unmarkedCallsCount) %></p>
            </div>
        <% } else { %>
            <div id='information-block' class='info-block shadow'>
                <p class="message">All of your phone calls are marked. Thank you for keeping your phone calls updated!</p>
            </div>
        <% } %>
                
        <div class='clear h25'></div>
    <% } %>

    <!-- START OF LEFT COLUMN -->
    <div style="float: left; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
        <div id='top-destination-countries' class='block wauto'>
            <div class='content wauto float-left mb10'>
                <ext:Panel
                    ID="TopDestinationCountriesPanel"
                    runat="server"
                    Width="465"
                    Height="380"
                    Header="True"
                    Title="Most Called Countries"
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
                                                <ext:ModelField Name="CallsCost" />
                                                <ext:ModelField Name="CallsDuration" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Series>
                                <ext:PieSeries
                                    AngleField="CallsDuration"
                                    ShowInLegend="true"
                                    Donut="30"
                                    Highlight="true"
                                    HighlightSegmentMargin="10">
                                    <Label Field="CountryName" Display="Rotate" Contrast="true" Font="16px Arial">
                                        <Renderer Fn="TopDestinationCountries_LableRenderer" />
                                    </Label>
                                    <Tips ID="Tips2" runat="server" TrackMouse="true" Width="200" Height="75">
                                        <Renderer Fn="TopDestinationCountries_TipRenderer" />
                                    </Tips>
                                </ext:PieSeries>
                            </Series>
                        </ext:Chart>
                    </Items>
                </ext:Panel>
            </div>
        </div>
        <!-- END OF BLOCk -->

        <div class='clear h15'></div>

        <div id='top-destination-numbers-block' class='block wauto'>
            <div class='content wauto float-left mb10'>
                <ext:GridPanel
                    ID="TOPDestinationNumbersGrid"
                    runat="server"
                    Title="Most Called Numbers"
                    Width="465"
                    Height="190"
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
                                        <ext:ModelField Name="CallsCount" Type="Int" />
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
                                Text="Phone Number"
                                Width="160"
                                DataIndex="PhoneNumber">
                            </ext:Column>

                            <ext:Column
                                ID="UserName"
                                runat="server"
                                Text="Addressbook Contact Name"
                                Width="180"
                                DataIndex="UserName" />

                            <ext:Column
                                ID="CallsCount"
                                runat="server"
                                Text="Number of Calls"
                                Width="120"
                                DataIndex="CallsCount" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </div>
        </div>
        <!-- END OF BLOCk -->
    </div>
    <!-- END OF LEFT COLUMN -->

    <!-- START OF RIGHT COLUMN -->
    <div style="float: right; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
        <div id='user-mail-statistics-block' class='block wauto'>
            <div class='content wauto float-left mb10'>
                <ext:Panel
                    ID="UserMailStatistics" 
                    runat="server"
                    Header="true"
                    Title="Mail Statistics"
                    PaddingSummary="10px 10px 10px 10px"
                    Width="465"
                    Height="180"
                    ButtonAlign="Center">
                    <Defaults>
                        <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                    </Defaults>

                    <Content>
                        <div class="p10 font-14">
                            <p class="mb5">Number of Received Mails: <span class="bold red-color"><%= userMailStatistics.ReceivedCount %></span></p>
                            <p class="mb5">Size of Received Mails: <span class="bold red-color"><%= userMailStatistics.ReceivedSize %> (in MB)</span></p>
                            <div class="clear h15"></div>
                            <p class="mb5">Number of Sent Mails: <span class="bold blue-color"><%= userMailStatistics.SentCount %></span></p>
                            <p class="mb5">Size of Sent Mails: <span class="bold blue-color"><%= userMailStatistics.SentSize %> (in MB)</span></p>
                        </div>
                    </Content>
                </ext:Panel>
            </div>
            <!-- END OF CONTENT -->
        </div>
        <!-- END OF BLOCk -->

        <div class='clear h15'></div>

        <div id='duration-cost-chart-block' class='block wauto'>
            <div class="content wauto float-left mb10">
                <ext:Panel
                    ID="DurationCostChartPanel"
                    runat="server"
                    Width="465"
                    Height="390"
                    Header="True"
                    Title="Calls Cost Chart"
                    Layout="FitLayout">
                    <Items>
                        <ext:Chart
                            ID="DurationCostChart"
                            runat="server"
                            Animate="true"
                            OnLoad="DurationCostChartStore_Load">
                            <Store>
                                <ext:Store ID="DurationCostChartStore" runat="server">
                                    <Model>
                                        <ext:Model ID="DurationCostChartModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="Date" />
                                                <ext:ModelField Name="Duration" />
                                                <ext:ModelField Name="PersonalCallsCost" />
                                                <ext:ModelField Name="BusinessCallsCost" />
                                                <ext:ModelField Name="UnmarkedCallsCost" />
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
                                    Title="Cost in Local Currency"
                                    Fields="PersonalCallsCost"
                                    Position="Left">
                                    <LabelTitle Fill="#115fa6" />
                                    <Label Fill="#115fa6" />
                                </ext:NumericAxis>
                            </Axes>

                            <Series>
                               <ext:LineSeries
                                    Titles="Personal"
                                    XField="Date"
                                    YField="PersonalCallsCost"
                                    Axis="Left"
                                    Smooth="3">
                                    <HighlightConfig Size="7" Radius="7" />
                                    <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                </ext:LineSeries>
                                 
                                <ext:LineSeries
                                    Titles="Business"
                                    XField="Date"
                                    YField="BusinessCallsCost"
                                    Axis="Left"
                                    Smooth="3">
                                    <HighlightConfig Size="7" Radius="7" />
                                    <MarkerConfig Size="4" Radius="4" StrokeWidth="0" />
                                </ext:LineSeries>
                                
                                <ext:LineSeries
                                    Titles="Unmarked"
                                    XField="Date"
                                    YField="UnmarkedCallsCost"
                                    Axis="Left"
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
</asp:Content>