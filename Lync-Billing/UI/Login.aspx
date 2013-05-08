<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Lync_Billing.UI.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Libs/BillingAPI.asmx" />
            </Services>
        </asp:ScriptManager>
        

        <div>
            <b>Email:</b><asp:TextBox ID="emailAddress" runat="server"></asp:TextBox><br />
            <b>Password:</b><asp:TextBox ID="password" runat="server"></asp:TextBox><br />
            <input type="button" id="signin" value="Signin" onclick="ajaxSignin();" /><br />
            <input type="button" id="getUser" value="Get User Data" onclick="getUserAttrs();" /><br />
            <input type="button" id="getJsonUser" value="Get JSON User Data" onclick="getJsonUserAttrs();" /><br /><br />
            <input type="button" id="sendJson" value="Send JSON Data" onclick="sendJsonInput();" /><br />
        </div>


        <script type="text/javascript">
            window.BillingAPI = { 'lib': {}, 'data': {} };
            window.BillingAPI.lib = Lync_Billing.Libs.BillingAPI;
            window.BillingAPI.data = { 'authUser': {}, 'getUserData': {}, 'getJsonUserData': {} };

            //getUserAttrs Function
            function getUserAttrs() {
                //Get the contents of the emailAddress textbox.
                var emailAddress = document.getElementById('emailAddress').value || "";
                BillingAPI['lib'].GetUserAttributes(
                    emailAddress,
                    function (onSuccessData) {
                        BillingAPI['data']['getUserData'] = onSuccessData;
                    },
                    function (onFailData) { }
                );

                return false;
            }

            //getJsonUserAttrs Function
            function getJsonUserAttrs() {
                //Get the contents of the emailAddress textbox.
                var emailAddress = document.getElementById('emailAddress').value || "";
                BillingAPI['lib'].GetJsonUserAttributes(
                    emailAddress,
                    function (onSuccessData) {
                        //BillingAPI['data']['getJsonUserData'] = jQuery.parseJSON(onSuccessData);
                        BillingAPI['data']['getJsonUserData'] = onSuccessData;
                    },
                    function (onFailData) { }
                );

                return false;
            }

            //ajaxSigin Function
            function ajaxSignin() {
                //Get the contents of the emailAddress textbox.
                var emailAddress = document.getElementById('emailAddress').value || "";

                //Get the contents of the password textbox.
                var password = document.getElementById('password').value || "";

                if (emailAddress != '' && password != '') {
                    //Call the authentication AJAX server-side function, and pass the required data to it.
                    //debugger;
                    BillingAPI['lib'].authenticateUser(
                        emailAddress,
                        password,
                        function (onSuccessData) {
                            BillingAPI['data']['authUser'] = onSuccessData;
                        },
                        function (onFailData) { }
                    );
                }

                return false;
            }


            function sendJsonInput() {
                var data = {
                    'name': 'Ahmad',
                    'age': 24,
                    'job': {
                        'title': 'Software Engineer',
                        'location': 'Athens, Greece',
                        'Hours per Week': 40
                    }
                };

                var data_json_formatted = JSON.stringify(data);

                //BillingAPI['lib'].GetJsonObject(data_json_formatted);

                return false;
            }

            //onComplete Function
            function onComplete(results) {
                if (results != null) {
                    document.getElementById('status').innerHTML = results;
                } else {
                    document.getElementById('status').innerHTML = "No data were returned!";
                }
            }
        </script>
    </form>
</body>
</html>
