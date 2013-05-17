<%@ Page Title="" Language="C#" CodeBehind="GridTest.aspx.cs" Inherits="Lync_Billing.UI.GridTest" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html>
    <head id="Head1" runat="server">
    <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
    
        <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
        <script type="text/javascript" src="js/browserdetector.js"></script>
        <script type="text/javascript" src="js/toolkit.js"></script>
        <style type="text/css">
            .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
            /*.x-grid-cell-inner, .x-column-header-inner { text-align: center !important; }*/
            .row-green  {   background-color: rgb(46, 143, 42);     }
            .row-red    {   background-color: rgb(201, 20, 20);   }
         
        .row-yellow {
            background-color: yellow;
        }
        </style>
          <script type="text/javascript">
              var myDateRenderer = function (value) {
                  value = Ext.util.Format.date(value, "d M Y h:m A");
                  return value;
              }
               
              function getRowClassForIsPersonal(value, meta, record, rowIndex, colIndex, store) {
                  if (record.data.UI_IsPersonal == 'YES' || record.data.UI_IsPersonal == 'Yes') {
                      meta.style = "color: rgb(201, 20, 20);";
                  }
                  if (record.data.UI_IsPersonal == 'NO' || record.data.UI_IsPersonal == 'No') {
                      meta.style = "color: rgb(46, 143, 42);";
                  }
                  //if (record.data.UI_IsInvoiced == 'Pending' || record.data.UI_IsInvoiced == 'PENDING') {
                  //    meta.style = "color: rgb(201, 20, 20);";
                  //}
                  //if (record.data.UI_IsInvoiced == 'Charged' || record.data.UI_IsInvoiced == 'CHARGED') {
                  //    meta.style = "color: rgb(46, 143, 42);";
                  //}
                  return value
              }


              function getRowClassForIsInvoiced(value, meta, record, rowIndex, colIndex, store) {
                  if (record.data.UI_IsInvoiced == 'Pending' || record.data.UI_IsInvoiced == 'PENDING') {
                      meta.style = "color: rgb(201, 20, 20);";
                  }
                  if (record.data.UI_IsInvoiced == 'Charged' || record.data.UI_IsInvoiced == 'CHARGED') {
                      meta.style = "color: rgb(46, 143, 42);";
                  }
                  return value
              }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:GridPanel 
            ID="PhoneCallsHistoryGrid" 
            runat="server" 
            Title="Manage Phone Calls"
            Width="660"
            Height="740"  
            AutoScroll="true"
            Header="true"
            Scroll="Both" 
            Layout="FitLayout"
            >

            <Store>
             <ext:Store ID="PhoneCallStore" runat="server" IsPagingStore="true"  PageSize="25">
                <Model>
                    <ext:Model ID="Model1" runat="server" IDProperty="SessionIdTime">
                        <Fields> 
                            <ext:ModelField Name="SessionIdTime" Type="String"/>  <%--RenderMilliseconds="true" DateWriteFormat="d M Y G:i"--%>
                             <ext:ModelField Name="SessionIdSeq" Type="Int"/>
                            <ext:ModelField Name="ResponseTime" Type="Date" RenderMilliseconds="true"/>
                            <ext:ModelField Name="SessionEndTime" Type="Date" RenderMilliseconds="true"/>
                            <ext:ModelField Name="marker_CallToCountry" Type="String" />
                            <ext:ModelField Name="DestinationNumberUri" Type="String" />
                            <ext:ModelField Name="Duration" Type="Float" />
                            <ext:ModelField Name="marker_CallCost"  Type="Float" />
                            <ext:ModelField Name="UI_IsPersonal" Type="String" />
                            <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                            <ext:ModelField Name="UI_IsPersonal" Type="String" />
                        </Fields>
                 </ext:Model>
               </Model>
            </ext:Store>
         </Store>
            <ColumnModel ID="PhoneCallsColumnModel" runat="server" Flex="1">
		        <Columns>
                    <ext:Column ID="SessionIdTime" 
                        runat="server" 
                        Text="Date" 
                        Width="130" 
                        DataIndex="SessionIdTime">
                        <Renderer Fn="myDateRenderer" />
                    </ext:Column>

                    <ext:Column ID="marker_CallToCountry"
                        runat="server"
                        Text="Country Code"
                        Width="80"
                        DataIndex="DestinationNumberUri Code" 
                        Align="Center"/>

                    <ext:Column ID="DestinationNumberUri"
                        runat="server"
                        Text="Destination"
                        Width="125"
                        DataIndex="DestinationNumberUri" />

                    <ext:Column ID="Duration"
                        runat="server"
                        Text="Duration"
                        Width="60"
                        DataIndex="Duration" />

                    <ext:Column ID="marker_CallCost"
                        runat="server"
                        Text="Cost"
                        Width="70"
                        DataIndex="marker_CallCost" />

                    <ext:Column ID="UI_IsPersonal"
                        runat="server"
                        Text="Is Personal"
                        Width="80"
                        DataIndex="UI_IsPersonal" >
                        <Renderer Fn="getRowClassForIsPersonal" />
                    </ext:Column>

                    <ext:Column ID="UI_MarkedOn"
                        runat="server"
                        Text="Updated On"
                        Width="80"
                        DataIndex="UI_MarkedOn" >
                        <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                    </ext:Column>
		        </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:CheckboxSelectionModel 
                    ID="PhoneCallsCheckBoxColumn" 
                    runat="server" 
                    Mode="Multi"
                    
                    >
                </ext:CheckboxSelectionModel>
            </SelectionModel>          
             <TopBar>
                  <ext:Toolbar ID="FilterToolBar" runat="server">
                     <Items>
                         <ext:ButtonGroup 
                             ID="MarkingBottonsGroup" 
                             runat="server"
                             Layout="TableLayout" 
                             Width="250" 
                             Frame="false"
                             Margins="0 0 0 390"
                             ButtonAlign="Right">
                             <Buttons>
                                 <ext:Button ID="Business" Text="Business" runat="server">
                                     <DirectEvents>
                                         <Click OnEvent="AssignBusiness">
                                             <EventMask ShowMask="true" />
                                             <ExtraParams>
                                               <%-- <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />--%>
                                                  <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                             </ExtraParams>
                                         </Click>
                                     </DirectEvents>
                                 </ext:Button>
                                 <ext:Button ID="Personal" Text="Personal" runat="server">
                                     <DirectEvents>
                                         <Click OnEvent="AssignPersonal">
                                             <EventMask ShowMask="true" />
                                              <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                             </ExtraParams>
                                         </Click>
                                     </DirectEvents>
                                 </ext:Button>
                                 <ext:Button ID="Dispute" Text="Dispute" runat="server">
                                     <DirectEvents>
                                         <Click OnEvent="AssignDispute">
                                             <EventMask ShowMask="true" />
                                              <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                             </ExtraParams>
                                         </Click>
                                     </DirectEvents>
                                 </ext:Button>
                             </Buttons>
                         </ext:ButtonGroup>
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
        </ext:GridPanel>
    </form>

</body>
</html>