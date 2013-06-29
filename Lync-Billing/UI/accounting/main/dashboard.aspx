<%@ Page Title="eBill Accounting | Dashboard" Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Lync_Billing.ui.accounting.main.dashboard" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=9" />

        <link rel="stylesheet" type="text/css" media="all" href="../../resources/css/reset.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../../resources/css/layouts.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../../resources/css/toolkit.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../../resources/css/global.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../../resources/css/green-layout.css" />
        <link rel="stylesheet" type="text/css" media="all" href="../../resources/css/dropdown-menu-white.css" />

        <script type="text/javascript" src="../../resources/js/jquery-1.9.1.min.js"></script>
        <script type="text/javascript" src="../../resources/js/browserdetector.js"></script>
        <script type="text/javascript" src="../../resources/js/ext-js-specific.js"></script>

		<!--[if lte IE 9]>
		    <link rel="stylesheet" type="text/css" media="all" href="resources/css/green-layout-ie-hacks.css" />
	    <![endif]-->

	    <!--[if lt IE 8]>
		    <style type="text/css">
			    #main { padding-top: 65px !important; }
		    </style>
	    <![endif]-->
    </head>

    <body>
        <form id="form1" runat="server">
            <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />

            <% if(Session.Contents["UserData"] != null) { %>
                <div  class="smart-block tool-dark-bg liquid toolbar " >
                    <div class="hwrapper rtl  ">
                        <div class="col size2of3">
                            <div class="ie7flot-fix" ><!--ie7 fix-->
                                <ul id="navigation-tabs" class="vertical-navigation " >
                                    <li id="access-tab">
                                        <a title="Drop accounting access" href="../../user/dashboard.aspx">Drop Accounting Access<span class="shutdown"></span></a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <!--end toolbar right nav-->
                        <div class="col size1of3 lastcol">
                            <a class="logo fl" href='../main/dashboard.aspx'>eBill</a>
                        </div>
                    </div>
                    <!--end toolbar wrapper--> 
                </div> 
                <!-- toolbar block-->
            <% } else { %>
                <div  class="block tool-dark-bg liquid toolbar " >
                    <div class="hwrapper rtl  ">
                        <div class="col size1of2 "></div>
                        <!--end toolbar right nav-->
                        <div class="col size1of2 lastcol">
                            <a class="logo fl" href='../../session/login.aspx'>eBill</a>
                        </div>
                    </div>
                    <!--end toolbar wrapper--> 
                </div>
                <!-- toolbar block-->
            <% } %>


            <div id='main' class='main-container bottom-rounded'>
                <asp:HiddenField ID="ThisPageReferrer" runat="server" Value="" />

                <!-- *** START OF SIDEBAR *** -->
                <div id='accounting-tools-sidebar' class='sidebar block float-left w20p'>
                    <div class="block-body">
                        <ext:Panel ID="AccountingToolsSidebar"
                            runat="server"
                            Height="230"
                            Width="180"
                            Title="Accounting Tools"
                            Collapsed="false"
                            Collapsible="true">
                            <Content>
                                <div class='sidebar-section'>
                                    <div class="sidebar-section-header">
                                        <p>Disputes</p>
                                    </div>
                                    <div class="sidebar-section-body">
                                        <p><a href='../main/disputes.aspx' id="ui_accounting_main_disputes">Manage Disputed Calls</a></p>
                                    </div>
                                </div>

                                <div class='sidebar-section'>
                                    <div class="sidebar-section-header">
                                        <p>Generate Reports</p>
                                    </div>
                                    <div class="sidebar-section-body">
                                        <p><a href='../reports/monthly.aspx' id="ui_accounting_reports_monthly">Monthly Reports</a></p>
                                        <p><a href='../reports/periodical.aspx' id="ui_accounting_reports_periodical">Periodical Reports</a></p>
                                    </div>
                                </div>
                            </Content>
                        </ext:Panel>
                    </div>
                </div>
                <!-- *** END OF SIDEBAR *** -->

                <!-- *** START OF MAIN PAGE CONTENT *** -->
                    <div id='dashboard-message' class='block float-right wauto h100p'>
                        <div class="block-body pt5">
                            <ext:Panel 
                                ID="AccountingAnnouncementsPanel"
                                runat="server" 
                                Title="Announcements"
                                PaddingSummary="10px 10px 10px 10px"
                                Width="740"
                                Height="120"
                                ButtonAlign="Center">
                                <Content>
                                    <div class="p10 font-12">
                                        <p>This is the Accounting dashboard, you'll find the tools you need in the left sidebar, categorized already into sections based on similarity.</p>
                                    </div>
                                </Content>
                            </ext:Panel>
                        </div>
                    </div>
                <!-- *** END OF MAIN PAGE CONTENT *** -->
            </div>

            <div class='clear h10'></div>

		    <div id='footer' class='footer-container'>
		    </div>
        </form>
    </body>
</html>