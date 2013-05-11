using System;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace Lync_Billing.Libs
{
    public class PDFLib 
    {
        StyleSheet styles = new StyleSheet();
        string fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\verdana.ttf";
        //FontFactory fontFactory = b

        public string WriteToPDF(string htmlString) 
        {
            return null;
        }

    }
}