using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Persistence
    {
        int ID { set; get; }
        string Module { set; get; }
        string ModuleKey { set; get; }
        string ModuleValue { set; get; }

        private static DBLib DBRoutines = new DBLib();

        public List<Persistence> GetDefinitions() 
        {
            Persistence definition;
            DataTable dt = new DataTable();
            List<Persistence> definitions = new List<Persistence>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName));

            foreach (DataRow row in dt.Rows)
            {
                definition = new Persistence();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Persistence.ID) && row[column.ColumnName] != System.DBNull.Value)
                        definition.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Persistence.Module) && row[column.ColumnName] != System.DBNull.Value)
                        definition.Module = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Persistence.ModuleKey) && row[column.ColumnName] != System.DBNull.Value)
                        definition.ModuleKey = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Persistence.ModuleValue) && row[column.ColumnName] != System.DBNull.Value)
                        definition.ModuleValue = (string)row[column.ColumnName];
                }
                definitions.Add(definition);
            }

            return definitions;
        }

        public Dictionary<string, string> GetDefinitionsDict() 
        {
            Persistence definition;
            DataTable dt = new DataTable();
            Dictionary<string, string> definitions = new Dictionary<string, string>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName));

            foreach (DataRow row in dt.Rows)
            {
                definition = new Persistence();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Persistence.ModuleKey) && row[column.ColumnName] != System.DBNull.Value)
                        definition.ModuleKey = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Persistence.ModuleValue) && row[column.ColumnName] != System.DBNull.Value)
                        definition.ModuleValue = (string)row[column.ColumnName];
                }
                definitions.Add(ModuleKey,ModuleValue);
            }

            return definitions;
        }

        public int InsertDefention(Persistence defention) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((defention.Module).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Persistence.Module), defention.Module);

            if ((defention.ModuleKey).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Persistence.ModuleKey), defention.ModuleKey);

            if ((defention.ModuleValue).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Persistence.ModuleValue), defention.ModuleValue);


            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Sites.TableName), columnsValues, Enums.GetDescription(Enums.Sites.SiteID));

            return rowID;
        }

        public void UpdateDefinition(Persistence defention) 
        {
            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((defention.Module).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Persistence.Module), defention.Module);

            if (defention.ModuleKey != null)
                setPart.Add(Enums.GetDescription(Enums.Persistence.ModuleKey), defention.ModuleKey);

            if (defention.ModuleValue != null)
                setPart.Add(Enums.GetDescription(Enums.Persistence.ModuleValue), defention.ModuleValue);

            //Execute Update
            DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Persistence.TableName),
                setPart,
                Enums.GetDescription(Enums.Persistence.ID),
                defention.ID);
        }

       
    }
}