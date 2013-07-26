using System;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data;
using System.Web;
using System.Collections.Generic;
using Lync_Billing.DB;

namespace Lync_Billing.Libs
{
    public class PDFLib 
    {
        private static Font titleFont = FontFactory.GetFont("Arial", 20, Font.BOLD);
        private static Font subTitleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        private static Font headerCommentsFont = FontFactory.GetFont("Arial", 9, Font.ITALIC);
        private static Font boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        private static Font endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
        private static Font bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
        private static Font bodyFontSmall = FontFactory.GetFont("Arial", 10, Font.NORMAL);

        

        public static Document InitializePDFDocument(HttpResponse response)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, response.OutputStream);
            document.Open();

            return document;
        }

        public static PdfPTable InitializePDFTable(int ColumnsCount, int[] widths)
        {
            //Create the actual data table
            PdfPTable pdfTable = new PdfPTable(ColumnsCount);
            pdfTable.HorizontalAlignment = 0;
            pdfTable.SpacingBefore = 30;
            pdfTable.SpacingAfter = 30;
            pdfTable.DefaultCell.Border = 0;
            //pdfTable.DefaultCell.Padding = 2;
            pdfTable.DefaultCell.PaddingBottom = 5;
            pdfTable.DefaultCell.PaddingTop = 5;
            pdfTable.DefaultCell.PaddingLeft = 2;
            pdfTable.DefaultCell.PaddingRight = 2;
            pdfTable.WidthPercentage = 100;
            if (widths.Length > 0 && widths.Length == ColumnsCount)
                pdfTable.SetWidths(widths);
            //else
            //    pdfTable.SetWidths(new int[] { 7, 4, 7, 4, 4 });

            return pdfTable;
        }

        public static Document AddPDFHeader(ref Document document, Dictionary<string, string> headers)
        {
            if (headers.Count > 0)
            {
                if (headers.ContainsKey("title"))
                {
                    var titleParagraph = new Paragraph("eBill | " + headers["title"], titleFont);
                    titleParagraph.SpacingAfter = 5;
                    document.Add(titleParagraph);
                }
                if (headers.ContainsKey("subTitle"))
                {
                    var subTitleParagraph = new Paragraph(headers["subTitle"], subTitleFont);
                    subTitleParagraph.SpacingAfter = 5;
                    document.Add(subTitleParagraph);
                }
                if (headers.ContainsKey("comments"))
                {
                    var commentsParagraph = new Paragraph(headers["comments"], headerCommentsFont);
                    commentsParagraph.SpacingAfter = 5;
                    document.Add(commentsParagraph);
                }
            }

            return document;
        }

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt) 
        {
            foreach (DataColumn c in dt.Columns)
            {
                pdfTable.AddCell(new Phrase(PDFDefinitions.GetDescription(c.ColumnName), boldTableFont));
            }

            foreach (DataRow r in dt.Rows)
            {
                foreach(DataColumn colum in dt.Columns)
                {
                    //Declare the pdfTable cell and fill it.
                    PdfPCell entryCell;

                    //Format the cell text if it's the case of Duration
                    if(PDFDefinitions.GetDescription(colum.ColumnName) == "Duration")
                        entryCell = new PdfPCell(new Phrase(Misc.ConvertSecondsToReadable(Convert.ToInt32(r[colum.ColumnName])), bodyFontSmall));
                    else
                        entryCell = new PdfPCell(new Phrase(r[colum.ColumnName].ToString(), bodyFontSmall));

                    //Set the cell padding, border configurations and then add it to the the pdfTable
                    entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                    entryCell.PaddingTop = 5;
                    entryCell.PaddingBottom = 5;
                    entryCell.PaddingLeft = 2;
                    entryCell.PaddingRight = 2;
                    pdfTable.AddCell(entryCell);
                }
            }

            // Add the Paragraph object to the document
            document.Add(pdfTable);
            return document;
        }

        public static Document AddPDFTableTotalsRow(ref Document document, Dictionary<string, object> totals, DataTable dt, int[] widths)
        {
            PdfPTable pdfTable = new PdfPTable(dt.Columns.Count);
            pdfTable.HorizontalAlignment = 0;
            pdfTable.DefaultCell.Border = 0;
            pdfTable.DefaultCell.PaddingBottom = 5;
            pdfTable.DefaultCell.PaddingTop = 5;
            pdfTable.DefaultCell.PaddingLeft = 2;
            pdfTable.DefaultCell.PaddingRight = 2;
            pdfTable.WidthPercentage = 100;
            if (widths.Length > 0 && widths.Length == dt.Columns.Count)
                pdfTable.SetWidths(widths);
            //else
            //    pdfTable.SetWidths(new int[] { 7, 4, 7, 4, 4 });

            foreach(DataColumn column in dt.Columns)
            {
                if (dt.Columns[0].ColumnName == column.ColumnName)
                {
                    pdfTable.AddCell(new Phrase("Total", boldTableFont));
                }
                else if (totals.ContainsKey(column.ColumnName))
                {
                    pdfTable.AddCell(new Phrase(totals[column.ColumnName].ToString(), boldTableFont));
                }
                else
                {
                    pdfTable.AddCell(new Phrase(string.Empty, boldTableFont));
                }
            }

            document.Add(pdfTable);
            return document;
        }

        public static void ClosePDFDocument(ref Document document)
        {
            document.Close();
        }

    }
}