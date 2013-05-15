<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Home.aspx.cs" Inherits="Lync_Billing.UI.User_Home" %>

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
			<asp:PlaceHolder ID="UserPhoneCallsSummaryPH" runat="server">
            </asp:PlaceHolder>
		</div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>

	<div id='history-block-1' class='block float-right w49p'>
		<div class='content wauto float-left clear mb10'>
			<asp:PlaceHolder ID="PlaceHolder2" runat="server">
            </asp:PlaceHolder>
		</div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>

	<div class='clear h15'></div>

	<div id='history-block-2' class='block float-left w49p'>
		<div class='content wauto float-left clear mb10'>
			<asp:PlaceHolder ID="PlaceHolder3" runat="server">
            </asp:PlaceHolder>
		</div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>

	<div id='history-block-3' class='block float-right w49p'>
		<div class='content wauto float-left clear mb10'>
			<asp:PlaceHolder ID="PlaceHolder4" runat="server">
            </asp:PlaceHolder>
		</div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>
</asp:Content>
