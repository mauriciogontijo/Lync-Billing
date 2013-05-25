<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Lync_Billing.UI.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Login</title>
    <style type="text/css">
        div#main {
            border: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- Start of ContentPage_Login Main HTML Content -->
    <div class="front-card">
		<div class="front-welcome float-left p10">
			<div class="front-welcome-text">
			    <h1>Welcome to eBill.</h1>
			    <p>Manage your phone calls, bills and your phone calls history from one place.</p>
			</div>
		</div>

		<div class="front-signin p10">
			<div class="signin mt5">
				<div class="placeholding-input username">
					<asp:TextBox
                        TextMode="Email"
                        runat="server" 
                        id="email" 
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
                        OnClick="signin_submit_Click" />
				</div>
			</div>
		</div>
	</div>
</asp:Content>
