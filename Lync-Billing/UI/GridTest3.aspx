<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridTest3.aspx.cs" Inherits="Lync_Billing.UI.GridTest3" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html>

 <head id="Head1" runat="server">
    <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
    <style>
        .x-grid-cell-fullName .x-grid-cell-inner {
            font-family : tahoma, verdana;
            display     : block;
            font-weight : normal;
            font-style  : normal;
            color       : #385F95;
            white-space : normal;
        }
        
        .x-grid-rowbody div {
            margin : 2px 5px 20px 5px !important;
            width  : 99%;
            color  : Gray;
        }
        
        .x-grid-row-expanded td.x-grid-cell{
            border-bottom-width:0px;
        }
    </style>

    <script>
        var fullName = function (value, metadata, record, rowIndex, colIndex, store) {
            return "<b>" + record.data.LastName + ' ' + record.data.FirstName + "</b>";
        };
    </script>
</head>
<body>
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />
    <form id="form1" runat="server">     
        <asp:Label ID="UserCallSummary" runat="server"></asp:Label>
    </form>
</body>
</html>
