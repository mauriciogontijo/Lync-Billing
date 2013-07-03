<%@ Page Title="" Language="C#" MasterPageFile="~/ui/MasterPage.Master" AutoEventWireup="true" CodeBehind="redbtn.aspx.cs" Inherits="Lync_Billing.ui.test.redbtn" %>

<asp:Content ID="HeaderContentPlaceholder" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../resources/css/css3-buttons.css" />
</asp:Content>

<asp:Content ID="BodyContentPlaceHolder" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <%--<div id='warning-block' class='warning-block shadow'>
        <p class="message"><%= String.Format("You have a total of <span class='bold'>{0}&nbsp;unmarked</span> calls, please click <a class='link bold' href='../user/phonecalls.aspx'>here</a> to mark them.", unmarked_calls_count) %></p>
    </div>--%>
              
    <nav>
        <ul class="nav">
            <li><a href="#" class="icon-home"></a></li>
            <li><a href="#" class="icon-cog"></a></li>
            <li><a href="#" class="icon-cw"></a></li>
            <li><a href="#" class="icon-location"></a></li>
        </ul>
    </nav>

    <div class='clear h25'></div>
</asp:Content>