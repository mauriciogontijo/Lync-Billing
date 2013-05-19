<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.User_Dashboard" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<?xml version="1.0" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>eBill | User Homepage</title>

    <link rel="stylesheet" type="text/css" href="css/reset.css" />
    <link rel="stylesheet" type="text/css" href="css/green-layout.css" />
    <link rel="stylesheet" type="text/css" href="css/toolkit.css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="js/browserdetector.js"></script>
    <script type="text/javascript" src="js/toolkit.js"></script>

    <!--[if lt IE 9]>
		    <link rel="stylesheet" type="text/css" href="css/green-layout-ie-8.css" />
	    <![endif]-->

    <!--[if lt IE 8]>
		    <style type="text/css">
			    #main { padding-top: 65px !important; }
		    </style>
	    <![endif]-->

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

        var myDateRenderer = function (value) {
            value = Ext.util.Format.date(value, "d M Y h:i A");
            return value;
        }

        function GetMinutes(value, meta, record, rowIndex, colIndex, store) {

            var sec_num = parseInt(record.data.Duration, 10);
            var hours = Math.floor(sec_num / 3600);
            var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
            var seconds = sec_num - (hours * 3600) - (minutes * 60);

            if (hours < 10) {
                hours = "0" + hours;
            }
            if (minutes < 10) {
                minutes = "0" + minutes;
            }
            if (seconds < 10) {
                seconds = "0" + seconds;
            }

            return hours + ':' + minutes + ':' + seconds;;
        }

        var submitValue = function (grid, hiddenFormat, format) {
            grid.submitData(false, { isUpload: true });
        };

        var tipRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0;
            
            App.PhoneCallsChart.getStore().each(function (rec) {
                total += rec.get('TotalCalls');
            });
            this.setTitle(storeItem.get('Name') + ': ' + Math.round(storeItem.get('TotalCalls') / total * 100) +  '%' + '<br>' + 'Total Calls: ' + storeItem.get('TotalCalls'));
        };

    </script>
</head>

