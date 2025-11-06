# Pharma263 Security & Quality Action Plan

**Created**: 2025-11-05
**Priority**: CRITICAL - Address before production deployment
**Estimated Total Effort**: 3-5 days

---

## 🚨 Phase 1: CRITICAL Security Fixes (Day 1-2)

**Goal**: Eliminate immediate security vulnerabilities
**Must complete before any production deployment**

### Task 1.1: Remove Hardcoded Secrets ⏱️ 2 hours

**Priority**: 🔴 CRITICAL
**Status**: ⬜ Not Started

**Steps**:

1. **Set up User Secrets for local development**
   ```bash
   cd Pharma263/Pharma263.Api
   dotnet user-secrets init

   # Add secrets
   dotnet user-secrets set "ConnectionStrings:Pharma263Connection" "your-connection-string"
   dotnet user-secrets set "ConnectionStrings:DapperConnection" "your-connection-string"
   dotnet user-secrets set "JwtSettings:Key" "your-secure-256-bit-key"
   dotnet user-secrets set "Smtp:Password" "your-smtp-password"
   dotnet user-secrets set "EmailSettings:ApiKey" "your-sendgrid-key"
   ```

2. **Update appsettings.json (remove all secrets)**
   ```json
   {
     "ConnectionStrings": {
       "Pharma263Connection": "",
       "DapperConnection": "",
       "LocalConnection": "Server=.; Database=Pharma263; Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     },
     "EmailSettings": {
       "ApiKey": "",
       "FromAddress": "no-reply@pharma263.com",
       "FromName": "Pharma263 System"
     },
     "Smtp": {
       "Host": "mail5019.site4now.net",
       "Port": 465,
       "Username": "noreply@pharma263.com",
       "Password": "",
       "From": "noreply@pharma263.com"
     },
     "JwtSettings": {
       "Key": "",
       "Issuer": "Pharma263.Api",
       "Audience": "Pharma263User",
       "DurationInMinutes": 30
     }
   }
   ```

3. **Repeat for MVC solution**
   ```bash
   cd Pharma263.MVC/Pharma263.MVC
   dotnet user-secrets init
   # Add any MVC-specific secrets
   ```

4. **Update .gitignore** (verify these are already ignored)
   ```
   appsettings.Development.json
   appsettings.Production.json
   appsettings.Staging.json
   **/appsettings.*.json
   !**/appsettings.json
   ```

5. **Document secret setup in README**

**Acceptance Criteria**:
- ✅ No passwords/keys in appsettings.json
- ✅ Application runs with user secrets
- ✅ Documentation updated
- ✅ Team notified of new setup process

**Dependencies**: None

---

### Task 1.2: Rotate Exposed Credentials ⏱️ 1 hour

**Priority**: 🔴 CRITICAL
**Status**: ⬜ Not Started

**Steps**:

1. **Database Password**
   - Log into SQL8002.site4now.net
   - Change password for `db_a9107a_pharma263_admin`
   - Update in Azure Key Vault / user secrets

2. **SMTP Password**
   - Change password for `noreply@pharma263.com`
   - Update in secure configuration

3. **Generate new JWT signing key**
   ```bash
   # Generate secure 256-bit key
   openssl rand -base64 32
   ```
   - Update in secure configuration

4. **SendGrid API Key**
   - Revoke old key in SendGrid dashboard
   - Generate new key
   - Update in secure configuration

**Acceptance Criteria**:
- ✅ All exposed credentials rotated
- ✅ New credentials stored securely
- ✅ Applications tested with new credentials
- ✅ Old credentials confirmed invalid

**Dependencies**: Task 1.1

---

### Task 1.3: Fix CORS Configuration ⏱️ 30 minutes

**Priority**: 🔴 CRITICAL
**Status**: ⬜ Not Started

**Files to modify**:
- `Pharma263/Pharma263.Api/Program.cs`
- `Pharma263.MVC/Pharma263.MVC/Program.cs`
- `Pharma263/Pharma263.Api/appsettings.json`

