<%@ Page Title="eBill Admin | Manage Application Translations" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="translations.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.main.translations" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='application-configuration' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel 
                ID="AppConfigGrid"
                runat="server" 
                Title="Manage Application Translations"
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
                        OnReadData="AppConfigStore_ReadData">
                        <Model>
                            <ext:Model ID="AppConfigStoreModel" runat="server">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="String" />
                                    <ext:ModelField Name="Module" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="ModuleKey" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="Modulevalue" Type="String" SortType="AsText" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>
                
                <ColumnModel
                    ID="AppConfigGridColumnModel"
                    runat="server">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:Column
                            ID="ModuleNameCol"
                            runat="server"
                            Text="Module Name"
                            Width="180"
                            DataIndex="Module"
                            Sortable="true">
                            <Editor>
                                <ext:TextField
                                    ID="ModuleNameTextbox"
                                    runat="server"
                                    DataIndex="Module" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="ModuleKeyCol"
                            runat="server"
                            DataIndex="ModuleKey"
                            Text="Module Key"
                            Width="220"
                            Sortable="true">
                            <Editor>
                                <ext:TextField
                                    ID="ModuleKeyTextbox"
                                    runat="server"
                                    DataIndex="ModuleKey" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="ModuleValueCol"
                            runat="server"
                            Text="Module Value"
                            Width="240"
                            Sortable="true">
                            <Editor>
                                <ext:TextField
                                    ID="ModuleValueTextbox"
                                    runat="server"
                                    DataIndex="ModuleValue" />
                            </Editor>
                        </ext:Column>

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
                        DisplayMsg="Cofiguration Records {0} - {1} of {2}" />
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
                                            <ext:Parameter Name="Values" Value="#{AppConfigGrid}.getChangedData()" Mode="Raw" />
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
                                <DirectEvents>
                                    <Click OnEvent="ShowAddDefinitionPanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:Window 
                                ID="AddNewDefinitionWindowPanel" 
                                runat="server" 
                                Icon="Add" 
                                Title="Add New Definition" 
                                Hidden="true"
                                Width="450"
                                Height="200"
                                Frame="true"
                                X="200"
                                Y="200">
                                <Defaults>
                                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                                </Defaults>
                                <Items>
                                    <ext:TextField
                                        ID="NewModuleName"
                                        runat="server"
                                        AllowBlank="false"
                                        Width="420"
                                        EmptyText="Empty Module Name"
                                        FieldLabel="Module Name:"
                                        LabelWidth="90">
                                    </ext:TextField>
                                    
                                    <ext:TextField
                                        ID="NewModuleKey"
                                        runat="server"
                                        AllowBlank="false"
                                        Width="420"
                                        EmptyText="Empty Module Key"
                                        FieldLabel="Module Key:"
                                        LabelWidth="90">
                                    </ext:TextField>

                                    <ext:TextField
                                        ID="NewModuleValue"
                                        runat="server"
                                        AllowBlank="false"
                                        Width="420"
                                        EmptyText="Empty Module Value"
                                        FieldLabel="Module Value:"
                                        LabelWidth="90">
                                    </ext:TextField>

                                    <ext:Button
                                        ID="AddNewDefinitionButton"
                                        runat="server"
                                        Text="Add"
                                        Icon="ApplicationFormAdd"
                                        Flat="true">
                                    </ext:Button>

                                    <ext:Button
                                        ID="CancelNewDefinitionButton"
                                        runat="server"
                                        Text="Cancel"
                                        Icon="Cancel"
                                        Flat="true">
                                    </ext:Button>
                                </Items>
                            </ext:Window>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>