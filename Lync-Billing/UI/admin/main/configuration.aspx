<%@ Page Title="eBill Admin " Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="configuration.aspx.cs" Inherits="Lync_Billing.ui.admin.main.configuration" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
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
                            Align="Left" />

                        <ext:Column
                            ID="ModuleKeyCol"
                            runat="server"
                            DataIndex="ModuleKey"
                            Width="200"
                            Text="Module Key"
                            Align="Left" />

                        <ext:Column
                            ID="ModuleValueCol"
                            runat="server"
                            DataIndex="ModuleValue"
                            Width="200"
                            Text="Module Value"
                            Align="Left" />
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
                                        ID="UpdateSelectedButton"
                                        Text="Update"
                                        runat="server"
                                        Icon="ApplicationEdit">
                                        <DirectEvents>
                                            <Click OnEvent="UpdateSelected_DirectEvent">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter
                                                        Name="Values"
                                                        Value="Ext.encode(#{AppConfigGrid}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
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
