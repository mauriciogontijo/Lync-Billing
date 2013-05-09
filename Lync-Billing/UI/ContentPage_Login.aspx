﻿<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="ContentPage_Login.aspx.cs" Inherits="Lync_Billing.UI.ContentPage_Login" %>

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
					<input type="text" id="signin-email" class="" name="session[username_or_email]" title="Email" tabindex="1" />
					<label for="signin-email" class="placeholder">Email</label>
				</div>

				<div class="placeholding-input password">
            		<input type="password" id="signin-password" class="" name="session[password]" title="Password" tabindex="2" />
                	<label for="signin-password" class="placeholder">Password</label>
              	</div>

              	<div class="placeholding-input">
					<input type="button" id='signin_btn' tabindex="4" value="Sign in" />
				</div>
			</div>
		</div>
	</div>
    <!-- End of ContentPage_Login Main HTML Content -->


    <!-- Start of Billing WebServices Javascript -->
    <script type="text/javascript">
        if (!window.BillingAPI) {
            window.BillingAPI = { 'lib': {}, 'data': {} };
        }

        BillingAPI.lib = Lync_Billing.Libs.BillingAPI;
        BillingAPI.data = { 'authUser': {}, 'JsonUserData': {} };

        $(document).ready(function () {
            $('#signin_btn').click(function (e) {
                e.preventDefault();
                var email = $('#signin-email').val();
                var password = $('#signin-password').val();

                if (email != '' && password != '') {
                    BillingAPI['lib'].authenticateUser(email, password, function(onSuccessData){BillingAPI['data']['authUser']=onSuccessData;}, function(onFailData){});
                }
            });
        });

        function getJsonUserAttrs() {
            var email = $('#signin-email').val();
            BillingAPI['lib'].GetJsonUserAttributes(email, function(onSuccessData){BillingAPI['data']['JsonUserData']=$.parseJSON(onSuccessData);}, function(onFailData){});
            return false;
        }
    </script>
    <!-- End of Billing WebServices Javascript -->
</asp:Content>
