﻿<%@ Page Language="C#" MasterPageFile="~/ui/AccountingMaster.Master" AutoEventWireup="true" CodeBehind="disputes.aspx.cs" Inherits="Lync_Billing.ui.accounting.disputes" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Accounting Mainpage</title>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#navigation-tabs>li.selected').removeClass('selected');
            $('#accounting-tab').addClass('selected');
        });

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

        function RoundCost(value, meta, record, rowIndex, colIndex, store) {
            return Math.round(record.data.Marker_CallCost * 100) / 100;
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

        function getRowClassForstatus(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.AC_DisputeStatus == 'Rejected') {
                meta.style = "color: rgb(201, 20, 20);";
            }
            if (record.data.AC_DisputeStatus == 'Accepted') {
                meta.style = "color: rgb(46, 143, 42);";
            }
            return value
        }

        var submitValue = function (grid, hiddenFormat, format) {
            grid.submitData(false, { isUpload: true });
        };



    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->

    <div id='Div1' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="AccountingToolsSidebar"
                runat="server"
                Height="340"
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
                            <p><a href='../accounting/disputes.aspx' class="selected">Manage Disputed Calls</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/users/monthly.aspx'>Monthly Users Reports</a></p>
                            <p><a href='../accounting/users/periodical.aspx'>Periodical Users Reports</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reportss</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='../accounting/sites/monthly.aspx'>Monthly Sites Reports</a></p>
                            <p><a href='../accounting/sites/periodical.aspx'>Periodical Sites Reports</a></p>
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='Div2' class='block float-right wauto h100p'>
        <div class="block-body pt5">
             <ext:GridPanel
                ID="ManageDisputesGrid"
                runat="server"
                Title="Manage Disputes"
                Width="740"
                Height="750"
                AutoScroll="true"
                Header="true"
                Scroll="Both"
                Layout="FitLayout">
                
                <Store>
                    <ext:Store 
                        ID="DisputesStore" 
                        runat="server" 
                        IsPagingStore="true" 
                        PageSize="25"
                        GroupField="SourceUserUri"
                        OnLoad="DisputesStore_Load"
                        OnSubmitData="DisputesStore_SubmitData"
                        OnReadData="DisputesStore_ReadData">
                        <Model>
                            <ext:Model ID="DisputesModel" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                    <ext:ModelField Name="SourceUserUri" Type="String" />
                                    <ext:ModelField Name="ResponseTime" Type="String"/>
                                    <ext:ModelField Name="SessionEndTime" Type="String"/>
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_IsDispute" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                    <ext:ModelField Name="AC_DisputeStatus" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="DisputesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="150"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="myDateRenderer" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="Marker_CallToCountry"
                            Align="Center" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="120"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="80"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>

                        <ext:Column
                            ID="UI_MarkedOn"
                            runat="server"
                            Text="Marked On"
                            Width="100"
                            DataIndex="UI_MarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                        </ext:Column>

                         <ext:Column
                            ID="AC_DisputeStatus"
                            runat="server"
                            Text="Status"
                            Width="90"
                            DataIndex="AC_DisputeStatus">
                             <Renderer fn="getRowClassForstatus" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="DisputesCheckboxSelectionModel" 
                        runat="server" 
                        Mode="Multi" 
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="DisputesToolbar" runat="server">
                        <Items>
                           <ext:Label
                               runat="server"
                               ID="button_group_lable"
                               Margins="5 0 0 5"
                               Width="90">
                                <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup ID="DisputesMarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="250"
                                Frame="false"
                                ButtonAlign="Left">
                                <Buttons>
                                    <ext:Button ID="Accepted" Text="Accepted" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AcceptDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManageDisputesGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Reject" Text="Rejected" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="RejectDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManageDisputesGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>
                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 310">
                                 <Listeners>
                                    <Click Handler="submitValue(#{ManageDisputesGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="DisputesPagingToolbar"
                        runat="server"
                        StoreID="DisputesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Disputes {0} - {1} of {2}" />
                </BottomBar>
                 <Features>               
                   <ext:Grouping ID="Grouping1" 
                    runat="server" 
                    HideGroupedHeader="false" 
                    EnableGroupingMenu="false"
                    GroupHeaderTplString='{name} : ({[values.rows.length]} {[values.rows.length > 1 ? "Disputes" : "Dispute"]})'/>
                 </Features>

            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>