using System;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data;
using System.Web;
using Lync_Billing.DB;

namespace Lync_Billing.Libs
{
    public class PDFLib 
    {
        public static Document CreatePDF(DataTable dt,HttpResponse response) 
        {
            Document document = new Document();
            
            string path = HttpRuntime.AppDomainAppPath;


            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path.ToString() + @"\Exported.pdf", FileMode.Create));
            PdfWriter writer = PdfWriter.GetInstance(document, response.OutputStream);
            document.Open();
            document.AddHeader("header", "semsem wa7ad");

            Font font5 = FontFactory.GetFont(FontFactory.HELVETICA, 5);

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            PdfPRow row = null;
            
            //float[] widths = new float[] { 4f, 4f, 4f, 4f };
            //table.SetWidths(widths);

            table.WidthPercentage = 100;
            int iCol = 0;
            string colname = "";
            PdfPCell cell = new PdfPCell(new Phrase("PhoneCalls"));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(PDFDefinitions.GetDescription(c.ColumnName), font5));
            }

            foreach (DataRow r in dt.Rows)
            {
                foreach(DataColumn colum in dt.Columns)
                {
                        table.AddCell(new Phrase(r[colum.ColumnName].ToString(),font5));
                }
            } document.Add(table);

            return document;
            //document.Close();
                      

        }

    }
}