<%@ Page Title="" Language="C#" CodeBehind="GridTest.aspx.cs" Inherits="Lync_Billing.UI.GridTest" %>
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
         <ext:XScript ID="FilterXScript" runat="server">
        <script>

        </script>
    </ext:XScript>
</head>
<body>
    <form id="Form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:GridPanel 
            ID="PhoneCallsHistoryGrid" 
            runat="server" 
            Title="Phone Calls History"
            Width="745"
            Height="450"  
            AutoScroll="true"
            Header="true"
            Scroll="Both" 
            Layout="FitLayout">

            <Store>
             <ext:Store ID="PhoneCallStore" runat="server" IsPagingStore="true"  PageSize="25"
                OnAfterRecordUpdated="PhoneCallStore_AfterRecordUpdated"
                OnAfterStoreChanged="PhoneCallStore_AfterStoreChanged"
                OnAfterDirectEvent="PhoneCallStore_AfterDirectEvent"
                OnBeforeDirectEvent="PhoneCallStore_BeforeDirectEvent"
                OnBeforeRecordUpdated="PhoneCallStore_BeforeRecordUpdated"
                OnBeforeStoreChanged="PhoneCallStore_BeforeStoreChanged">
                <Model>
                    <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                        <Fields>
                            <ext:ModelField Name="SessionIdTime" Type="Date" />
                            <ext:ModelField Name="marker_CallToCountry" Type="String" />
                            <ext:ModelField Name="DestinationNumberUri" Type="String" />
                            <ext:ModelField Name="Duration" Type="Float" />
                            <ext:ModelField Name="marker_CallCost"  Type="Float" />
                            <ext:ModelField Name="ui_IsPersonal" Type="Boolean" />
                            <ext:ModelField Name="ui_MarkedOn" Type="Date" />
                            <ext:ModelField Name="ui_IsInvoiced" Type="Boolean" />
                        </Fields>
                 </ext:Model>
               </Model>
            </ext:Store>
         </Store>
            <ColumnModel ID="PhoneCallsColumnModel" runat="server">
		        <Columns>
                    <ext:Column ID="SessionIdTime" 
                        runat="server" 
                        Text="Date" 
                        Width="80" 
                        DataIndex="SessionIdTime" 
                        Resizable="false" 
                        MenuDisabled="true">
                       
                        <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                    </ext:Column>

                    <ext:Column ID="marker_CallToCountry"
                        runat="server"
                        Text="Country Code"
                        Width="80"
                        DataIndex="DestinationNumberUri Code" />

                    <ext:Column ID="DestinationNumberUri"
                        runat="server"
                        Text="Destination"
                        Width="130"
                        DataIndex="DestinationNumberUri" />

                    <ext:Column ID="Duration"
                        runat="server"
                        Text="Duration"
                        Width="70"
                        DataIndex="Duration" />

                    <ext:Column ID="marker_CallCost"
                        runat="server"
                        Text="Cost"
                        Width="70"
                        DataIndex="marker_CallCost" />

                    <ext:Column ID="ui_IsPersonal"
                        runat="server"
                        Text="Type"
                        Width="100"
                        DataIndex="ui_IsPersonal" />

                    <ext:Column ID="ui_MarkedOn"
                        runat="server"
                        Text="Updated On"
                        Width="80"
                        DataIndex="ui_MarkedOn" />

                    <ext:Column ID="ui_IsInvoiced"
                        runat="server"
                        Text="Billing Status"
                        Width="90"
                        DataIndex="ui_IsInvoiced" />
		        </Columns>
            </ColumnModel>
             <TopBar>
                  <ext:Toolbar ID="FilterToolBar" runat="server">
                     <Items>
                      <ext:ComboBox 
                            ID="FilterTypeComboBox" 
                            runat="server" 
                            Icon="Find" 
                            TriggerAction="All" 
                            QueryMode="Local" 
                            DisplayField="TypeName" 
                            ValueField="TypeValue">
                            <Items>
                                <ext:ListItem Text="Unmarked" Value="1" />
                                <ext:ListItem Text="Marked" Value="2" />
                                <ext:ListItem Text="Business" Value="3" />
                                <ext:ListItem Text="Personal" Value="4" />
                                <ext:ListItem Text="Charged" Value="5" />
                                <ext:ListItem Text="Uncharged" Value="6" />
                            </Items>
                            <DirectEvents>
                                <Change OnEvent="FilterTypeChange"></Change>
                            </DirectEvents>
                        </ext:ComboBox>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar 
                    ID="PhoneCallsPagingToolbar" 
                    runat="server" 
                    StoreID="PhoneCallStore" 
                    DisplayInfo="true" 
                    Weight="25" 
                    DisplayMsg="Phone Calls {0} - {1} of {2}"
                     />
            </BottomBar>
                    
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="PhoneCallsCheckBoxColumn" runat="server" Mode="Multi" />
            </SelectionModel>            
                    
            <Buttons>
                <ext:Button ID="GridSubmitChanges" runat="server" Text="Save Changes">
                    <DirectEvents>
                        <Click OnEvent="GridSubmitChanges_Click">
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
        </ext:GridPanel>
    </form>
</body>
</html>