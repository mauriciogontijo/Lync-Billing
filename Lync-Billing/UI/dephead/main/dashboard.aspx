<%@ Page Title="tBill Depart. Head | Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.dephead.main.dashboard" %>

<%-- DEPARTMENT-HEAD # MAIN # STATISTICS --%>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var DepartmentSummary_TipRenderer = function (storeItem, item) {
            // UnmarkedCallsCost = Unmarked
            if (item.yField == 'Unmarked') {
                this.setTitle(
                    "Unmarked Calls: " +
                    "<br/>" + 
                    "<br/>Total : " + storeItem.get("UnmarkedCallsCount") +
                    "<br/>Cost : " + String(item.value[1].toFixed(2)) + 
                    "<br/>Duration : " + chartsDurationFormat(storeItem.get("UnmarkedCallsDuration")) + ' hours.'
                );
            }

            // Personal = PersonalCallsCost
            else if (item.yField == 'Personal') {
                this.setTitle(
                    "Personal Calls: " +
                    "<br/>" +
                    "<br/>Total : " + storeItem.get("PersonalCallsCount") +
                    "<br/>Cost : " + String(item.value[1].toFixed(2)) +
                    "<br/>Duration : " + chartsDurationFormat(storeItem.get("PersonalCallsDuration")) + ' hours.'
                );
            }

            // Business = BusinessCallsCost
            else if (item.yField == 'Business') {
                this.setTitle(
                    "Business Calls: " +
                    "<br/>" +
                    "<br/>Total : " + storeItem.get("BusinessCallsCount") +
                    "<br/>Cost : " + String(item.value[1].toFixed(2)) +
                    "<br/>Duration : " + chartsDurationFormat(storeItem.get("BusinessCallsDuration")) + ' hours.'
                );
            }
        };

        var TopDestinationCountries_LableRenderer = function (storeItem, item) {
            var total = 0,
                all_countries_data = {},
                component_name = "main_content_place_holder_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('CallsCount');

                var country_name = rec.get('CountryName');
                if (country_name != 0 && all_countries_data[country_name] == undefined) {
                    all_countries_data[country_name] = rec.get('CallsCount');
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
        var TopDestinationCountries_TipRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0,
                component_name = "main_content_place_holder_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('CallsCount');
            });

            this.setTitle(
                storeItem.get('CountryName') + ': ' + ((storeItem.get('CallsCount') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br />' + 
                '<br />' + 'Total Calls: ' + storeItem.get('CallsCount') + 
                '<br />' + 'Total Duration: ' + chartsDurationFormat(storeItem.get('CallsDuration')) + ' hours.' +
                '<br />' + 'Total Cost: ' + RoundCostsToTwoDecimalDigits(storeItem.get('CallsCost')) + ' euros'
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
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

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
                    Layout="AnchorLayout"
                    ComponentCls="fix-ui-vertical-align">

                    <TopBar>
                        <ext:Toolbar
                            ID="FilterToolbar1"
                            runat="server">
                            <Items>
                                <ext:ComboBox
                                    ID="FilterSitesComboBox"
                                    runat="server"
                                    Icon="Find"
                                    Editable="true"
                                    DisplayField="SiteName"
                                    ValueField="SiteID"
                                    FieldLabel="Site:"
                                    LabelWidth="25"
                                    Width="180"
                                    Margins="5 15 0 5">
                                    <Store>
                                        <ext:Store ID="FilterSitesComboBoxStore" runat="server" OnLoad="FilterSitesComboBoxStore_Load">
                                            <Model>
                                                <ext:Model ID="SitesModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="SiteID" />
                                                        <ext:ModelField Name="SiteName" />
                                                        <ext:ModelField Name="CountryCode" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <DirectEvents>
                                        <Select OnEvent="FilterDepartmentsBySite_Selected" />
                                    </DirectEvents>

                                    <ListConfig MinWidth="200">
                                        <ItemTpl ID="SitesItemTpl" runat="server">
                                            <Html>
                                                <div>{SiteName}&nbsp;({CountryCode})</div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                </ext:ComboBox>

                                <ext:ComboBox
                                    ID="FilterDepartments"
                                    runat="server"
                                    Icon="Find"
                                    Editable="true"
                                    DisplayField="DepartmentName"
                                    ValueField="ID"
                                    FieldLabel="Department:"
                                    LabelWidth="60"
                                    Width="200"
                                    Margins="5 15 5 5"
                                    Disabled="true">
                                    <Store>
                                        <ext:Store ID="DepartmentsFilterStore" runat="server">
                                            <Model>
                                                <ext:Model ID="DepartmentHeadsStoreModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="SiteID" />
                                                        <ext:ModelField Name="SiteName" />
                                                        <ext:ModelField Name="DepartmentID" />
                                                        <ext:ModelField Name="DepartmentName" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    
                                    <DirectEvents>
                                        <Select OnEvent="DrawStatisticsForDepartment">
                                            <EventMask ShowMask="true" />
                                        </Select>
                                    </DirectEvents>

                                    <ListConfig MinWidth="220" />
                                </ext:ComboBox>

                                <ext:Button
                                    ID="AdvancedFilterBtn"
                                    runat="server"
                                    Icon="Cog"
                                    Text="Advanced Filter"
                                    TextAlign="Center"
                                    ShrinkWrap="Width"
                                    Margins="0 0 0 200"
                                    Width="105"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="AdvancedFilterBtn_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
            </div>
        </div>

        <div class="h5 clear"></div>

        <div id='top-5-destinations-by-cost-chart' class='block float-right w49p hauto'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="TopDestinationCountriesPanel"
                    runat="server"
                    Width="510"
                    Height="300"
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
                            Width="470"
                            Height="300"
                            Theme="Base:gradients">
                            
                            <LegendConfig Position="Right" />
                            
                            <Store>
                                <ext:Store ID="TopDestinationCountriesStore"
                                    runat="server">
                                    <Model>
                                        <ext:Model ID="TopDestinationCountriesModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="CountryName" />
                                                <ext:ModelField Name="CallsCost" />
                                                <ext:ModelField Name="CallsCount" />
                                                <ext:ModelField Name="CallsDuration" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <Series>
                                <ext:PieSeries
                                    AngleField="CallsCount"
                                    ShowInLegend="true"
                                    Donut="30"
                                    Highlight="true"
                                    HighlightSegmentMargin="10">
                                    <Label Field="CountryName" Display="Rotate" Contrast="true" Font="16px Arial">
                                        <Renderer Fn="TopDestinationCountries_LableRenderer" />
                                    </Label>
                                    <Tips ID="Tips2" runat="server" TrackMouse="true" Width="230" Height="80">
                                        <Renderer Fn="TopDestinationCountries_TipRenderer" />
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
                    Title="Mail Statistics for this Month"
                    PaddingSummary="10px 10px 10px 10px"
                    Width="220"
                    Height="300"
                    ButtonAlign="Center">
                    <Defaults>
                        <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                    </Defaults>
                    
                    <Items>

                    </Items>
                </ext:Panel>
            </div>
        </div>

        <div class="h10 clear"></div>

        <div id='business-to-personal-phonecalls-chart' class='block float-right w100p display-none'>
            <div class="block-body pt5">
                <ext:Panel ID="DepartmentCallsPerMonthChartPanel" 
                    runat="server"
                    Width="740"
                    Height="520"
                    Title="Phonecalls Distribution"
                    Layout="FitLayout">
                    <Items>
                        <ext:Chart 
                            ID="DepartmentCallsPerMonthChart" 
                            runat="server"
                            Shadow="true"
                            Animate="true">
                            <Store>
                                <ext:Store ID="DepartmentCallsPerMonthChartStore" runat="server">
                                    <Model>
                                        <ext:Model ID="DepartmentCallsPerMonthChartStoreModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="Month" />
                                                <ext:ModelField Name="Year" />
                                                <ext:ModelField Name="MonthDate" />
                                                <ext:ModelField Name="Business" ServerMapping="BusinessCallsCost" />
                                                <ext:ModelField Name="Personal" ServerMapping="PersonalCallsCost" />
                                                <ext:ModelField Name="Unmarked" ServerMapping="UnmarkedCallsCost" />
                                                <ext:ModelField Name="BusinessCallsCount" />
                                                <ext:ModelField Name="PersonalCallsCount" />
                                                <ext:ModelField Name="UnmarkedCallsCount" />
                                                <ext:ModelField Name="BusinessCallsDuration" />
                                                <ext:ModelField Name="PersonalCallsDuration" />
                                                <ext:ModelField Name="UnmarkedCallsDuration" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <LegendConfig Position="Bottom" />

                            <Axes>
                                <ext:CategoryAxis
                                    Position="Left"
                                    Fields="MonthDate">
                                    <Label>
                                        <Renderer Handler="return Ext.util.Format.date(value, 'M');" />
                                    </Label>
                                </ext:CategoryAxis>

                                <ext:NumericAxis
                                    Title="Cost"
                                    Fields="Personal"
                                    Position="Bottom"
                                    Grid="true">
                                </ext:NumericAxis>
                            </Axes>

                            <Series>
                                <ext:BarSeries 
                                    Axis="Left"
                                    Gutter="80"
                                    XField="MonthDate" 
                                    YField="Business,Personal,Unmarked"
                                    Stacked="true">
                                    <Tips runat="server" TrackMouse="true" Width="170" Height="80">
                                        <Renderer Fn="DepartmentSummary_TipRenderer" />
                                        <%--<Renderer Handler="this.setTitle('Cost: ' + String(item.value[1].toFixed(2))); console.log(item);" />--%>
                                    </Tips>
                                </ext:BarSeries>
                            </Series>
                        </ext:Chart>
                    </Items>
                </ext:Panel>


                <ext:Window
                    ID="CustomizeStatisticsWindow"
                    runat="server"
                    Frame="true"
                    Resizable="false"
                    Hidden="true"
                    Title="Customize Statistics"
                    Width="280"
                    Icon="Cog"
                    X="700"
                    Y="150"
                    BodyStyle="background-color: #f9f9f9;"
                    ComponentCls="fix-ui-vertical-align">

                    <Defaults>
                        <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                    </Defaults>

                    <Items>
                        <ext:ComboBox
                            ID="CustomizeStats_Years"
                            runat="server"
                            Editable="false"
                            DisplayField="YearAsText"
                            ValueField="YearAsNumber"
                            Width="250"
                            LabelWidth="60"
                            FieldLabel="Year"
                            LabelSeparator=":">
                            <Store>
                                <ext:Store ID="CustomizeStats_YearStore" runat="server" OnLoad="CustomizeStats_YearStore_Load">
                                    <Model>
                                        <ext:Model ID="CustomizeStats_YearStoreModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="YearAsText" Type="String" />
                                                <ext:ModelField Name="YearAsNumber" Type="Int" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <SelectedItems>
                                <ext:ListItem Index="0" />
                            </SelectedItems>

                            <DirectEvents>
                                <Select OnEvent="CustomizeStats_Years_Select" />
                            </DirectEvents>
                        </ext:ComboBox>

                        <ext:ComboBox
                            ID="CustomizeStats_Quarters"
                            runat="server"
                            DisplayField="QuarterAsText"
                            ValueField="QuarterAsNumber"
                            Editable="false"
                            FieldLabel="Quarter"
                            Width="250"
                            LabelWidth="60"
                            LabelSeparator=":"
                            Hidden="true">
                            <Store>
                                <ext:Store ID="CustomizeStats_QuartersStore" runat="server" OnLoad="CustomizeStats_QuartersStore_Load">
                                    <Model>
                                        <ext:Model ID="CustomizeStats_QuartersStoreModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="QuarterAsText" Type="String" />
                                                <ext:ModelField Name="QuarterAsNumber" Type="Int" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <SelectedItems>
                                <ext:ListItem Index="4" />
                            </SelectedItems>
                        </ext:ComboBox>
                    </Items>

                    <BottomBar>
                        <ext:StatusBar
                            ID="CustomizeStatsWindowBottomBar"
                            runat="server"
                            StatusAlign="Right"
                            Height="30">
                            <Items>
                                <ext:Button
                                    ID="SubmitCustomizeStatisticsBtn"
                                    runat="server"
                                    Text="Apply"
                                    Icon="ApplicationGo"
                                    Height="25">
                                    <DirectEvents>
                                        <Click OnEvent="SubmitCustomizeStatisticsBtn_Click">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />

                                <ext:Button
                                    ID="CancelCustomizeStatsBtn"
                                    runat="server"
                                    Text="Cancel"
                                    Icon="Cancel"
                                    Height="25">
                                    <DirectEvents>
                                        <Click OnEvent="CancelCustomizeStatsBtn_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:StatusBar>
                    </BottomBar>
                </ext:Window>
            </div>
        </div>
        
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