**Implementation**:

1. **Add CORS configuration to appsettings.json**
   ```json
   {
     "AllowedOrigins": [
       "https://pharma263.com",
       "https://www.pharma263.com",
       "https://localhost:7000"
     ]
   }
   ```

2. **Update Program.cs**
   ```csharp
   // Read allowed origins from configuration
   var allowedOrigins = builder.Configuration
       .GetSection("AllowedOrigins")
       .Get<string[]>() ?? Array.Empty<string>();

   builder.Services.AddCors(options =>
   {
       // Development policy (only if environment is Development)
       if (builder.Environment.IsDevelopment())
       {
           options.AddPolicy("development", builder => builder
               .AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod());
       }

       // Production policy
       options.AddPolicy("production", builder => builder
           .WithOrigins(allowedOrigins)
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .SetIsOriginAllowedToAllowWildcardSubdomains());
   });
   ```

3. **Use environment-specific CORS policy**
   ```csharp
   var corsPolicy = app.Environment.IsDevelopment() ? "development" : "production";
   app.UseCors(corsPolicy);
   ```

**Acceptance Criteria**:
- ✅ Only configured origins allowed in production
- ✅ Development still flexible for local testing
- ✅ Credentials enabled for production policy
- ✅ API accessible from legitimate domains only

**Dependencies**: None

---

### Task 1.4: Implement Default Authorization ⏱️ 3 hours

**Priority**: 🔴 CRITICAL
**Status**: ⬜ Not Started

**Files to modify**:
- `Pharma263/Pharma263.Api/Program.cs`
- All controller files (add `[AllowAnonymous]` where needed)

**Implementation**:

1. **Add global authorization filter**
   ```csharp
   // In Program.cs, after AddControllers()
   builder.Services.AddControllers(options =>
   {
       var policy = new AuthorizationPolicyBuilder()
           .RequireAuthenticatedUser()
           .Build();
       options.Filters.Add(new AuthorizeFilter(policy));
   });
   ```

2. **Mark public endpoints as anonymous**
   ```csharp
   // AccountController.cs
   [AllowAnonymous]
   [HttpPost("login")]
   public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] AuthRequest request)

   [AllowAnonymous]
   [HttpGet("confirm-email")]
   public async Task<IActionResult> ConfirmEmailAsync(...)

   [AllowAnonymous]
   [HttpPost("forgot-password")]
   public async Task<IActionResult> ForgotPassword(...)

   [AllowAnonymous]
   [HttpPost("reset-password")]
   public async Task<IActionResult> ResetPassword(...)
   ```

3. **Test all endpoints**
   - Verify authenticated endpoints return 401 without token
   - Verify public endpoints work without authentication
   - Document which endpoints should be public

**Acceptance Criteria**:
- ✅ All endpoints require authentication by default
- ✅ Public endpoints explicitly marked with `[AllowAnonymous]`
- ✅ 401 Unauthorized returned for missing/invalid tokens
- ✅ Integration tests pass

**Dependencies**: None

---

### Task 1.5: Secure Error Handling ⏱️ 30 minutes

**Priority**: 🔴 CRITICAL
**Status**: ⬜ Not Started

**Files to modify**:
- `Pharma263/Pharma263.Api/Middleware/ExceptionMiddleware.cs`

**Implementation**:

