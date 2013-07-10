<%@ Page Title="eBill Admin | Users Bills Notifications" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="bills.aspx.cs" Inherits="Lync_Billing.ui.admin.notifications.bills" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>
                         
            var applyFilter = function (field) {                
                var store = #{UsersBillsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{FullNameFilter}.reset();
                
                #{UsersBillsStore}.clearFilter();
            }
 
            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if (typeof val != "string") {
                    return value.length == 0;
                }
                
                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            };
 
            var getRecordFilter = function () {
                var f = [];
 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{FullNameFilter}.getValue(), "FullName", record);
                    }
                });
 
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
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='email-notifications' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="UsersBillsGrid" 
                runat="server" 
                Title="Users Bills Notifications"
                Width="740"
                Height="765"  
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
                        IsPagingStore="true"
                        PageSize="25">
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

                <Features>
                    <ext:GridFilters ID="UsersBillsGridFilters" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="FullName" />
                        </Filters>
                    </ext:GridFilters>
                </Features>

                <ColumnModel ID="BillsColumnModel" runat="server">
		            <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="21" />

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
                            Align="Left">
                            <HeaderItems>
                                <ext:TextField ID="FullNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearFullNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column ID="MonthDate" 
                            runat="server" 
                            Text="Date" 
                            Width="160" 
                            DataIndex="MonthDate"
                            Groupable="false"
                            Align="Center">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column ID="CallsInformationColum"
                            runat="server"
                            Text="Calls Information"
                            MenuDisabled="true"
                            Sortable="false"
                            Resizable="false">
                            <Columns>
                                <ext:Column ID="TotalDuration"
                                    runat="server"
                                    Text="Duration"
                                    Width="120"
                                    DataIndex="PersonalCallsDuration"
                                    Groupable="false"
                                    Align="Left">
                                    <Renderer Fn="GetMinutes" />
                                </ext:Column>

                                <ext:Column ID="TotalCalls"
                                    runat="server"
                                    Text="Number of Calls"
                                    Width="100"
                                    DataIndex="PersonalCallsCount"
                                    Groupable="false" 
                                    Align="Left"/>
                                
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
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:DateField 
                                ID="BillDateField"
                                runat="server" 
                                FieldLabel="Choose Date:"
                                LabelWidth="70"
                                EmptyText="Empty Date"
                                Width="220"
                                Margins="5 15 0 5"
                                Editable="false">
                                <DirectEvents>
                                    <Select OnEvent="BillDateField_Selection" />
                                </DirectEvents>
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
                                Margins="5 15 0 5"
                                Disabled="true">
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

                            <ext:Button 
                                ID="EmailAlertButton" 
                                runat="server" 
                                Text="Email Alert" 
                                Icon="EmailAdd" 
                                Margins="5 5 0 160">
                                <DirectEvents>
                                    <Click OnEvent="NotifyUsers">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{UsersBillsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw"/>
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
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

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingBottomBar"
                        runat="server"
                        StoreID="UsersBillsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
