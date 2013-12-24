<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dids.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.manage.dids" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            
            <ext:GridPanel
                ID="ManageDIDsGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Delegates"
                ComponentCls="fix-ui-vertical-align">

                <Store>
                    <ext:Store
                        ID="ManageDIDsGridStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25"
                        OnLoad="ManageDIDsGridStore_Load">
                        <Model>
                            <ext:Model ID="ManageDIDsGridStoreModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="ID" Type="Int" />
                                    <ext:ModelField Name="DIDString" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:RowEditing ID="ManageDIDs_RowEditing" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ManageDIDsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="DIDStringColumn"
                            runat="server"
                            Text="DID Name"
                            Width="380"
                            DataIndex="DIDString"
                            Sortable="false"
                            Groupable="false">

                            <HeaderItems>
                                <ext:TextField ID="DIDStringFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDIDFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Editor>
                                <ext:TextField
                                    ID="Editor_DIDStringTextField"
                                    runat="server"
                                    DataIndex="DIDString" />
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="DescriptionColumn"
                            runat="server"
                            Text="Description"
                            Width="280"
                            DataIndex="Description"
                            Sortable="false"
                            Groupable="false">

                            <HeaderItems>
                                <ext:TextField ID="DescriptionFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDescFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Editor>
                                <ext:TextField
                                    ID="Editor_DescriptionTextField"
                                    runat="server"
                                    DataIndex="Description" />
                            </Editor>
                        </ext:Column>

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

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesDIDsToolBar" runat="server">
                        <Items>
                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New DID"
                                Icon="Add"
                                Margins="5 5 0 5">
                                <%--<DirectEvents>
                                    <Click OnEvent="ShowAddNewDIDWindowPanel" />
                                </DirectEvents>--%>
                            </ext:Button>

                            <ext:ToolbarSeparator
                                ID="ToolbarSeparaator"
                                runat="server" />

                            <ext:Button
                                ID="SaveChangesButton"
                                runat="server"
                                Text="Save Changes"
                                Icon="DatabaseSave"
                                Margins="5 5 0 5">
                                <%--<DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageDIDsGridStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDIDsGridStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>--%>
                            </ext:Button>

                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDIDsPagingToolbar"
                        runat="server"
                        StoreID="ManageDIDsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="DIDs {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
