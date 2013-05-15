<%@ Page Title="" Language="C#" CodeBehind="GridTest.aspx.cs" Inherits="Lync_Billing.UI.GridTest" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html>
    <head id="Head1" runat="server">
    <title>GridPanel with ObjectDataSource - Ext.NET Examples</title>
    
         <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
        <script type="text/javascript" src="js/browserdetector.js"></script>
        <script type="text/javascript" src="js/toolkit.js"></script>
    <style>
        .x-grid-cell-fullName .x-grid-cell-inner {
            font-family : tahoma, verdana;
            display     : block;
            font-weight : normal;
            font-style  : normal;
            color       : #385F95;
            white-space : normal;
        }
        
        .x-grid-rowbody div {
            margin : 2px 5px 20px 5px !important;
            width  : 99%;
            color  : Gray;
        }
        
        .x-grid-row-expanded td.x-grid-cell{
            border-bottom-width:0px;
        }
    </style>
    <ext:XScript ID="XScript1" runat="server">
        <script type="text/javascript">
            debugger;
            var applyFilter = function (field) {
                if(field){
                    var id = field.id,
                        task = new Ext.util.DelayedTask(function(){
                            var f = Ext.getCmp(id);
                            f.focus();
                            f.el.dom.value = f.el.dom.value;
                        });
                    task.delay(100);
                }
                #{PhoneCallsHistoryGrid}.getStore().filterBy(getRecordFilter());                                
            };
 
            var clearFilter = function () {                 
                #{PhoneCallsHistoryGrid}.getStore().clearFilter();
            }
            
            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                if (typeof val != "string") {
                    return value.length == 0;
                }
 
                var retValue = value!=undefined && val.toLowerCase().indexOf(value.toLowerCase()) > -1;
                return retValue;
            };
  
            var filterDate = function (value, dataIndex, record) {
                var val = record.get(dataIndex).clearTime(true).getTime();
  
                if (!Ext.isEmpty(value, false) && val != value.clearTime(true).getTime()) {
                    return false;
                }
                return true;
            };
  
            var filterNumber = function (value, dataIndex, record) {
                var val = record.get(dataIndex);                
  
                if (!Ext.isEmpty(value, false) && val != value) {
                    return false;
                }
                return true;
            };
 
            var getRecordFilter = function () {
                var f = [];
 
                f.push({
                    filter: function (record) {
                        var FilterValue = #{FilterTypeComboBox}.getValue();
                        switch(FilterValue)
                        {
                            case 4:
                                return filterString('NO', 'UI_IsPersonal', record);
                                break;
                            case 5:
                                return filterString('YES', 'UI_IsPersonal', record);
                                break;
                            default:
                                return filterString(#{FilterTypeComboBox}.getValue(), 'UI_IsPersonal', record);
                        }
                        
                    }
                });
 
                var len = f.length;
                  
                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            };
        </script>
    </ext:XScript>
</head>
<body>
    <form id="Form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:GridPanel 
            ID="PhoneCallsHistoryGrid" 
            runat="server" 
            Title="Phone Calls History"
            Width="745"
            Height="450"  
            AutoScroll="true"
            Header="true"
            Scroll="Both" 
            Layout="FitLayout">

            <Store>
             <ext:Store ID="PhoneCallStore" runat="server" IsPagingStore="true"  PageSize="25"
                OnAfterRecordUpdated="PhoneCallStore_AfterRecordUpdated"
                OnAfterStoreChanged="PhoneCallStore_AfterStoreChanged"
                OnAfterDirectEvent="PhoneCallStore_AfterDirectEvent"
                OnBeforeDirectEvent="PhoneCallStore_BeforeDirectEvent"
                OnBeforeRecordUpdated="PhoneCallStore_BeforeRecordUpdated"
                OnBeforeStoreChanged="PhoneCallStore_BeforeStoreChanged">
                <Model>
                    <ext:Model ID="Model1" runat="server" IDProperty="PhoneCallModel">
                        <Fields>
                            <ext:ModelField Name="SessionIdTime" Type="Date" />
                            <ext:ModelField Name="marker_CallToCountry" Type="String" />
                            <ext:ModelField Name="DestinationNumberUri" Type="String" />
                            <ext:ModelField Name="Duration" Type="Float" />
                            <ext:ModelField Name="marker_CallCost"  Type="Float" />
                            <ext:ModelField Name="UI_IsPersonal" Type="String" />
                            <ext:ModelField Name="UI_MarkedOn" Type="Date" />
                            <ext:ModelField Name="UI_IsPersonal" Type="String" />
                        </Fields>
                 </ext:Model>
               </Model>
            </ext:Store>
         </Store>
            <ColumnModel ID="PhoneCallsColumnModel" runat="server">
		        <Columns>
                    <ext:Column ID="SessionIdTime" 
                        runat="server" 
                        Text="Date" 
                        Width="80" 
                        DataIndex="SessionIdTime" 
                        Resizable="false" 
                        MenuDisabled="true">
                       
                        <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                    </ext:Column>

                    <ext:Column ID="marker_CallToCountry"
                        runat="server"
                        Text="Country Code"
                        Width="80"
                        DataIndex="DestinationNumberUri Code" />

                    <ext:Column ID="DestinationNumberUri"
                        runat="server"
                        Text="Destination"
                        Width="130"
                        DataIndex="DestinationNumberUri" />

                    <ext:Column ID="Duration"
                        runat="server"
                        Text="Duration"
                        Width="70"
                        DataIndex="Duration" />

                    <ext:Column ID="marker_CallCost"
                        runat="server"
                        Text="Cost"
                        Width="70"
                        DataIndex="marker_CallCost" />

                    <ext:Column ID="UI_IsPersonal"
                        runat="server"
                        Text="Is Personal"
                        Width="100"
                        DataIndex="UI_IsPersonal" />

                    <ext:Column ID="UI_MarkedOn"
                        runat="server"
                        Text="Updated On"
                        Width="80"
                        DataIndex="UI_MarkedOn" />

                    <ext:Column ID="UI_IsInvoiced"
                        runat="server"
                        Text="Billing Status"
                        Width="90"
                        DataIndex="UI_IsInvoiced" />
		        </Columns>
            </ColumnModel>
             <TopBar>
                  <ext:Toolbar ID="FilterToolBar" runat="server">
                     <Items>
                      <ext:ComboBox 
                            ID="FilterTypeComboBox" 
                            runat="server" 
                            Icon="Find" 
                            TriggerAction="All" 
                            QueryMode="Local" 
                            DisplayField="TypeName" 
                            ValueField="TypeValue">
                            <Items>
                                <ext:ListItem Text="Everything" Value="1"/>
                                <ext:ListItem Text="Unmarked" Value="2" />
                                <ext:ListItem Text="Marked" Value="3" />
                                <ext:ListItem Text="Business" Value="4" />
                                <ext:ListItem Text="Personal" Value="5" />
                                <ext:ListItem Text="Charged" Value="6" />
                                <ext:ListItem Text="Uncharged" Value="7" />
                            </Items>
                             <Listeners>
                                <Select Handler="applyFilter(this);" />
                            </Listeners>
                           <%-- <DirectEvents>
                                <Change OnEvent="FilterTypeChange"></Change>
                            </DirectEvents>--%>
                        </ext:ComboBox>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolbar 
                    ID="PhoneCallsPagingToolbar" 
                    runat="server" 
                    StoreID="PhoneCallStore" 
                    DisplayInfo="true" 
                    Weight="25" 
                    DisplayMsg="Phone Calls {0} - {1} of {2}"
                     />
            </BottomBar>
                    
            <SelectionModel>
                <ext:CheckboxSelectionModel ID="PhoneCallsCheckBoxColumn" runat="server" Mode="Multi" />
            </SelectionModel>            
                    
            <Buttons>
                <ext:Button ID="GridSubmitChanges" runat="server" Text="Save Changes">
                    <DirectEvents>
                        <Click OnEvent="GridSubmitChanges_Click">
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
            
        </ext:GridPanel>
    </form>

</body>
</html>