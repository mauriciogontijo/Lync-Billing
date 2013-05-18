﻿<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="User_ManagePhoneCalls.aspx.cs" Inherits="Lync_Billing.UI.User_ManagePhoneCalls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>eBill | User Tools</title>

    <style type="text/css">
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell {
            height: 25px !important;
        }

        .row-green {
            background-color: rgb(46, 143, 42);
        }

        .row-red {
            background-color: rgb(201, 20, 20);
        }

        .row-yellow {
            background-color: yellow;
        }
        /* end manage-phone-calls grid styling */
    </style>

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

        function RoundCost(value, meta, record, rowIndex, colIndex, store) {

            return Math.round(record.data.Marker_CallCost * 100) / 100;
        }

        //Manage-Phone-Calls Grid JavaScripts
        var myDateRenderer = function (value) {
            value = Ext.util.Format.date(value, "d M Y h:i A");
            return value;
        }

        function getRowClassForIsPersonal(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.UI_IsPersonal == 'YES' || record.data.UI_IsPersonal == 'Yes') {
                meta.style = "color: rgb(201, 20, 20);";
            }
            if (record.data.UI_IsPersonal == 'NO' || record.data.UI_IsPersonal == 'No') {
                meta.style = "color: rgb(46, 143, 42);";
            }
            return value
        }


        function getRowClassForIsInvoiced(value, meta, record, rowIndex, colIndex, store) {
            if (record.data.UI_IsInvoiced == 'Pending' || record.data.UI_IsInvoiced == 'PENDING') {
                meta.style = "color: rgb(201, 20, 20);";
            }
            if (record.data.UI_IsInvoiced == 'Charged' || record.data.UI_IsInvoiced == 'CHARGED') {
                meta.style = "color: rgb(46, 143, 42);";
            }
            return value
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

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='sidebar' class='sidebar block float-left w20p'>
        <div class='block-header top-rounded bh-shadow'>
            <p class='font-1-2-em bold'>User Management</p>
        </div>
        <div class='block-body bottom-rounded bb-shadow'>
            <div class='wauto float-left mb15'>
                <p class='section-header'>Manage</p>
                <p class='section-item'><a href='User_ManagePhoneCalls.aspx'>Business Personal</a></p>
                <p class='section-item'><a href='#'>Delegates</a></p>
                 <p class='section-item'><a href='#'>Address Book</a></p>
                
            </div>

            <div class='wauto float-left mb15'>
                <p class='section-header'>History</p>
                <p class='section-item'><a href='User_ViewHistory.aspx'>Phone Calls History</a></p>
            </div>

            <div class='wauto float-left mb15'>
                <p class='section-header'>Statistics</p>
                <p class='section-item'><a href='#'>Phone Calls Statistics</a></p>
            </div>

            <div class='clear h5'></div>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->

    <!-- *** START OF MANAGE PHONE CALLS GRID *** -->
    <div id='manage-phone-calls-block' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <ext:GridPanel
                ID="ManagePhoneCallsGrid"
                runat="server"
                Title="Manage Phone Calls"
                Width="740"
                Height="750"
                AutoScroll="true"
                Header="true"
                Scroll="Both"
                Layout="FitLayout">

                <Store>
                    <ext:Store 
                        ID="PhoneCallsStore" 
                        runat="server" 
                        IsPagingStore="true" 
                        PageSize="25"
                        OnSubmitData="PhoneCallsStore_SubmitData"
                        OnReadData="PhoneCallsStore_ReadData">
                        <Model>
                            <ext:Model ID="Model2" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="SessionIdTime" Type="String" />
                                    <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                    <ext:ModelField Name="ResponseTime" Type="String"/>
                                    <ext:ModelField Name="SessionEndTime" Type="String"/>
                                    <ext:ModelField Name="Marker_CallToCountry" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="Marker_CallCost" Type="Float" />
                                    <ext:ModelField Name="UI_IsPersonal" Type="String" />
                                    <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                                    <ext:ModelField Name="UI_IsPersonal" Type="String" />
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
                            Text="Country Code"
                            Width="90"
                            DataIndex="Marker_CallToCountry"
                            Align="Center" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="130"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="70"
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

                        <ext:Column ID="UI_IsPersonal"
                            runat="server"
                            Text="Is Personal"
                            Width="80"
                            DataIndex="UI_IsPersonal">
                            <Renderer Fn="getRowClassForIsPersonal" />
                        </ext:Column>

                        <ext:Column
                            ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="100"
                            DataIndex="UI_MarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" Mode="Multi"></ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                           <ext:Label runat="server" ID="button_group_lable" Margin="5">
                                <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup ID="MarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="250"
                                Frame="false"
                                ButtonAlign="Right">
                                <Buttons>
                                    <ext:Button ID="Business" Text="Business" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignBusiness">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Personal" Text="Personal" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignPersonal">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="Dispute" Text="Dispute" runat="server">
                                        <DirectEvents>
                                            <Click OnEvent="AssignDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManagePhoneCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>
                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 290">
                                 <Listeners>
                                    <Click Handler="submitValue(#{ManagePhoneCallsGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PhoneCallStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Phone Calls {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
    <!-- *** END OF MANAGE PHONE CALLS GRID *** -->
</asp:Content>
