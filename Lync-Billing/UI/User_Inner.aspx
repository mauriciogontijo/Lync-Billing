<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Inner.aspx.cs" Inherits="Lync_Billing.UI.User_Inner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

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

		    $('.show-content-0-btn').click(function (e) {
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#content-block-0').fadeIn(150);
		            $('#content-block-0').addClass('visibility-marker');
		        });
		    });

		    $('.show-content-1-btn').click(function (e) {
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#content-block-1').fadeIn(150);
		            $('#content-block-1').addClass('visibility-marker');
		        });
		    });

		    $('.show-content-2-btn').click(function (e) {
		        $('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#content-block-2').fadeIn(150);
		            $('#content-block-2').addClass('visibility-marker');
		        });
		    });

		    $('.show-phone-call-history-btn').click(function (e) {
		        $('#phone-call-history').fadeIn(150);
		        //$('#phone-call-history').css('display', 'block');
		        /*$('#main div.visibility-marker').fadeOut(400, function (e) {
		            $(this).removeClass('visibility-marker');
		            $('#phone-call-history').fadeIn(150);
		            $('#phone-call-history').addClass('visibility-marker');
		        });*/
		    }); 
		});
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <div id='sidebar' class='sidebar block float-left w20p'>
	    <div class='block-header top-rounded bh-shadow'>
		    <p class='font-18'>SIDEBAR</p>
	    </div>
	    <div class='block-body bottom-rounded bb-shadow'>
		    <div class='block-content wauto float-left mb15'>
			    <p class='font-16 bold mb5'>Manage Phone Calls</p>
			    <p class='font-14 ml15 show-content-0-btn'><a href='#'>Content Block 0</a></p>
			    <p class='font-14 ml15 show-content-1-btn'><a href='#'>Content Block 1</a></p>
			    <p class='font-14 ml15 show-content-2-btn'><a href='#'>Content Block 2</a></p>
		    </div>
					
		    <div class='block-content wauto float-left mb15'>
			    <p class='font-16 bold mb5'>Manage Statistics</p>
			    <p class='font-14 ml15 show-content-0-btn'><a href='#'>Content Block 0</a></p>
			    <p class='font-14 ml15 show-content-1-btn'><a href='#'>Content Block 1</a></p>
			    <p class='font-14 ml15 show-content-2-btn'><a href='#'>Content Block 2</a></p>
                <asp:LinkButton ID="TEST1" runat="server" OnClick="TEST1_Click" Text="TEST" ></asp:LinkButton>
		    </div>

		    <div class='block-content wauto float-left mb15'>
			    <p class='font-16 bold mb5'>Phone Calls History</p>
                <asp:LinkButton ID="view_call_history" runat="server" OnClick="view_call_history_Click" Text="View Calls History" ></asp:LinkButton>
		    </div>

		    <div class='clear h5'></div>
	    </div>
    </div>
    
    <!--<div id='content-block-0' class='block float-right w80p h100p visibility-marker'>
		<div class='block-header top-rounded bh-shadow'>
			<p class='font-18'>Content Block #0</p>
		</div>
		<div class='block-body bottom-rounded bb-shadow'>
			<div class='block-content wauto float-left'>
			</div>
			<div class='clear h5'></div>
		</div>
	</div>

	<div id='content-block-1' class='block float-right w80p h100p' style='display: none;'>
		<div class='block-header top-rounded bh-shadow'>
			<p class='font-18'>Content Block #1</p>
		</div>
		<div class='block-body bottom-rounded bb-shadow'>
			<div class='block-content wauto float-left'>
			</div>
			<div class='clear h5'></div>
		</div>
	</div>

	<div id='content-block-2' class='block float-right w80p h100p' style='display: none;'>
		<div class='block-header top-rounded bh-shadow'>
			<p class='font-18'>Content Block #2</p>
		</div>
		<div class='block-body bottom-rounded bb-shadow'>
			<div class='wauto float-left'>						
			</div>
			<div class='clear h5'></div>
		</div>
	</div>-->

    <div id='phone-call-history' class='block float-right w80p h100p'>
		<div class='block-body'>
			<div class='wauto float-left'>
                <asp:PlaceHolder ID="UserPhoneCallsHistoryPH" runat="server">
                </asp:PlaceHolder>
                 <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                     HASDGHKASDGHKASD
                     BJKASDGKASDASDJK
                     KASDASDASDASD
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
