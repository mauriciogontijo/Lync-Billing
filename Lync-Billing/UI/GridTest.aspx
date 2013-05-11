<%@ Page Title="" Language="C#" CodeBehind="GridTest.aspx.cs" Inherits="Lync_Billing.UI.GridTest" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html>
    <head id="Head1" runat="server">
    <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
    <link href="/resources/css/examples.css" rel="stylesheet" />

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
    <form id="Form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"  SelectMethod="GetPhoneCalls" TypeName="Lync_Billing.UI.GridTest" >
            <asp:SelectParameters>
                <asp:ControlParameter name="SourceUserUri" Type="String" />
            </asp:SelectParameters>
        </asp:ObjectDataSource>
        
        <ext:GridPanel ID="GridPanel1" 
            runat="server" 
            Title="PhoneCalls" 
            Frame="true" 
            Height="600">
            <Store>
                <ext:Store ID="Store1" runat="server" DataSourceID="ObjectDataSource1">
                    <Model>
                        <ext:Model ID="Model1" runat="server" IDProperty="ResponseTime">
                            <Fields>
                                <ext:ModelField Name="ResponseTime" />
                                <ext:ModelField Name="SourceUserUri" />
                                <ext:ModelField Name="Duration" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column ID="Column1" runat="server" DataIndex="ResponseTime" Text="ResponseTimeCol" Width="150" />
                    <ext:Column ID="Column2" runat="server" DataIndex="SourceUserUri" Text="SourceUserUriCol" Width="150" />
                    <ext:Column ID="Column3" runat="server" DataIndex="Duration" Text="DurationCol" Width="150" />
                </Columns>
            </ColumnModel>
            <View>
                <ext:GridView ID="GridView1" runat="server">
                    <GetRowClass Handler="return 'x-grid-row-expanded';" />
                </ext:GridView>        
            </View>  
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" Mode="Multi" />
            </SelectionModel>
            <Features>
                <ext:RowBody ID="RowBody1" runat="server" >
                    <GetAdditionalData Handler="orig.rowBody = '<div>' + data.Notes + '</div>'; orig.rowBodyColspan = record.fields.getCount();" />
                </ext:RowBody>
            </Features>
        </ext:GridPanel>
    </form>
</body>
</html>