<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterPage.Master" CodeBehind="GridTest3.aspx.cs" Inherits="Lync_Billing.UI.GridTest3" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
            <ext:Panel ID="UserPhoneCallsSummary"
                runat="server"         
                Height="200" 
                Width="350"
                Layout="AccordionLayout"
                Title="Your Phone Calls Summary">
                <Loader ID="SummaryLoader" 
                    runat="server" 
                    DirectMethod="#{DirectMethods}.GetSummaryData"
                    Mode="Component">
                    <LoadMask ShowMask="true" />
                </Loader>
            </ext:Panel>

            <br />
            <br />

            <asp:PlaceHolder ID="UserPhoneCallsHistoryPH" runat="server">
            </asp:PlaceHolder>
</asp:Content>