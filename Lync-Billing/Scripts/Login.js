/*
* The onClick AJAX Sing-in event.
* It calls the authentication method from the server-side in an asynchronous manner.
* @param: void
* @return: void
*/
function ajaxSignin() {
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
function onComplete(results) {
    if (results != null) {
        document.getElementById('status').innerHTML = results;
    } else {
        document.getElementById('status').innerHTML = "No data were returned!";
    }
}
