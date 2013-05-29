<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridTest3.aspx.cs" Inherits="Lync_Billing.UI.GridTest3" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<?xml version="1.1" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
        <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
        <link rel="stylesheet" type="text/css" href="../css/green-layout.css" />
        <link rel="stylesheet" type="text/css" href="../css/green-layout-ie-8.css" />
        <script type="text/javascript" src="../js/browserdetector.js"></script>

        <script type="text/javascript">
            BrowserDetect.init();

            var myDateRenderer = function (value) {
                if (typeof value != undefined && value != 0) {
                    if (BrowserDetect.browser != "Explorer") {
                        value = Ext.util.Format.date(value, "d M Y h:i A");
                        return value;
                    } else {
                        var my_date = {};
                        var value_array = value.split(' ');
                        var months = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

                        my_date["date"] = value_array[0];
                        my_date["time"] = value_array[1];

                        var date_parts = my_date["date"].split('-');
                        my_date["date"] = {
                            year: date_parts[0],
                            month: months[parseInt(date_parts[1])],
                            day: date_parts[2]
                        }

                        var time_parts = my_date["time"].split(':');
                        my_date["time"] = {
                            hours: time_parts[0],
                            minutes: time_parts[1],
                            period: (time_parts[0] < 12 ? 'AM' : 'PM')
                        }

                        //var date_format = Date(my_date["date"].year, my_date["date"].month, my_date["date"].day, my_date["time"].hours, my_date["time"].minutes);
                        return (
                            my_date.date.day + " " + my_date.date.month + " " + my_date.date.year + " " +
                            my_date.time.hours + ":" + my_date.time.minutes + " " + my_date.time.period
                        );
                    }//END ELSE
                }//END OUTER IF
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
                return hours + ':' + minutes + ':' + seconds;
            }

            var GetHoursFromMinutes = function (value) {
                var sec_num = parseInt(value, 10);
                var hours = Math.floor(sec_num / 60);
                var minutes = Math.floor((sec_num - (hours * 60)));
                return hours + "." + minutes;
            };
        </script>
    </head>

    <body>
        <ext:ResourceManager id="resourceManager" runat="server" Theme="Gray" />
        <form id="form1" runat="server">
            <div id='main' class='main bottom-rounded'>
                <div id='announcements' class='announcements shadow mb20 p10'>
                    <div class='m10'>
                        <p class='font-18'>ANNOUNCEMENTS!</p>
                    </div>
                    <div class="block-body">
                        <p class='font-14'>Welcome to the new eBill, it's now more customized and personal. Please take your time going through your personal analytics and have a look at our new personal management tools.</p>
                    </div>
                </div>

                <div class='clear h15'></div>

                <div style="float: right; width: 49%; overflow: hidden; display: block; height: auto; min-height: 650px;">
                    <div id='Div1' class='block wauto'>
                        <div class='content wauto float-left mb10'>
                            <ext:Panel ID="UserPhoneCallsSummary"
                                runat="server"         
                                Height="200" 
                                Width="350"
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
                    </div>

                    <div class="clear h5"></div>

                    <%--<asp:PlaceHolder ID="UserPhoneCallsHistoryPH" runat="server">
                    </asp:PlaceHolder>--%>

                    <div id='brief-history-block' class='block wauto'>
                        <div class='content wauto float-left mb10'>
                            <ext:GridPanel
                                ID="PhoneCallsHistoryGrid"
                                runat="server"
                                Title="History Brief"
                                Width="465"
                                Height="210"
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

                                <TopBar>
                                    <ext:Toolbar ID="CallsHistoryGridToolbar" runat="server">
                                        <Items>
                                            <ext:Button ID="HistoryGridViewMore" runat="server" Text="View More..." Icon="ApplicationGo" Margins="0 0 0 365">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                            </ext:GridPanel>
                        </div><!-- END OF CONTENT -->
                    </div><!-- END OF BLOCk -->
                </div>
            </div>
        </form>
    </body>
</html>
