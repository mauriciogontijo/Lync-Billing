<%@ Page Title="eBill Admin | View Statistics" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="Lync_Billing.ui.admin.main.statistics" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div class="block float-right w80p h100p">
        <div id='first-chart' class='block float-right w100p'>
            <div class="block-body pt5">
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
                        
                    </Items>
                </ext:Panel>
            </div>
        </div>
    </div>
</asp:Content>
