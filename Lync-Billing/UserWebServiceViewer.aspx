<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserWebServiceViewer.aspx.cs" Inherits="Lync_Billing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

    <script type="text/javascript">
        /*
        * The onClick AJAX Sing-in event.
        * It calls the authentication method from the server-side in an asynchronous manner.
        * @param: void
        * @return: void
        */
        function ajaxSignin()
        {
            //Get the contents of the emailAddress textbox.
            var emailAddress = document.getElementById('emailAddress').value;

            //Get the contents of the password textbox.
            var password = document.getElementById('emailAddress').value;

            //Call the authentication AJAX server-side function, and pass the required data to it.
            Lync_Billing.WebServices.UsersWebServices.authenticateUser(emailAddress, password, onComplete);
        }

        /*
        * WebService AJAX Callback function.
        * It writes the contents of the .NET AJAX function results inside a label beneath the login tools.
        * @param: the results from the .NET AJAX function
        * @return: void
        */
        function onComplete(results)
        {
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebServices/UsersWebServices.cs" />
            </Services>
        </asp:ScriptManager>
        <div>
            <b>Email Address:</b>&nbsp;&nbsp;<asp:TextBox ID="emailAddress" runat="server"></asp:TextBox><br />
            <b>Password:</b>&nbsp;&nbsp;<asp:TextBox ID="password" runat="server"></asp:TextBox>

            <br /><br />

            <input type="button" id="submit" value="Signin" onclick="ajaxSignin();" />

            <br /><br />

            <label id="status"></label>
        </div>
    </form>
</body>
</html>
