using iTextSharp.text;
using iTextSharp.text.pdf;
using Pharma263.MVC.Services.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Pharma263.MVC.Services
{
    public class PdfReportService : IPdfReportService
    {
        // Standard colors for consistent branding
        private readonly BaseColor _primaryColor = new BaseColor(102, 126, 234); // #667eea
        private readonly BaseColor _headerColor = new BaseColor(248, 249, 250); // #f8f9fa
        private readonly BaseColor _textColor = new BaseColor(44, 62, 80); // #2c3e50

        // Fonts - initialized with methods to avoid field reference issues
        private Font GetTitleFont() => FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, _textColor);
        private Font GetHeaderFont() => FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
        private Font GetNormalFont() => FontFactory.GetFont(FontFactory.HELVETICA, 10, _textColor);
        private Font GetSmallFont() => FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);

        public byte[] GenerateStandardReport<T>(string title, IEnumerable<T> data, string startDate = null, string endDate = null, Dictionary<string, string> customColumns = null)
        {
            var (document, stream) = CreateStandardDocument();
            
            try
            {
                document.Open();
                
                // Add standard branding and header
                AddLogo(document);
                AddTitleAndDates(document, title, startDate, endDate);
                
                // Convert data to table format
                var properties = typeof(T).GetProperties();
                var headers = customColumns?.Values.ToArray() ?? properties.Select(p => p.Name).ToArray();
                var rows = new List<string[]>();
                
                foreach (var item in data)
                {
                    var row = new string[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(item);
                        row[i] = FormatValue(value);
                    }
                    rows.Add(row);
                }
                
                // Add table to document
                var table = CreateStandardTable(headers, rows);
                document.Add(table);
                
                // Add footer
                AddFooter(document);
                
                document.Close();
                return stream.ToArray();
            }
            finally
            {
                document?.Close();
                stream?.Dispose();
            }
        }

        public byte[] GenerateTableReport(string title, string[] headers, List<string[]> rows, string startDate = null, string endDate = null)
        {
            var (document, stream) = CreateStandardDocument();
            
            try
            {
                document.Open();
                
                // Add standard branding and header
                AddLogo(document);
                AddTitleAndDates(document, title, startDate, endDate);
                
                // Add table to document
                var table = CreateStandardTable(headers, rows);
                document.Add(table);
                
                // Add footer
                AddFooter(document);
                
                document.Close();
                return stream.ToArray();
            }
            finally
            {
                document?.Close();
                stream?.Dispose();
            }
        }

        public (Document document, MemoryStream stream) CreateStandardDocument()
        {
            var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 40, 40, 60, 60); // margins: left, right, top, bottom
            PdfWriter.GetInstance(document, stream);
            return (document, stream);
        }

        public void AddLogo(Document document)
        {
            try
            {
                string imageName = "pharma263Logo.jpg";
                string resourceNamespace = "Pharma263.MVC.Images";
                string imagePath = $"{resourceNamespace}.{imageName}";

                using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(imagePath))
                {
                    if (resourceStream != null)
                    {
                        Image img = Image.GetInstance(resourceStream);
                        img.ScaleToFit(100f, 100f);
                        img.SpacingAfter = 15f;
                        img.SpacingBefore = 5f;
                        img.Alignment = Element.ALIGN_LEFT;
                        document.Add(img);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue - don't fail PDF generation for missing logo
                System.Diagnostics.Debug.WriteLine($"Failed to add logo: {ex.Message}");
            }
        }

        public void AddTitleAndDates(Document document, string title, string startDate = null, string endDate = null)
        {
            // Company header
            var companyParagraph = new Paragraph("Pharma263 Management System", GetTitleFont());
            companyParagraph.Alignment = Element.ALIGN_CENTER;
            companyParagraph.SpacingAfter = 10f;
            document.Add(companyParagraph);

            // Report title
            var titleParagraph = new Paragraph(title, GetTitleFont());
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph.SpacingAfter = 20f;
            document.Add(titleParagraph);

            // Date information table for better alignment
            var dateTable = new PdfPTable(2);
            dateTable.WidthPercentage = 100;
            dateTable.DefaultCell.Border = Rectangle.NO_BORDER;
            dateTable.DefaultCell.PaddingBottom = 8f;

            // Generated date
            var generatedCell = new PdfPCell(new Phrase($"Generated: {DateTime.Now:MMM dd, yyyy HH:mm}", GetNormalFont()));
            generatedCell.Border = Rectangle.NO_BORDER;
            generatedCell.HorizontalAlignment = Element.ALIGN_LEFT;
            dateTable.AddCell(generatedCell);

            // Date range (if provided)
            string dateRangeText = "";
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                dateRangeText = $"Period: {startDate} - {endDate}";
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                dateRangeText = $"As of: {startDate}";
            }

            var periodCell = new PdfPCell(new Phrase(dateRangeText, GetNormalFont()));
            periodCell.Border = Rectangle.NO_BORDER;
            periodCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            dateTable.AddCell(periodCell);

            dateTable.SpacingAfter = 20f;
            document.Add(dateTable);
        }

        public PdfPTable CreateStandardTable(string[] headers, List<string[]> rows)
        {
            var table = new PdfPTable(headers.Length);
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;

            // Add headers with consistent styling
            foreach (var header in headers)
            {
                var headerCell = new PdfPCell(new Phrase(header, GetHeaderFont()));
                headerCell.BackgroundColor = _primaryColor;
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerCell.Padding = 8f;
                headerCell.Border = Rectangle.NO_BORDER;
                table.AddCell(headerCell);
            }

            // Add data rows with alternating colors
            bool isEvenRow = false;
            foreach (var row in rows)
            {
                foreach (var cell in row)
                {
                    var dataCell = new PdfPCell(new Phrase(cell ?? "", GetNormalFont()));
                    dataCell.BackgroundColor = isEvenRow ? BaseColor.WHITE : _headerColor;
                    dataCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    dataCell.Padding = 6f;
                    dataCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(dataCell);
                }
                isEvenRow = !isEvenRow;
            }

            return table;
        }

        private void AddFooter(Document document)
        {
            // Add some spacing before footer
            document.Add(new Paragraph(" ") { SpacingBefore = 20f });

            // Footer text
            var footer = new Paragraph($"© {DateTime.Now.Year} Pharma263 Management System - Confidential Report", GetSmallFont());
            footer.Alignment = Element.ALIGN_CENTER;
            footer.SpacingBefore = 10f;
            document.Add(footer);
        }

        private string FormatValue(object value)
        {
            if (value == null) return "";
            
            return value switch
            {
                DateTime date => date.ToString("MMM dd, yyyy"),
                decimal money => money.ToString("C2"),
                double number => number.ToString("N2"),
                float number => number.ToString("N2"),
                bool boolean => boolean ? "Yes" : "No",
                _ => value.ToString()
            };
        }
    }
}