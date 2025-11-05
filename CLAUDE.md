# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This codebase contains two separate .NET solutions for Pharma263, a comprehensive pharmaceutical management system:

1. **Pharma263** - Clean Architecture API solution (.NET 8.0) - Primary business logic and data access
2. **Pharma263.MVC** - MVC web application with integration API (.NET 8.0) - User interface and external integrations

## Business Domain

Pharma263 is an enterprise pharmaceutical management system that handles:
- **Inventory Management**: Medicine catalog, stock tracking, batch numbers, expiry dates
- **Sales Operations**: Customer sales, invoicing, payment tracking, returns management
- **Purchase Operations**: Supplier management, purchase orders, inventory receipts
- **Financial Management**: Quotations, supplier statements, banking integration
- **Reporting**: Comprehensive PDF reports for all business operations
- **Company Management**: Multi-company support with custom branding and policies

## Architecture

### Pharma263 (API Solution)
Follows Clean Architecture principles with clear separation of concerns:

- **Core Layer**: `Pharma263.Domain` (entities, interfaces), `Pharma263.Application` (business logic, services, models)
- **Infrastructure Layer**: `Pharma263.Infrastructure`, `Pharma263.Persistence` (data access, external services)
- **API Layer**: `Pharma263.Api` (REST endpoints, controllers)

**Key Features:**
- Advanced PDF report generation using BaseReport<TModel> pattern
- Service layer for Sales, Purchase, and Quotation operations
- Clean separation of models for different report types

### Pharma263.MVC (MVC Solution)
Traditional MVC application with:

- **Presentation**: `Pharma263.MVC` (ASP.NET Core MVC, views, controllers)
- **Integration**: `Pharma263.Integration.Api` (API for external integrations)

**Key Features:**
- Comprehensive reporting dashboard with interactive charts
- PdfReportService for standardized PDF generation
- jQuery-based dynamic report generation

## Development Commands

### Building the Solutions

```bash
# For API solution
cd Pharma263
dotnet restore
dotnet build

# For MVC solution  
cd Pharma263.MVC
dotnet restore
dotnet build
```

### Running the Applications

```bash
# Run API (from Pharma263 directory)
dotnet run --project Pharma263.Api

# Run MVC application (from Pharma263.MVC directory)
dotnet run --project Pharma263.MVC
```

### Publishing for Deployment

```bash
# Publish API for Windows x64 (self-contained)
dotnet publish Pharma263.Api --configuration Release --framework net8.0 -r win-x64 --self-contained

# Publish MVC application
dotnet publish Pharma263.MVC --configuration Release --framework net8.0 -r win-x64 --self-contained
```

## Key Technologies

### API Solution
- ASP.NET Core Web API (.NET 8.0)
- Entity Framework Core 8.0.4
- Swagger/OpenAPI
- Serilog for logging
- EPPlus for Excel operations
- Newtonsoft.Json
- **iTextSharp** for enterprise PDF report generation
- **BaseReport<TModel>** pattern for consistent report styling

### MVC Solution  
- ASP.NET Core MVC (.NET 8.0)
- AutoMapper for object mapping
- CsvHelper for CSV operations
- iTextSharp for PDF generation
- JWT tokens for authentication
- **jQuery** for dynamic report interactions
- **Chart.js** for data visualization
- **Bootstrap** for responsive UI

## Database

The repository includes `DbStructure.sql` which contains the database schema. Both solutions likely share the same database structure.

## CI/CD

Azure DevOps pipelines are configured (`azure-pipelines.yml`) for:
- .NET 8.0 SDK
- Automated restore, build, and publish
- Windows-latest build agents
- Self-contained deployment artifacts

## PDF Report System

### API Solution - BaseReport Pattern
The API solution uses a sophisticated BaseReport<TModel> abstract class for consistent PDF generation:

**Location**: `Pharma263.Application\Services\Printing\BaseReport.cs`

**Key Features:**
- Template method pattern for consistent report structure
- Standardized cell styling with Tahoma fonts and LIGHT_GRAY headers
- Helper methods for consistent formatting:
  - `CreateHeaderCell()` - Standard table headers
  - `CreateStandardCell()` - Data cells with center alignment
  - `CreateSummaryCell()` - Borderless summary rows
- Company branding integration with logo and contact details
- Currency formatting with company-specific prefixes

**Implementation Classes:**
- `SalesInvoiceReport` - Complete sales invoice with returns policy and banking details
- `PurchaseInvoiceReportNew` - Purchase orders with supplier information
- `QuotationInvoiceReport` - Quotation documents with terms and conditions

**Usage Pattern:**
```csharp
var report = new SalesInvoiceReport();
var pdfBytes = report.PrepareReport(salesReportViewModel);
```

### MVC Solution - PdfReportService
The MVC solution uses a service-based approach for PDF generation:

**Location**: `Pharma263.MVC\Services\PdfReportService.cs`

**Key Features:**
- Dependency injection ready with IPdfReportService interface
- Standardized methods for each report type
- Consistent branding and company information
- Integration with ReportController for web-based report generation

## Working with the Codebase

### General Guidelines
1. **Identify the correct solution**: API for business logic, MVC for UI
2. **Follow established patterns**: Use BaseReport for API PDFs, PdfReportService for MVC
3. **Maintain consistency**: Follow existing styling and architectural patterns
4. **Preserve enterprise standards**: Keep clean separation of concerns

### PDF Report Development
When working with PDF reports:

**For API Reports (Pharma263.Application):**
- Extend BaseReport<TModel> for new report types
- Use existing helper methods for consistent styling
- Maintain exact visual compatibility with existing reports
- Follow the established pattern: Header → Table → Summary → Footer

**For MVC Reports (Pharma263.MVC):**
- Use PdfReportService through dependency injection
- Add new methods following existing naming conventions
- Integrate with ReportController for web access

### Common Patterns
- **Currency Formatting**: Always use `_model.Company.Currency + amount.ToString("### ###0.00")`
- **Date Formatting**: Use `date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)`
- **Cell Alignment**: Center alignment with middle vertical alignment for data
- **Summary Rows**: Borderless cells for totals and summary information
- **Company Branding**: Include company logo, contact details, and custom policies

### JavaScript Integration
- Global `reports.js` file handles report generation across all views
- Use `generateReport(url, data)` function for AJAX report requests
- Include Chart.js for dashboard visualizations
- Maintain jQuery compatibility for existing functionality

## Testing

No explicit test projects were found in either solution. When implementing new features:
- Consider adding unit tests for business logic in the Application layer
- Add integration tests for API endpoints
- Test PDF generation with various data scenarios
- Verify cross-browser compatibility for MVC reports

## Enterprise Considerations

The system demonstrates enterprise-level architecture with:
- Clean separation of concerns between API and MVC solutions
- Standardized PDF generation reducing code duplication by ~80%
- Consistent branding and styling across all reports
- Scalable patterns for future report types
- Professional document layouts matching business requirements