```csharp
private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
{
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
    CustomProblemDetails problem = new();

    // Get environment from configuration
    var isDevelopment = httpContext.RequestServices
        .GetRequiredService<IWebHostEnvironment>()
        .IsDevelopment();

    switch (ex)
    {
        case BadRequestException badRequestException:
            statusCode = HttpStatusCode.BadRequest;
            problem = new CustomProblemDetails
            {
                Title = badRequestException.Message,
                Status = (int)statusCode,
                Detail = isDevelopment ? badRequestException.InnerException?.Message : null,
                Type = nameof(BadRequestException),
                Errors = badRequestException.ValidationErrors
            };
            break;
        case NotFoundException NotFound:
            statusCode = HttpStatusCode.NotFound;
            problem = new CustomProblemDetails
            {
                Title = NotFound.Message,
                Status = (int)statusCode,
                Type = nameof(NotFoundException),
                Detail = isDevelopment ? NotFound.InnerException?.Message : null,
            };
            break;
        default:
            problem = new CustomProblemDetails
            {
                Title = isDevelopment ? ex.Message : "An error occurred processing your request.",
                Status = (int)statusCode,
                Type = nameof(HttpStatusCode.InternalServerError),
                Detail = isDevelopment ? ex.StackTrace : null, // Only show stack trace in development
            };
            break;
    }

    httpContext.Response.StatusCode = (int)statusCode;
    var logMessage = JsonConvert.SerializeObject(problem);
    _logger.LogError(ex, logMessage); // Log full exception for monitoring
    await httpContext.Response.WriteAsJsonAsync(problem);
}
```

**Acceptance Criteria**:
- ✅ Stack traces hidden in production
- ✅ Generic error messages in production
- ✅ Full details logged for debugging
- ✅ Development environment shows full details

**Dependencies**: None

---

## 🟠 Phase 2: HIGH Priority Improvements (Day 3)

**Goal**: Strengthen security posture and add testing foundation

### Task 2.1: Implement Azure Key Vault ⏱️ 3 hours

**Priority**: 🟠 HIGH
**Status**: ⬜ Not Started

**Steps**:

1. **Create Azure Key Vault resource**
   ```bash
   # Via Azure CLI
   az keyvault create \
     --name pharma263-keyvault \
     --resource-group pharma263-rg \
     --location eastus
   ```

2. **Add secrets to Key Vault**
   ```bash
   az keyvault secret set --vault-name pharma263-keyvault \
     --name "ConnectionStrings--Pharma263Connection" \
     --value "your-connection-string"

   az keyvault secret set --vault-name pharma263-keyvault \
     --name "JwtSettings--Key" \
     --value "your-jwt-key"
   ```

3. **Install NuGet packages**
   ```bash
   cd Pharma263/Pharma263.Api
   dotnet add package Azure.Identity
   dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets
   ```

4. **Update Program.cs**
   ```csharp
   if (!builder.Environment.IsDevelopment())
   {
       var keyVaultName = builder.Configuration["KeyVaultName"];
       var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

       builder.Configuration.AddAzureKeyVault(
           keyVaultUri,
           new DefaultAzureCredential());
   }
   ```

5. **Configure Managed Identity**
   - Enable Managed Identity on App Service
   - Grant Key Vault access to Managed Identity

**Acceptance Criteria**:
- ✅ Secrets stored in Key Vault
- ✅ Application loads secrets from Key Vault in production
- ✅ Local development uses User Secrets
- ✅ No secrets in configuration files

**Dependencies**: Task 1.1, Task 1.2

---

### Task 2.2: Strengthen JWT Configuration ⏱️ 2 hours

**Priority**: 🟠 HIGH
**Status**: ⬜ Not Started

**Files to modify**:
- `Pharma263/Pharma263.Application/Services/Identity/AuthService.cs`
- Create new `RefreshTokenService.cs`
- Database migration for refresh tokens

**Implementation**:

1. **Update JWT settings**
   ```json
   {
     "JwtSettings": {
       "Key": "",
       "Issuer": "Pharma263.Api",
       "Audience": "Pharma263User",
       "AccessTokenDurationInMinutes": 15,
       "RefreshTokenDurationInDays": 7,
       "ClockSkew": 0
     }
   }
   ```

2. **Create RefreshToken entity**
   ```csharp
   public class RefreshToken : BaseEntity
   {
       public string Token { get; set; }
       public string UserId { get; set; }
       public DateTime ExpiryDate { get; set; }
       public bool IsRevoked { get; set; }
       public DateTime? RevokedDate { get; set; }
       public string RevokedByIp { get; set; }
       public string CreatedByIp { get; set; }
   }
   ```

