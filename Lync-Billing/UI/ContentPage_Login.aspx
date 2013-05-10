<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="ContentPage_Login.aspx.cs" Inherits="Lync_Billing.UI.ContentPage_Login" %>

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
					<input type="button" id='signin_btn' tabindex="4" value="Sign in" /> <!--onclick="authenticateUser(); getUserAttrs(); getUser(); return false;" />-->
				</div>

                <asp:HiddenField id="local_ip_address" value="" runat="server"></asp:HiddenField>
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
        BillingAPI.data = { 'AuthUserStatus': {}, 'UserData': {}, 'Users': [], 'InsertUserStatus': {} };


        function authenticateUser() {
            var email = $('#signin-email').val();
            var password = $('#signin-password').val();

            if (email != '' && password != '') {
                var clientData = getClientData();
                BillingAPI['lib'].authenticateUser(email, password, clientData, function (onSuccessData) { BillingAPI['data']['AuthUserStatus'] = $.parseJSON(onSuccessData); }, function (onFailData) { });
            }
        }

        function getUserAttrs() {
            var email = $('#signin-email').val();

            BillingAPI['lib'].GetUserAttributes(
                email,
                function (onSuccessData) { BillingAPI['data']['UserData'] = $.parseJSON(onSuccessData); },
                function (onFailData) { }
            );

            console.log('finished getUserAttrs API Call | ' + BillingAPI['data']['UserData']['SipAccount']);
        }

        function getUser() {
            var sip_account = BillingAPI['data']['UserData']['SipAccount'] || '';
            var jsonWhereSt = JSON.stringify({ 'SipAccount': sip_account });

            BillingAPI['lib'].GetUsers(
                null,
                jsonWhereSt,
                1,
                function (onSuccessData) { BillingAPI['data']['Users'] = $.parseJSON(onSuccessData); },
                function (onFailData) { }
            );

            console.log('finished getUser API Call | ' + BillingAPI['data']['Users'].length);
        }

        function getClientData() {
            var ip_address = $('#main_content_place_holder_local_ip_address').val();
            var browser_name = navigator.appName;
            var browser_version = navigator.appVersion;
            var user_agent = navigator.userAgent;

            return JSON.stringify({
                'IPAddress': ip_address,
                'BrowserName': BrowserDetect.browser,
                'BrowserVersion': BrowserDetect.version,
                'OS': BrowserDetect.OS,
                'UserAgent': user_agent
            });
        }


        //DOCUMENT READY - EVENTS BIND
        $(document).ready(function () {
            $('#signin_btn').click(function (e) {
                e.preventDefault();
                authenticateUser();
                return false;
            });
        });
    </script>
    <!-- End of Billing WebServices Javascript -->
</asp:Content>
