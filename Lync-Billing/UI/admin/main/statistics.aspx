<%@ Page Title="eBill Admin | View Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.admin.main.statistics" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <script>
        var form = false,
            rec = false,
            selectedStoreItem = false;

        var selectItem = function (storeItem) {
            var name = storeItem.get('company'),
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
            //var series = App.main_content_place_holder_GatewaysBarChart.series.get(0),
            //    index = Ext.Array.indexOf(series.items, item),
            //    selectionModel = App.CompanyGrid.getSelectionModel();

            //selectedStoreItem = item.storeItem;
            //selectionModel.select(index);
        };

        var onSelectionChange = function (model, records) {
            var json,
                    name,
                    i,
                    l,
                    items,
                    series,
                    fields;

            if (records[0]) {
                rec = records[0];
                if (!form) {
                    form = this.up('form').getForm();
                    fields = form.getFields();
                    fields.each(function (field) {
                        if (field.name != 'GatewayName') {
                            field.setDisabled(false);
                        }
                    });
                } else {
                    fields = form.getFields();
                }

                // prevent change events from firing
                fields.each(function (field) {
                    field.suspendEvents();
                });

                form.loadRecord(rec);
                updateRecord(rec);

                fields.each(function (field) {
                    field.resumeEvents();
                });
            }
        };
    </script>

    <style>
        .x-panel-framed {
            padding: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div class="block float-right w80p h100p">
        <div id='first-chart' class='block float-right w49p hauto'>
            <div class="block-body pt5">
                <ext:FormPanel
                    ID="NumberOfCallsChartPanel"
                    runat="server"
                    Width="740"
                    Height="765"
                    Header="True"
                    Title="Empty Statistics Chart Panel 1">
                    <Bin>
                        <ext:Store 
                            ID="GatewaysDataStore" 
                            runat="server" 
                            AutoDataBind="true">
                            <Model>
                                <ext:Model ID="Model1" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="GatewayName" Mapping="GatewayName" Type="String" />
                                        <ext:ModelField Name="NumberOfOutgoingCalls" Mapping="NumberOfOutgoingCalls" Type="Int" />
                                        <ext:ModelField Name="TotalDuration" Mapping="TotalDuration" Type="Int" />
                                        <ext:ModelField Name="TotalCost" Mapping="TotalCost" Type="Float" />
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
                            Height="200"
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
                                                <Renderer Handler="return Ext.String.ellipsis(value, 10, false);" />
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
                                                Orientation="Vertical" 
                                                TextAnchor="middle"
                                                />
                                            <Listeners>
                                                <ItemMouseUp Fn="onMouseUp" />
                                            </Listeners>
                                        </ext:ColumnSeries>
                                    </Series>
                                </ext:Chart>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:FormPanel>
            </div>
        </div>

        <%--<div id='second-chart' class='block float-right w49p hauto'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="GatewaysUsageChartPanel"
                    runat="server"
                    Width="364"
                    Height="320"
                    Header="True"
                    Title="Empty Statistics Chart Panel 1"
                    Layout="FitLayout">
                    <Items>
                        
                    </Items>
                </ext:Panel>
            </div>
        </div>

        <div class="clear h5"></div>

        <div id='third-chart' class='block float-left w49p hauto'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="Panel1"
                    runat="server"
                    Width="364"
                    Height="320"
                    Header="True"
                    Title="Empty Statistics Chart Panel 1"
                    Layout="FitLayout">
                    <Items>
                        
                    </Items>
                </ext:Panel>
            </div>
        </div>--%>
    </div>
</asp:Content>
