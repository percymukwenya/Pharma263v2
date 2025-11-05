using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Pharma263.Application.Models
{
    public class SalesReport
    {
        #region Declaration

        private int _totalColumn = 8;
        private Document _document;
        private Font _fontStyle;
        private Font _fontStyleNormal;
        private PdfPTable _pdfPTable;
        private PdfPCell _pdfPCell;
        private MemoryStream _memoryStream;
        private PdfWriter writer;

        private PdfPTable paymentTable;
        private PdfPTable table;

        public SaleReportViewModel _model;

        public SalesReport()
        {

            paymentTable = new PdfPTable(2);
            _pdfPTable = new PdfPTable(_totalColumn);
            _memoryStream = new MemoryStream();
            table = new PdfPTable(3);

        }
        #endregion
        public byte[] CreateReport(SaleReportViewModel model)
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
            _pdfPTable.SetWidths(new float[] { 40f, 140f, 70f, 70f, 70f, 70f, 70f, 70f });

            paymentTable.WidthPercentage = 100;
            paymentTable.HorizontalAlignment = Element.ALIGN_LEFT;
            paymentTable.SetWidths(new float[] { 100f, 100f });

            table.WidthPercentage = 100;

            reportHeader();
            reportBody();

            _pdfPTable.HeaderRows = 2;
            _document.Add(paymentTable);
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
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Email, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();


            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.VATNumber, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Phone, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();



            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Address, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();


            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("TO : ", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("INVOICE :  ", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);


            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.CustomerName, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.Id.ToString("D6"), _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.CustomerPhone, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.SalesDate.DateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.CustomerAddress, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

        }
        private void reportBody()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();



            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("PRODUCT LIST PURCHASED BY CUSTOMER", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            #region TableHeader

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("SL No", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Item", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Batch #", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Expiry Date", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Price", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Quantity", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Discount", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Amount", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfPCell);


            _pdfPTable.CompleteRow();
            #endregion

            #region Table body
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            int sl = 0;
            decimal totalDiscount = 0;
            foreach (var item in _model.Sale.Items)
            {
                ++sl;
                totalDiscount += item.Discount;
                _pdfPCell = new PdfPCell(new Phrase(sl.ToString(), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(item.MedicineName, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(item.BatchNo, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(item.ExpiryDate.DateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + item.Price.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(item.Quantity.ToString(), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + item.Discount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + item.Amount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfPCell);
                _pdfPTable.CompleteRow();

            }

            _pdfPCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfPCell.Colspan = 7;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();


            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase("Total :", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfPCell.Colspan = 7;
            _pdfPCell.Border = 0;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + _model.Sale.Total.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.Border = 0;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();


            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase("Discount :", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfPCell.Colspan = 7;
            _pdfPCell.Border = 0;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + totalDiscount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.Border = 0;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();



            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Grand Total :", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfPCell.Colspan = 7;
            _pdfPCell.Border = 0;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Company.Currency + _model.Sale.GrandTotal.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.Border = 0;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Status  :", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfPCell.Colspan = 7;
            _pdfPCell.Border = 0;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.SaleStatus, _fontStyle));
            _pdfPCell.Border = 0;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            #endregion          

            // Add an empty row
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();


            PdfPTable nestedTable = new PdfPTable(2);
            nestedTable.WidthPercentage = 100;

            // Add the "RETURNS POLICY" cell
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfPCell returnPolicyCell = new PdfPCell(new Phrase("RETURNS POLICY", _fontStyle));
            returnPolicyCell.HorizontalAlignment = Element.ALIGN_CENTER;
            returnPolicyCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            returnPolicyCell.Border = Rectangle.RIGHT_BORDER; // Add a left border
            returnPolicyCell.PaddingRight = 5f; // Add padding to separate from the border
            returnPolicyCell.PaddingLeft = 5f;
            nestedTable.AddCell(returnPolicyCell);

            // Add the "Banking Details" cell
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfPCell bankingDetailsCell = new PdfPCell(new Phrase("Banking Details", _fontStyle));
            bankingDetailsCell.HorizontalAlignment = Element.ALIGN_CENTER;
            bankingDetailsCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            bankingDetailsCell.Border = Rectangle.LEFT_BORDER; // Add a right border
            bankingDetailsCell.PaddingLeft = 5f; // Add padding to separate from the border
            bankingDetailsCell.PaddingRight = 5f;
            nestedTable.AddCell(bankingDetailsCell);

            nestedTable.CompleteRow();

            // Add the content for "Return Policy"
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string returnPolicyContent = !string.IsNullOrEmpty(_model.Company.ReturnsPolicy)
                ? _model.Company.ReturnsPolicy
                : "1. Any returns must be within seven (7) days from date of invoice\n"
                + "2. Returned goods must be undamaged, in original packaging with no broken seals.\n"
                + "3. No returns on cold chain products";

            PdfPCell returnPolicyContentCell = new PdfPCell(new Phrase(returnPolicyContent, _fontStyle));
            returnPolicyContentCell.Colspan = 2; // This cell spans one column
            returnPolicyContentCell.Border = 0;
            nestedTable.AddCell(returnPolicyContentCell);

            // Add the content for "Banking Details"
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string bankingDetailsContent = !string.IsNullOrEmpty(_model.Company.BankingDetails)
                ? _model.Company.BankingDetails  // New lines in string will be preserved
                : "Account Name: Langson Legacy \n"
                  + "FBC N. Mandela Branch (USD Nostro) Acc No. 6870288590122\n"
                  + " \n"
                  + "NMB Avondale Banch (zwl) Acc No. 00280593346 ";

            PdfPCell bankingDetailsContentCell = new PdfPCell(new Phrase(bankingDetailsContent, _fontStyle));
            bankingDetailsContentCell.Colspan = 2; // This cell spans one column
            bankingDetailsContentCell.Border = 0;
            bankingDetailsContentCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nestedTable.AddCell(bankingDetailsContentCell);

            nestedTable.CompleteRow();

            // Add the nested table
            PdfPCell nestedTableCell = new PdfPCell(nestedTable);
            nestedTableCell.Colspan = _totalColumn;
            nestedTableCell.BorderWidthBottom = 1f; // Add a bottom border
            nestedTableCell.PaddingBottom = 5f; // Add padding to separate from main content
            _pdfPTable.AddCell(nestedTableCell);
            _pdfPTable.CompleteRow();

            // Add an empty row
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Add the "Signature" row
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Signature", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

        }
    }
}
