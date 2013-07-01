<%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="authenticate.aspx.cs" Inherits="Lync_Billing.ui.session.authenticate" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Login</title>
    <style type="text/css">
        div#main {
            border: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- Start of ContentPage_Login Main HTML Content -->
    <div class="front-card">
		<div class="front-welcome float-left p10">
			<div class="front-welcome-text">
			    <h1 class="italic">You have requested an elevated access</h1>
			    <p>Please note that you must authenticate your information before proceeding any further.</p>
			</div>
		</div>

		<div class="front-signin p10">
			<div class="signin mt5">
				<div class="placeholding-input username">
                    <asp:label ID="Email_Label" 
                        Text="Email"  
                        CssClass="placeholder" 
                        runat="server"
                        Width="70">
                	</asp:label>

					<label id="email" class="bold">
                        <%= sipAccount %>
					</label>
				</div>

				<div class="placeholding-input password">
            		<asp:label ID="Label2" 
                        Text="Password"  
                        CssClass="placeholder" 
                        runat="server"
                        Width="70">
                	</asp:label>

                    <asp:TextBox 
                        id="password" 
                        runat="server" 
                        TextMode="Password" 
                        Width="160"
                        tabindex="2" />
              	</div>

              	<div class="placeholding-input">
                    <asp:Button 
                        ID="signin_submit" 
                        runat="server" 
                        Text="Signin" 
                        OnClick="authenticate_user"/>
				</div>
			</div>
		</div>

        <asp:HiddenField ID="redirect_to_url" runat="server" />
        <asp:HiddenField ID="access_level" runat="server" />
	</div>
</asp:Content>
