<%@ Page Title="eBill Admin | Manage Rates" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="rates.aspx.cs" Inherits="Lync_Billing.ui.admin.gateways.rates" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div id='generate-report-block' class='block float-right wauto h100p'>

        <div class="block-body pt5">

            <ext:GridPanel
                ID="ManageRatesGrid"
                runat="server"
                Width="740"
                Height="740"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Gateways Rates">

                <Store>
                    <ext:Store
                        ID="ManageRatesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageRatesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="RateID" Type="Int" />
                                    <ext:ModelField Name="CountryCode" Type="String" />
                                    <ext:ModelField Name="FixedLineRate" Type="String" />
                                    <ext:ModelField Name="MobileLineRate" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageRatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                        <ext:Column
                            ID="RateIDCol"
                            runat="server"
                            Text="ID"
                            Width="160"
                            DataIndex="RateID"
                            Visible="false" />

                        <ext:Column
                            ID="CountryCodeCol"
                            runat="server"
                            Text="Country"
                            Width="200"
                            DataIndex="CountryCode">
                            <Editor>
                                <ext:ComboBox
                                    ID="CountryCodeComboBox"
                                    runat="server"
                                    DataIndex="CountryCode"
                                    EmptyText="Please Select Country"
                                    SelectOnFocus="true"
                                    SelectOnTab="true"
                                    Selectable="true"
                                    Width="160"
                                    AllowBlank="false"
                                    AllowOnlyWhitespace="false">
                                    <Store>
                                        <ext:Store
                                            ID="CountryCodeComboBoxStore"
                                            runat="server">
                                            <Model>
                                                <ext:Model ID="CountryCode" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="CountryID" />
                                                        <ext:ModelField Name="CountryCode" />
                                                        <ext:ModelField Name="CountryName" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>            
                                        </ext:Store>
                                    </Store>

                                    <%--<ListConfig>
                                        <ItemTpl ID="ItemTpl1" runat="server">
                                            <Html>
                                                <div data-qtip="{CountryCode}. {CountryName}">
                                                    {CountryName} ({CountryCode})
                                                </div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>--%>
                                </ext:ComboBox>
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="FixedlineRateCol"
                            runat="server"
                            Text="Fixedline Rate"
                            Width="200"
                            DataIndex="FixedLineRate">
                            <Editor>
                                <ext:TextField
                                    ID="FixedLineRateTextbox"
                                    runat="server"
                                    DataIndex="FixedLineRate" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="MobileLineRateCol"
                            runat="server"
                            Text="Mobile Rate"
                            Width="200"
                            DataIndex="MobileLineRate">
                            <Editor>
                                <ext:TextField
                                    ID="MobileLineRateTextbox"
                                    runat="server"
                                    DataIndex="MobileLineRate" />
                            </Editor>
                        </ext:Column>

                        <ext:ImageCommandColumn
                            ID="DeleteButtonsColumn"
                            runat="server"
                            Width="30"
                            Sortable="false"
                            Align="Center">
                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Delegate" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>
                            <Listeners>
                                <Command Handler="this.up(#{ManageRatesGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>

                         <ext:CommandColumn ID="RejectChange" runat="server" Width="70">
                            <Commands>
                                <ext:GridCommand Text="Reject" ToolTip-Text="Reject row changes" CommandName="reject" Icon="ArrowUndo" />
                            </Commands>
                            <PrepareToolbar Handler="toolbar.items.get(0).setVisible(record.dirty);" />
                            <Listeners>
                                <Command Handler="record.reject();" />
                            </Listeners>
                        </ext:CommandColumn>

                    </Columns>
                </ColumnModel>
                
                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterRatesByGateway"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="GatewayName"
                                ValueField="GatewayId"
                                FieldLabel="Choose Gateway"
                                LabelWidth="90"
                                Width="300"
                                Margins="5 200 0 5">
                                <Store>
                                    <ext:Store
                                        ID="GatewaysStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="GatewaysModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="GatewayId" />
                                                    <ext:ModelField Name="GatewayName" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Change OnEvent="GetGateways" />
                                </DirectEvents>
                            </ext:ComboBox>

                            <ext:Button
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save Changes"
                                Icon="ApplicationEdit"
                                Margins="5 10 0 5">
                                <DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent" before="return #{ManageRatesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageRatesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelChangesButton"
                                Text="Cancel Changes"
                                Icon="Cancel"
                                runat="server"
                                Margins="5 0 0 0">
                                <DirectEvents>
                                    <Click OnEvent="RejectChanges_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageRatesPagingToolbar"
                        runat="server"
                        StoreID="ManageRatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Delegates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
