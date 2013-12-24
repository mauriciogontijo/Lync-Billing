<%@ Page Title="tBill Admin | Manage DIDs" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="dids.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.manage.dids" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <ext:XScript ID="FiltersXScript" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ManageDIDsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{DIDPatternFilter}.reset();
                #{DescriptionFilter}.reset();
                
                #{ManageDIDsGridStore}.clearFilter();
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
                        return filterString(#{DIDPatternFilter}.getValue(), "DIDPattern", record);
                    }
                });
                
                f.push({
                    filter: function(record) {
                        return filterString(#{DescriptionFilter}.getValue(), "Description", record);
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
                Title="Manage DIDs"
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
                                    <ext:ModelField Name="DIDPattern" Type="String" />
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
                        <ext:Column ID="DIDPatternColumn"
                            runat="server"
                            Text="DID String"
                            Width="350"
                            DataIndex="DIDPattern"
                            Sortable="false"
                            Groupable="false">

                            <HeaderItems>
                                <ext:TextField ID="DIDPatternFilter" runat="server" Icon="Magnifier">
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
                                    ID="Editor_DIDPatternTextField"
                                    runat="server"
                                    DataIndex="DIDPattern" />
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

                        
                        <ext:ImageCommandColumn
                            ID="DeleteButtonsColumn"
                            runat="server"
                            Width="30"
                            Align="Center"
                            Sortable="false"
                            Groupable="false">

                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Delegate" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>

                            <Listeners>
                                <Command Handler="this.up(#{ManageDIDsGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>


                        <ext:CommandColumn
                            ID="RejectChange" 
                            runat="server" 
                            Width="70">
                            
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
                    <ext:Toolbar ID="FilterDIDsDIDsToolBar" runat="server">
                        <Items>
                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New DID"
                                Icon="Add"
                                Margins="5 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddNewDIDWindowPanel" />
                                </DirectEvents>
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
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageDIDsGridStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDIDsGridStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>


                            <ext:Window 
                                ID="AddNewDIDWindowPanel" 
                                runat="server" 
                                Frame="true"
                                Resizable="false"
                                Title="New DID Pattern" 
                                Hidden="true"
                                Width="510"
                                Icon="Add" 
                                X="50"
                                Y="100"
                                BodyStyle="background-color: #f9f9f9;"
                                ComponentCls="fix-ui-vertical-align">
                                
                                <Defaults>
                                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                                </Defaults>

                                <Items>
                                    <ext:TextField
                                        ID="NewDID_DIDPattern"
                                        runat="server"
                                        AllowBlank="false"
                                        AllowOnlyWhitespace="false"
                                        EmptyText="Empty DID Pattern"
                                        Width="480"
                                        FieldLabel="DID Pattern"
                                        LabelSeparator=":"
                                        LabelWidth="80" />
                                    
                                    <ext:TextField
                                        ID="NewDID_Description"
                                        runat="server"
                                        EmptyText="Empty Description"
                                        Width="480"
                                        FieldLabel="Description"
                                        LabelSeparator=":"
                                        LabelWidth="80" />
                                </Items>

                                <BottomBar>
                                    <ext:StatusBar
                                        ID="NewDIDWindowBottomBar"
                                        runat="server"
                                        StatusAlign="Right"
                                        DefaultText=""
                                        Height="30">
                                        <Items>
                                            <ext:Button
                                                ID="AddNewDIDButton"
                                                runat="server"
                                                Text="Add"
                                                Icon="ApplicationFormAdd"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="AddNewDIDButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:Button
                                                ID="CancelNewDIDButton"
                                                runat="server"
                                                Text="Cancel"
                                                Icon="Cancel"
                                                Height="25">
                                                <DirectEvents>
                                                    <Click OnEvent="CancelNewDIDButton_Click" />
                                                </DirectEvents>
                                            </ext:Button>

                                            <ext:ToolbarSeparator
                                                ID="NewDIDWindow_BottomBarSeparator"
                                                runat="server" />

                                            <ext:ToolbarTextItem
                                                ID="NewDID_StatusMessage"
                                                runat="server"
                                                Height="15"
                                                Text=""
                                                Margins="0 0 0 5" />
                                        </Items>
                                    </ext:StatusBar>
                                </BottomBar>

                                <DirectEvents>
                                    <BeforeHide OnEvent="AddNewDIDWindowPanel_BeforeHide" />
                                </DirectEvents>
                            </ext:Window>

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
