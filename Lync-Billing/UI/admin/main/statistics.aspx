<%@ Page Title="eBill Admin | View Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.admin.main.statistics" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <script>
        var form = false,
            rec = false,
            selectedStoreItem = false;

        var selectItem = function (storeItem) {
            var name = storeItem.get('GatewayName'),
                series = App.main_content_place_holder_GatewaysBarChart.series.get(0),
                i, items, l;

            series.highlight = true;
            series.unHighlightItem();
            series.cleanHighlights();

            for (i = 0, items = series.items, l = items.length; i < l; i++) {
                if (name == items[i].storeItem.get('GatewayName')) {
                    selectedStoreItem = items[i].storeItem;
                    series.highlightItem(items[i]);
                    break;
                }
            }
            series.highlight = false;
        };

        var onMouseUp = function (item) {
            var series = App.main_content_place_holder_GatewaysBarChart.series.get(0),
                index = Ext.Array.indexOf(series.items, item),
                selectionModel = App.main_content_place_holder_GatewaysGrid.getSelectionModel();

            selectedStoreItem = item.storeItem;
            selectionModel.select(index);
        };

        var onSelectionChange = function (model, records) {
            var json;

            if (records[0]) {
                rec = records[0];
                updateRecord(rec);
            }
        };

        var updateRecord = function (rec) {
            var name,
                series,
                i,
                l,
                items,
                json = [{
                    'Name': 'Calls %',
                    'Data': rec.get('NumberOfOutgoingCallsPercentage')
                }, {
                    'Name': 'Duration %',
                    'Data': rec.get('TotalDurationPercentage')
                }, {
                    'Name': 'Cost %',
                    'Data': rec.get('TotalCostPercentage')
                }];
            App.main_content_place_holder_RadarStore.loadData(json);
            selectItem(rec);
        };
    </script>

    <style>
        .x-panel-framed {
            padding: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div class="block float-right wauto h100p">
        <div class="block-body pt5">
            <ext:FormPanel
                ID="NumberOfCallsChartPanel"
                runat="server"
                Width="740"
                Height="820"
                Header="True"
                Frame="true"
                BodyPadding="5"
                Title="Gateways Statistics">
                <Bin>
                    <ext:Store 
                        ID="GatewaysDataStore" 
                        runat="server" 
                        AutoDataBind="true"
                        IsPagingStore="true"
                        PageSize="11">
                        <Model>
                            <ext:Model ID="Model1" runat="server">
                                <Fields>
                                    <ext:ModelField Name="GatewayName" Mapping="GatewayName" Type="String" />

                                    <ext:ModelField Name="NumberOfOutgoingCalls" Mapping="NumberOfOutgoingCalls" Type="Int" />
                                    <ext:ModelField Name="TotalDuration" Mapping="TotalDuration" Type="Int" />
                                    <ext:ModelField Name="TotalCost" Mapping="TotalCost" Type="Float" />

                                    <ext:ModelField Name="NumberOfOutgoingCallsPercentage" Mapping="NumberOfOutgoingCallsPercentage" Type="Float" />
                                    <ext:ModelField Name="TotalDurationPercentage" Mapping="TotalDurationPercentage" Type="Float" />
                                    <ext:ModelField Name="TotalCostPercentage" Mapping="TotalCostPercentage" Type="Float" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Bin>
            
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />    
                </LayoutConfig>

                <Items>
                    <ext:Panel ID="Panel1"
                        runat="server"
                        Height="400"
                        Layout="FitLayout"
                        MarginSpec="0 0 3 0">
                        <Items>
                            <ext:Chart 
                                ID="GatewaysBarChart" 
                                runat="server" 
                                Flex="1" 
                                Shadow="true" 
                                Animate="true" 
                                StoreID="GatewaysDataStore">
                                <Axes>
                                    <ext:NumericAxis Position="Left" Fields="NumberOfOutgoingCalls" Minimum="0" Hidden="true" />
                                        
                                    <ext:CategoryAxis Position="Bottom" Fields="GatewayName">
                                        <Label Font="9px Arial">
                                            <Rotate Degrees="270" />
                                            <Renderer Handler="return Ext.String.ellipsis(value, 20, false);" />
                                        </Label>
                                    </ext:CategoryAxis>
                                </Axes> 

                                <Series>
                                    <ext:ColumnSeries 
                                        Axis="Left" 
                                        Highlight="true" 
                                        XField="GatewayName" 
                                        YField="NumberOfOutgoingCalls">
                                        <Style Fill="#456d9f" />
                                        <HighlightConfig Fill="#a2b5ca" />
                                        <Label 
                                            Contrast="true" 
                                            Display="InsideEnd" 
                                            Field="NumberOfOutgoingCalls" 
                                            Color="#000" 
                                            Orientation="Vertical" />

                                        <Listeners>
                                            <ItemMouseUp Fn="onMouseUp" />
                                        </Listeners>
                                    </ext:ColumnSeries>
                                </Series>
                            </ext:Chart>
                        </Items>
                    </ext:Panel>
                    

                    <ext:Panel ID="Panel2" 
                        runat="server" 
                        Flex="1" 
                        Border="false" 
                        BodyStyle="background-color: transparent;">
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>

                        <Items>
                            <ext:GridPanel 
                                ID="GatewaysGrid" 
                                runat="server" 
                                Flex="6" 
                                Title="Gateways Data"
                                StoreID="GatewaysDataStore"
                                MarginSpec="5 5 0 0">
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column 
                                            ID="GatewayNameCol" 
                                            runat="server" 
                                            Text="Gateway" 
                                            Flex="1" 
                                            DataIndex="GatewayName" 
                                            Width="60" />

                                        <ext:Column ID="NumberOfCallsCol" 
                                            runat="server" 
                                            Text="Total Calls"
                                            Width="75" 
                                            DataIndex="NumberOfOutgoingCallsPercentage" 
                                            Align="Right">
                                        </ext:Column>
                                        
                                        <ext:Column ID="DurationPercentageCol" 
                                            runat="server" 
                                            Text="Duration" 
                                            Width="75" 
                                            DataIndex="TotalDurationPercentage" 
                                            Align="Right">
                                            <Renderer Handler="return value + '%';" />
                                        </ext:Column>
                                        
                                        <ext:Column ID="CostPercentageCol" 
                                            runat="server" 
                                            Text="Cost" 
                                            Width="75" 
                                            DataIndex="TotalCostPercentage" 
                                            Align="Right">
                                            <Renderer Handler="return value + '%';" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                
                                <Listeners>
                                    <SelectionChange Fn="onSelectionChange" />
                                </Listeners>

                                <BottomBar>
                                    <ext:PagingToolbar
                                        ID="PagingToolbar1"
                                        runat="server"
                                        StoreID="GatewaysDataStore"
                                        DisplayInfo="true"
                                        Weight="25"
                                        DisplayMsg="Gateways {0} - {1} of {2}" />
                                </BottomBar>
                            </ext:GridPanel>

                            <ext:Panel ID="Panel3" 
                                runat="server" 
                                Flex="4" 
                                Header="true"
                                frame="true"
                                MarginSpec="5 0 0 0">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>

                                <Items>
                                    <ext:Chart ID="Chart1" 
                                        runat="server" 
                                        Margin="0" 
                                        InsetPadding="20" 
                                        Flex="1" 
                                        StandardTheme="Blue" 
                                        Animate="true">
                                
                                        <Store>
                                            <ext:Store 
                                                ID="RadarStore" 
                                                runat="server" 
                                                AutoDataBind="true"
                                                Data="<%# RadarData %>">
                                                <Model>
                                                    <ext:Model ID="Model2" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Data" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Axes>
                                            <ext:RadialAxis Steps="5" Maximum="100" />
                                        </Axes>
                                        <Series>
                                            <ext:RadarSeries 
                                                XField="Name" 
                                                YField="Data" 
                                                ShowInLegend="false" 
                                                ShowMarkers="true">
                                                <MarkerConfig Radius="4" Size="4" />
                                                <Style Fill="rgb(194,214,240)" Opacity="0.5" StrokeWidth="0.5" />
                                            </ext:RadarSeries>
                                        </Series>
                                    </ext:Chart>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:FormPanel>
        </div>
    </div>
</asp:Content>


<%--
    <ext:Panel ID="Panel3" 
        runat="server" 
        Flex="4" 
        Title="Gateway Details" 
        MarginSpec="0 0 0 5">
        <LayoutConfig>
            <ext:VBoxLayoutConfig Align="Stretch" />
        </LayoutConfig>

        <Items>
            <ext:Chart ID="Chart1" 
                runat="server" 
                Margin="0" 
                InsetPadding="20" 
                Flex="1"
                StandardTheme="Blue" 
                Animate="true">
                <Store>
                    <ext:Store 
                        ID="RadarStore" 
                        runat="server" 
                        AutoDataBind="true">
                        <Model>
                            <ext:Model ID="Model2" runat="server">
                                <Fields>
                                    <ext:ModelField Name="Name" />
                                    <ext:ModelField Name="Data" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>
                <Axes>
                    <ext:RadialAxis Steps="5" Maximum="100" />
                </Axes>
                <Series>
                    <ext:RadarSeries 
                        XField="Name" 
                        YField="Data" 
                        ShowInLegend="false" 
                        ShowMarkers="true">
                        <MarkerConfig Radius="4" Size="4" />
                        <Style Fill="rgb(194,214,240)" Opacity="0.5" StrokeWidth="0.5" />
                    </ext:RadarSeries>
                </Series>
            </ext:Chart>
        </Items>
    </ext:Panel>
--%>