<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="resourceManager" runat="server" Theme="Gray" />

        <div id='header-container'>
            <div id='nav-toolbar' class='nav-toolbar text-center'>
                <div id='logo' class='logo float-left'>
                    <p><a href='User_Dashboard.aspx'>LOGO</a></p>
                </div>

                <% if (false)
                   { %>
                <ul id='nav-buttons'>
                    <li class='nav-button'>
                        <!--<a href='#' class='nav-button-item'><img src='images/settings.png' width='32' /></a>-->
                        <a id='settings-menu-button' href="javascript:void(0)">
                            <img alt='Settings Menu' src='images/settings.png' width='32' /></a>
                        <div id="settings-more-list-container" style="display: block;">
                            <!---<ul id="settings-more-list">
						                <li><a class='settings-menu-text' title="jQuery Cheat Sheet" href="/jquery">Example</a></li>
						                <li><a class='settings-menu-text' title="NodeJS Cheat Sheet" href="/nodejs">Example</a></li>
						                <li><a class='settings-menu-text' title="PHP Cheat Sheet" href="/php">Example</a></li>
						                <li><a class='settings-menu-text' title="Java Cheat Sheet" href="/java">Example</a></li>
						                <li><a class='settings-menu-text' href="/#more">Even More ></a></li>
						            </ul>---->
                            <a href="#">One 123123</a>
                            <a href="#">Two 123123</a>
                            <a href="#">Three 123123</a>
                        </div>
                    </li>
                    <li class='nav-button'><a href='user-inner-page.html' class='nav-button-item'>
                        <img alt='Manage My Bills' src='images/mybills.png' width='32' /></a></li>
                    <li class='nav-button'><a href='user-inner-page.html' class='nav-button-item'>
                        <img alt='Manage My Phone Calls' src='images/phonecalls.png' width='40' /></a></li>
                </ul>
                <% } %>
            </div>
        </div>

        <div id='main' class='main bottom-rounded'>
            <%--<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">--%>
            <div id='announcements' class='announcements shadow mb10 p10'>
                <div class='m10'>
                    <p class='font-18'>ANNOUNCEMENTS!</p>
                </div>
                <div class="block-body">
                    <p class='font-14'>Welcome to the new eBill, it's now more customized and personal. Please take your time going through your personal analytics and have a look at our new personal management tools.</p>
                </div>
            </div>

            <div class='clear h15'></div>

            <div id='user-phone-calls-history-block' class='block float-left w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:GridPanel
                        ID="PhoneCallsHistoryGrid"
                        runat="server"
                        Title="Phone Calls History"
                        Width="465"
                        Height="240"
                        AutoScroll="true"
                        Header="true"
                        Scroll="Both"
                        Layout="FitLayout">

                        <Store>
                            <ext:Store
                                ID="PhoneCallsHistoryStore"
                                runat="server"
                                IsPagingStore="true">
                                <Model>
                                    <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                        <Fields>
                                            <ext:ModelField Name="SessionIdTime" Type="String" />
                                            <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                            <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                            <ext:ModelField Name="Duration" Type="Float" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
                            <Columns>
                                <ext:Column
                                    ID="SessionIdTime"
                                    runat="server"
                                    Text="Date"
                                    Width="160"
                                    DataIndex="SessionIdTime">
                                    <Renderer Fn="myDateRenderer" />
                                </ext:Column>
                                <ext:Column
                                    ID="Marker_CallToCountry"
                                    runat="server"
                                    Text="Country"
                                    Width="60"
                                    DataIndex="Marker_CallToCountry"
                                    Align="Center" />
                                <ext:Column
                                    ID="DestinationNumberUri"
                                    runat="server"
                                    Text="Destination"
                                    Width="140"
                                    DataIndex="DestinationNumberUri" />
                                <ext:Column
                                    ID="Duration"
                                    runat="server"
                                    Text="Duration"
                                    Width="100"
                                    DataIndex="Duration">
                                    <Renderer Fn="GetMinutes" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
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
                    <%--<a href='User_ManagePhoneCalls.aspx' class='font-10'>view more >></a>--%>
                </div>
            </div>

            <div class='clear h15'></div>

            <div id='user-phone-calls-Chart-block' class='block float-left w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:Panel ID="PhoneCallsChartPanel" 
            runat="server"
            Title="Phone Calls Chart"
            Width="465"
            Height="400"
            Layout="FitLayout">
            <Items>
                <ext:Chart 
                     ID="PhoneCallsChart" 
                    runat="server"
                    Animate="true"
                    Shadow="true"
                    InsetPadding="20"
                    Width="465"
                    Height="465"
                    Theme="Base:gradients">
                    <LegendConfig Position="Right" />
                   <Store>
                        <ext:Store ID="PhoneCallsChartStore" 
                            runat="server" 
                            >                           
                            <Model>
                                <ext:Model ID="PhoneCallsChartModel" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="TotalCalls" />
                                         <ext:ModelField Name="TotalCost" />
                                         <ext:ModelField Name="TotalDuration" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <Series>
                        <ext:PieSeries 
                            AngleField="TotalCost" 
                            ShowInLegend="true" 
                            Donut="30" 
                            Highlight="true" 
                            HighlightSegmentMargin="10">
                            <Label Field="Name" Display="Rotate" Contrast="true" Font="16px Arial" />
                            <Tips runat="server" TrackMouse="true" Width="140" Height="28">
                                <Renderer Fn="tipRenderer" />
                            </Tips>
                        </ext:PieSeries>
                    </Series>
                </ext:Chart>
            </Items>
        </ext:Panel>
                </div>
                <div class="clear"></div>
                <div class='more-button wauto float-right'>
                  <%--  <a href='#' class='font-10'>view more >></a>--%>
                </div>
            </div>

            <div id='history-block-3' class='block float-right w49p'>
                <div class='content wauto float-left mb10'>
                    <ext:Panel ID="Panel2"
                        runat="server"
                        Height="240"
                        Width="465"
                        Layout="AccordionLayout"
                        Title="Something">

                    </ext:Panel>
                </div>
                <div class="clear"></div>
                <div class='more-button wauto float-right'>
                    <a href='#' class='font-10'>view more >></a>
                </div>
            </div>
            <%--</asp:Content>--%>
        </div>

        <div class='clear h10'></div>

        <div id='footer' class='footer'>
        </div>
    </form>
</body>
</html>


<%--
    <%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.User_Dashboard" %>
    <asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server"></asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server"></asp:Content>
--%>
