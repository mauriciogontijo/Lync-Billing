<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserWebServiceViewer.aspx.cs" Inherits="Lync_Billing.WebServices.UserWebServiceViewer" %>
<html>
<head>
    <script type="text/javascript" >
        function ajaxSignin() {
            //Get the contents of the emailAddress textbox.
            var emailAddress = document.getElementById('emailAddress').value;

            //Get the contents of the password textbox.
            var password = document.getElementById('emailAddress').value;

            //Call the authentication AJAX server-side function, and pass the required data to it.
            Lync_Billing.WebServices.UserWebService.authenticateUser(emailAddress, password, onComplete);

        }

        /*
        * WebService AJAX Callback function.
        * It writes the contents of the .NET AJAX function results inside a label beneath the login tools.
        * @param: the results from the .NET AJAX function
        * @return: void
        */
        function onComplete(results) {
            if (results != null) {
                document.getElementById('status').innerHTML = results;
            } else {
                document.getElementById('status').innerHTML = "No data were returned!";
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:scriptmanager ID="ScriptManager1" runat="server" ScriptMode="Debug" >
            <Services>
               <asp:ServiceReference Path="~/WebServices/UserWebService.asmx"/>
            </Services>
           
        </asp:scriptmanager>

         <!--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>-->
    
    <style type="text/css">
        #form1 {
            height: 244px;
        }
    </style>
        <div>
            <b>Email:</b>&nbsp;&nbsp;<asp:TextBox ID="emailAddress" runat="server"></asp:TextBox><br />
            <b>Password:</b>&nbsp;&nbsp;<asp:TextBox ID="password" runat="server"></asp:TextBox>

            <br /><br />

            <input type="button" id="submit" value="Signin" onclick="ajaxSignin();" />

            <br /><br />

            <label id="status"></label>
        </div>
   </form>
</body>
    </html>