3. **Implement refresh token endpoint**
   ```csharp
   [HttpPost("refresh-token")]
   [AllowAnonymous]
   public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
   {
       var result = await _authService.RefreshToken(request.Token, GetIpAddress());
       return StatusCode(result.StatusCode, result);
   }
   ```

4. **Add token validation on each request**
   - Validate token hasn't been revoked
   - Check expiry with zero clock skew

**Acceptance Criteria**:
- ✅ Refresh token mechanism implemented
- ✅ Short-lived access tokens (15 min)
- ✅ Long-lived refresh tokens (7 days)
- ✅ Token revocation support
- ✅ IP tracking for security audit

**Dependencies**: None

---

### Task 2.3: Add Input Validation Tests ⏱️ 4 hours

**Priority**: 🟠 HIGH
**Status**: ⬜ Not Started

**Steps**:

1. **Create test project**
   ```bash
   cd Pharma263
   dotnet new xunit -n Pharma263.Api.Tests
   dotnet sln add Pharma263.Api.Tests/Pharma263.Api.Tests.csproj
   ```

2. **Add test dependencies**
   ```bash
   cd Pharma263.Api.Tests
   dotnet add package Microsoft.AspNetCore.Mvc.Testing
   dotnet add package FluentAssertions
   dotnet add package Moq
   dotnet add reference ../Pharma263.Api/Pharma263.Api.csproj
   ```

3. **Create WebApplicationFactory**
   ```csharp
   public class PharmaApiFactory : WebApplicationFactory<Program>
   {
       protected override void ConfigureWebHost(IWebHostBuilder builder)
       {
           builder.ConfigureServices(services =>
           {
               // Replace database with in-memory version
               // Configure test dependencies
           });
       }
   }
   ```

4. **Write authentication tests**
   ```csharp
   public class AuthenticationTests : IClassFixture<PharmaApiFactory>
   {
       [Fact]
       public async Task GetSales_WithoutToken_Returns401()
       {
           // Arrange
           var client = _factory.CreateClient();

           // Act
           var response = await client.GetAsync("/api/Sale/GetSales");

           // Assert
           response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
       }

       [Fact]
       public async Task Login_WithValidCredentials_ReturnsToken()
       {
           // Test implementation
       }
   }
   ```

5. **Write validation tests**
   ```csharp
   public class SaleValidationTests
   {
       [Fact]
       public async Task CreateSale_WithInvalidData_Returns400()
       {
           // Test validation logic
       }
   }
   ```

**Acceptance Criteria**:
- ✅ Test project created and configured
- ✅ Authentication tests pass
- ✅ Validation tests cover key scenarios
- ✅ Tests run in CI/CD pipeline
- ✅ Minimum 50% code coverage

**Dependencies**: Task 1.4 (authorization)

---

### Task 2.4: Upgrade iTextSharp ⏱️ 4 hours

**Priority**: 🟠 HIGH
**Status**: ⬜ Not Started

**Decision Required**: Choose replacement library
- **Option A**: iText 7+ (Commercial license required)
- **Option B**: QuestPDF (MIT license, modern API)
- **Option C**: PdfSharpCore (MIT license, mature)

**Recommended**: QuestPDF for modern API and MIT license

**Steps**:

1. **Install QuestPDF**
   ```bash
   cd Pharma263/Pharma263.Application
   dotnet remove package iTextSharp
   dotnet add package QuestPDF
   ```

2. **Create new BaseReport implementation**
   ```csharp
   public abstract class BaseReport<TModel>
   {
       protected abstract void BuildDocument(IContainer container, TModel model);

       public byte[] PrepareReport(TModel model)
       {
           return Document.Create(container =>
           {
               container.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(2, Unit.Centimetre);
                   page.Content().Element(c => BuildDocument(c, model));
               });
           }).GeneratePdf();
       }
   }
   ```

3. **Migrate existing reports** (one at a time)
   - SalesInvoiceReport
   - PurchaseInvoiceReportNew
   - QuotationInvoiceReport

