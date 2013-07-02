<%@ Page Title="eBill Admin | Manage Application Configuration" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="configuration.aspx.cs" Inherits="Lync_Billing.ui.admin.main.configuration" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
    <title>eBill Admin | Manage Application Configuration</title>

     <ext:XScript ID="XScript1" runat="server">
        <script>
            var addRecord = function () {
                var grid = #{AppConfigGrid};
                grid.editingPlugin.cancelEdit();

                // Create a record instance through the ModelManager
                var r = Ext.ModelManager.create({
                    module: 'New Module',
                    modulekey: 'New Module Key',
                    modulevalue: 'New Module Value',
                }, 'Record');

                grid.store.insert(0, r);
                grid.editingPlugin.startEdit(0, 0);
            };
            
            //var removeRecord = function () {
            //    var grid = #{AppConfigGrid},
            //        sm = grid.getSelectionModel();

            //    grid.editingPlugin.cancelEdit();
            //    grid.store.remove(sm.getSelection());
            //    if (grid.store.getCount() > 0) {
            //        sm.select(0);
            //    }
            //};
        </script>
    </ext:XScript>
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
                                    <ext:ModelField Name="ID" Type="Int" />
                                    <ext:ModelField Name="Module" Type="String" />
                                    <ext:ModelField Name="ModuleKey" Type="String" />
                                    <ext:ModelField Name="ModuleValue" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:RowEditing
                        ID="RowEditing1"
                        runat="server"
                        ClicksToMoveEditor="1"
                        AutoCancel="false" />
                </Plugins> 

                <ColumnModel
                    ID="AppConfigGridColumnModel"
                    runat="server">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:Column
                            ID="ModuleNameCol"
                            runat="server"
                            DataIndex="Module"
                            Width="200"
                            Text="Module Name"
                            Align="Left"
                            Flex="1">
                            <Editor>
                                <ext:TextField
                                    ID="ModuleNameTextbox"
                                    runat="server"
                                    AllowBlank="false" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="ModuleKeyCol"
                            runat="server"
                            DataIndex="ModuleKey"
                            Width="200"
                            Text="Module Key"
                            Align="Left">
                            <Editor>
                                <ext:TextField
                                    ID="ModuleKeyTextbox"
                                    runat="server"
                                    AllowBlank="false" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="ModuleValueCol"
                            runat="server"
                            DataIndex="ModuleValue"
                            Width="200"
                            Text="Module Value"
                            Align="Left">
                            <Editor>
                                <ext:TextField
                                    ID="ModuleValueTextbox"
                                    runat="server"
                                    AllowBlank="false" />
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
                            <ext:Label
                                ID="ButtonGroupLable"
                                runat="server"
                                Margins="5 5 5 5">
                                <Content>Selected:</Content>
                            </ext:Label>

                            <ext:ButtonGroup
                                ID="AddEditDeleteButtonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="300"
                                Frame="false"
                                ButtonAlign="Left"
                                Margins="0 0 0 0">
                                <Buttons>
                                    <ext:Button
                                        ID="AddRecordButton"
                                        runat="server"
                                        Text="Add Record"
                                        Icon="Add">
                                        <Listeners>
                                            <Click Fn="addRecord" />
                                        </Listeners>
                                    </ext:Button>

                                    <ext:Button
                                        ID="DeleteSelectedButton"
                                        runat="server"
                                        Text="Delete"
                                        Icon="Delete">
                                        <%--<DirectEvents>
                                            <Click OnEvent="DeleteSelected_DirectEvent">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter
                                                        Name="Values"
                                                        Value="Ext.encode(#{AppConfigGrid}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>--%>
                                        <Listeners>
                                            <Click Handler="App.direct.DoConfirm()" />
                                        </Listeners>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>


<%--<ext:ComponentColumn
    ID="ComponentColumn1"
    runat="server"
    Text="Module Name"
    Width="240"
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
    ID="ComponentColumn2"
    runat="server"
    Text="Module Key"
    Width="240"
    Editor="true"
    OverOnly="true"
    DataIndex="ModuleKey"
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
    ID="ComponentColumn3"
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
</ext:ComponentColumn>--%>