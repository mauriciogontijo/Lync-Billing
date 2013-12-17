<%@ Page Title="tBill User | Address Book" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="addressbook.aspx.cs" Inherits="Lync_Billing.ui.user.addressbook" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        .row-green { background-color: rgb(46, 143, 42); }
        .row-red { background-color: rgb(201, 20, 20); }
        .row-yellow { background-color: yellow; }
        /* end manage-phone-calls grid styling */
    </style>

    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{AddressBookGrid}.getStore();
                store.filterBy(getRecordFilter("addressbook"));
            };
             
            var ImportContactGrid_ApplyFilter = function (field) {
                var store = #{ImportContactsGrid}.getStore();
                store.filterBy(getRecordFilter("import"));
            };

            var clearFilter = function () {
                #{DestNumberFilter}.reset();
                #{DestCountryFilter}.reset();
                #{ContactNameFilter}.reset();
                #{ContactTypeFilter}.reset();
                
                #{AddressBookStore}.clearFilter();
            }
 
            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if (typeof val != "string") {
                    return value.length == 0;
                }
                
                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            };
 
            var getRecordFilter = function (grid) {
                var f = [];
 
                if(grid != undefined && grid == "addressbook")
                {
                    f.push({
                        filter: function (record) {                         
                            return filterString(#{DestNumberFilter}.getValue(), "DestinationNumber", record);
                        }
                    });
                    
                    f.push({
                        filter: function (record) {                         
                            return filterString(#{ContactNameFilter}.getValue(), "Name", record);
                        }
                    });
                }
                else if(grid != undefined && grid == "import")
                {
                    f.push({
                        filter: function (record) {                         
                            return filterString(#{ImportContactNumberFilter}.getValue(), "DestinationNumber", record);
                        }
                    });

                    f.push({
                        filter: function (record) {                         
                            return filterString(#{ImportContactDestCountryFilter}.getValue(), "DestinationCountry", record);
                        }
                    });
                }
 
                var len = f.length;
                 
                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            };
        </script>
    </ext:XScript>
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
                Height="785"
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
                                    Width="150"
                                    DataIndex="DestinationNumber">
                                    <HeaderItems>
                                        <ext:TextField ID="DestNumberFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearDestNumFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>
                                </ext:Column>

                                <ext:Column
                                    ID="ADContactNameCol" 
                                    runat="server"
                                    DataIndex="Name"
                                    Width="250"
                                    Text="Contact Name"
                                    Selectable="true"
                                    Flex="1">
                                    <HeaderItems>
                                        <ext:TextField ID="ContactNameFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearContactNameFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>
                                    <Editor>
                                        <ext:TextField
                                            ID="ADContactNameTextbox"
                                            runat="server"
                                            DataIndex="Name" />
                                    </Editor>
                                </ext:Column>

                                <ext:Column ID="ABContactInformationColumn"
                                    runat="server"
                                    Text="Contact Information"
                                    MenuDisabled="true"
                                    Sortable="false"
                                    Resizable="false">
                                    <Columns>
                                        <ext:Column
                                            ID="DestCountry"
                                            runat="server"
                                            Text="Country"
                                            Width="120"
                                            DataIndex="DestinationCountry"
                                            Sortable="true"
                                            Groupable="true"
                                            Resizable="false"
                                            MenuDisabled="true" />

                                        <ext:Column
                                            ID="ADContactTypeCol"
                                            runat="server"
                                            DataIndex="Type"
                                            Text="Type"
                                            Width="90"
                                            Flex="1"
                                            Selectable="true"
                                            Sortable="true"
                                            Groupable="true"
                                            Resizable="false"
                                            MenuDisabled="true">
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
                                    </Columns>
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
                    

                    <%-- *********************************************************************************************************************** --%>


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
                                    Width="150"
                                    DataIndex="DestinationNumber">
                                    <HeaderItems>
                                        <ext:TextField ID="ImportContactNumberFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="ImportContactGrid_ApplyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearButton1" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>
                                </ext:Column>

                                <ext:Column
                                    ID="ImportedContactDestinationCountry"
                                    runat="server"
                                    Text="Country"
                                    Width="130"
                                    DataIndex="DestinationCountry"
                                    Sortable="true"
                                    Resizable="false"
                                    Groupable="true">
                                    <HeaderItems>
                                        <ext:TextField ID="ImportContactDestCountryFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="ImportContactGrid_ApplyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearCountryNameFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>
                                </ext:Column>

                                <ext:Column ID="ContactInformationColumn"
                                    runat="server"
                                    Text="Contact Information"
                                    MenuDisabled="true"
                                    Sortable="false"
                                    Resizable="false">
                                    <Columns>
                                        <ext:Column
                                            ID="ContanctNameCol" 
                                            runat="server"
                                            DataIndex="Name"
                                            Width="250"
                                            Text="Name"
                                            Selectable="true"
                                            Flex="1"
                                            Resizable="false">
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
                                            Text="Type"
                                            Width="110"
                                            Flex="1"
                                            Selectable="true"
                                            Resizable="false">
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
                                    </Columns>
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
                                                    <ext:Parameter Name="Values" Value="
                                                        (#{ImportContactsGrid}.getRowsValues(true))" Mode="Raw" />
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