4. **Test all report generation**
   - Visual comparison with old reports
   - Performance testing
   - Browser compatibility

**Acceptance Criteria**:
- ✅ iTextSharp fully removed
- ✅ All reports migrated to new library
- ✅ Visual output matches original reports
- ✅ License compliance verified
- ✅ Performance acceptable

**Dependencies**: None (can be done in parallel)

---

## 🟡 Phase 3: MEDIUM Priority Quality Improvements (Day 4-5)

**Goal**: Improve code quality, testability, and maintainability

### Task 3.1: Add Unit Tests ⏱️ 8 hours

**Priority**: 🟡 MEDIUM
**Status**: ⬜ Not Started

**Target Coverage**: 80% for business logic

**Areas to test**:

1. **Calculation Services**
   ```csharp
   public class CalculationServiceTests
   {
       [Theory]
       [InlineData(100, 10, 10, 0, "percentage", 90)] // price, qty, discount%, discountAmt, type, expected
       [InlineData(100, 10, 0, 100, "fixed", 900)]
       public async Task CalculateSalesTotal_CalculatesCorrectly(...)
       {
           // Test calculation logic
       }
   }
   ```

2. **Validation Logic**
   - SaleItemCommandValidator tests
   - Stock validation tests
   - Pharmaceutical compliance tests

3. **Repository Layer**
   - CRUD operations
   - Pagination
   - Filtering

4. **Service Layer**
   - AuthService
   - SalesService
   - PurchaseService

**Acceptance Criteria**:
- ✅ 80%+ code coverage for Application layer
- ✅ All calculation methods tested
- ✅ Edge cases covered
- ✅ Tests run in < 30 seconds

**Dependencies**: Task 2.3 (test infrastructure)

---

### Task 3.2: Enable XML Documentation ⏱️ 1 hour

**Priority**: 🟡 MEDIUM
**Status**: ⬜ Not Started

**Implementation**:

1. **Enable in .csproj files**
   ```xml
   <PropertyGroup>
     <GenerateDocumentationFile>true</GenerateDocumentationFile>
     <NoWarn>$(NoWarn);1591</NoWarn> <!-- Suppress missing XML comment warnings initially -->
   </PropertyGroup>
   ```

2. **Configure Swagger to use XML comments**
   ```csharp
   builder.Services.AddSwaggerGen(options =>
   {
       var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
       var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
       options.IncludeXmlComments(xmlPath);
   });
   ```

3. **Add XML comments to key controllers**
   ```csharp
   /// <summary>
   /// Manages sales transactions and invoices
   /// </summary>
   [Route("api/[controller]")]
   [ApiController]
   public class SaleController : ControllerBase
   {
       /// <summary>
       /// Retrieves all sales transactions
       /// </summary>
       /// <returns>List of sales transactions</returns>
       /// <response code="200">Returns the list of sales</response>
       /// <response code="401">User is not authenticated</response>
       [HttpGet("GetSales")]
       [ProducesResponseType(typeof(ApiResponse<List<SaleListResponse>>), 200)]
       [ProducesResponseType(401)]
       public async Task<ActionResult<ApiResponse<List<SaleListResponse>>>> GetSales()
   }
   ```

**Acceptance Criteria**:
- ✅ XML documentation enabled
- ✅ Swagger shows method descriptions
- ✅ Response types documented
- ✅ All public APIs documented

**Dependencies**: None

---

### Task 3.3: Align Dependency Versions ⏱️ 1 hour

**Priority**: 🟡 MEDIUM
**Status**: ⬜ Not Started

**Implementation**:

1. **Create Directory.Build.props in repository root**
   ```xml
   <Project>
     <PropertyGroup>
       <TargetFramework>net8.0</TargetFramework>
       <LangVersion>latest</LangVersion>
       <Nullable>enable</Nullable>
     </PropertyGroup>

     <ItemGroup>
       <!-- Centrally managed package versions -->
       <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="8.0.4" />
       <PackageVersion Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.4" />
       <PackageVersion Include="System.IdentityModel.Tokens.Jwt" Version="8.0.0" />
       <PackageVersion Include="Serilog" Version="3.1.1" />
       <PackageVersion Include="Serilog.AspNetCore" Version="8.0.1" />
       <PackageVersion Include="FluentValidation" Version="11.9.1" />
       <PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
     </ItemGroup>
   </Project>
   ```

