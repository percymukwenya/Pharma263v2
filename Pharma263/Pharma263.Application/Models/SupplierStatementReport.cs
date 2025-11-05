using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Pharma263.Application.Models
{
    public class SupplierStatementReport
    {
        #region Declaration

        private int _totalColumn = 6;
        private Document _document;
        private Font _fontStyle;
        private Font _fontStyleNormal;
        private PdfPTable _pdfPTable;
        private PdfPCell _pdfPCell;
        private MemoryStream _memoryStream;
        private PdfWriter writer;

        private PdfPTable headerTable;
        private PdfPTable summaryTable;

        public SupplierStatementViewModel _model;

        public SupplierStatementReport()
        {
            headerTable = new PdfPTable(2);
            _pdfPTable = new PdfPTable(_totalColumn);
            summaryTable = new PdfPTable(4);
            _memoryStream = new MemoryStream();
        }
        #endregion

        public byte[] CreateReport(SupplierStatementViewModel model)
        {
            _model = model;

            #region page
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfPTable.WidthPercentage = 100;
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            writer = PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfPTable.SetWidths(new float[] { 80f, 120f, 120f, 80f, 80f, 100f });

            headerTable.WidthPercentage = 100;
            headerTable.HorizontalAlignment = Element.ALIGN_LEFT;
            headerTable.SetWidths(new float[] { 100f, 100f });

            summaryTable.WidthPercentage = 100;
            summaryTable.HorizontalAlignment = Element.ALIGN_LEFT;
            summaryTable.SetWidths(new float[] { 100f, 100f, 100f, 100f });

            reportHeader();
            accountSummary();
            reportBody();

            _pdfPTable.HeaderRows = 2;
            _document.Add(headerTable);
            _document.Add(summaryTable);
            _document.Add(_pdfPTable);
            _document.Close();
            return _memoryStream.ToArray();

            #endregion
        }

        private void reportHeader()
        {
            string imageName = "pharma263Logo.jpg";
            string resourceNamespace = "Pharma263.Application.Images";
            string imagePath = $"{resourceNamespace}.{imageName}";

            using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(imagePath))
            {
                if (resourceStream != null)
                {
                    Image img = Image.GetInstance(resourceStream);
                    img.ScaleToFit(100f, 100f);
                    img.SpacingAfter = 10f;
                    img.SpacingBefore = 5f;
                    img.Alignment = Element.ALIGN_LEFT;
                    _document.Add(img);
                }
            }

            _fontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.StoreName, _fontStyle));
            _pdfPCell.Colspan = 2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Email, _fontStyle));
            _pdfPCell.Colspan = 2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Phone + " | " + _model.Company.Address, _fontStyle));
            _pdfPCell.Colspan = 2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();

            // Empty space
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = 2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();

            // Statement Title
            _fontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _pdfPCell = new PdfPCell(new Phrase("SUPPLIER ACCOUNT STATEMENT", _fontStyle));
            _pdfPCell.Colspan = 2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 5;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();

            // Supplier Information
            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("SUPPLIER:", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("STATEMENT PERIOD:", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Statement.SupplierName, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            var periodText = $"{_model.Statement.StatementPeriod.StartDate:dd/MM/yyyy} - {_model.Statement.StatementPeriod.EndDate:dd/MM/yyyy}";
            _pdfPCell = new PdfPCell(new Phrase(periodText, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Statement.Email + " | " + _model.Statement.Phone, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase($"Generated: {_model.Statement.GeneratedDate:dd/MM/yyyy HH:mm}", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            headerTable.AddCell(_pdfPCell);
            headerTable.CompleteRow();
        }

        private void accountSummary()
        {
            // Empty space
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = 4;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            summaryTable.AddCell(_pdfPCell);
            summaryTable.CompleteRow();

            // Summary Header
            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("ACCOUNT SUMMARY", _fontStyle));
            _pdfPCell.Colspan = 4;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);
            summaryTable.CompleteRow();

            // Summary Values
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Total Purchases", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Total Payments", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Amount Owed", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Transactions", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            summaryTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + _model.Statement.TotalCredits.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + _model.Statement.TotalDebits.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            var balanceColor = _model.Statement.CurrentBalance > 0 ? BaseColor.RED : BaseColor.BLACK;
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _fontStyle.Color = balanceColor;
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + _model.Statement.CurrentBalance.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Statement.Transactions.Count.ToString(), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            _pdfPCell.Padding = 5f;
            summaryTable.AddCell(_pdfPCell);

            summaryTable.CompleteRow();
        }

        private void reportBody()
        {
            // Empty space
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Transaction History Title
            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("TRANSACTION HISTORY", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPCell.Padding = 5f;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            #region TableHeader

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Date", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Type", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Description", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Purchases", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Payments", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Balance Owed", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();
            #endregion

            #region Table body
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            var sortedTransactions = _model.Statement.Transactions.OrderBy(t => t.TransactionDate).ToList();

            foreach (var transaction in sortedTransactions)
            {
                _pdfPCell = new PdfPCell(new Phrase(transaction.TransactionDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(transaction.TransactionType, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(transaction.Description, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                var creditText = transaction.CreditAmount > 0 ? _model.Company.Currency + transaction.CreditAmount.ToString("### ###0.00", CultureInfo.InvariantCulture) : "-";
                _pdfPCell = new PdfPCell(new Phrase(creditText, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                var debitText = transaction.DebitAmount > 0 ? _model.Company.Currency + transaction.DebitAmount.ToString("### ###0.00", CultureInfo.InvariantCulture) : "-";
                _pdfPCell = new PdfPCell(new Phrase(debitText, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                var balanceColor = transaction.RunningBalance > 0 ? BaseColor.RED : BaseColor.BLACK;
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _fontStyle.Color = balanceColor;
                _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + transaction.RunningBalance.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);
                _pdfPTable.CompleteRow();

                // Reset font style
                _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            }

            if (!_model.Statement.Transactions.Any())
            {
                _pdfPCell = new PdfPCell(new Phrase("No transactions found for the selected period.", _fontStyle));
                _pdfPCell.Colspan = _totalColumn;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.Padding = 10f;
                _pdfPTable.AddCell(_pdfPCell);
                _pdfPTable.CompleteRow();
            }

            #endregion

            // Footer note
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _fontStyle.Color = BaseColor.GRAY;
            var footerText = "For any queries regarding this statement, please contact us at " + _model.Company.Email + " or " + _model.Company.Phone;
            _pdfPCell = new PdfPCell(new Phrase(footerText, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.Padding = 10f;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();
        }
    }
}
