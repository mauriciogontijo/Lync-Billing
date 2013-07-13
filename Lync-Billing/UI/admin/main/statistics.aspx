<%@ Page Title="eBill Admin | View Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.admin.main.statistics" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div class="block float-right w80p h100p">
        <div id='first-chart' class='block float-right w100p'>
            <div class="block-body pt5">
                <ext:ChartTheme ID="ChartTheme1" 
                    runat="server" 
                    ThemeName="GatewaysUsage" 
                    Colors="<%# COLORS %>"
                    AutoDataBind="true" /> 

                <ext:Panel
                    ID="FirstChartPanel"
                    runat="server"
                    Height="420"
                    Width="740"
                    Header="True"
                    Title="Empty Statistics Chart Panel 1"
                    Layout="FitLayout">
                    <Items>
                        
                    </Items>
                </ext:Panel>
            </div>
        </div>

        <div class="clear h5"></div>

        <div id='second-chart' class='block float-right w100p'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="SecondChartPanel"
                    runat="server"
                    Height="420"
                    Width="740"
                    Header="True"
                    Title="Empty Statistics Chart Panel 1"
                    Layout="FitLayout">
                    <Items>
                        <ext:Chart 
                            ID="GatewaysUsageAreaChart" 
                            runat="server"
                            StyleSpec="background:#fff;"                   
                            Theme="GatewaysUsage:gradients"                    
                            Animate="true">
                            <LegendConfig Position="Right" />
                            <Store>
                                <ext:Store ID="GatewaysUsageAreaChartStore" runat="server">                           
                                    <Model>
                                        <ext:Model ID="GatewaysUsageAreaChartStoreModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="GatewayName" Type="String" />
                                                <ext:ModelField Name="Year" Type="Int" />
                                                <ext:ModelField Name="Month" Type="Int" />
                                                <ext:ModelField Name="NumberOfOutgoingCalls" Type="Int" />
                                                <ext:ModelField Name="TotalDuration" Type="Int" />
                                                <ext:ModelField Name="TotalCost" Type="Float" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Axes>
                                <%--Fields="IE,Chrome,Firefox,Safari,Opera,Other"--%>
                                <ext:NumericAxis
                                    Fields="GatewayName,NumberOfOutgoingCalls"
                                    Title="Usage % (Number of Calls)"
                                    Grid="true"
                                    Decimals="0"
                                    Minimum="0"
                                    Maximum="100"
                                    />

                                <ext:CategoryAxis 
                                    Position="Bottom"
                                    Fields="Date"
                                    Title="Month of the Year"
                                    Grid="true">
                                    <Label>
                                        <Renderer Handler="return Ext.Date.format(value, 'M');" />
                                    </Label>
                                </ext:CategoryAxis>
                            </Axes>

                            <Series>
                                <ext:AreaSeries 
                                    Axis="Left"
                                    Highlight="true"
                                    YField="GatewayName,NumberOfOutgoingCalls">
                                        <Style Opacity="0.86" StrokeWidth="1" Stroke="#666"  />
                                        <Tips TrackMouse="true" Width="170" Height="28">                                   
                                            <Renderer Handler="this.setTitle(item.storeField + ' - ' + Ext.Date.format(new Date(storeItem.get('Date')), 'M y') + ' - ' + storeItem.get(item.storeField) + '%');" />
                                        </Tips>
                                    </ext:AreaSeries>
                            </Series>
                        </ext:Chart>
                    </Items>
                </ext:Panel>
            </div>
        </div>
    </div>
</asp:Content>
