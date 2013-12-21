﻿<%@ Page Title="tBill Admin | Manage Sites" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="sites.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.manage.sites" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var ViewUpperCase = function (sourceString) {
            if (typeof sourceString !== undefined) {
                return sourceString.toString().toUpperCase();
            } else {
                return "";
            }
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            
            <ext:GridPanel
                ID="ManageSitesGrid"
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
                        ID="ManageSitesGridStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25"
                        OnLoad="ManageSitesGridStore_Load">
                        <Model>
                            <ext:Model ID="ManageSitesModel" runat="server" IDProperty="SiteID">
                                <Fields>
                                    <ext:ModelField Name="SiteID" Type="Int" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="CountryCode" Type="String" />
                                    <ext:ModelField Name="CountryName" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:RowEditing ID="RowEditingPlugin" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ManageSitesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="Column4"
                            runat="server"
                            Text="Site Name"
                            Width="150"
                            DataIndex="SiteName"
                            Sortable="false"
                            Groupable="false">

                            <HeaderItems>
                                <ext:TextField ID="SiteNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSiteFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Editor>
                                <ext:TextField
                                    ID="Editor_SiteNameTextField"
                                    runat="server"
                                    DataIndex="SiteName" />
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="Column2"
                            runat="server"
                            Text="Country Name"
                            Width="280"
                            DataIndex="CountryName"
                            Sortable="false"
                            Groupable="false">
                            
                            <HeaderItems>
                                <ext:TextField ID="TextField1" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearButton1" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Editor>
                                <ext:ComboBox
                                    ID="Editor_CountryNameCombobox"
                                    runat="server"
                                    DataIndex="CountryCode"
                                    TriggerAction="Query"
                                    QueryMode="Local"
                                    Editable="true"
                                    DisplayField="CountryName"
                                    ValueField="CountryName"
                                    EmptyText="Please Select Country"
                                    Width="70"
                                    AllowBlank="true"
                                    AllowOnlyWhitespace="false">
                                    <Store>
                                        <ext:Store ID="Editor_CountryNameComboboxStore" runat="server" OnLoad="Editor_CountryNameComboboxStore_Load">
                                            <Model>
                                                <ext:Model ID="Editor_CountryNameComboboxStoreModel" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="TwoDigitsCountryCode" Type="String" />
                                                        <ext:ModelField Name="ThreeDigitsCountryCode" Type="String" />
                                                        <ext:ModelField Name="CountryName" Type="String" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="Column3"
                            runat="server"
                            Text="Description"
                            Width="220"
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
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New Site"
                                Icon="Add"
                                Margins="5 5 0 5">
                                <%--<DirectEvents>
                                    <Click OnEvent="ShowAddSitePanel" />
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
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageSitesGridStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageSitesGridStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageSitesPagingToolbar"
                        runat="server"
                        StoreID="ManageSitesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Sites {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>

    </div>
</asp:Content>
