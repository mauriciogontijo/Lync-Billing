<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridTest2.aspx.cs" MasterPageFile="~/UI/MasterPage.Master" Inherits="Lync_Billing.UI.GridTest2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Login</title>
    <link rel="stylesheet" type="text/css" href="css/reset.css" />
	<link rel="stylesheet" type="text/css" href="css/green-layout.css" />
	<link rel="stylesheet" type="text/css" href="css/toolkit.css" />

    <!--[if lt IE 9]>
			<link rel="stylesheet" type="text/css" href="css/green-layout-ie-8.css" />
		<![endif]-->

		<!--[if lt IE 8]>
			<style type="text/css">
				#main { padding-top: 65px !important; }
			</style>
		<![endif]-->

        <style type="text/css">
            .x-panel-header-default {
                background: -webkit-gradient(linear, left top, left bottom, from(#A8DBA8), to(#79BD9A)) !important;
                border-top-left-radius: 5px;
                border-top-right-radius: 5px;
            }

            div#main_content_place_holder_PhoneCallsGrid-body {
                box-sizing: border-box;
                -moz-box-sizing: border-box;
                -ms-box-sizing: border-box;
                -webkit-box-sizing: border-box;
                border-bottom-left-radius: 5px;
                border-bottom-right-radius: 5px;
            }
        </style>

		<!--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>-->
		<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>

		<script type="text/javascript">
		    $(document).ready(function () {
		        $('settings-menu-button').click(function (e) {
		            e.preventDefault();

		            if ($('#settings-more-list-container').css('display') == 'none') {
		                $('#settings-more-list-container').fadeIn();
		                $('#settings-more-list-container').css('display', 'block');
		            } else {
		                $('#settings-more-list-container').fadeOut();
		                $('#settings-more-list-container').css('display', 'none');
		            }

		            return false;
		        });

		        $('#nav-more').click(function (e) {
		            e.preventDefault();
		            var top = $(this).offset().top;
		            var right = $(this).offset().right;

		            $('#more-list-container').css({ right: right - 1, top: top + 4 }).fadeIn('fast');
		            return false;
		        });

		        $('body').click(function (e) {
		            $('#more-list-container').fadeOut('fast');
		        });
		    });
		</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

    <div id='announcements' class='announcements shadow mb10 p10'>
		<div class='mb20'>
			<p class='font-18'>ANNOUNCEMENTS!</p>
		</div>
		<div>
			<p class='font-14'>Please do ... 1 2 3 .... now!</p>
			<p class='font-14'>Please do ... 4 5 6 .... later!</p>
		</div>
	</div>

	<div class='clear h15'></div>

	<div id='history-block-0' class='block float-left w49p'>
		<div class='content wauto float-left clear mb10'>
			<asp:PlaceHolder ID="ph" runat="server">
            </asp:PlaceHolder>
		</div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>

	<div id='history-block-1' class='block float-right w49p'>
		<div class='block-header top-rounded bh-shadow'>
			<p class='font-18'>Personal History</p>
		</div>
		<div class='block-body bottom-rounded bb-shadow'>
			<div class='content wauto float-left'>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
			</div>
			<div class='clear h5'></div>
			<div class='more-button wauto float-right'>
				<a href='#' class='font-10'>view more >></a>
			</div>
		</div>
	</div>

	<div class='clear h15'></div>

	<div id='history-block-2' class='block float-left w49p'>
		<div class='block-header top-rounded bh-shadow'>
			<p class='font-18'>Personal History</p>
		</div>
		<div class='block-body bottom-rounded bb-shadow'>
			<div class='content wauto float-left'>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
			</div>
			<div class='clear h5'></div>
			<div class='more-button wauto float-right'>
				<a href='#' class='font-10'>view more >></a>
			</div>
		</div>
	</div>

	<div id='history-block-3' class='block float-right w49p'>
		<div class='block-header top-rounded bh-shadow'>
			<p class='font-18'>Personal History</p>
		</div>
		<div class='block-body bottom-rounded bb-shadow'>
			<div class='content wauto float-left'>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
				<p class='font-14'>aasdasdasdasdasdasdasd</p>
			</div>
			<div class='clear h5'></div>
			<div class='more-button wauto float-right'>
				<a href='#' class='font-10'>view more >></a>
			</div>
		</div>
	</div>
</asp:Content>