﻿<%@ Page Title="eBill User | Manage Phonecalls" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="phonecalls.aspx.cs" Inherits="Lync_Billing.ui.user.phonecalls" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        /* end manage-phone-calls grid styling */
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF MANAGE PHONE CALLS GRID *** -->
    <div id='manage-phone-calls-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:Hidden ID="FormatType" runat="server" />

            <ext:Menu
                ID="PhoneCallsAllocationToolsMenu"
                runat="server"
                Width="170"
                Header="true"
                Title="Allocate Phonecalls"
                Frame="false">
                <Items>
                    <ext:MenuItem
                        runat="server"
                        ID="AllocPhonecallsFieldLabel"
                        Text="Selected Phonecall(s):"
                        Margins="5 0 5 0"
                        Disabled="true"
                        DisabledCls=""
                        Cls="font-12 popup-menu-field-label-background" />

                    <ext:MenuItem
                        ID="AllocatePhonecallsAsBusiness"
                        Text="As Business"
                        runat="server">
                        <DirectEvents>
                            <Click OnEvent="AssignBusiness">
                                <EventMask ShowMask="true" />
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:MenuItem>

                    <ext:MenuItem
                        ID="AllocatePhonecallsAsPersonal"
                        Text="As Personal"
                        runat="server">
                        <DirectEvents>
                            <Click OnEvent="AssignPersonal">
                                <EventMask ShowMask="true" />
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:MenuItem>

                    <ext:MenuSeparator ID="MenuSeparator" runat="server" />

                    <ext:MenuItem
                        runat="server"
                        ID="AllocDestinationsFieldLabel"
                        Text="Selected Destination(s):"
                        Margins="5 0 5 0"
                        Disabled="true"
                        DisabledCls=""
                        Cls="font-12 popup-menu-field-label-background" />

                    <ext:MenuItem
                        ID="AllocateDestinationsAsAlwaysBusiness"
                        Text="As Always Business"
                        runat="server"
                        Margins="0 0 5 0">
                        <DirectEvents>
                            <Click OnEvent="AssignAlwaysBusiness">
                                <EventMask ShowMask="true" />
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:MenuItem>

                    <ext:MenuItem
                        ID="AllocateDestinationsAsAlwaysPersonal"
                        Text="As Always Personal"
                        runat="server"
                        Margins="0 0 5 0">
                        <DirectEvents>
                            <Click OnEvent="AssignAlwaysPersonal">
                                <EventMask ShowMask="true" />
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:MenuItem>
                </Items>
            </ext:Menu>

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
                                Margins="5 390 5 5">
                                <Items>
                                    <ext:ListItem Text="Unallocated" Value="Unmarked" />
                                    <ext:ListItem Text="Business" Value="Business" />
                                    <ext:ListItem Text="Personal" Value="Personal" />
                                    <ext:ListItem Text="Disputed" Value="Disputed" />
                                </Items>
                                <SelectedItems>
                                    <ext:ListItem Text="Unallocated" Value="Unmarked" />
                                </SelectedItems>
                                <DirectEvents>
                                    <Select OnEvent="PhoneCallsTypeFilter" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <%--<ext:Button
                                ID="ExportToPDFButton"
                                runat="server"
                                Text="To PDF"
                                Icon="PageSave">
                                <Listeners>
                                    <Click Handler="submitValue(#{ManagePhoneCallsGrid}, #{FormatType}, 'pdf');" />
                                </Listeners>
                            </ext:Button>

                            <ext:Button
                                ID="ExportToExcelButton"
                                runat="server"
                                Text="To Excel"
                                Icon="PageExcel">
                                <Listeners>
                                    <Click Handler="submitValue(#{ManagePhoneCallsGrid}, #{FormatType}, 'xls');" />
                                </Listeners>
                            </ext:Button>--%>
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
                Height="715"
                AutoScroll="true"
                Scroll="Both"
                Layout="TableLayout"
                ContextMenuID="PhoneCallsAllocationToolsMenu">
                <Store>
                    <ext:Store
                        ID="PhoneCallsStore"
                        runat="server"
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
                                    <ext:ModelField Name="PhoneCallTable" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SessionIdTime" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:CellEditing ID="CellEditingPlugin" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn
                            ID="RowNumbererColumn2"
                            runat="server"
                            Width="25" />

                        <ext:Column
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="135"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country"
                            Width="90"
                            DataIndex="Marker_CallToCountry" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column ID="PhoneBookNameCol"
                            runat="server"
                            Text="Contact Name"
                            Width="190"
                            DataIndex="PhoneBookName">
                            <Editor>
                                <ext:TextField
                                    ID="PhoneBookNameEditorTextbox"
                                    runat="server"
                                    DataIndex="PhoneBookName"
                                    EmptyText="Add a contact name"
                                    ReadOnly="false" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="90"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="70"
                            DataIndex="Marker_CallCost">
                            <Renderer Fn="RoundCost" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                
                <SelectionModel>
                    <ext:RowSelectionModel ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:RowSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Label
                                runat="server"
                                ID="button_group_lable"
                                Margins="5 0 0 5"
                                Width="110">
                                <Content>Allocate Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup
                                ID="MarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="255"
                                Frame="false"
                                ButtonAlign="Left"
                                Margins="0 260 0 0">
                                <Buttons>
                                    <ext:Button
                                        ID="AllocateAsBusinessButton"
                                        Text="Business"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignAlwaysBusiness">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button
                                        ID="AllocateAsPersonalButton"
                                        Text="Personal"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignAlwaysPersonal">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    
                                    <ext:Button
                                        ID="AllocateAsDisputeButton"
                                        Text="Dispute"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>

                                <%--<Buttons>
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
                                                    <ext:MenuItem ID="AllocateAsAlwaysBusiness" runat="server" Text="Allocate as Always Business" Icon="GroupEdit">
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

                                                    <ext:MenuItem ID="AllocateAsAlwaysPersonal" runat="server" Text="Allocate as Always Personal" Icon="GroupEdit">
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

                                                    <ext:MenuItem ID="AllocateAsDispute" runat="server" Text="Allocate as Disputed" Icon="GroupError">
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
                                                                Html="This will mark the selected phonecalls(s) as DISPUTE. Please note that this will not affect any future phonecalls. Only the selected phonecalls at the moment will be marked as Disputed."
                                                             />
                                                        </ToolTips>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                </Buttons>--%>
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
