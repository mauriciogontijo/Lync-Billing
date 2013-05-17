<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.User_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Homepage</title>

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
			<p class='font-14'>Welcome to the new eBill, it's now more customized and personal. Please take your time going through your personal analytics and have a look at our new personal management tools.</p>
		</div>
	</div>

	<div class='clear h15'></div>

	<div id='user-phone-calls-history-block' class='block float-left w49p'>
		<div class='content wauto float-left mb10'>
			<asp:PlaceHolder ID="UserPhoneCallsHistoryPH" runat="server">
            </asp:PlaceHolder>
		</div>
        <div class="clear"></div>
		<div class='more-button wauto float-right'>
			<a href='User_ViewHistory.aspx' class='font-10'>view more >></a>
		</div>
	</div>

	<div id='user-phone-calls-summary-block' class='block float-right w49p'>
		<div class='content wauto float-left mb10'>
			<ext:Panel ID="UserPhoneCallsSummary"
                runat="server"         
                Height="240" 
                Width="465"
                Layout="AccordionLayout"
                Title="Your Phone Calls Summary">
                <Loader ID="SummaryLoader" 
                    runat="server" 
                    DirectMethod="#{DirectMethods}.GetSummaryData"
                    Mode="Component">
                    <LoadMask ShowMask="true" />
                </Loader>
            </ext:Panel>
		</div>
        <div class="clear"></div>
		<div class='more-button wauto float-right'>
			<a href='User_ManagePhoneCalls.aspx' class='font-10'>view more >></a>
		</div>
	</div>

	<div class='clear h15'></div>

	<div id='history-block-2' class='block float-left w49p'>
		<div class='content wauto float-left mb10'>
			<asp:PlaceHolder ID="PlaceHolder3" runat="server">
            </asp:PlaceHolder>
		</div>
        <div class="clear"></div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>

	<div id='history-block-3' class='block float-right w49p'>
		<div class='content wauto float-left mb10'>
			<asp:PlaceHolder ID="PlaceHolder4" runat="server">
            </asp:PlaceHolder>
		</div>
        <div class="clear"></div>
		<div class='more-button wauto float-right'>
			<a href='#' class='font-10'>view more >></a>
		</div>
	</div>
</asp:Content>