2. **Update all .csproj files to use centralized versions**
   ```xml
   <ItemGroup>
     <PackageReference Include="Microsoft.EntityFrameworkCore" />
     <PackageReference Include="FluentValidation" />
   </ItemGroup>
   ```

3. **Verify all projects build**
   ```bash
   dotnet restore
   dotnet build
   ```

**Acceptance Criteria**:
- ✅ All packages use consistent versions
- ✅ JWT Bearer version aligned (8.0.4)
- ✅ All projects build successfully
- ✅ No version conflicts

**Dependencies**: None

---

### Task 3.4: Add Rate Limiting ⏱️ 2 hours

**Priority**: 🟡 MEDIUM
**Status**: ⬜ Not Started

**Implementation**:

1. **Add rate limiting (built-in .NET 8)**
   ```csharp
   // Program.cs
   builder.Services.AddRateLimiter(options =>
   {
       // Global rate limit
       options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
           RateLimitPartition.GetFixedWindowLimiter(
               partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
               factory: partition => new FixedWindowRateLimiterOptions
               {
                   AutoReplenishment = true,
                   PermitLimit = 100,
                   QueueLimit = 0,
                   Window = TimeSpan.FromMinutes(1)
               }));

       // Specific policy for login attempts
       options.AddFixedWindowLimiter("login", options =>
       {
           options.PermitLimit = 5;
           options.Window = TimeSpan.FromMinutes(5);
           options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
           options.QueueLimit = 2;
       });
   });

   // Use middleware
   app.UseRateLimiter();
   ```

2. **Apply to login endpoint**
   ```csharp
   [EnableRateLimiting("login")]
   [HttpPost("login")]
   public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] AuthRequest request)
   ```

**Acceptance Criteria**:
- ✅ Rate limiting configured
- ✅ Login endpoint protected (5 attempts per 5 min)
- ✅ Global limit prevents abuse (100 req/min)
- ✅ 429 Too Many Requests returned when exceeded

**Dependencies**: None

---

### Task 3.5: Add Health Checks ⏱️ 2 hours

**Priority**: 🟡 MEDIUM
**Status**: ⬜ Not Started

**Implementation**:

1. **Install packages**
   ```bash
   dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
   dotnet add package AspNetCore.HealthChecks.SqlServer
   dotnet add package AspNetCore.HealthChecks.UI
   ```

2. **Configure health checks**
   ```csharp
   // Program.cs
   builder.Services.AddHealthChecks()
       .AddDbContextCheck<ApplicationDbContext>("database")
       .AddSqlServer(
           builder.Configuration.GetConnectionString("Pharma263Connection"),
           name: "sql-server",
           timeout: TimeSpan.FromSeconds(5))
       .AddCheck("api", () => HealthCheckResult.Healthy("API is running"));

   // Map health check endpoints
   app.MapHealthChecks("/health", new HealthCheckOptions
   {
       ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
   });

   app.MapHealthChecks("/health/ready", new HealthCheckOptions
   {
       Predicate = check => check.Tags.Contains("ready"),
       ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
   });
   ```

**Acceptance Criteria**:
- ✅ Health check endpoint responding
- ✅ Database connectivity verified
- ✅ JSON response with detailed status
- ✅ Kubernetes-ready (liveness/readiness probes)

**Dependencies**: None

---

## 🔵 Phase 4: LOW Priority Enhancements (Future Sprints)

### Task 4.1: Add API Versioning ⏱️ 3 hours

**Priority**: 🔵 LOW
**Status**: ⬜ Not Started

**Implementation**:

```bash
dotnet add package Asp.Versioning.Mvc
dotnet add package Asp.Versioning.Mvc.ApiExplorer
```

