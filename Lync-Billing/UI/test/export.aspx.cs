﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Xml.Serialization;
using iTextSharp.text;

namespace Lync_Billing.ui.test
{
    public partial class export : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();

        private string sipAccount = string.Empty;
        private string pageData = string.Empty;
        private string PhoneCallsPerPage = string.Empty;
        private StoreReadDataEventArgs e;

        [XmlArray("records")]
        [XmlArrayItem("record")]
        private List<PhoneCall> PhoneCallsList = new List<PhoneCall>();

        string xmldoc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            sipAccount = "AAlhour@ccc.gr";
        }


        protected void getPhoneCalls(bool force = false)
        {
            sipAccount = "AAlhour@ccc.gr";
            
            if (PhoneCallsList == null || PhoneCallsList.Count == 0 || force == true)
            {
                PhoneCallsList = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null).ToList();
                xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(PhoneCallsList);
            }
        }

        protected void PhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (this.e.Start != -1)
                e.InputParameters["start"] = this.e.Start;
            else
                e.InputParameters["start"] = 0;

            if (this.e.Limit != -1)
                e.InputParameters["limit"] = this.e.Limit;
            else
                e.InputParameters["limit"] = 25;

            if (!string.IsNullOrEmpty(this.e.Parameters["sort"]))
                e.InputParameters["sort"] = this.e.Sort[0];
            else
                e.InputParameters["sort"] = null;

            if (!string.IsNullOrEmpty(this.e.Parameters["filter"]))
                e.InputParameters["filter"] = this.e.Filter[0];
            else
                e.InputParameters["filter"] = null;
        }

        protected void PhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.PhoneCallsStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        protected void PhoneCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.FormatType.Value.ToString();

            XmlNode xml = e.Xml;

            this.Response.Clear();

            switch (format)
            {
                case "xls":
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "pdf":
                    sipAccount = "AAlhour@ccc.gr";

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    Dictionary<string, string> headers = new Dictionary<string,string>()
                    {
                        {"title", "Ahmad Alhour (#595233)"},
                        {"subtitle", "History of Phone Calls"}
                    };

                    Document doc = new Document();
                    //PhoneCall.ExportUserPhoneCalls(sipAccount, Response, out doc, headers);

                    Response.Write(doc);
                    
                    break;
            }

            this.Response.End();
        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            PhoneCallsStore.DataBind();
            PhoneCallsPerPage = PhoneCallsStore.JsonData;
        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            sipAccount = "AAlhour@ccc.gr";
            getPhoneCalls();

            IQueryable<PhoneCall> result;

            if (filter == null)
                result = PhoneCallsList.Where(phoneCall => phoneCall.UI_CallType == null).AsQueryable();
            else
                result = PhoneCallsList.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();

            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                Expression<Func<PhoneCall, object>> sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.Property(param, sort.Property), param);
                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    result = result.OrderByDescending(sortExpression);
                else
                    result = result.OrderBy(sortExpression);
            }

            int resultCount = result.Count();

            if (start >= 0 && limit > 0)
                result = result.Skip(start).Take(limit);

            count = resultCount;

            return result.ToList();
        }

        [DirectMethod]
        protected void PhoneCallsHistoryFilter(object sender, DirectEventArgs e)
        {
            PhoneCallsStore.ClearFilter();

            if (FilterTypeComboBox.SelectedItem.Value != "Unmarked")
                PhoneCallsStore.Filter("UI_CallType", FilterTypeComboBox.SelectedItem.Value);

            PhoneCallsStore.LoadPage(1);
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManagePhoneCallsGrid.GetStore().RejectChanges();
        }

    }
}