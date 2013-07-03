<%@ Page Title="eBill Accounting | Manage Disputes" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="disputes.aspx.cs" Inherits="Lync_Billing.ui.accounting.main.disputes" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='Div2' class='block float-right wauto h100p'>
        <div class="block-body pt5">
             <ext:GridPanel
                ID="ManageDisputesGrid"
                runat="server"
                Title="Manage Disputes"
                Width="740"
                Height="750"
                AutoScroll="true"
                Header="true"
                Scroll="Both"
                Layout="FitLayout">
                
                <Store>
                    <ext:Store 
                        ID="DisputesStore" 
                        runat="server" 
                        IsPagingStore="true" 
                        PageSize="25"
                        GroupField="SourceUserUri"
                        OnLoad="DisputesStore_Load"
                        OnSubmitData="DisputesStore_SubmitData"
                        OnReadData="DisputesStore_ReadData">
                        <Model>
                            <ext:Model ID="DisputesModel" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                    <ext:ModelField Name="SourceUserUri" Type="String" />
                                    <ext:ModelField Name="ResponseTime" Type="String"/>
                                    <ext:ModelField Name="SessionEndTime" Type="String"/>
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_IsDispute" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                    <ext:ModelField Name="AC_DisputeStatus" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="DisputesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="150"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="Marker_CallToCountry"
                            Align="Center" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="120"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="80"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>

                        <ext:Column
                            ID="UI_MarkedOn"
                            runat="server"
                            Text="Marked On"
                            Width="100"
                            DataIndex="UI_MarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                        </ext:Column>

                         <ext:Column
                            ID="AC_DisputeStatus"
                            runat="server"
                            Text="Status"
                            Width="90"
                            DataIndex="AC_DisputeStatus">
                             <Renderer fn="getRowClassForstatus" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="DisputesCheckboxSelectionModel" 
                        runat="server" 
                        Mode="Multi" 
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="DisputesToolbar" runat="server">
                        <Items>
                           <ext:Label
                               runat="server"
                               ID="button_group_lable"
                               Margins="5 0 0 5"
                               Width="90">
                                <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup ID="DisputesMarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="250"
                                Frame="false"
                                ButtonAlign="Left">
                                <Buttons>
                                    <ext:Button ID="Accepted" Text="Accepted" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AcceptDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManageDisputesGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Reject" Text="Rejected" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="RejectDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManageDisputesGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>
                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 310">
                                 <Listeners>
                                    <Click Handler="submitValue(#{ManageDisputesGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="DisputesPagingToolbar"
                        runat="server"
                        StoreID="DisputesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Disputes {0} - {1} of {2}" />
                </BottomBar>
                 <Features>               
                   <ext:Grouping ID="Grouping1" 
                    runat="server" 
                    HideGroupedHeader="false" 
                    EnableGroupingMenu="false"
                    GroupHeaderTplString='{name} : ({[values.rows.length]} {[values.rows.length > 1 ? "Disputes" : "Dispute"]})'/>
                 </Features>

            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>