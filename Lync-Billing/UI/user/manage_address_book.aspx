<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="manage_address_book.aspx.cs" Inherits="Lync_Billing.UI.user.manage_address_book" %>

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
        
        var submitValue = function (grid) {
            grid.submitData(false, { isUpload: true });
        };

        var pinEditors = function (btn, pressed) {
            var columnConfig = btn.column,
                column = columnConfig.column;

            if (pressed) {
                column.pinOverComponent();
                column.showComponent(columnConfig.record, true);
            } else {
                column.unpinOverComponent();
                column.hideComponent(true);
            }
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="UserToolsSidebar"
                runat="server"
                Height="330"
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
                            <p><a href='/UI/user/manage_phone_calls.aspx'>My Phone Calls</a></p>
                            <p><a href="/UI/user/manage_address_book.aspx" class="selected">My Address Book</a></p>

                            <%
                                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                if (condition) {
                            %>
                                <p><a href='/UI/user/manage_delegates.aspx'>My Delegated Users</a></p>
                            <% } %>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/user/view_history.aspx'>Phone Calls History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='/UI/user/view_statistics.aspx'>Phone Calls Statistics</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
            
            <div class="clear h20"></div>

            <% 
                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAccountant || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                if ( condition )
                {
            %>
                <ext:Panel ID="AccountingToolsSidebar"
                    runat="server"
                    Height="330"
                    Width="180"
                    Title="Accounting Tools"
                    Collapsed="true"
                    Collapsible="true">
                    <Content>
                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Disputes</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='/UI/accounting/manage_disputes.aspx'>Manage Disputed Calls</a></p>
                            </div>
                        </div>

                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Generate User Reports</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='/UI/accounting/monthly_user_reports.aspx'>Monthly Users Reports</a></p>
                                <p><a href='/UI/accounting/periodical_user_reports.aspx'>Periodical Users Reports</a></p>
                            </div>
                        </div>

                        <div class='sidebar-section'>
                            <div class="sidebar-section-header">
                                <p>Generate Site Reports</p>
                            </div>
                            <div class="sidebar-section-body">
                                <p><a href='/UI/accounting/monthly_site_reports.aspx'>Monthly Sites Reports</a></p>
                                <p><a href='/UI/accounting/periodical_site_reports.aspx'>Periodical Sites Reports</a></p>
                            </div>
                        </div>
                    </Content>
                </ext:Panel>
            <% } %>
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
                    <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                    <ext:Parameter Name="autoScroll" Value="true" Mode="Raw" />
                </Defaults>
                <Items>
                    <ext:Panel ID="AddressBook"
                        runat="server"
                        Title="Address Book"
                        AutoDataBind="true">
                        <Items>
                            <ext:GridPanel
                                ID="AddressBookGrid"
                                runat="server"
                                Header="false"
                                Layout="FitLayout"
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
                                                    <ext:ModelField Name="SipAccount" Type="String"/>
                                                    <ext:ModelField Name="DestinationNumber" Type="String" />
                                                    <ext:ModelField Name="DestinationCountry" Type="String"/>
                                                    <ext:ModelField Name="Name" Type="String"/>
                                                    <ext:ModelField Name="Type" Type="String"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <ColumnModel ID="AddressBookColumnModel" runat="server" Flex="1">
                                    <Columns>
                                        <ext:Column
                                            ID="DestNumber"
                                            runat="server"
                                            Text="Number"
                                            Width="150"
                                            DataIndex="DestinationNumber" />

                                        <ext:Column
                                            ID="DestCountry"
                                            runat="server"
                                            Text="Country"
                                            Width="100"
                                            DataIndex="DestinationCountry" />

                                        <ext:Column
                                            ID="ContactName"
                                            runat="server"
                                            Text="Contact Name"
                                            Width="260"
                                            DataIndex="Name" />

                                        <ext:Column
                                            ID="ContactType"
                                            runat="server"
                                            Text="Contact Type"
                                            Width="160"
                                            DataIndex="Type" />
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
                                                ID="Button1"
                                                Text="Delete Selected"
                                                Icon="Delete"
                                                runat="server">
                                                <DirectEvents>
                                                    <Click OnEvent="DeleteFromAddressBook">
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
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>


                    <ext:Panel ID="ImportContactsFromHistoryTab"
                        runat="server" 
                        Title="Import Contacts from History"  
                        AutoDataBind="true">
                        <Items>
                            <ext:GridPanel
                                ID="ImportContactsGrid"
                                runat="server"
                                Header="false"
                                Layout="FitLayout"
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
                                                    <ext:ModelField Name="ID" Type="Int"/>
                                                    <ext:ModelField Name="SipAccount" Type="String"/>
                                                    <ext:ModelField Name="DestinationNumber" Type="String" />
                                                    <ext:ModelField Name="DestinationCountry" Type="String"/>
                                                    <ext:ModelField Name="Name" Type="String"/>
                                                    <ext:ModelField Name="Type" Type="String"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <ColumnModel ID="ImportContactsColumnModel" runat="server" Flex="1">
                                    <Columns>
                                        <ext:Column
                                            ID="ImportedContactNumber"
                                            runat="server"
                                            Text="Number"
                                            Width="150"
                                            DataIndex="DestinationNumber" />

                                        <ext:Column
                                            ID="ImportedContactDestinationCountry"
                                            runat="server"
                                            Text="Country"
                                            Width="100"
                                            DataIndex="DestinationCountry" />

                                        <ext:ComponentColumn
                                            ID="ImportedContactName" 
                                            runat="server" 
                                            DataIndex="Name"
                                            Editor="true"
                                            Width="270"
                                            Flex="1"
                                            Text="Contact Name"
                                            Pin="true"
                                            OverOnly="true"
                                            MoveEditorOnTab="true">
                                            <Component>
                                                <ext:TextField 
                                                    runat="server"
                                                    EmptyText="Example: John Smith" 
                                                    DataIndex="Name" />
                                            </Component>
                                        </ext:ComponentColumn>

                                        <ext:ComponentColumn
                                            ID="ImpotedContactType"
                                            runat="server" 
                                            DataIndex="Type" 
                                            Editor="true"
                                            Flex="1"
                                            Text="Contact Type"
                                            Width="120"
                                            Pin="true"
                                            OverOnly="true">
                                            <Component>
                                                <ext:ComboBox
                                                    runat="server"
                                                    EmptyText="Please Select Type"
                                                    DataIndex="Type">
                                                    <Items>
                                                        <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                                        <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Component>
                                        </ext:ComponentColumn>

                                        <ext:ComponentColumn
                                            ID="ImportPhoneBookComponentColumn" 
                                            runat="server"
                                            Width="25"
                                            PinAllColumns="false"
                                            AutoWidthComponent="false"
                                            OverOnly="true">
                                            <Component>
                                                <ext:Button ID="EditPhoneBookButton" 
                                                    runat="server" 
                                                    ToolTip="Pin editors" 
                                                    Icon="Pencil" 
                                                    AllowDepress="true" 
                                                    EnableToggle="true"
                                                    FocusOnToFront="true">
                                                    <Listeners>
                                                        <Focus Fn="pinEditors" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Component>
                                    </ext:ComponentColumn>
                                    </Columns>
                                </ColumnModel>

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
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
        </div>
    </div>
    <!-- *** END OF ADDRESS BOOK PANEL *** -->
</asp:Content>
