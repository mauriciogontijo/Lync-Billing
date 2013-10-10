<%@ Page Title="eBill Depart. Head | Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.dephead.main.statistics" %>

<%-- DEPARTMENT-HEAD # MAIN # STATISTICS --%>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var TopCountries_LableRenderer = function (storeItem, item) {
            var total = 0,
                all_countries_data = {},
                component_name = "main_content_place_holder_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('TotalDuration');

                var country_name = rec.get('CountryName');
                if (country_name != 0 && all_countries_data[country_name] == undefined) {
                    all_countries_data[country_name] = rec.get('TotalDuration');
                }
            });

            if (all_countries_data[storeItem] != undefined) {
                var percentage = ((all_countries_data[storeItem] * 1.0 / total).toFixed(4) * 100.0).toFixed(2);
                if (percentage < 3.3) {
                    return '';
                } else {
                    return percentage + '%'
                }
            }
        };


        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopCountries_TipRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0,
                component_name = "main_content_place_holder_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
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
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div class="block float-right w80p h100p">
        <div id='personal-duration-cost-chart' class='block float-right w100p'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="FilterDepartmentStatisticsPanel"
                    runat="server"
                    Header="true"
                    Title="Department Statistics"
                    Width="740"
                    Height="61"
                    Layout="AnchorLayout">
                    <TopBar>
                        <ext:Toolbar
                            ID="FilterToolbar1"
                            runat="server">
                            <Items>
                                <ext:ComboBox
                                    ID="FilterDepartments"
                                    runat="server"
                                    Icon="Find"
                                    TriggerAction="All"
                                    QueryMode="Local"
                                    DisplayField="DepartmentName"
                                    ValueField="DepartmentName"
                                    FieldLabel="Department:"
                                    LabelWidth="60"
                                    Width="300"
                                    Margins="5 15 5 5"
                                    Editable="false">
                                    <Store>
                                        <ext:Store 
                                            ID="DepartmentsFilterStore"
                                            runat="server">
                                            <Model>
                                                <ext:Model 
                                                    ID="DepartmentHeadsStoreModel"
                                                    runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="DepartmentName" />
                                                        <ext:ModelField Name="SiteName" />
                                                        <ext:ModelField Name="SiteID" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <ListConfig
                                        Border="true"
                                        EmptyText="Please select a department...">
                                        <ItemTpl ID="ItemTpl2" runat="server">
                                            <Html>
                                                <div>{DepartmentName}&nbsp;({SiteName})</div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>

                                    <DirectEvents>
                                        <Select OnEvent="DrawStatisticsForDepartment">
                                            <EventMask ShowMask="true" />
                                        </Select>
                                    </DirectEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
            </div>
        </div>

        <div class="h5 clear display-none"></div>

        <div id='business-to-personal-phonecalls-chart' class='block float-right w100p display-none'>
            <div class="block-body pt5">
                <ext:Panel ID="DepartmentCallsPerMonthChartPanel" 
                    runat="server"
                    Width="740"
                    Height="400"
                    Title="Total Phonecalls Distribution For this Department"
                    Layout="FitLayout"
                    Visible="false">
                    <Items>
                        <ext:Chart 
                            ID="DepartmentCallsPerMonthChart" 
                            runat="server"
                            Shadow="true"
                            Animate="true">
                            <Store>
                                <ext:Store ID="DepartmentCallsPerMonthChartStore" 
                                    runat="server" 
                                    AutoDataBind="true">                           
                                    <Model>
                                        <ext:Model ID="DepartmentCallsPerMonthChartStoreModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="MonthDate" />
                                                <ext:ModelField Name="BusinessCallsCount" />
                                                <ext:ModelField Name="PersonalCallsCount" />
                                                <ext:ModelField Name="UnallocatedCallsCount" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <LegendConfig Position="Right" />

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
                                    Title="Total Phonecalls"
                                    Fields="Business,Personal,Unallocated"
                                    Position="Left"
                                    Grid="true">
                                    <%--<Label>
                                        <Renderer Handler="return String(value).replace(/00$/, 'M');" />
                                    </Label>--%>
                                </ext:NumericAxis>
                            </Axes>

                            <Series>
                                <ext:BarSeries 
                                    Axis="Left"
                                    Gutter="80"
                                    XField="Month" 
                                    YField="Business,Personal,Unallocated"
                                    Stacked="true">
                                    <%--<Tips TrackMouse="true" Width="28" Height="65">
                                        <Renderer Handler="this.setTitle(String(item.value[1] / 100) + 'M');" />
                                    </Tips>--%>
                                </ext:BarSeries>
                            </Series>
                        </ext:Chart>
                    </Items>
                </ext:Panel>
            </div>
        </div>

        <div class="h5 clear"></div>

        <div id='top-5-destinations-by-cost-chart' class='block float-right w49p hauto'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="TopDestinationCountriesPanel"
                    runat="server"
                    Width="470"
                    Height="350"
                    Header="True"
                    Title="Top Destinations By Cost"
                    Layout="FitLayout">
                    <Items>
                        <ext:Chart
                            ID="TopDestinationCountriesChart"
                            runat="server"
                            Animate="true"
                            Shadow="true"
                            InsetPadding="20"
                            Width="470"
                            Height="350"
                            Theme="Base:gradients">
                            
                            <LegendConfig Position="Right" />
                            
                            <Store>
                                <ext:Store ID="TopDestinationCountriesStore"
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

        <div id='department-mails-statistics-textblock' class='block float-right w49p hauto'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="DepartmentMailStatistics" 
                    runat="server"
                    Header="true"
                    Title="Mail Statistics"
                    PaddingSummary="10px 10px 10px 10px"
                    Width="260"
                    Height="350"
                    ButtonAlign="Center">
                    <Defaults>
                        <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                    </Defaults>
                    
                    <Items>

                    </Items>
                </ext:Panel>
            </div>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
