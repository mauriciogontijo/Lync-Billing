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
                            <p><a href='../user/manage_phone_calls.aspx'>Phone Calls</a></p>
                            <p><a href="../user/manage_address_book.aspx" class="selected">Address Book</a></p>

                            <%
                                bool condition = ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDelegate || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
                                if (condition) {
                            %>
                                <p><a href='../user/manage_delegates.aspx'>Delegated Users</a></p>
                            <% } %>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>History</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/view_history.aspx'>View Calls History</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Statistics</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../user/view_statistics.aspx'>View Calls Statistics</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
            
            <div class="clear h20"></div>

            <% 
                bool condition = false; //((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsAccountant || ((Lync_Billing.DB.UserSession)Session.Contents["UserData"]).IsDeveloper;
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
                                
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing2" runat="server" ClicksToMoveEditor="1" AutoCancel="false" />
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
                                            runat="server"
                                            Text="Contact Name"
                                            Width="240"
                                            DataIndex="Name">
                                              <Editor>
                                                <ext:TextField ID="ContactName" 
                                                    runat="server"
                                                    EmptyText="Example: John Smith" 
                                                    DataIndex="Name" />
                                                </Editor>
                                             </ext:Column>

                                        <ext:Column
                                            ID="ContactType"
                                            runat="server"
                                            Text="Contact Type"
                                            Width="130"
                                            DataIndex="Type">
                                            <Editor>
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
                                            </Editor>
                                        </ext:Column>
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
                                 <Plugins>
                                    <ext:RowEditing ID="RowEditing1" runat="server" ClicksToMoveEditor="1" AutoCancel="false" />
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
                                            runat="server" 
                                            DataIndex="Name"
                                            Width="240"
                                            Text="Contact Name">
                                            <Editor>
                                                <ext:TextField 
                                                    runat="server"
                                                    EmptyText="Example: John Smith" 
                                                    DataIndex="Name" />
                                                </Editor>
                                        </ext:Column>
                                        
                                        <ext:Column
                                            runat="server" 
                                            DataIndex="Type" 
                                            Text="Contact Type"
                                            Width="130">
                                            <Editor>
                                                <ext:ComboBox
                                                    runat="server"
                                                    EmptyText="Please Select Type"
                                                    DataIndex="Type"
                                                    Width="110">
                                                    <Items>
                                                        <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                                        <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
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
