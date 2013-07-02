<%@ Page Title="eBill Admin | Manage Delegates" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="delegates.aspx.cs" Inherits="Lync_Billing.ui.admin.users.delegates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Manage Delegates</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div id='generate-report-block' class='block float-right wauto h100p'>

        <div class="block-body pt5">

            <ext:GridPanel
                ID="ManageDelegatesGrids"
                runat="server"
                Width="740"
                Height="720"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Delegates">

                <Store>
                    <ext:Store
                        ID="ManageDelegatesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageDelegatesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="Int" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="DelegeeAccount" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageDelegatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:ComponentColumn
                            ID="IDCol"
                            runat="server"
                            Text="ID"
                            Width="160"
                            DataIndex="ID"
                            Visible="false"
                            Editor="true"
                            OverOnly="true"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="IDTextbox"
                                    runat="server"
                                    DataIndex="ID" />
                            </Component>

                        </ext:ComponentColumn>

                        <ext:ComponentColumn
                            ID="SipAccountCol"
                            runat="server"
                            Text="User"
                            Width="160"
                            DataIndex="SipAccount"
                            Editor="true"
                            OverOnly="true"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="SipAccountTextbox"
                                    runat="server"
                                    DataIndex="SipAccount" />
                            </Component>
                        </ext:ComponentColumn>

                        <ext:ComponentColumn
                            ID="DelegeeAccountCol"
                            runat="server"
                            Text="Delegate"
                            Width="160"
                            DataIndex="DelegeeAccount"
                            Editor="true"
                            OverOnly="true"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="DelegeeAccountTextbox"
                                    runat="server"
                                    DataIndex="DelegeeAccount" />
                            </Component>
                        </ext:ComponentColumn>

                        <ext:ComponentColumn
                            ID="DescriptionCol"
                            runat="server"
                            Text="Description"
                            Width="330"
                            DataIndex="Description"
                            Editor="true"
                            OverOnly="true"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="DescriptionTextbox"
                                    runat="server"
                                    DataIndex="Description" />
                            </Component>
                        </ext:ComponentColumn>

                        <ext:ImageCommandColumn
                            ID="DeleteRecordColumn"
                            runat="server"
                            Width="30"
                            Sortable="false">
                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Record" CommandName="delete" />
                            </Commands>

                            <Listeners>
                                <Command Handler="this.up('ManageDelegatesGrids').store.removeAt(recordIndex);" />
                            </Listeners>
                        </ext:ImageCommandColumn>
                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterDelegatesBySite"
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
                                        ID="DelegatesSitesStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="DelegatesSitesModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SiteID" />
                                                    <ext:ModelField Name="SiteName" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Change OnEvent="GetDelegates" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save Changes"
                                Icon="ApplicationEdit">
                                <DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent" before="return #{ManageDelegatesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDelegatesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="ManageDelegatesCheckboxSelectionModel"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDelegatesPagingToolbar"
                        runat="server"
                        StoreID="ManageDelegatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Delegates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
