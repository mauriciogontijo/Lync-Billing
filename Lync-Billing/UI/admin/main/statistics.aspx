<%@ Page Title="eBill Admin | View Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.admin.main.statistics" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div class="block float-right w80p h100p">
        <div id='first-chart' class='block float-right w49p hauto'>
            <div class="block-body pt5">
                <ext:Panel
                    ID="NumberOfCallsChartPanel"
                    runat="server"
                    Width="364"
                    Height="700"
                    Header="True"
                    Title="Empty Statistics Chart Panel 1"
                    Layout="FitLayout">
                    <Items>
                        <ext:Chart
                            ID="NumberOfCallsChart"
                            runat="server"
                            Animate="true"
                            Shadow="true"
                            InsetPadding="20"
                            Width="465"
                            Height="350"
                            Theme="Base:gradients">
                            <LegendConfig Position="Right" />
                            <Store>
                                <ext:Store
                                    ID="NumberOfCallsChartStore"
                                    runat="server">
                                    <Model>
                                        <ext:Model ID="NumberOfCallsChartChartModel" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="ToGateway" />
                                                <ext:ModelField Name="NumberOfOutgoingCalls" />
                                                <ext:ModelField Name="TotalDuration" />
                                                <ext:ModelField Name="TotalCost" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>

                            <Series>
                                <ext:PieSeries
                                    AngleField="ToGateway"
                                    ShowInLegend="true"
                                    Donut="30"
                                    Highlight="true"
                                    HighlightSegmentMargin="10">
                                    <%--<Label Field="Name" Display="Rotate" Contrast="true" Font="16px Arial">
                                        <Renderer Fn="TotalDuration_LableRenderer" />
                                    </Label>
                                    <Tips ID="Tips1" runat="server" TrackMouse="true" Width="200" Height="75">
                                        <Renderer Fn="TotalDuration_TipRenderer" />
                                    </Tips>--%>
                                </ext:PieSeries>
                            </Series>
                        </ext:Chart>
                    </Items>
                </ext:Panel>
            </div>
        </div>

        <div id='second-chart' class='block float-right w49p hauto'>
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
        </div>
    </div>
</asp:Content>
