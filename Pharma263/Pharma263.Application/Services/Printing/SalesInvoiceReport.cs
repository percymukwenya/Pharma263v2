using iTextSharp.text;
using iTextSharp.text.pdf;
using Pharma263.Application.Models;
using System.Globalization;

namespace Pharma263.Application.Services.Printing
{
    public class SalesInvoiceReport : BaseReport<SaleReportViewModel>
    {
        public SalesInvoiceReport() : base(totalColumns: 8)
        {
        }

        protected override float[] GetColumnWidths()
        {
            // Sales invoice columns: Item, Description, Batch, Exp Date, Qty, Unit Price, VAT, Total
            return new float[] { 40f, 140f, 70f, 70f, 70f, 70f, 70f, 70f };
        }

        protected override void BuildReportSpecificHeader()
        {
            // Add company information
            AddCompanyInfo(_model.Company);

            // Add spacing
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // TO and INVOICE headers (two-column layout like original)
            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("TO : ", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("INVOICE :  ", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Customer Name and Invoice ID
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.CustomerName, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.Id.ToString("D6"), _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Customer Phone and Date
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.CustomerPhone ?? "", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.SalesDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Customer Address (spans one column)
            _pdfPCell = new PdfPCell(new Phrase(_model.Sale.CustomerAddress ?? "", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
        }

        protected override void BuildTableHeaders()
        {
            // Add section title first
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("PRODUCT LIST PURCHASED BY CUSTOMER", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Add spacing
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Table headers
            _pdfPTable.AddCell(CreateHeaderCell("SL No"));
            _pdfPTable.AddCell(CreateHeaderCell("Item"));
            _pdfPTable.AddCell(CreateHeaderCell("Batch #"));
            _pdfPTable.AddCell(CreateHeaderCell("Exp Date"));
            _pdfPTable.AddCell(CreateHeaderCell("Unit Price"));
            _pdfPTable.AddCell(CreateHeaderCell("Qty"));
            _pdfPTable.AddCell(CreateHeaderCell("Discount"));
            _pdfPTable.AddCell(CreateHeaderCell("Total"));
        }

        protected override void BuildTableBody()
        {
            // Table rows
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            decimal grandTotal = 0;
            decimal totalDiscount = 0;
            int serialNumber = 1;

            foreach (var item in _model.Sale.Items)
            {
                _pdfPTable.AddCell(CreateStandardCell(serialNumber.ToString(), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(item.MedicineName ?? "", _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(item.BatchNo ?? "", _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(item.ExpiryDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(_model.Company.Currency + item.Price.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(item.Quantity.ToString(), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(_model.Company.Currency + item.Discount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(_model.Company.Currency + item.Amount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                
                totalDiscount += item.Discount;
                grandTotal += item.Amount;
                serialNumber++;
            }

            // Add empty row spacing like original
            _pdfPCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfPCell.Colspan = 7;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Summary rows using borderless style like original
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            
            // Total
            _pdfPTable.AddCell(CreateSummaryCell("Total :", _fontStyle, 7));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Company.Currency + _model.Sale.Total.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Discount
            _pdfPTable.AddCell(CreateSummaryCell("Discount :", _fontStyle, 7));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Company.Currency + totalDiscount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Grand Total (bold style)
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPTable.AddCell(CreateSummaryCell("Grand Total :", _fontStyle, 7));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Company.Currency + _model.Sale.GrandTotal.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Status (after Grand Total like original)
            _pdfPTable.AddCell(CreateSummaryCell("Status  :", _fontStyle, 7));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Sale.SaleStatus ?? "Completed", _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Add empty row
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Footer section - Returns Policy and Banking Details
            PdfPTable nestedTable = new PdfPTable(2);
            nestedTable.WidthPercentage = 100;

            // Returns Policy header
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfPCell returnPolicyCell = new PdfPCell(new Phrase("RETURNS POLICY", _fontStyle));
            returnPolicyCell.HorizontalAlignment = Element.ALIGN_CENTER;
            returnPolicyCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            returnPolicyCell.Border = Rectangle.RIGHT_BORDER;
            returnPolicyCell.PaddingRight = 5f;
            returnPolicyCell.PaddingLeft = 5f;
            nestedTable.AddCell(returnPolicyCell);

            // Banking Details header
            PdfPCell bankingDetailsCell = new PdfPCell(new Phrase("Banking Details", _fontStyle));
            bankingDetailsCell.HorizontalAlignment = Element.ALIGN_CENTER;
            bankingDetailsCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            bankingDetailsCell.Border = Rectangle.LEFT_BORDER;
            bankingDetailsCell.PaddingLeft = 5f;
            bankingDetailsCell.PaddingRight = 5f;
            nestedTable.AddCell(bankingDetailsCell);
            nestedTable.CompleteRow();

            // Returns Policy content
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string returnPolicyContent = !string.IsNullOrEmpty(_model.Company.ReturnsPolicy)
                ? _model.Company.ReturnsPolicy
                : "1. Any returns must be within seven (7) days from date of invoice\n"
                + "2. Returned goods must be undamaged, in original packaging with no broken seals.\n"
                + "3. No returns on cold chain products";

            PdfPCell returnPolicyContentCell = new PdfPCell(new Phrase(returnPolicyContent, _fontStyle));
            returnPolicyContentCell.Colspan = 2;
            returnPolicyContentCell.Border = 0;
            nestedTable.AddCell(returnPolicyContentCell);

            // Banking Details content
            string bankingDetailsContent = !string.IsNullOrEmpty(_model.Company.BankingDetails)
                ? _model.Company.BankingDetails
                : "Account Name: Langson Legacy \n"
                  + "FBC N. Mandela Branch (USD Nostro) Acc No. 6870288590122\n"
                  + " \n"
                  + "NMB Avondale Banch (zwl) Acc No. 00280593346 ";

            PdfPCell bankingDetailsContentCell = new PdfPCell(new Phrase(bankingDetailsContent, _fontStyle));
            bankingDetailsContentCell.Colspan = 2;
            bankingDetailsContentCell.Border = 0;
            bankingDetailsContentCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            nestedTable.AddCell(bankingDetailsContentCell);
            nestedTable.CompleteRow();

            // Add the nested table
            PdfPCell nestedTableCell = new PdfPCell(nestedTable);
            nestedTableCell.Colspan = _totalColumn;
            nestedTableCell.BorderWidthBottom = 1f;
            nestedTableCell.PaddingBottom = 5f;
            _pdfPTable.AddCell(nestedTableCell);
            _pdfPTable.CompleteRow();

            // Add empty row
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Signature row
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Signature", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();
        }
    }
}