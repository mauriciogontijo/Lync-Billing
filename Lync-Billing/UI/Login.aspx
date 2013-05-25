﻿<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Lync_Billing.UI.Login" %>

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
			    <h1>Welcome to eBill</h1>
			    <p>Manage your phone calls, bills and your phone calls history from one place.</p>
			</div>
		</div>

		<div class="front-signin p10">
			<div class="signin mt5">
				<div class="placeholding-input username">
					<ext:TextField
                        runat="server"
                        ID="email"
                        EmptyText="Email Address"
                        Width="200"
                        TabIndex="1"
                        InputType="Email"
                        ValidateBlank="true"
                        Vtype="email"
                        StandardVtype="Email"
                        SelectOnFocus="true"
                        Selectable="true"
                        AllowBlank="false"
                        AllowOnlyWhitespace="false">
					</ext:TextField>
				</div>

				<div class="placeholding-input password">
                    <ext:TextField
                        runat="server"
                        ID="password"
                        EmptyText="Password"
                        Width="200"
                        TabIndex="2"
                        InputType="Password"
                        SelectOnFocus="true"
                        Selectable="true"
                        AllowBlank="false"
                        AllowOnlyWhitespace="false">
                        
                    </ext:TextField>
              	</div>

                <style type="text/css">
                    .x-field-indicator {
                        position: absolute;
                        display: block;
                        top: -3px;
                        left: 5px;
                    }
                </style>

                <div class="placeholding-input">
                    <div class="float-left">
                        <ext:Checkbox runat="server" ID="remember_me" IndicatorText="Remember Me" LabelStyle="vertical-align:bottom;" TabIndex="4" />
                    </div>
                    <div class="float-right">
                        <ext:Button ID="SigninButton" runat="server" Text="<p class='font-13 tahoma'>Signin</p>"  OnDirectClick="SigninButton_DirectClick" Width="60" Height="25" TabIndex="3" Type="Submit" />
                    </div>
				</div>
			</div>
		</div>
    </div>
</asp:Content>
