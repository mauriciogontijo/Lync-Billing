<%@ Page Title="eBill User | Calls History" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="history.aspx.cs" Inherits="Lync_Billing.ui.user.history" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner { font-family: tahoma, verdana; display: block; font-weight: normal; font-style: normal; color:#385F95; white-space: normal; }
        .x-grid-rowbody div { margin: 2px 5px 20px 5px !important; width: 99%; color: Gray; }
        .x-grid-row-expanded td.x-grid-cell { border-bottom-width: 0px; }
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF PHONE CALLS HISTORY GRID *** -->
    <div id='phone-call-history' class='block float-right wauto h100p' style="visibility: visible;">
        <div class="block-body pt5">
            <ext:Hidden ID="FormatType" runat="server" />

            <asp:ObjectDataSource 
                ID="PhoneCallsDataSource" 
                runat="server" 
                OnSelecting="CallsHistoryDataSource_Selecting"
                OnSelected="CallsHistoryDataSource_Selected"
                SelectMethod="GetCallsHistoryFilter"
                TypeName="Lync_Billing.ui.user.history">
                <SelectParameters>
                    <asp:Parameter Name="start" Type="Int32" />
                    <asp:Parameter Name="limit" Type="Int32" />
                    <asp:Parameter Name="sort" Type="Object" />                
                    <asp:Parameter Name="count" Direction="Output" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <ext:GridPanel
                ID="PhoneCallsHistoryGrid" 
                runat="server" 
                Title="Phone Calls History"
                Width="740"
                Height="740"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="PhoneCallStore" 
                        runat="server" 
                        RemoteSort="true" 
                        DataSourceID="PhoneCallsDataSource"
                        OnReadData="PhoneCallStore_ReadData"
                        PageSize="25">

                        <Proxy>
                            <ext:PageProxy CacheString=""/>
                        </Proxy>

                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_CallType" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                    <ext:ModelField Name="AC_IsInvoiced" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        
                        <Sorters>
                            <ext:DataSorter Property="SessionIdTime" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="PhoneCallsColumnModel" runat="server">
		            <Columns>
                        <ext:RowNumbererColumn
                            ID="RowNumbererColumn2"
                            runat="server"
                            Width="45" />

                        <ext:Column ID="SessionIdTime" 
                            runat="server" 
                            Text="Date" 
                            Width="140" 
                            DataIndex="SessionIdTime"
                            Groupable="false">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country"
                            Width="100"
                            DataIndex="Marker_CallToCountry" 
                            Groupable="true"/>

                        <ext:Column ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="120"
                            DataIndex="DestinationNumberUri"
                            Groupable="true" />

                        <ext:Column ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="90"
                            DataIndex="Duration"
                            Groupable="false" >
                            <Renderer Fn="GetMinutes"/>
                        </ext:Column>

                        <ext:Column ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost"
                            Groupable="false">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>

                        <ext:Column ID="UI_CallType"
                            runat="server"
                            Text="Type"
                            Width="80"
                            DataIndex="UI_CallType" 
                            Groupable="false">
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="100"
                            DataIndex="UI_MarkedOn"
                             Groupable="true">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                        </ext:Column>
		            </Columns>
                </ColumnModel>

                <Features>               
                    <ext:GroupingSummary 
                        ID="GroupingPhoneCallsHistory" 
                        runat="server" 
                        GroupHeaderTplString="{name}" 
                        HideGroupedHeader="true" 
                        EnableGroupingMenu="true"
                        EnableNoGroups="true"
                        ShowSummaryRow="true"/>
                    <ext:GridFilters ID="StatusTypeFilter" runat="server">
                        <Filters>
                            <ext:StringFilter DataIndex="UI_CallType"/>
                        </Filters>
                    </ext:GridFilters>

                </Features>

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
                                ValueField="TypeValue"
                                Width="200"
                                FieldLabel="View:"
                                LabelWidth="30"
                                Margins="5 390 0 5">
                                
                                <Items>
                                    <ext:ListItem Text="Everything" Value="Everything"/>
                                    <ext:ListItem Text="Business" Value="Business" />
                                    <ext:ListItem Text="Personal" Value="Personal" />
                                    <ext:ListItem Text="Disputed" Value="Disputed" />
                                </Items>

                                 <DirectEvents>
                                     <Select OnEvent="PhoneCallsHistoryFilter" />
                                 </DirectEvents>

                                <SelectedItems>
                                    <ext:ListItem Text="Everything" Value="Everything" />
                                </SelectedItems>
                            </ext:ComboBox>

                            <ext:Button
                                ID="ExportToPDFButton"
                                runat="server"
                                Text="To PDF"
                                Icon="PageSave">
                                <Listeners>
                                    <Click Handler="submitValue(#{PhoneCallsHistoryGrid}, #{FormatType}, 'pdf');" />
                                </Listeners>
                            </ext:Button>

                            <ext:Button
                                ID="ExportToExcelButton"
                                runat="server"
                                Text="To Excel"
                                Icon="PageExcel">
                                <Listeners>
                                    <Click Handler="submitValue(#{PhoneCallsHistoryGrid}, #{FormatType}, 'xls');" />
                                </Listeners>
                            </ext:Button>
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
                    <ext:CheckboxSelectionModel ID="PhoneCallsCheckBoxColumn" runat="server" Mode="Multi"  Visible="false"/>
                </SelectionModel>
            </ext:GridPanel>
        </div>
        <!-- *** END OF PHONE CALLS HISTORY GRID *** -->
    </div>
</asp:Content>
