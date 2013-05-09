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
					<input type="button" id='signin_btn' tabindex="4" value="Sign in" /> <!--onclick="authenticateUser(); getUserAttrs(); getUsers(); return false;" />-->
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
        BillingAPI.data = { 'AuthUserStatus': false, 'UserData': {}, 'Users': [], 'InsertUserStatus': {} };


        function authenticateUser() {
            //debugger;
            var email = $('#signin-email').val();
            var password = $('#signin-password').val();

            if (email != '' && password != '') {
                BillingAPI['lib'].authenticateUser(email, password, function (onSuccessData) { BillingAPI['data']['AuthUserStatus'] = $.parseJSON(onSuccessData); }, function (onFailData) { });
            }
        }

        function getUserAttrs() {
            //debugger;
            var email = $('#signin-email').val();

            BillingAPI['lib'].GetUserAttributes(
                email,
                function (onSuccessData) { BillingAPI['data']['UserData'] = $.parseJSON(onSuccessData); },
                function (onFailData) { }
            );

            console.log('finished getUserAttrs API Call | ' + BillingAPI['data']['UserData']['SipAccount']);
        }

        function getUsers() {
            //debugger;
            var email = $('#signin-email').val();
            var jsonWhereSt = JSON.stringify({ 'SipAccount': email });

            BillingAPI['lib'].GetUsers(
                null,
                jsonWhereSt,
                1,
                function (onSuccessData) { BillingAPI['data']['Users'] = $.parseJSON(onSuccessData); },
                function (onFailData) { }
            );

            console.log('finished getUsers API Call | ' + BillingAPI['data']['Users'].length);
        }

        function onAuthSuccess(onSuccessData) {
            BillingAPI['data']['AuthUserStatus'] = $.parseJSON(onSuccessData);
            console.log("authentication status: " + BillingAPI['data']['AuthUserStatus'] + " | " + typeof BillingAPI['data']['AuthUserStatus']);
        }


        //DOCUMENT READY - EVENTS BIND
        $(document).ready(function () {
            $('#signin_btn').mousedown(function (e) {
                e.preventDefault();

                authenticateUser();
                getUserAttrs();
                getUsers();

                return false;
            });

            $('#signin_btn').mouseup(function (e) {
                //debugger;
                if (BillingAPI['data']['AuthUserStatus'] == true) {
                    if (BillingAPI['data']['Users'].length > 0) {
                        //do nothing
                    }
                    else if (BillingAPI['data']['UserData'].hasOwnProperty('SipAccount')) {
                        var currentUserData = BillingAPI['data']['UserData'];
                        var userInfo = JSON.stringify({
                            'SipAccount': currentUserData.SipAccount.substring(4, currentUserData.SipAccount.length),
                            'SiteName': currentUserData.physicalDeliveryOfficeName,
                            'UserID': currentUserData.EmployeeID
                        });

                        console.log('User was not found. Record to be inserted is: ' + userInfo.toString());
                         BillingAPI['lib'].InsertUser(
                            userInfo,
                            function (onSuccessData) {
                                var data = $.parseJSON(onSuccessData);
                                BillingAPI['data']['InsertUserStatus'] = data;
                                console.log('InsertUser API Call was successful | returned data: ' + data);
                            }
                        );
                    }
                    else {
                        console.log('User data could not be retrieved!');
                    }
                }
            });
        });
    </script>
    <!-- End of Billing WebServices Javascript -->
</asp:Content>
