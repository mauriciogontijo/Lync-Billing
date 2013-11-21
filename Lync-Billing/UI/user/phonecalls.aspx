<%@ Page Title="eBill User | Manage Phonecalls" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="phonecalls.aspx.cs" Inherits="Lync_Billing.ui.user.phonecalls" %>

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

                    <ext:MenuItem
                        ID="AllocatePhonecallsAsDispute"
                        Text="As Dispute"
                        runat="server">
                        <DirectEvents>
                            <Click OnEvent="AssignDispute">
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
                Header="true"
                Title="Manage Phone Calls"
                Width="740"
                Height="745"
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
                                Margins="5 470 5 5">
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

                            <ext:Button
                                ID="HelpDialogButton"
                                runat="server"
                                Text="Help"
                                Icon="Help">
                                <DirectEvents>
                                    <Click OnEvent="ShowUserHelpPanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:Window 
                                ID="UserHelpPanel" 
                                runat="server" 
                                Layout="Accordion" 
                                Icon="Help" 
                                Title="User Help" 
                                Hidden="true" 
                                Width="320" 
                                Height="420" 
                                Frame="true" 
                                X="150" 
                                Y="100">
                                <Items>
                                    <ext:Panel ID="MultipleSelectPanel" runat="server" Icon="Anchor" Title="How do I select multiple Phonecalls?">
                                        <Content>
                                            <div class="text-left p10">
                                                <p class='font-14 line-height-1-5'>You can select multiple phonecalls by pressing the <span class="bold red-color">&nbsp;[Ctrl]&nbsp;</span> button.</p>
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel ID="AllocatePhonecalls" runat="server" Icon="ApplicationEdit" Title="How do I allocate my Phonecalls?">
                                        <Content>
                                            <div class="p10 text-left font-14 line-height-1-5 over-h">
                                                <p class="mb10">You can allocate your phonecalls by <span class="bold red-color">&nbsp;[Right Clicking]&nbsp;</span> on the selected phonecalls and choosing your preferred action from the first section of the menu - <span class="blue-color">Selected Phonecalls</span> section.</p>
                                                <p>The list of actions is:</p>
                                                <ol class="ml35" style="list-style-type: decimal">
                                                    <li>As Business</li>
                                                    <li>As Personal</li>
                                                    <li>As Dispute</li>
                                                </ol>
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel ID="MarkPhoneCallsAndDestinations" runat="server" Icon="ApplicationSideList" Title="How do I allocate Destinations?">
                                        <Content>
                                            <div class="p10 text-left font-14 line-height-1-5">
                                                <p class="mb10">If you <span class="bold red-color">&nbsp;[Right Click]&nbsp;</span> on the grid, you can mark the destination(s) of your selected phonecall(s) as either <span class="bold">Always Business</span> or <span class="bold">Always Personal</span> from the second section of menu - <span class="blue-color">Selected Destinations</span> section.</p>
                                                <p>Please note that marking a destination results in adding it to your phonebook, and from that moment on any phonecall to that destination will be marked automatically as the type of this phonebook contact (Business/Personal).</p>
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel ID="AssignContactNamesToDestinations" runat="server" Icon="User" Title="How do I assign &quot;Contact Names&quot; to Destinations?">
                                        <Content>
                                            <div class="text-left p10">
                                                <p class="font-14 line-height-1-5">You can add Contact Name to a phonecall destination by <span class="bold red-color">&nbsp;[Double Clicking]&nbsp;</span> on the <span class="blue-color">&nbsp;&quot;Contact Name&quot;&nbsp;</span> field and then filling the text box, please note that this works for the Unallocated phonecalls.</p>
                                            </div>
                                        </Content>
                                    </ext:Panel>
                                </Items>
                            </ext:Window>
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
