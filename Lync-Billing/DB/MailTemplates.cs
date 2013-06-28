using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class MailTemplates
    {
        public int TemplateID { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<MailTemplates> GetMailTemplates() 
        {
            MailTemplates mailTemplate;
            DataTable dt = new DataTable();
            List<MailTemplates> mailTemplates = new List<MailTemplates>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.MailTemplates.TableName));

            foreach (DataRow row in dt.Rows)
            {
                mailTemplate = new MailTemplates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.MailTemplates.TemplateID) && row[column.ColumnName] != System.DBNull.Value)
                        mailTemplate.TemplateID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MailTemplates.TemplateSubject) && row[column.ColumnName] != System.DBNull.Value)
                        mailTemplate.TemplateSubject = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.MailTemplates.TemplateBody) && row[column.ColumnName] != System.DBNull.Value)
                        mailTemplate.TemplateBody = (string)row[column.ColumnName];
                }
                mailTemplates.Add(mailTemplate);
            }
            return mailTemplates;
        }

        public static MailTemplates GetMailTemplates(int templateID)
        {
            MailTemplates mailTemplate = new MailTemplates();
            DataTable dt = new DataTable();
            List<MailTemplates> mailTemplates = new List<MailTemplates>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.MailTemplates.TableName),
                                    Enums.GetDescription(Enums.MailTemplates.TemplateID),templateID);
            
            DataRow row = dt.Rows[0];

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == Enums.GetDescription(Enums.MailTemplates.TemplateID))
                    mailTemplate.TemplateID = (int)row[column.ColumnName];

                if (column.ColumnName == Enums.GetDescription(Enums.MailTemplates.TemplateSubject))
                    mailTemplate.TemplateSubject = (string)row[column.ColumnName];

                if (column.ColumnName == Enums.GetDescription(Enums.MailTemplates.TemplateBody))
                    mailTemplate.TemplateBody = (string)row[column.ColumnName];
            }
            
            return mailTemplate;
        }
    }
}