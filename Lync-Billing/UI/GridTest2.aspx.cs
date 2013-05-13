using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lync_Billing.DB;
using Ext.Net;

namespace Lync_Billing.UI
{
    public partial class GridTest2 : System.Web.UI.Page
    {
        GridPanel grid;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string sipAccount = "SGhaida@ccc.gr";
            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri",sipAccount);
            columns.Add("SessionIdTime");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");

           //Store store = DBStores.PhoneCallsStore(columns,wherePart,10);

            Ext.Net.Panel panel = new Ext.Net.Panel();
            panel.ID = "GridPanel";

            grid = Grids.PhoneCallsGrid(columns, wherePart, 7);
            grid.SelectionModel.Add(new RowSelectionModel()
            {
                AllowDeselect = true,
                Mode = SelectionMode.Multi
            });
          
            //Grid editor plugin : enables editing
            grid.Plugins.Add(new CellEditing() 
            {
                ClicksToEdit = 2
            });
            Ext.Net.TextField textField = new TextField();
            textField.ID = "SessionTime";
            //Add Editor to SessionTime 
            //grid.ColumnModel.Columns[2].Editor.Add(textField);
            //Formating Date
            grid.ColumnModel.Columns[0].Renderer.Handler = "return Ext.util.Format.date(value, 'Y-m-d');";
            
            grid.ID = "PhoneCallsGrid";
            grid.Layout = "Table";
            //grid.SetSize(400, 300);
            grid.Width = 465;
            grid.ColumnModel.Columns[0].Text = "Date";
            grid.ColumnModel.Columns[1].Text = "Destination";
            grid.ColumnModel.Columns[1].Width = 165;

            grid.ColumnModel.Columns[3].Text = "Cost";
            grid.Header = true;
            grid.Title = "Calls History";

            //Table Layout modifications


            //Ext.Net.ComboBox markingStatus = new ComboBox();
            //markingStatus.ID = "markingStatus";

            //markingStatus.FieldLabel = "MarkingStatus";
            //markingStatus.DisplayField = "MarkingStatus";
            //markingStatus.ValueField = "MarkingValue";
            //markingStatus.Store.Add(DBStores.PhoneCallMarkingStore());
            //markingStatus.QueryMode = DataLoadMode.Local;
            //markingStatus.TypeAhead = true;
            //markingStatus.TriggerAction = TriggerAction.All;

            //Toolbar toolbar = new Toolbar();
            //toolbar.ID = "Toolbar";

            //Ext.Net.Button save = new Ext.Net.Button();
            //save.Selectable = true;
            //save.ID = "Save";
            //save.Text = "Save";
            //save.DirectClick +=save_DirectClick;

            //toolbar.Add(markingStatus);
            //grid.TopBar.Add(toolbar);
            //grid.Buttons.Add(save);
            //panel.Add(grid);
            //viewport.Add(panel);
            //form1.Controls.Add(viewport);
            ph.Controls.Add(grid);
            
        }

        private void save_DirectClick(object sender, DirectEventArgs e)
        {
            (grid.GetStore()).Sync();
            //(grid.GetStore()).CommitChanges();
            
            
        }
         public string formatDateTime(string value) {
             return "2013-10-10";
            }
      
     }
}