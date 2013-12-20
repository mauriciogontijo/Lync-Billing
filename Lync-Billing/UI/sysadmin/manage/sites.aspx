<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="sites.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.manage.sites" %>

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
                        ID="ManageSitesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25"
                        OnLoad="ManageSitesStore_Load">
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

                <ColumnModel ID="ManageSitesColumnModel" runat="server" Flex="1">
                    <Columns>

                        <ext:Column ID="Column1"
                            runat="server"
                            Text="Site ID"
                            Width="80"
                            DataIndex="SiteID"
                            Sortable="false"
                            Groupable="false">
                            <HeaderItems>
                                <ext:TextField ID="SiteIDFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

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
                        </ext:Column>

                        <ext:Column ID="Column5"
                            runat="server"
                            Text="Country Code"
                            Width="100"
                            DataIndex="CountryCode"
                            Sortable="false"
                            Groupable="false">
                            
                            <HeaderItems>
                                <ext:TextField ID="CountryCodeFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDepartmentFiler" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>

                            <Renderer Fn="ViewUpperCase" />
                        </ext:Column>

                        <ext:Column ID="Column2"
                            runat="server"
                            Text="Country Name"
                            Width="200"
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
                        </ext:Column>

                        <ext:Column ID="Column3"
                            runat="server"
                            Text="Description"
                            Width="200"
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
                        </ext:Column>

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
                                <%--<DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageSitesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageSitesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>--%>
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
