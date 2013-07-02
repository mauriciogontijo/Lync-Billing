<%@ Page Title="eBill Admin " Language="C#" MasterPageFile="~/ui/SuperUserMasterPage.Master" AutoEventWireup="true" CodeBehind="configuration.aspx.cs" Inherits="Lync_Billing.ui.admin.main.configuration" %>

<asp:Content ID="HeaderContentPlaceHolder" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="MainBodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF ADMIN MAIN BODY *** -->
    <div id='application-configuration' class='block float-right wauto h100p'>
        <div class="block-body pt5">
            <ext:GridPanel 
                ID="AppConfigurationGrid"
                runat="server" 
                Title="Manage Configuration"
                PaddingSummary="5px 5px 0"
                Width="740"
                Height="120"
                ButtonAlign="Center">
                
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ADMIN MAIN BODY *** -->
</asp:Content>
