<%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="addressbook.aspx.cs" Inherits="Lync_Billing.ui.user.addressbook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        .row-green { background-color: rgb(46, 143, 42); }
        .row-red { background-color: rgb(201, 20, 20); }
        .row-yellow { background-color: yellow; }
        /* end manage-phone-calls grid styling */
    </style>

    <script type="text/javascript">
        BrowserDetect.init();

        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#user-tab').addClass('selected');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="420"
                Width="180"
                Title="User Tools"
                Collapsed="false"
                Collapsible="true">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Manage</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/phonecalls.aspx'>Phone Calls</a></p>
                            <p><a href="../user/addressbook.aspx" class="selected">Address Book</a></p>
                            <!--<p><a href="#">Authorized Delegate</a></p>-->
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/bills.aspx'>Bills History</a></p>
                            <p><a href='../user/history.aspx'>Phone Calls History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/statistics.aspx'>Phone Calls Statistics</a></p>
                        </div>
                    </div>

                    <%
                        Lync_Billing.DB.UserSession session = (Lync_Billing.DB.UserSession)Session.Contents["UserData"];
                        bool is_delegate = (session.IsDelegate || session.IsDeveloper);
                        if (is_delegate)
                        {
                    %>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Delegee Accounts</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/delegees.aspx'>Manage Delegee(s)</a></p>
                        </div>
                    </div>
                    <% } %>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ADDRESS BOOK PANEL *** -->
    <div id='manage-phone-calls-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:TabPanel ID="AddressBookTabPanel"
                runat="server"
                Width="740"
                Height="760"
                Margins="0 0 20 0"
                Frame="true">
                <Defaults>
                    <ext:Parameter Name="autoScroll" Value="true" Mode="Raw" />
                </Defaults>
                <Items>
                    <ext:GridPanel
                        ID="AddressBookGrid"
                        runat="server"
                        Header="false"
                        Layout="FitLayout"
                        Title="Address Book"
                        Height="700">
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

                       <%-- <Plugins>
                            <ext:RowEditing ID="RowEditing2" runat="server" ClicksToMoveEditor="1" AutoCancel="false" />
                        </Plugins>--%>

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

                                <ext:ComponentColumn ID="ComponentColumn1"
                                    runat="server"
                                    Text="Contact Name"
                                    Width="240"
                                    Editor="true"
                                    OverOnly="true"
                                    DataIndex="Name"
                                    PinEvents="expand"
                                    UnpinEvents="collapse">
                                    <Component>
                                        <ext:TextField ID="ContactName"
                                            runat="server"
                                            EmptyText="Example: John Smith"
                                            DataIndex="Name" />
                                    </Component>
                                </ext:ComponentColumn>

                                <ext:ComponentColumn ID="ComponentColumn2"
                                    runat="server"
                                    Text="Contact Type"
                                    Width="130"
                                    Editor="true"
                                    OverOnly="true"
                                    DataIndex="Type"
                                    PinEvents="expand"
                                    UnpinEvents="collapse">
                                    <Component>
                                        <ext:ComboBox ID="ComboBox1"
                                            runat="server"
                                            EmptyText="Please Select Type"
                                            Width="110"
                                            DataIndex="Type">
                                            <Items>
                                                <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                                <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                            </Items>
                                        </ext:ComboBox>
                                    </Component>
                                </ext:ComponentColumn>
                            </Columns>
                        </ColumnModel>

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
                            <ext:Toolbar
                                ID="Toolbar1"
                                runat="server">
                                <Items>
                                    <ext:Button
                                        ID="UpdateAddressBookButton"
                                        Text="Update Edited Contacts"
                                        Icon="Add"
                                        runat="server"
                                        Margins="5 15 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="AddressBookUpdateContacts">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{AddressBookGrid}.getRowsValues(true))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button
                                        ID="Button1"
                                        Text="Delete Selected"
                                        Icon="Delete"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AddressBookDeleteContacts">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{AddressBookGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
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
                        Layout="FitLayout"
                        Title="Import"
                        Height="700">
                        <Store>
                            <ext:Store
                                ID="ImportContactsStore"
                                runat="server"
                                IsPagingStore="true"
                                PageSize="24"
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
                            </Columns>
                        </ColumnModel>
                         <SelectionModel>
                            <ext:CellSelectionModel ID="CheckboxSelectionModel2"
                                runat="server"
                                Mode="Single"
                                AllowDeselect="true"
                                IgnoreRightMouseSelection="true"
                                CheckOnly="true">
                            </ext:CellSelectionModel>
                        </SelectionModel>
                        <TopBar>
                            <ext:Toolbar
                                ID="ImportContactsToolbar"
                                runat="server">
                                <Items>
                                    <ext:Button
                                        ID="ImportItems"
                                        Text="Import To Address Book"
                                        Icon="Add"
                                        runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="ImportContactsFromHistory">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ImportContactsGrid}.getRowsValues(true))" Mode="Raw" />
                                                </ExtraParams>
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
