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
                    Paragraph titleParagraph = new Paragraph("eBill | " + headers["title"], titleFont);
                    titleParagraph.SpacingAfter = 5;
                    document.Add(titleParagraph);
                }
                if (headers.ContainsKey("subTitle"))
                {
                    Paragraph subTitleParagraph;
                    if (headers.ContainsKey("siteName"))
                    {
                        subTitleParagraph = new Paragraph(headers["siteName"] + " site | " + headers["subTitle"], subTitleFont);
                    }
                    else
                    {
                        subTitleParagraph = new Paragraph(headers["subTitle"], subTitleFont);
                    }
                    subTitleParagraph.SpacingAfter = 5;
                    document.Add(subTitleParagraph);
                }
                if (headers.ContainsKey("comments"))
                {
                    var commentsParagraph = new Paragraph(headers["comments"], headerCommentsFont);
                    commentsParagraph.SpacingBefore = 10;
                    commentsParagraph.SpacingAfter = 5;
                    document.Add(commentsParagraph);
                }
            }

            return document;
        }

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt) 
        {
            string cellText = string.Empty;

            foreach (DataColumn c in dt.Columns)
            {
                pdfTable.AddCell(new Phrase(PDFDefinitions.GetDescription(c.ColumnName), boldTableFont));
            }

            foreach (DataRow r in dt.Rows)
            {
                foreach(DataColumn column in dt.Columns)
                {
                    //Declare the pdfTable cell and fill it.
                    PdfPCell entryCell;
                    
                    //Check if the cell being processed in not  empty nor null.
                    cellText = r[column.ColumnName].ToString();
                    if (string.IsNullOrEmpty(cellText))
                        cellText = "N/A";

                    //Format the cell text if it's the case of Duration
                    if (PDFDefinitions.GetDescription(column.ColumnName) == "Duration" && cellText != "N/A")
                    {
                        entryCell = new PdfPCell(new Phrase(Misc.ConvertSecondsToReadable(Convert.ToInt32(cellText)), bodyFontSmall));
                    }
                    else
                    {
                        entryCell = new PdfPCell(new Phrase(cellText, bodyFontSmall));
                    }

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

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt, List<string> columnSchema)
        {
            if(columnSchema != null && columnSchema.Count > 0)
            {
                foreach (string column in columnSchema)
                {
                    if (dt.Columns.Contains(column))
                    {
                        pdfTable.AddCell(new Phrase(PDFDefinitions.GetDescription(column), boldTableFont));
                    }
                }

                foreach (DataRow r in dt.Rows)
                {
                    //foreach (DataColumn column in dt.Columns)
                    foreach(string column in columnSchema)
                    {
                        //Declare the pdfTable cell and fill it.
                        PdfPCell entryCell;

                        //Format the cell text if it's the case of Duration
                        if (dt.Columns.Contains(column))
                        {
                            if (PDFDefinitions.GetDescription(column) == "Duration")
                            {
                                entryCell = new PdfPCell(new Phrase(Misc.ConvertSecondsToReadable(Convert.ToInt32(r[column])), bodyFontSmall));
                            }
                            else
                            {
                                string rowText = r[column].ToString();

                                if (string.IsNullOrEmpty(rowText))
                                    rowText = "N/A";

                                entryCell = new PdfPCell(new Phrase(rowText, bodyFontSmall));
                            }

                            //Set the cell padding, border configurations and then add it to the the pdfTable
                            entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                            entryCell.PaddingTop = 5;
                            entryCell.PaddingBottom = 5;
                            entryCell.PaddingLeft = 2;
                            entryCell.PaddingRight = 2;
                            pdfTable.AddCell(entryCell);
                        }
                    }
                }

                // Add the pdf-contents-table object to the document
                document.Add(pdfTable);
            }

            return document;
        }

        public static Document AddCombinedPDFTablesContents(ref Document document, DataTable dt, int[] pdfReportColumnsWidths, List<string> handles)
        {
            string cellText = string.Empty;
            DataRow[] selectedDataRows;
            string selectExpression = string.Empty;
            handles.Sort();

            if(handles != null && handles.Count > 0)
            {
                foreach(string handleItem in handles)
                {
                    PdfPTable pdfTable = InitializePDFTable(dt.Columns.Count, pdfReportColumnsWidths);
                    document.NewPage();

                    Paragraph pageTitleParagraph = new Paragraph(handleItem.Split('@')[0].ToUpper(), subTitleFont);
                    pageTitleParagraph.SpacingAfter = 25;
                    document.Add(pageTitleParagraph);

                    selectExpression = "SourceUserUri = '" + handleItem + "'";
                    selectedDataRows = dt.Select(selectExpression);

                    foreach (DataColumn c in dt.Columns)
                    {
                        pdfTable.AddCell(new Phrase(PDFDefinitions.GetDescription(c.ColumnName), boldTableFont));
                    }

                    foreach (DataRow r in selectedDataRows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            //Declare the pdfTable cell and fill it.
                            PdfPCell entryCell;

                            //Check if the cell being processed in not  empty nor null.
                            cellText = r[column.ColumnName].ToString();
                            if (string.IsNullOrEmpty(cellText))
                                cellText = "N/A";

                            //Format the cell text if it's the case of Duration
                            if (PDFDefinitions.GetDescription(column.ColumnName) == "Duration" && cellText != "N/A")
                            {
                                entryCell = new PdfPCell(new Phrase(Misc.ConvertSecondsToReadable(Convert.ToInt32(cellText)), bodyFontSmall));
                            }
                            else
                            {
                                entryCell = new PdfPCell(new Phrase(cellText, bodyFontSmall));
                            }

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
                    selectExpression = string.Empty;
                    Array.Clear(selectedDataRows, 0, selectedDataRows.Length);
                }
            }
            return document;
        }

        public static Document AddCombinedPDFTablesContents(ref Document document, DataTable dt, int[] pdfReportColumnsWidths, string handleName, List<string> handles, List<string> pdfReportColumnScheme, string siteName = "")
        {
            DataRow[] selectedDataRows;
            string selectExpression = string.Empty;
            string pageTitleText = string.Empty;
            string cellText = string.Empty;
            Paragraph pageTitleParagraph;
            ADUserInfo userInfo = new ADUserInfo();

            //Exit the function in case the handles array is empty or the pdfReportColumnScheme is either empty or it's size exceeds the DataTable's Columns number.
            if (handles == null || handles.Count == 0 || pdfReportColumnScheme == null || pdfReportColumnScheme.Count == 0 || pdfReportColumnScheme.Count > dt.Columns.Count)
            {
                return document;
            }
            else
            {
                //start by sorting the handles array.
                handles.Sort();

                //Get the users information in case we are dealing with a Users Related Data Table.
                Dictionary<string, Dictionary<string, string>> usersInformation = new Dictionary<string, Dictionary<string, string>>();
                if (handleName == "SourceUserUri")
                {
                    //List<Users> usersList = Users.GetUsers(siteName);
                    Dictionary<string, string> info;
                    List<string> columns = new List<string>();
                    Dictionary<string, object> where = new Dictionary<string, object>() { { "AD_PhysicalDeliveryOfficeName", siteName } };
                    List<Users> usersList = Users.GetUsers(columns, where, 0);

                    foreach(Users user in usersList)
                    {
                        info = new Dictionary<string, string>() { 
                            {"FullName", user.FullName},
                            {"ID", user.UserID.ToString()}
                        };

                        usersInformation.Add(user.SipAccount.ToLower(), info);
                    }
                }

                //Begin the construction of the document.
                foreach (string handleItem in handles)
                {
                    PdfPTable pdfTable = InitializePDFTable(pdfReportColumnScheme.Count, pdfReportColumnsWidths);
                    document.NewPage();

                    if (handleName == "SourceUserUri")
                        pageTitleText = usersInformation[handleItem.ToLower()]["FullName"] + " [Group Number: " + usersInformation[handleItem.ToLower()]["ID"] + "]";
                    else
                        pageTitleText = handleItem;

                    pageTitleParagraph = new Paragraph(pageTitleText, subTitleFont);
                    pageTitleParagraph.SpacingAfter = 20;
                    document.Add(pageTitleParagraph);

                    //Select the rows that are associated to the supplied handles
                    selectExpression = handleName + " = '" + handleItem + "'";
                    selectedDataRows = dt.Select(selectExpression);

                    //Print the report table columns headers
                    foreach (string column in pdfReportColumnScheme)
                    {
                        if (dt.Columns.Contains(column))
                        {
                            pdfTable.AddCell(new Phrase(PDFDefinitions.GetDescription(column), boldTableFont));
                        }
                    }

                    //Bind the data cells to the respective columns
                    foreach (DataRow r in selectedDataRows)
                    {
                        foreach (string column in pdfReportColumnScheme)
                        {
                            if (dt.Columns.Contains(column))
                            {
                                //Declare the pdfTable cell and fill it.
                                PdfPCell entryCell;

                                //Check if the cell being processed in not  empty nor null.
                                cellText = r[column].ToString();
                                if (string.IsNullOrEmpty(cellText))
                                    cellText = "N/A";

                                //Format the cell text if it's the case of Duration
                                if (PDFDefinitions.GetDescription(column) == "Duration" && cellText != "N/A")
                                {
                                    entryCell = new PdfPCell(new Phrase(Misc.ConvertSecondsToReadable(Convert.ToInt32(cellText)), bodyFontSmall));
                                }
                                else
                                {
                                    entryCell = new PdfPCell(new Phrase(cellText, bodyFontSmall));
                                }

                                //Set the cell padding, border configurations and then add it to the the pdfTable
                                entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                                entryCell.PaddingTop = 5;
                                entryCell.PaddingBottom = 5;
                                entryCell.PaddingLeft = 2;
                                entryCell.PaddingRight = 2;
                                pdfTable.AddCell(entryCell);
                            }
                        }
                    }

                    // Add the Paragraph object to the document
                    document.Add(pdfTable);
                    selectExpression = string.Empty;
                    Array.Clear(selectedDataRows, 0, selectedDataRows.Length);
                }
            }
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


        public static IElement titleParagraph { get; set; }
    }
}