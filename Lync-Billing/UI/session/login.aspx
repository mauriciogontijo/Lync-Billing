<%@ Page Title="eBill | Login" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Lync_Billing.ui.session.login" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        div#main {
            border: 0;
            background: #ffffff !important;
            background-image: none !important;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- Start of ContentPage_Login Main HTML Content -->
    <div class="front-card">
        <div class="front-welcome float-left p10">
			<div class="front-welcome-text">
			    <h1>Welcome to eBill</h1>
			    <p>Manage your phone calls, bills, your phone calls statistics and history from one place, now!</p>
			</div>
		</div>

		<div class="front-signin p10">
			<div class="signin mt5">
				<div class="placeholding-input username">
					<asp:TextBox
                        TextMode="Email"
                        runat="server" 
                        id="email" 
                        Width="180"
                        TabIndex="1" />

					<asp:label ID="Label1" 
                        Text="Email" 
                        CssClass="placeholder" 
                        runat="server"></asp:label>
				</div>

				<div class="placeholding-input password">
            		<asp:TextBox 
                        id="password" 
                        runat="server" 
                        TextMode="Password" 
                        Width="180"
                        tabindex="2" />

                	<asp:label ID="Label2" 
                        Text="Password"  
                        CssClass="placeholder" 
                        runat="server">
                	</asp:label>
              	</div>

              	<div class="placeholding-input">
                    <asp:Button 
                        ID="signin_submit" 
                        runat="server" 
                        Text="Signin" 
                        OnClick="SigninButton_Click"/>
				</div>
                
                <div class="placeholding-input">
                    <div class="auth-msg red-color float-left"><%= AuthenticationMessage.ToString() %></div>
                </div>
			</div>
		</div>

        <asp:HiddenField ID="redirect_to_url" runat="server" />
	</div>
</asp:Content>
