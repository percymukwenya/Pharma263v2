using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Reflection;

namespace Pharma263.Application.Services.Printing
{
    public abstract class BaseReport<TModel>
    {
        #region Declaration
        protected int _totalColumn;
        protected Document _document;
        protected Font _fontStyle;
        protected Font _fontStyleNormal;
        protected PdfPTable _pdfPTable;
        protected PdfPCell _pdfPCell;
        protected MemoryStream _memoryStream;
        protected PdfWriter writer;
        protected PdfPTable paymentTable;
        protected PdfPTable table;
        protected TModel _model;
        #endregion

        public BaseReport(int totalColumns)
        {
            _totalColumn = totalColumns;
            paymentTable = new PdfPTable(2);
            _pdfPTable = new PdfPTable(_totalColumn);
            _memoryStream = new MemoryStream();
            table = new PdfPTable(3);
        }

        // Abstract method to get column widths - each report defines its own
        protected abstract float[] GetColumnWidths();

        // Abstract methods for report-specific content
        protected abstract void BuildReportSpecificHeader();
        protected abstract void BuildTableHeaders();
        protected abstract void BuildTableBody();

        public byte[] CreateReport(TModel model)
        {
            _model = model;

            #region Page Setup
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfPTable.WidthPercentage = 100;
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            writer = PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfPTable.SetWidths(GetColumnWidths());

            paymentTable.WidthPercentage = 100;
            paymentTable.HorizontalAlignment = Element.ALIGN_LEFT;
            paymentTable.SetWidths(new float[] { 100f, 100f });

            table.WidthPercentage = 100;
            #endregion

            BuildStandardHeader();
            BuildReportSpecificHeader();
            BuildTableSection();

            _pdfPTable.HeaderRows = 2;
            _document.Add(paymentTable);
            _document.Add(_pdfPTable);
            _document.Close();
            return _memoryStream.ToArray();
        }

        protected virtual void BuildStandardHeader()
        {
            // Logo section
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
        }

        protected void AddCompanyInfo(dynamic companyInfo)
        {
            // Company Name
            _fontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            _pdfPCell = new PdfPCell(new Phrase(companyInfo.StoreName, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Email
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(companyInfo.Email, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // VAT Number
            _pdfPCell = new PdfPCell(new Phrase(companyInfo.VATNumber, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Phone
            _pdfPCell = new PdfPCell(new Phrase(companyInfo.Phone, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Address
            _pdfPCell = new PdfPCell(new Phrase(companyInfo.Address, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Add spacing
            AddSpacingRows(paymentTable, 2);
        }

        protected void AddSpacingRows(PdfPTable table, int count)
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            for (int i = 0; i < count; i++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
                _pdfPCell.Colspan = _totalColumn;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                table.AddCell(_pdfPCell);
                table.CompleteRow();
            }
        }

        protected virtual void BuildTableSection()
        {
            AddSpacingRows(paymentTable, 2);

            BuildTableHeaders();
            BuildTableBody();
        }

        // Helper method to create table body cells (centered alignment like original)
        protected PdfPCell CreateStandardCell(string text, Font font, int colspan = 1, int alignment = Element.ALIGN_CENTER, BaseColor backgroundColor = null)
        {
            return new PdfPCell(new Phrase(text, font))
            {
                Colspan = colspan,
                HorizontalAlignment = alignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = backgroundColor ?? BaseColor.WHITE
            };
        }

        // Helper method to create summary row cells (no borders like original)
        protected PdfPCell CreateSummaryCell(string text, Font font, int colspan = 1, int alignment = Element.ALIGN_RIGHT, BaseColor backgroundColor = null)
        {
            return new PdfPCell(new Phrase(text, font))
            {
                Colspan = colspan,
                HorizontalAlignment = alignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BackgroundColor = backgroundColor ?? BaseColor.WHITE
            };
        }

        // Helper method to create header cells with standard gray background
        protected PdfPCell CreateHeaderCell(string text)
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            return new PdfPCell(new Phrase(text, _fontStyle))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY
            };
        }
    }
}
