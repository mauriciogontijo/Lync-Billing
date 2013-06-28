﻿using System;
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

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Pools.TableName));

            foreach (DataRow row in dt.Rows)
            {
                mailTemplate = new MailTemplates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Pools.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        pool.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Pools.PoolFQDN) && row[column.ColumnName] != System.DBNull.Value)
                        pool.PoolFQDN = (string)row[column.ColumnName];
                }
                mailTemplates.Add(mailTemplate);
            }
            return mailTemplates;
        }

        public static MailTemplates GetMailTemplates(int templateID)
        {

        }

        
    }
}