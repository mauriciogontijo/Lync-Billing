<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title></title>
    </head>

    <body>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
                <Services>
                    <asp:ServiceReference Path="~/WebServices/UserWebService.asmx" />
                </Services>
            </asp:ScriptManager>

            <!--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>-->

            <script type="text/javascript">
                window.myObject = {};
                window.myObject['authUser'] = {};
                window.myObject['getUserData'] = {};

                //getUserAttrs Function
                function getUserAttrs()
                {
                    //Get the contents of the emailAddress textbox.
                    var emailAddress = document.getElementById('emailAddress').value || "";
                    Lync_Billing.WebServices.UserWebService.GetUserAttributes(
                        emailAddress,
                        function (onSuccessData) {
                            window.myObject['getUserData'] = onSuccessData;
                        },
                        function (onFailData) { }
                    );
                }


                //ajaxSigin Function
                function ajaxSignin()
                {
                    //Get the contents of the emailAddress textbox.
                    var emailAddress = document.getElementById('emailAddress').value || "";

                    //Get the contents of the password textbox.
                    var password = document.getElementById('password').value || "";

                    if (emailAddress != '' && password != '') {
                        //Call the authentication AJAX server-side function, and pass the required data to it.
                        //debugger;
                        Lync_Billing.WebServices.UserWebService.authenticateUser(
                            emailAddress,
                            password,
                            function (onSuccessData) {
                                window.myObject['authUser'] = onSuccessData;
                            },
                            function (onFailData) { }
                        );
                    }

                    return false;
                }


                //onComplete Function
                function onComplete(results)
                {
                    if (results != null) {
                        document.getElementById('status').innerHTML = results;
                    } else {
                        document.getElementById('status').innerHTML = "No data were returned!";
                    }
                }
            </script>
            <style type="text/css">
                #form1 {
                    height: 244px;
                }
            </style>

            <div>
                <b>Email:</b>&nbsp;&nbsp;<asp:TextBox ID="emailAddress" runat="server"></asp:TextBox><br />
                <b>Password:</b>&nbsp;&nbsp;<asp:TextBox ID="password" runat="server"></asp:TextBox>
                <br /><br />
                <input type="button" id="ajaxSignin_btn" value="Signin" onclick="ajaxSignin();" />
                <br /><br />
                <input type="button" id="getUserAttrs_btn" value="Get User Attributes" onclick="getUserAttrs();" />

                <br /><br />
                <label id="status"></label>
            </div>
        </form>
    </body>
</html>
