<%@ Page Title="eBill User | Manage Phonecalls" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="phonecalls.aspx.cs" Inherits="Lync_Billing.ui.user.phonecalls" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        .row-green { background-color: rgb(46, 143, 42); }
        .row-red { background-color: rgb(201, 20, 20); }
        .row-yellow { background-color: yellow; }
        /* end manage-phone-calls grid styling */
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF MANAGE PHONE CALLS GRID *** -->
    <div id='manage-phone-calls-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Panel
                ID="FilterPhoneCallsPanel"
                runat="server"
                Header="true"
                Title="Manage Phone Calls"
                Width="740"
                Height="61"
                Layout="AnchorLayout">
                <TopBar>
                    <ext:Toolbar
                        ID="FilterToolbar1"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterTypeComboBox"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="TypeName"
                                ValueField="TypeValue"
                                FieldLabel="View Calls:"
                                LabelWidth="60"
                                Width="200"
                                Margins="5 450 5 5">
                                <Items>
                                    <ext:ListItem Text="Unmarked" Value="Unmarked" />
                                    <ext:ListItem Text="Business" Value="Business" />
                                    <ext:ListItem Text="Personal" Value="Personal" />
                                    <ext:ListItem Text="Dispute" Value="Dispute" />
                                </Items>
                                <SelectedItems>
                                    <ext:ListItem Text="Unmarked" Value="Unmarked" />
                                </SelectedItems>
                                <DirectEvents>
                                    <Select OnEvent="PhoneCallsHistoryFilter" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="ExportToExcel"
                                runat="server"
                                Text="To Excel"
                                Icon="PageExcel">
                                <Listeners>
                                    <Click Handler="submitValue(#{ManagePhoneCallsGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>

            <div class="h5 clear"></div>

            <asp:ObjectDataSource
                ID="PhoneCallsDataSource"
                runat="server"
                OnSelecting="PhoneCallsDataSource_Selecting"
                OnSelected="PhoneCallsDataSource_Selected"
                SelectMethod="GetPhoneCallsFilter"
                TypeName="Lync_Billing.ui.user.phonecalls">
                <SelectParameters>
                    <asp:Parameter Name="start" Type="Int32" />
                    <asp:Parameter Name="limit" Type="Int32" />
                    <asp:Parameter Name="sort" Type="Object" />
                    <asp:Parameter Name="count" Direction="Output" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <ext:GridPanel
                ID="ManagePhoneCallsGrid"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">
                <Store>
                    <ext:Store
                        ID="PhoneCallsStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25"
                        DataSourceID="PhoneCallsDataSource"
                        OnSubmitData="PhoneCallsStore_SubmitData"
                        OnReadData="PhoneCallsStore_ReadData">
                        <Proxy>
                            <ext:PageProxy CacheString="" />
                        </Proxy>
                        <Model>
                            <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                    <ext:ModelField Name="ResponseTime" Type="String" />
                                    <ext:ModelField Name="SessionEndTime" Type="String" />
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_CallType" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                    <ext:ModelField Name="PhoneBookName" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SessionIdTime" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />
                        <ext:Column
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="140"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="Marker_CallToCountry" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="70"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost">
                            <Renderer Fn="RoundCost" />
                        </ext:Column>

                        <ext:Column ID="UI_CallType"
                            runat="server"
                            Text="Type"
                            Width="80"
                            DataIndex="UI_CallType">
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column
                            ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="114"
                            DataIndex="UI_MarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <Features>
                    <ext:GridFilters ID="StatusTypeFilter" runat="server">
                        <Filters>
                            <ext:StringFilter DataIndex="UI_CallType" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Label
                                runat="server"
                                ID="button_group_lable"
                                Margins="5 0 0 5"
                                Width="90">
                                <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup
                                ID="MarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="255"
                                Frame="false"
                                ButtonAlign="Left"
                                Margins="5 260 0 5">
                                <Buttons>
                                    <ext:Button
                                        ID="Business"
                                        Text="Business"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignAllBusiness">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button
                                        ID="Personal"
                                        Text="Personal"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignAllPersonal">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button
                                        ID="AdvancedActions"
                                        runat="server"
                                        Text="Advanced">
                                        <Menu>
                                            <ext:Menu ID="AdvancedActionsMenu" runat="server">
                                                <Items>                    
                                                    <ext:MenuItem ID="MarkAsAlwaysBusiness" runat="server" Text="Mark as Always Business" Icon="GroupEdit">
                                                        <DirectEvents>
                                                            <Click OnEvent="AssignAlwaysBusiness">
                                                                <EventMask ShowMask="true" />
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                        <ToolTips>
                                                            <ext:ToolTip ID="AlwaysBusinessTooltips" 
                                                                runat="server" 
                                                                Height="80"
                                                                Width="500"
                                                                Html="This will mark the selected phonecalls(s) as BUSINESS, then it will add the destination number(s) to your addressbook as BUSINESS CONTACTS, which will tell the system to automatically mark all future phonecall(s) - with respect to these destinations - as BUSINESS."
                                                             />
                                                        </ToolTips>
                                                    </ext:MenuItem>

                                                    <ext:MenuItem ID="MarkAsAlwaysPersonal" runat="server" Text="Mark as Always Personal" Icon="GroupEdit">
                                                        <DirectEvents>
                                                            <Click OnEvent="AssignAlwaysPersonal">
                                                                <EventMask ShowMask="true" />
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                        <ToolTips>
                                                            <ext:ToolTip ID="ToolTip1" 
                                                                runat="server" 
                                                                Height="80"
                                                                Width="500"
                                                                Html="This will mark the selected phonecalls(s) as PERSONAL, then it will add the destination number(s) to your addressbook as PERSONAL CONTACTS, which will tell the system to automatically mark all future phonecall(s) - with respect to these destinations - as PERSONAL."
                                                             />
                                                        </ToolTips>
                                                    </ext:MenuItem>

                                                    <ext:MenuItem ID="MarkAsDispute" runat="server" Text="Mark as Dispute" Icon="GroupError">
                                                        <DirectEvents>
                                                            <Click OnEvent="AssignDispute">
                                                                <EventMask ShowMask="true" />
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                        <ToolTips>
                                                            <ext:ToolTip ID="ToolTip2" 
                                                                runat="server" 
                                                                Height="70"
                                                                Width="500"
                                                                Html="This will mark the selected phonecalls(s) as DISPUTE. Please note that this will not affect any future phonecalls. Only the selected phonecalls at the moment will be marked as Dispute."
                                                             />
                                                        </ToolTips>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>

                            <%--<ext:Button
                                ID="CancelChangesButton"
                                Text="Cancel Changes"
                                Icon="Cancel"
                                runat="server"
                                Margins="5 5 0 0">
                                <DirectEvents>
                                    <Click OnEvent="RejectChanges_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>--%>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PhoneCallStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Phone Calls {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF MANAGE PHONE CALLS GRID *** -->
</asp:Content>
