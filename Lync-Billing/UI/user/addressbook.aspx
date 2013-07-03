<%@ Page Title="eBill User | Address Book" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="addressbook.aspx.cs" Inherits="Lync_Billing.ui.user.addressbook" %>

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
    <!-- *** START OF ADDRESS BOOK PANEL *** -->
    <div id='manage-phone-calls-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:TabPanel
                ID="AddressBookTabPanel"
                runat="server"
                Header="true"
                Title="Manage Address Book"
                Width="740"
                Height="765"
                Plain="false">
                
                <Defaults>
                    <ext:Parameter Name="autoScroll" Value="true" Mode="Raw" />
                </Defaults>

                <Items>
                    <ext:GridPanel
                        ID="AddressBookGrid"
                        runat="server"
                        Header="false"
                        Layout="TableLayout"
                        Title="Address Book"
                        Height="740"
                        Icon="BookOpen">
                        <Store>
                            <ext:Store
                                ID="AddressBookStore"
                                runat="server"
                                IsPagingStore="true"
                                PageSize="25"
                                OnLoad="AddressBookStore_Load">
                                <Model>
                                    <ext:Model ID="AddressBookStoreModel" runat="server" IDProperty="DestinationNumber">
                                        <Fields>
                                            <ext:ModelField Name="ID" Type="Int" />
                                            <ext:ModelField Name="SipAccount" Type="String" />
                                            <ext:ModelField Name="DestinationNumber" Type="String" />
                                            <ext:ModelField Name="DestinationCountry" Type="String" />
                                            <ext:ModelField Name="Name" Type="String" />
                                            <ext:ModelField Name="Type" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Plugins>
                            <ext:CellEditing ID="CellEditing2" runat="server" ClicksToEdit="2" />
                        </Plugins>

                        <ColumnModel ID="AddressBookColumnModel" runat="server" Flex="1">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />
                                <ext:Column
                                    ID="DestNumber"
                                    runat="server"
                                    Text="Number"
                                    Width="120"
                                    DataIndex="DestinationNumber" />

                                <ext:Column
                                    ID="DestCountry"
                                    runat="server"
                                    Text="Country"
                                    Width="150"
                                    DataIndex="DestinationCountry" />

                                <ext:Column
                                    ID="ADContactNameCol" 
                                    runat="server"
                                    DataIndex="Name"
                                    Width="250"
                                    Text="Contact Name"
                                    Selectable="true"
                                    Flex="1">
                                    <Editor>
                                        <ext:TextField
                                            ID="ADContactNameTextbox"
                                            runat="server"
                                            DataIndex="Name" />
                                    </Editor>
                                </ext:Column>

                                <ext:Column
                                    ID="ADContactTypeCol"
                                    runat="server"
                                    DataIndex="Type"
                                    Text="Contact Type"
                                    Width="90"
                                    Flex="1"
                                    Selectable="true">
                                    <Editor>
                                        <ext:ComboBox
                                            ID="ADContactTypeCombo"
                                            runat="server"
                                            DataIndex="Type"
                                            EmptyText="Please Select Type"
                                            SelectOnFocus="true"
                                            SelectOnTab="true"
                                            Selectable="true"
                                            Width="70"
                                            AllowBlank="false"
                                            AllowOnlyWhitespace="false">
                                            <Items>
                                                <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                                <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                            </Items>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>

                                <ext:ImageCommandColumn
                                    ID="DeleteButtonsColumn"
                                    runat="server"
                                    Width="30"
                                    Sortable="false"
                                    Align="Center">
                                    <Commands>
                                        <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Contact" CommandName="delete">                            
                                        </ext:ImageCommand>
                                    </Commands>
                                    <Listeners>
                                        <Command Handler="this.up(#{AddressBookGrid}.store.removeAt(recordIndex));" />
                                    </Listeners>
                                </ext:ImageCommandColumn>

                                <ext:CommandColumn ID="RejectChange" runat="server" Width="70">
                                    <Commands>
                                        <ext:GridCommand Text="Reject" ToolTip-Text="Reject row changes" CommandName="reject" Icon="ArrowUndo" />
                                    </Commands>
                                    <PrepareToolbar Handler="toolbar.items.get(0).setVisible(record.dirty);" />
                                    <Listeners>
                                        <Command Handler="record.reject();" />
                                    </Listeners>
                                </ext:CommandColumn>

                            </Columns>
                        </ColumnModel>

                        <TopBar>
                            <ext:Toolbar
                                ID="Toolbar1"
                                runat="server">
                                <Items>
                                    <ext:Button
                                        ID="UpdateAddressBookButton"
                                        Text="Save Changes"
                                        Icon="Add"
                                        runat="server"
                                        Margins="0 20 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="UpdateAddressBook_DirectEvent" before="return #{AddressBookStore}.isDirty();">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="#{AddressBookStore}.getChangedData()" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button
                                        ID="CancelChangesButton"
                                        Text="Cancel Changes"
                                        Icon="Cancel"
                                        runat="server"
                                        Margins="0 0 0 0">
                                        <DirectEvents>
                                            <Click OnEvent="RejectAddressBookChanges_DirectEvent">
                                                <EventMask ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <BottomBar>
                            <ext:PagingToolbar
                                ID="AddressBookPagination"
                                runat="server"
                                StoreID="PhoneCallStore"
                                DisplayInfo="true"
                                Weight="25"
                                DisplayMsg="Contacts {0} - {1} of {2}" />
                        </BottomBar>
                    </ext:GridPanel>
                    


                    <ext:GridPanel
                        ID="ImportContactsGrid"
                        runat="server"
                        Header="false"
                        Layout="TableLayout"
                        Title="Import Contacts"
                        Height="740"
                        Icon="BookEdit">
                        <Store>
                            <ext:Store
                                ID="ImportContactsStore"
                                runat="server"
                                IsPagingStore="true"
                                PageSize="25"
                                OnLoad="ImportContactsStore_Load">
                                <Model>
                                    <ext:Model ID="ImportContactsStoreModel" runat="server" IDProperty="DestinationNumber">
                                        <Fields>
                                            <ext:ModelField Name="ID" Type="Int" />
                                            <ext:ModelField Name="SipAccount" Type="String" />
                                            <ext:ModelField Name="DestinationNumber" Type="String" />
                                            <ext:ModelField Name="DestinationCountry" Type="String" />
                                            <ext:ModelField Name="Name" Type="String" />
                                            <ext:ModelField Name="Type" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Plugins>
                            <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                        </Plugins>

                        <ColumnModel ID="ImportContactsColumnModel" runat="server" Flex="1">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Width="25" />
                                <ext:Column
                                    ID="ImportedContactNumber"
                                    runat="server"
                                    Text="Number"
                                    Width="140"
                                    DataIndex="DestinationNumber" />

                                <ext:Column
                                    ID="ImportedContactDestinationCountry"
                                    runat="server"
                                    Text="Country"
                                    Width="150"
                                    DataIndex="DestinationCountry" />

                                <ext:Column
                                    ID="ContanctNameCol" 
                                    runat="server"
                                    DataIndex="Name"
                                    Width="240"
                                    Text="Contact Name"
                                    Selectable="true"
                                    Flex="1">
                                    <Editor>
                                        <ext:TextField
                                            ID="NameTextbox"
                                            runat="server"
                                            DataIndex="Name" />
                                    </Editor>
                                </ext:Column>

                                <ext:Column
                                    ID="ContactTypeCol"
                                    runat="server"
                                    DataIndex="Type"
                                    Text="Contact Type"
                                    Width="130"
                                    Flex="1"
                                    Selectable="true">
                                    <Editor>
                                        <ext:ComboBox
                                            ID="ContactTypeDropdown"
                                            runat="server"
                                            DataIndex="Type"
                                            EmptyText="Please Select Type"
                                            SelectOnFocus="true"
                                            SelectOnTab="true"
                                            Selectable="true"
                                            Width="110">
                                            <Items>
                                                <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                                <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                            </Items>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>
                                 <ext:CommandColumn ID="RejectChanges" runat="server" Width="70">
                                    <Commands>
                                        <ext:GridCommand Text="Reject" ToolTip-Text="Reject row changes" CommandName="reject" Icon="ArrowUndo" />
                                    </Commands>
                                    <PrepareToolbar Handler="toolbar.items.get(0).setVisible(record.dirty);" />
                                    <Listeners>
                                        <Command Handler="record.reject();" />
                                    </Listeners>
                                </ext:CommandColumn>
                            </Columns>
                        </ColumnModel>

                         <SelectionModel>
                            <ext:RowSelectionModel ID="ImportContactsGridRowSelectionModel"
                                runat="server"
                                Mode="Single"
                                AllowDeselect="true"
                                CheckOnly="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>

                        <TopBar>
                            <ext:Toolbar
                                ID="ImportContactsToolbar"
                                runat="server">
                                <Items>
                                    <ext:Button
                                        ID="ImportItems"
                                        Text="Sync with Address Book"
                                        Icon="Add"
                                        runat="server"
                                        Margins="0 20 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="ImportContactsFromHistory">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ImportContactsGrid}.getRowsValues(true))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button
                                        ID="CancelImportContactsChangesChanges"
                                        Text="Cancel Changes"
                                        Icon="Cancel"
                                        runat="server"
                                        Margins="0 0 0 0">
                                        <DirectEvents>
                                            <Click OnEvent="RejectImportChanges_DirectEvent">
                                                <EventMask ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <BottomBar>
                            <ext:PagingToolbar
                                ID="ImportContactsPagingBottomBar"
                                runat="server"
                                StoreID="PhoneCallStore"
                                DisplayInfo="true"
                                Weight="25"
                                DisplayMsg="Contacts {0} - {1} of {2}" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:TabPanel>
        </div>
    </div>
    <!-- *** END OF ADDRESS BOOK PANEL *** -->
