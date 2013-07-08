<%@ Page Title="eBill Admin | Unmarekd Calls Notifications" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="calls.aspx.cs" Inherits="Lync_Billing.ui.admin.notifications.calls" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='email-unmarked-calls-alert' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="UnmarkedCallsGrid"
                Header="true"
                Title="Users With Unmarked Calls"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">
                <Store>
                    <ext:Store
                        ID="UnmarkedCallsStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="UnmarkedCallsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="EmployeeID" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="Site" Type="String" />
                                    <ext:ModelField Name="UnmarkedCallsCost" Type="Float" />
                                    <ext:ModelField Name="UnmarkedCallsCount" Type="Int" />
                                    <ext:ModelField Name="UnmarkedCallsDuration" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="UnmarkedCallsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="UnmarkedCalls_ColumnRowNumber" runat="server" Width="25" />

                        <ext:Column
                            ID="EmployeeIDCol"
                            runat="server"
                            Text="Employee ID"
                            Width="80"
                            DataIndex="EmployeeID" />

                        <ext:Column
                            ID="SipAccountCol"
                            runat="server"
                            Text="Sip Account"
                            Width="160"
                            DataIndex="SipAccount" />

                        <ext:Column
                            ID="FullNameCol"
                            runat="server"
                            Text="Full Name"
                            Width="180"
                            DataIndex="FullName" />

                        <ext:Column
                            ID="UnmarkedCallsDurationCol"
                            runat="server"
                            Text="Duration"
                            Width="110"
                            DataIndex="UnmarkedCallsDuration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="UnmarkedCallsCountCol"
                            runat="server"
                            Text="Count"
                            Width="70"
                            DataIndex="UnmarkedCallsCount" />

                        <ext:Column
                            ID="UnmarkedCallsCostCol"
                            runat="server"
                            Text="Cost"
                            Width="70"
                            DataIndex="UnmarkedCallsCost" />
                    </Columns>
                </ColumnModel>
                
                <TopBar>
                    <ext:Toolbar ID="UnamrkedCalls_FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="SiteName" 
                                ValueField="SiteName"
                                Width="250"
                                Margins="5 390 0 5"
                                FieldLabel="Choose Site"
                                LabelSeparator=":"
                                LabelWidth="70">
                                <Store>
                                    <ext:Store
                                        ID="SitesStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="SitesModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SiteID" />
                                                    <ext:ModelField Name="SiteName" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>            
                                    </ext:Store>
                                </Store>

                                <DirectEvents>
                                    <Change OnEvent="GetUnmarkedCallsForSite" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button 
                                ID="UnamrkedCalls_EmailAlertButton" 
                                runat="server" 
                                Text="Email Alert" 
                                Icon="EmailAdd" 
                                Margins="5 5 0 0">
                               <DirectEvents>
                                   <Click OnEvent="NotifyUsers">
                                       <ExtraParams>
                                           <ext:Parameter Name="Values" Value="Ext.encode(#{UnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw"/>
                                       </ExtraParams>
                                   </Click>
                                   
                               </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <Features>
                    <ext:GridFilters ID="UnmarkedCallsGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="EmployeeID" />
                            <ext:StringFilter DataIndex="SipAccount" />
                            <ext:StringFilter DataIndex="FullName" />
                            <ext:StringFilter DataIndex="Site" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="UnmarkedCallsCheckboxSelectionModel"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingBottomBar"
                        runat="server"
                        StoreID="UnmarkedCallsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="USers Bills {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
