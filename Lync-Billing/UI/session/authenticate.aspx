<%@ Page Title="tBill | Authenticate" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="authenticate.aspx.cs" Inherits="Lync_Billing.ui.session.authenticate" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        html { background: #fff; }
        div#main { border: 0; background: #ffffff !important;  background-image: none !important; }
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- Start of ContentPage_Login Main HTML Content -->
    <div class="front-card">
		<div class="front-welcome float-left p10">
			<div class="front-welcome-text">
			    <h1><%= HeaderAuthBoxMessage %></h1>
			    <p><%= ParagraphAuthBoxMessage %></p>
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
                        Width="180"
                        tabindex="2" />
              	</div>

              	<div class="placeholding-input">
                    <asp:Button 
                        ID="signin_submit" 
                        runat="server" 
                        Text="Log In" 
                        OnClick="AuthenticateUser"/>
				</div>

                <div class="auth-msg red-color"><%= AuthenticationMessage.ToString() %></div>
			</div>
		</div>

        <asp:HiddenField ID="redirect_to_url" runat="server" />
        <asp:HiddenField ID="delegee_identity" runat="server" />
        <asp:HiddenField ID="access_level" runat="server" />
	</div>
</asp:Content>
