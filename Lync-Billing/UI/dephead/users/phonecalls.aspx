<%@ Page Title="eBill Depart. Head | Users Phonecalls" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="phonecalls.aspx.cs" Inherits="Lync_Billing.ui.dephead.users.phonecalls" %>

<%-- DEPARTMENT-HEAD # USERS # PHONECALLS --%>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <style>
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        .row-green { background-color: rgb(46, 143, 42); }
        .row-red { background-color: rgb(201, 20, 20); }
        .row-yellow { background-color: yellow; }
        /* end manage-phone-calls grid styling */

        /* start users search query result styling */
        .search-item {
            font          : normal 11px tahoma, arial, helvetica, sans-serif;
            padding       : 3px 10px 3px 10px;
            border        : 1px solid #fff;
            border-bottom : 1px solid #eeeeee;
            white-space   : normal;
            color         : #555;
        }
        
        .search-item h3 {
            display     : block;
            font        : inherit;
            font-weight : bold;
            color       : #222;
            margin      : 0px;
        }

        .search-item h3 span {
            float       : right;
            font-weight : normal;
            margin      : 0 0 5px 5px;
            width       : 175px;
            display     : block;
            clear       : none;
        } 
        
        p { width: 650px; }
        
        .ext-ie .x-form-text { position : static !important; }
        /* end users search query result styling */
    </style>
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='users-phonecalls' class='block float-right wauto h100p'>
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
                                ID="FilterDepartments"
                                runat="server"
                                Icon="Find"
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="DepartmentName"
                                ValueField="DepartmentName"
                                FieldLabel="Department:"
                                LabelWidth="60"
                                Width="250"
                                Margins="5 15 5 5">
                                <Store>
                                    <ext:Store 
                                        ID="DepartmentsFilterStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model 
                                                ID="DepartmentHeadsStoreModel"
                                                runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="DepartmentName" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Change OnEvent="GetUsersPerDepartment">
                                        <EventMask ShowMask="true" />
                                    </Change>
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:ComboBox
                                ID="FilterUsersByDepartment"
                                runat="server"
                                Icon="Find"
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="SipAccount"
                                ValueField="SipAccount"
                                FieldLabel="User:"
                                LabelWidth="30"
                                Width="450"
                                Disabled="true">
                                <Store>
                                    <ext:Store 
                                        ID="FilterUsersByDepartmentStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model 
                                                ID="FilterUsersByDepartmentModel"
                                                runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="EmployeeID" />
                                                    <ext:ModelField Name="SipAccount" />
                                                    <ext:ModelField Name="SiteName" />
                                                    <ext:ModelField Name="FullName" />
                                                    <ext:ModelField Name="Department" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ListConfig  LoadingText="Searching...">
                                    <ItemTpl ID="ItemTpl1" runat="server">
                                        <Html>
                                            <div class="search-item">
							                    <h3><span>{SipAccount}</span>{FullName}</h3>
						                    </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                                <DirectEvents>
                                    <Select OnEvent="GetPhoneCallsForUser">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>

            <div class="h5 clear"></div>

            <ext:GridPanel
                ID="ViewPhoneCallsGrid"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="TableLayout">
                <Store>
                    <ext:Store
                        ID="PhoneCallsStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="SourceUserUri" Type="String" />
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
                        <ext:RowNumbererColumn
                            ID="RowNumbererColumn2"
                            runat="server"
                            Width="45" />

                        <ext:Column
                            ID="SipAccountCol"
                            runat="server"
                            Text="User Email"
                            Width="140"
                            DataIndex="SourceUserUri">
                        </ext:Column>

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
                            Text="Country"
                            Width="90"
                            DataIndex="Marker_CallToCountry" />

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
                            <Renderer Fn="RoundCost" />
                        </ext:Column>

                        <ext:Column ID="UI_CallType"
                            runat="server"
                            Text="Type"
                            Width="80"
                            DataIndex="UI_CallType">
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
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
                                <%--<DirectEvents>
                                    <Select OnEvent="PhoneCallsHistoryFilter" />
                                </DirectEvents>--%>
                            </ext:ComboBox>
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
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
