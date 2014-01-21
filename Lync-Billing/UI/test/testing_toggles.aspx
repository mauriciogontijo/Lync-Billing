<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testing_toggles.aspx.cs" Inherits="Lync_Billing.ui.test.testing_toggles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <link rel="icon" href="resources/images/favicon.ico" type="image/ico">
        <link rel="shortcut icon" href="resources/images/favicon.ico" type="image/ico">

        <!-- [if IE]>
            <link rel="icon" href="<%= @"~/favicon.ico" %>" type="image/x-ico" />
            <link rel="shortcut icon" href="<%= @"~/favicon.ico" %>" type="image/x-ico" />
            <link rel="icon" href="<%= @"~/favicon.ico" %>" type="image/image/vnd.microsoft.icon" />
            <link rel="shortcut icon" href="<%= @"~/favicon.ico" %>" type="image/image/vnd.microsoft.icon" />
        <![endif]-->

        <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
        <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>

        <link rel="stylesheet" type="text/css" media="all" href="../resources/css/reset.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../resources/css/layouts.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../resources/css/toolkit.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../resources/css/global.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../resources/css/main.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../resources/css/dropdown-menu-white.css" />

        <script type="text/javascript" src="../resources/js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="../resources/js/browserdetector.js"></script>
        <script type="text/javascript" src="../resources/js/ext-js-specific.js"></script>
        <script type="text/javascript" src="../resources/js/feedback.js"></script>

		<!--[if lte IE 9]>
		    <link rel="stylesheet" type="text/css" media="all" href="resources/css/ie-hacks.css" />
	    <![endif]-->

        
        <script type="text/javascript">
            $(document).ready(function () {
                $('#toggle-button').click(function () {
                    event.preventDefault();
                    $("#toggle-block").slideToggle("slow");
                });

                $('#toggle-button-2').click(function () {
                    event.preventDefault();
                    $("#toggle-block").slideToggle("slow");
                });
            });
        </script>
    </head>


    <body>
        <form id="form1" runat="server">
            <!-- *** START OF MAIN BODY CONTENT *** -->
            <div id='main' class='main-container bottom-rounded'>

                <a href="#" id="toggle-button">toggle me</a>

                <button id="toggle-button-2">toggle me</button>

                <div class="clear h20"></div>

                <div class="clear mb20">
                    <p class="black-font">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque imperdiet enim et purus lobortis fringilla. Nullam tincidunt velit sed erat commodo, id pretium nibh posuere. Aenean sit amet neque sodales, accumsan tellus at, euismod leo. Vestibulum dapibus eleifend dui, sed hendrerit magna facilisis vitae. Aliquam mattis libero sit amet sem iaculis, vitae molestie tortor semper. Proin at pulvinar turpis, ac sollicitudin lorem. Morbi quis diam iaculis, molestie felis ac, cursus purus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam vitae dignissim purus. Donec massa magna, lobortis at imperdiet vel, tempus porta lacus. Aenean consectetur, tellus nec aliquet luctus, libero diam dapibus leo, in congue elit purus eget odio. Aliquam nisi est, ultrices sed hendrerit in, fringilla id mi. Praesent euismod quam metus, placerat iaculis tellus posuere quis. Donec in lacus justo. Proin non nulla eget sapien ornare rutrum.</p>
                </div>

                <div id="toggle-block" class="clear toggled-block" style="">
                    <p class="black-font">This is the paragraph to end all paragraphs. You should feel <em>lucky</em> to have seen such a paragraph in your life. Congratulations!</p>
                </div>

                <div class="clear mb20">
                    <p class="black-font">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque imperdiet enim et purus lobortis fringilla. Nullam tincidunt velit sed erat commodo, id pretium nibh posuere. Aenean sit amet neque sodales, accumsan tellus at, euismod leo. Vestibulum dapibus eleifend dui, sed hendrerit magna facilisis vitae. Aliquam mattis libero sit amet sem iaculis, vitae molestie tortor semper. Proin at pulvinar turpis, ac sollicitudin lorem. Morbi quis diam iaculis, molestie felis ac, cursus purus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam vitae dignissim purus. Donec massa magna, lobortis at imperdiet vel, tempus porta lacus. Aenean consectetur, tellus nec aliquet luctus, libero diam dapibus leo, in congue elit purus eget odio. Aliquam nisi est, ultrices sed hendrerit in, fringilla id mi. Praesent euismod quam metus, placerat iaculis tellus posuere quis. Donec in lacus justo. Proin non nulla eget sapien ornare rutrum.</p>
                </div>
            </div>

            <div id="toggle-block-2" class="clear toggle-enabled" style="height:100px; width:1000px; border: #000000 1px solid; padding: 20px; display: none; margin: 20px auto 20px auto;">
                <p class="black-font">This is the paragraph to end all paragraphs. You should feel <em>lucky</em> to have seen such a paragraph in your life. Congratulations!</p>
            </div>
        </form>
    </body>
</html>
