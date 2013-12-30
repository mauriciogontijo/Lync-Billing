<%@ Page Title="tBill Depart. Head | Users Phonecalls" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="phonecalls.aspx.cs" Inherits="Lync_Billing.ui.dephead.users.phonecalls" %>

<%-- DEPARTMENT-HEAD # USERS # PHONECALLS --%>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <style>
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
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
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

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
                Layout="AnchorLayout"
                ComponentCls="fix-ui-vertical-align">

                <TopBar>
                    <ext:Toolbar
                        ID="FilterToolbar1"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterSitesComboBox"
                                runat="server"
                                Icon="Find"
                                Editable="true"
                                DisplayField="SiteName"
                                ValueField="SiteID"
                                FieldLabel="Site:"
                                LabelWidth="25"
                                Width="150"
                                Margins="5 20 0 5">
                                <Store>
                                    <ext:Store ID="FilterSitesComboBoxStore" runat="server" OnLoad="FilterSitesComboBoxStore_Load">
                                        <Model>
                                            <ext:Model ID="SitesModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SiteID" />
                                                    <ext:ModelField Name="SiteName" />
                                                    <ext:ModelField Name="CountryCode" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <DirectEvents>
                                    <Select OnEvent="FilterDepartmentsBySite_Selected" />
                                </DirectEvents>

                                <ListConfig
                                    Border="true"
                                    MinWidth="250"
                                    MinHeight="300">
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div>{SiteName}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                            </ext:ComboBox>

                            <ext:ComboBox
                                ID="FilterDepartments"
                                runat="server"
                                Icon="Find"
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="DepartmentName"
                                ValueField="ID"
                                FieldLabel="Department:"
                                LabelWidth="60"  
                                Width="220"
                                Margins="5 20 5 5"
                                Disabled="true">
                                <Store>
                                    <ext:Store 
                                        ID="DepartmentsFilterStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model 
                                                ID="DepartmentHeadsStoreModel"
                                                runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="ID" />
                                                        <ext:ModelField Name="SiteID" />
                                                        <ext:ModelField Name="SiteName" />
                                                        <ext:ModelField Name="DepartmentID" />
                                                        <ext:ModelField Name="DepartmentName" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <ListConfig
                                    MinWidth="280"
                                    MinHeight="300"
                                    Border="true"
                                    EmptyText="Please select a department...">
                                    <ItemTpl ID="ItemTpl2" runat="server">
                                        <Html>
                                            <div>{DepartmentName}&nbsp;({SiteName})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetUsersPerDepartment">
                                        <EventMask ShowMask="true" />
                                    </Select>
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
                                Width="300"
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
                                <ListConfig
                                    LoadingText="Searching..."
                                    MinWidth="430"
                                    MaxWidth="450"
                                    Border="true"
                                    EmptyText="Please select user..."
                                    Frame="true">
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
                Height="680"
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
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_CallType" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SessionIdTime" Direction="ASC" />
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
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="150"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country"
                            Width="100"
                            DataIndex="Marker_CallToCountry" />

                        <ext:Column
                            ID="Column1"
                            runat="server"
                            Text="Destination Number"
                            Width="160"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="100"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="100"
                            DataIndex="Marker_CallCost">
                            <Renderer Fn="RoundCostsToTwoDecimalDigits" />
                        </ext:Column>

                        <ext:Column ID="UI_CallType"
                            runat="server"
                            Text="Type"
                            Width="80"
                            DataIndex="UI_CallType">
                            <Renderer Fn="getCssColorForPhoneCallRow" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <%--<TopBar>
                    <ext:Toolbar
                        ID="Toolbar1"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterPhoneCallsByType"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="TypeName"
                                ValueField="TypeValue"
                                FieldLabel="View Calls:"
                                LabelWidth="60"
                                Width="200"
                                Margins="5 5 5 5"
                                Disabled="true"
                                ReadOnly="true">
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
                                    <Select OnEvent="PhoneCallsHistoryFilter" />
                                </DirectEvents>
                            </ext:ComboBox>
                        </Items>
                    </ext:Toolbar>
                </TopBar>--%>

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
