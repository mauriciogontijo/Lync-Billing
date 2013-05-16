<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridTest3.aspx.cs" Inherits="Lync_Billing.UI.GridTest3" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html>
    <head id="Head1" runat="server">
        <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
    </head>

    <body>
        <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />
        <form id="form1" runat="server">
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
        </form>
    </body>
</html>