</asp:Content>


<%--
<ext:ComponentColumn ID="ComponentColumn3" 
    runat="server"
    DataIndex="Name"
    Width="240"
    Text="Contact Name"
    Editor="true"
    OverOnly="true"
    PinEvents="expand"
    UnpinEvents="collapse"
    Selectable="true"
    Flex="1">
    <Component>
        <ext:TextField ID="TextField1"
            runat="server"
            Selectable="true"
            SelectOnFocus="true"
                SelectOnTab="true"
            EmptyText="Example: John Smith"
            DataIndex="Name" />
    </Component>
</ext:ComponentColumn>

<ext:ComponentColumn ID="ComponentColumn4"
    runat="server"
    DataIndex="Type"
    Text="Contact Type"
    Width="130"
    Editor="true"
    OverOnly="true"
    PinEvents="expand"
    UnpinEvents="collapse"
    Flex="1"
    Selectable="true">
    <Component>
        <ext:ComboBox ID="ComboBox2"
            runat="server"
            EmptyText="Please Select Type"
            DataIndex="Type"
            SelectOnFocus="true"
            SelectOnTab="true"
            Selectable="true"
            Width="110">
            <Items>
                <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                <ext:ListItem Text="Business" Value="Business" Mode="Value" />
            </Items>
        </ext:ComboBox>
    </Component>
</ext:ComponentColumn>
--%>