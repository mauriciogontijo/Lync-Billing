﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;


namespace Lync_Billing.UI
{
    public static class Grids
    {
        public static GridPanel PhoneCallsGrid(List<string> columns, Dictionary<string, object> wherePart, int limits) 
        {
            GridPanel grid;
            if (columns == null)
            {
                grid = new GridPanel()
                {
                    Store = { DBStores.PhoneCallsStore(columns, wherePart, limits) },
                    ColumnModel =
                    {
                        Columns = 
                        {
                            new Column 
                            {
                                ID = "SessionIdTime",
                                Text = "SessionIdTime",
                                DataIndex = "SessionIdTime"
                            },
                            new Column 
                            {
                                ID = "SessionIdSeq",
                                Text = "SessionIdSeq",
                                DataIndex = "SessionIdSeq"
                            },
                            new Column 
                            {
                                ID = "ResponseTime",
                                Text = "ResponseTime",
                                DataIndex = "ResponseTime"
                            },
                            new Column 
                            {
                                ID = "SessionEndTime",
                                Text = "SessionEndTime",
                                DataIndex = "SessionEndTime"
                            },
                            new Column 
                            {
                                ID = "SourceUserUri",
                                Text = "SourceUserUri",
                                DataIndex = "SourceUserUri"
                            },
                            new Column 
                            {
                                ID = "SourceNumberUri",
                                Text = "SourceNumberUri",
                                DataIndex = "SourceNumberUri"
                            },
                            new Column 
                            {
                                ID = "DestinationNumberUri",
                                Text = "DestinationNumberUri",
                                DataIndex = "DestinationNumberUri"
                            },
                            new Column 
                            {
                                ID = "ServerFQDN",
                                Text = "ServerFQDN",
                                DataIndex = "ServerFQDN"
                            },
                            new Column 
                            {
                                ID = "PoolFQDN",
                                Text = "PoolFQDN",
                                DataIndex = "PoolFQDN"
                            },
                            new Column 
                            {
                                ID = "Marker_CallToCountry",
                                Text = "Marker_CallToCountry",
                                DataIndex = "Marker_CallToCountry"
                            },
                            new Column 
                            {
                                ID = "marker_CallType",
                                Text = "marker_CallType",
                                DataIndex = "marker_CallType"
                            },
                            new Column 
                            {
                                ID = "Duration",
                                Text = "Duration",
                                DataIndex = "Duration"
                            },
                            new Column 
                            {
                                ID = "Marker_CallCost",
                                Text = "Marker_CallCost",
                                DataIndex = "Marker_CallCost"
                            },
                            new Column 
                            {
                                ID = "UI_UpdatedByUser",
                                Text = "UI_UpdatedByUser",
                                DataIndex = "UI_UpdatedByUser"
                            },
                            new Column 
                            {
                                ID = "UI_MarkedOn",
                                Text = "UI_MarkedOn",
                                DataIndex = "UI_MarkedOn"
                            },
                            new Column 
                            {
                                ID = "UI_IsPersonal",
                                Text = "UI_IsPersonal",
                                DataIndex = "UI_IsPersonal"
                            },
                            new Column 
                            {
                                ID = "UI_Dispute",
                                Text = "UI_Dispute",
                                DataIndex = "UI_Dispute"
                            },
                            new Column 
                            {
                                ID = "UI_IsInvoiced",
                                Text = "UI_IsInvoiced",
                                DataIndex = "UI_IsInvoiced"
                            }
                        }
                    }
                };
            }
            else 
            {
                grid = new GridPanel();
                grid.Store.Add(DBStores.PhoneCallsStore(columns, wherePart, limits));
                foreach (string column in columns) 
                {
                    Column gridColumn = new Column();
                    gridColumn.ID=column;
                    gridColumn.DataIndex = column;
                    gridColumn.Text = column;

                    grid.ColumnModel.Columns.Add(gridColumn);
                }
            }

            return grid;
        } 
    }
}