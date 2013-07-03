<%@ Page Title="eBill Admin | Manage Application Configuration" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="configuration.aspx.cs" Inherits="Lync_Billing.ui.admin.main.configuration" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Manage Application Configuration</title>
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='application-configuration' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel 
                ID="AppConfigGrid"
                runat="server" 
                Title="Manage Application Configuration"
                PaddingSummary="5px 5px 0"
                Width="740"
                Height="740"
                ButtonAlign="Center">
                <Store>
                    <ext:Store
                        ID="AppConfigStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25"
                        OnReadData="AppConfigStore_ReadData"
                        OnLoad="AppConfigStore_Load">
                        <Model>
                            <ext:Model ID="AppConfigStoreModel" runat="server">
                                <Fields>
                                    <ext:ModelField Name="module" ServerMapping="Module" Type="String" />
                                    <ext:ModelField Name="modulekey" ServerMapping="ModuleKey" Type="String" />
                                    <ext:ModelField Name="modulevalue" ServerMapping="Modulevalue" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel
                    ID="AppConfigGridColumnModel"
                    runat="server">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:ComponentColumn
                            ID="ModuleNameCol"
                            runat="server"
                            Text="Module Name"
                            Width="180"
                            Editor="true"
                            OverOnly="true"
                            DataIndex="Module"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="ModuleNameTextbox"
                                    runat="server"
                                    DataIndex="Module" />
                            </Component>
                        </ext:ComponentColumn>

                        <ext:ComponentColumn
                            ID="ModuleKeyCol"
                            runat="server"
                            DataIndex="ModuleKey"
                            Text="Module Key"
                            Width="220"
                            Editor="true"
                            OverOnly="true"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="ModuleKeyTextbox"
                                    runat="server"
                                    DataIndex="ModuleKey" />
                            </Component>
                        </ext:ComponentColumn>

                        <ext:ComponentColumn
                            ID="ModuleValueCol"
                            runat="server"
                            Text="Module Value"
                            Width="240"
                            Editor="true"
                            OverOnly="true"
                            DataIndex="ModuleValue"
                            PinEvents="expand"
                            UnpinEvents="collapse">
                            <Component>
                                <ext:TextField
                                    ID="ModuleValueTextbox"
                                    runat="server"
                                    DataIndex="ModuleValue" />
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
                                <Command Handler="this.up('AppConfigGrid').store.removeAt(recordIndex);" />
                            </Listeners>
                        </ext:ImageCommandColumn>
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

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PhoneCallStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Phone Calls {0} - {1} of {2}" />
                </BottomBar>

                <TopBar>
                    <ext:Toolbar
                        ID="AppConfigGridToolbar"
                        runat="server">
                        <Items>
                            <ext:Button
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save Changes"
                                Icon="ApplicationEdit"
                                Margins="5 15 0 5">
                                <DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{AppConfigGrid}.getRowsValues(true))" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelChangesButton"
                                Text="Cancel Changes"
                                Icon="Cancel"
                                runat="server"
                                Margins="5 410 0 0">
                                <DirectEvents>
                                    <Click OnEvent="RejectChanges_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="Add Record"
                                Icon="Add">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>