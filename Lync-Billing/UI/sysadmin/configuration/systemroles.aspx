<%@ Page Title="" Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="systemroles.aspx.cs" Inherits="Lync_Billing.ui.sysadmin.configuration.systemroles" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div id='generate-report-block' class='block float-right wauto h100p'>
        <div class="block-body pt5">

            <ext:GridPanel
                ID="SystemRolesGrid"
                runat="server"
                Width="740"
                Height="765"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Users Roles">

                <Store>
                    <ext:Store
                        ID="SystemRolesStore"
                        runat="server"
                        RemoteSort="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="SystemRolesModel" runat="server" IDProperty="ID">
                                <Fields>
                                    <ext:ModelField Name="RoleID" Type="Int" />
                                    <ext:ModelField Name="RoleName" Type="String" />
                                    <ext:ModelField Name="RoleDescription" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="SystemRolesColumnModel" runat="server" Flex="1">
                    <Columns>

                    </Columns>
                </ColumnModel>
                
                <BottomBar>
                    <ext:PagingToolbar
                        ID="SystemRolesPagingToolbar"
                        runat="server"
                        StoreID="SystemRolesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Roles {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>

        </div>
    </div>
</asp:Content>