﻿<%@ Page Language="C#" MasterPageFile="~/UI/MasterPage.Master" AutoEventWireup="true" CodeBehind="Accounting_Dashboard.aspx.cs" Inherits="Lync_Billing.UI.Accounting_Dashboard" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <title>eBill | Accounting Mainpage</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content_place_holder" runat="server">
    <!-- *** START OF SIDEBAR *** -->
    <div id='Div1' class='sidebar block float-left w20p'>
        <div class="block-body">
            <ext:Panel ID="SidebarPanel"
                runat="server"
                Height="380"
                Title="User Tools">
                <Content>
                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Pages</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_Dashboard.aspx' class='selected'>Accounting Dashboard</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate User Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            <p><a href='Accounting_MonthlyUserReport.aspx'>Monthly Users Report</a></p>
                            <p><a href='Accounting_PeriodicalUserReport.aspx'>Periodical Users Report</a></p>
                            <p><a href='Accounting_ValidateCycle.aspx'>Validate Accounting Cycle</a></p>
                        </div>
                    </div>

                    <div class='sidebar-section'>
                        <div class="sidebar-section-header">
                            <p>Generate Site Reports</p>
                        </div>
                        <div class="sidebar-section-body">
                            
                        </div>
                    </div>
                </Content>
            </ext:Panel>
        </div>
    </div>
    <!-- *** END OF SIDEBAR *** -->


    <!-- *** START OF ACCOUNTING MAIN BODY *** -->
    <div id='Div2' class='block float-right w80p h100p'>
        <div class="block-body pt5">
            <p class="font-18">Generate Periodical User Reports!</p>
        </div>
    </div>
    <!-- *** END OF ACCOUNTING MAIN BODY *** -->
</asp:Content>