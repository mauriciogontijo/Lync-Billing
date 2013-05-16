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
            <%--<ext:GridPanel 
                ID="UserPhoneCallsSummaryGrid" 
                runat="server" 
                Title="User Phone Calls Summary"
                Width="745"
                Height="450"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store ID="UserPhoneCallsSummaryStore" runat="server" PageSize="5">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                                <Fields>
                                    <ext:ModelField Name="BusinessCallsCount" Type="Int" />
                                    <ext:ModelField Name="BusinessCallsCost" Type="Int" />
                                    <ext:ModelField Name="BusinessCallsDuration" Type="Int" />
                                    <ext:ModelField Name="PersonalCallsCount" Type="Int" />
                                    <ext:ModelField Name="PersonalCallsDuration"  Type="Int" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsCount" Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsDuartion"  Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsCost" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="PhoneCallsColumnModel" runat="server" Fixed="true" Flex="1">
		            <Columns>
                        <ext:Column ID="SessionIdTime" runat="server" Text="Date" Width="80" DataIndex="SessionIdTime" Resizable="false" MenuDisabled="true"></ext:Column>
                        <ext:Column ID="marker_CallToCountry" runat="server" Text="Country Code" Width="80" DataIndex="DestinationNumberUri Code" Align="Center"/>
                        <ext:Column ID="DestinationNumberUri" runat="server" Text="Destination" Width="130" DataIndex="DestinationNumberUri" />
                        <ext:Column ID="Duration" runat="server" Text="Duration" Width="70" DataIndex="Duration" />
                        <ext:Column ID="marker_CallCost" runat="server" Text="Cost" Width="70" DataIndex="marker_CallCost" />
                        <ext:Column ID="UI_IsPersonal" runat="server" Text="Is Personal" Width="100" DataIndex="UI_IsPersonal" />
                        <ext:Column ID="UI_MarkedOn" runat="server" Text="Updated On" Width="80" DataIndex="UI_MarkedOn" />
                        <ext:Column ID="UI_IsInvoiced" runat="server" Text="Billing Status" Width="90" DataIndex="UI_IsInvoiced" />
		            </Columns>
                </ColumnModel>
                
                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                         <Items>
                             <ext:ButtonGroup 
                                 ID="MrkingBottonsGroup" 
                                 runat="server"
                                 Layout="TableLayout" 
                                 Width="250" 
                                 Frame="false"
                                 Margins="0 0 0 480"
                                 ButtonAlign="Right">
                                 <Buttons>
                                     <ext:Button ID="Business" Text="Business" runat="server">
                                         <DirectEvents>
                                             <Click OnEvent="AssignBusiness">
                                                 <EventMask ShowMask="true" />
                                                 <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{PhoneCallsHistoryGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                 </ExtraParams>
                                             </Click>
                                         </DirectEvents>
                                     </ext:Button>
                                     <ext:Button ID="Personal" Text="Personal" runat="server">
                                         <DirectEvents>
                                             <Click OnEvent="AssignPersonal">
                                                 <EventMask ShowMask="true" />
                                             </Click>
                                         </DirectEvents>
                                     </ext:Button>
                                     <ext:Button ID="Dispute" Text="Dispute" runat="server">
                                         <DirectEvents>
                                             <Click OnEvent="AssignDispute">
                                                 <EventMask ShowMask="true" />
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
                    
                <SelectionModel>
                    <ext:CheckboxSelectionModel 
                        ID="PhoneCallsCheckBoxColumn" 
                        runat="server" 
                        Mode="Multi">
                    </ext:CheckboxSelectionModel>

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
            </ext:GridPanel>--%>

            <ext:Panel ID="Summary"
                runat="server"         
                Height="200" 
                Width="350"
                Layout="AccordionLayout"
                Title="User Summary">
                <Loader ID="SummaryLoader" 
                    runat="server" 
                    DirectMethod="#{DirectMethods}.Items"
                    Mode="Component">
                    <LoadMask ShowMask="true" />
                </Loader>
            </ext:Panel>
        </form>
    </body>
</html>