```csharp
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Update routes
[Route("api/v{version:apiVersion}/[controller]")]
```

**Acceptance Criteria**:
- ✅ v1 endpoints created
- ✅ Swagger shows version selector
- ✅ Backwards compatibility maintained

**Dependencies**: None

---

### Task 4.2: Add Application Insights ⏱️ 2 hours

**Priority**: 🔵 LOW
**Status**: ⬜ Not Started

```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

```csharp
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});
```

**Acceptance Criteria**:
- ✅ Telemetry flowing to Azure
- ✅ Performance metrics captured
- ✅ Exception tracking enabled
- ✅ Custom events for business operations

**Dependencies**: Azure subscription

---

### Task 4.3: Evaluate Solution Consolidation ⏱️ 8 hours (Analysis)

**Priority**: 🔵 LOW
**Status**: ⬜ Not Started

**Questions to answer**:
- Is MVC solution still actively maintained?
- Can MVC be replaced with modern SPA framework (React/Vue/Blazor)?
- What's the migration path?
- Cost/benefit analysis

**Deliverable**: Recommendation document

**Dependencies**: Business stakeholder input

---

## 📊 Progress Tracking

### Sprint Summary

| Phase | Tasks | Estimated Time | Priority |
|-------|-------|----------------|----------|
| Phase 1 | 5 | 7.5 hours | 🔴 CRITICAL |
| Phase 2 | 4 | 13 hours | 🟠 HIGH |
| Phase 3 | 5 | 14 hours | 🟡 MEDIUM |
| Phase 4 | 3 | 13 hours | 🔵 LOW |
| **Total** | **17** | **47.5 hours** | |

### Daily Breakdown

**Day 1 (8 hours)**
- ✅ Task 1.1: Remove hardcoded secrets (2h)
- ✅ Task 1.2: Rotate credentials (1h)
- ✅ Task 1.3: Fix CORS (0.5h)
- ✅ Task 1.4: Default authorization (3h)
- ✅ Task 1.5: Secure error handling (0.5h)
- Buffer: 1h

**Day 2 (8 hours)**
- ✅ Task 2.1: Azure Key Vault (3h)
- ✅ Task 2.2: JWT improvements (2h)
- ✅ Task 2.4: Upgrade iTextSharp (4h) - START
- Buffer: 1h

**Day 3 (8 hours)**
- ✅ Task 2.4: Upgrade iTextSharp (4h) - COMPLETE
- ✅ Task 2.3: Validation tests (4h)

**Day 4 (8 hours)**
- ✅ Task 3.1: Unit tests (8h)

**Day 5 (8 hours)**
- ✅ Task 3.2: XML documentation (1h)
- ✅ Task 3.3: Align versions (1h)
- ✅ Task 3.4: Rate limiting (2h)
- ✅ Task 3.5: Health checks (2h)
- Final testing & documentation: 2h

---

## 🚀 Deployment Checklist

Before deploying to production:

- [ ] All Phase 1 tasks completed
- [ ] No hardcoded secrets in repository
- [ ] All credentials rotated
- [ ] CORS configured for production domains
- [ ] Authorization required on all sensitive endpoints
- [ ] Error messages don't leak sensitive information
- [ ] Integration tests pass
- [ ] Azure Key Vault configured
- [ ] JWT tokens strengthened
- [ ] Rate limiting enabled
- [ ] Health checks responding
- [ ] Monitoring configured
- [ ] Documentation updated
- [ ] Team trained on new security practices

---

## 📞 Support & Questions

**Security Questions**: [Security Team Email]
**Architecture Questions**: [Tech Lead Email]
**Access Issues**: [DevOps Team Email]

**Documentation**:
- User Secrets: https://learn.microsoft.com/aspnet/core/security/app-secrets
- Azure Key Vault: https://learn.microsoft.com/azure/key-vault/
- Rate Limiting: https://learn.microsoft.com/aspnet/core/performance/rate-limit

---

**Last Updated**: 2025-11-05
**Next Review**: After Phase 1 completion
