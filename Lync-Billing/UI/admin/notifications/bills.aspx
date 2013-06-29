<%@ Page Title="eBill Admin | Users Bills Notifications" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="bills.aspx.cs" Inherits="Lync_Billing.ui.admin.notifications.bills" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Users Bills Notifications</title>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='email-notifications' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="UsersBillsGrid" 
                runat="server" 
                Title="Bills History"
                Width="740"
                Height="740"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="UsersBillsStore" 
                        runat="server" 
                        OnLoad="UsersBillsStore_Load"
                        OnReadData="UsersBillsStore_ReadData"
                        IsPagingStore="false">
                        <Model>
                            <ext:Model ID="UsersBillsModel" runat="server" IDProperty="BillsModel">
                                <Fields>
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="FullName" Type="String" />
                                    <ext:ModelField Name="EmployeeID" Type="String" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="MonthDate" Type="Date" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="Float" />
                                    <ext:ModelField Name="PersonalCallsCount" Type="Int" />
                                    <ext:ModelField Name="PersonalCallsDuration" Type="Int" />
                                </Fields>
                         </ext:Model>
                       </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="BillsColumnModel" runat="server">
		            <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <%--<ext:Column ID="UserSipAccount" 
                            runat="server" 
                            Text="User's Email" 
                            Width="180" 
                            DataIndex="SipAccount"
                            Groupable="false"
                            Align="Left" />--%>

                        <ext:Column ID="UserFullName" 
                            runat="server" 
                            Text="Employee" 
                            Width="180" 
                            DataIndex="FullName"
                            Groupable="false"
                            Align="Left" />

                        <ext:Column ID="MonthDate" 
                            runat="server" 
                            Text="Date" 
                            Width="160" 
                            DataIndex="MonthDate"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column ID="TotalCalls"
                            runat="server"
                            Text="Number of Calls"
                            Width="100"
                            DataIndex="PersonalCallsCount"
                            Groupable="false" 
                            Align="Left"/>
                        
                        <ext:Column ID="TotalDuration"
                            runat="server"
                            Text="Duration"
                            Width="120"
                            DataIndex="PersonalCallsDuration"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

		                <ext:Column ID="TotalCost"
                            runat="server"
                            Text="Total Cost"
                            Width="100"
                            DataIndex="PersonalCallsCost"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:DateField 
                                ID="reportDateField"
                                runat="server" 
                                FieldLabel="Choose Date:"
                                LabelWidth="70"
                                EmptyText="Empty Date"
                                Width="220"
                                Margins="5 15 5 5">
                            </ext:DateField>

                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="SiteName" 
                                ValueField="SiteName"
                                FieldLabel="Choose Site"
                                LabelWidth="65"
                                Margins="5 15 5 5">
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
                                    <Change OnEvent="GetUsersBillsForSite" />
                                </DirectEvents>
                            </ext:ComboBox>
                            
                            <%--<ext:Button
                                ID="ViewMonthlyBills"
                                runat="server"
                                Icon="UserMagnify"
                                Text="Get Bills"
                                Width="70"
                                Height="22"
                                OnDirectClick ="GetUsersBills_DirectClick"
                                Margins="5 5 5 5">
                            </ext:Button>--%>

                            <ext:Button ID="EmailAlertButton" runat="server" Text="Email Alert" Icon="EmailAdd" Margins="5 5 5 160">
                                 <Listeners>
                                    <Click Handler="submitValue(#{PhoneCallsHistoryGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
