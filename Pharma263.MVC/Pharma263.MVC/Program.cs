using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pharma263.MVC.Middleware;
using Pharma263.MVC.Services;
using Pharma263.MVC.Services.IService;
using System;
using Pharma263.Integration.Api.Extensions;
using System.Net.Http.Headers;
using Pharma263.MVC.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Use System.Text.Json instead of Newtonsoft.Json for better performance
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddHttpClient("PharmaApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:PharmaApi"]);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddHttpClient("PharmaAuthClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:PharmaApi"]);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddTransient<IApiService, ApiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(90);
    });

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddHttpClient<IPurchaseService, PurchaseService>();
builder.Services.AddHttpClient<IQuarantineService, QuarantineService>();
builder.Services.AddHttpClient<IReturnService, ReturnService>();
builder.Services.AddHttpClient<ISaleService, SaleService>();
builder.Services.AddHttpClient<ISaleStatusService, SaleStatusService>();
builder.Services.AddHttpClient<IStockService, StockService>();
builder.Services.AddHttpClient<IStoreSettingService, StoreSettingService>();
builder.Services.AddHttpClient<ISupplierService, SupplierService>();
builder.Services.AddHttpClient<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddHttpClient<IPurchaseStatusService, PurchaseStatusService>();
builder.Services.AddHttpClient<ICustomerTypeService, CustomerTypeService>();
builder.Services.AddHttpClient<IReturnReasonService, ReturnReasonService>();
builder.Services.AddHttpClient<IReturnDestinationService, ReturnDestinationService>();
builder.Services.AddHttpClient<IReportService, ReportService>();
builder.Services.AddHttpClient<IQuotationService, QuotationService>();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<ICalculationService, CalculationService>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IQuarantineService, QuarantineService>();
builder.Services.AddScoped<IReturnService, ReturnService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISelectionsService, SelectionsService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStoreSettingService, StoreSettingService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IPurchaseStatusService, PurchaseStatusService>();
builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
builder.Services.AddScoped<IReturnReasonService, ReturnReasonService>();
builder.Services.AddScoped<IReturnDestinationService, ReturnDestinationService>();
builder.Services.AddScoped<ISaleStatusService, SaleStatusService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPdfReportService, PdfReportService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAccountsPayableService, AccountsPayableService>();
builder.Services.AddScoped<IAccountsReceivableService, AccountsReceivableService>();
builder.Services.AddScoped<IPaymentMadeService, PaymentMadeService>();
builder.Services.AddScoped<IPaymentReceivedService, PaymentReceivedService>();
builder.Services.AddScoped<IStatementService, StatementService>();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddPharmaApi(builder.Configuration);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(90); // Aligned with cookie auth timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add response caching for improved performance
builder.Services.AddResponseCaching();

// Add WebOptimizer for JS/CSS bundling and minification
builder.Services.AddWebOptimizer(pipeline =>
{
    // Bundle and minify JavaScript files
    pipeline.AddJavaScriptBundle("/js/bundle.js",
        "/js/pharma263.core.js",
        "/js/pharma263.forms.js",
        "/js/pharma263.calculations.js",
        "/js/utility.js",
        "/js/reports.js"
    ).MinifyJavaScript();

    // Bundle and minify main CSS files
    pipeline.AddCssBundle("/css/bundle.css",
        "/css/site.css",
        "/css/site2.css"
    ).MinifyCss();

    // Bundle CSS modules for better caching and organization
    pipeline.AddCssBundle("/css/modules-bundle.css",
        "/css/modules/common-overrides.css",
        "/css/modules/forms.css",
        "/css/modules/sales.css",
        "/css/modules/purchases.css",
        "/css/modules/inventory.css",
        "/css/modules/reports.css",
        "/css/modules/customers.css"
    ).MinifyCss();

    // Minify individual files that aren't bundled
    pipeline.MinifyJsFiles("/js/**/*.js");
    pipeline.MinifyCssFiles("/css/**/*.css");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWebOptimizer(); // Enable WebOptimizer for bundling/minification
app.UseStaticFiles();
app.UseRouting();
app.UseResponseCaching(); // Enable response caching for lookup data
app.UseCors("all");

app.UseAuthentication();
app.UseMiddleware<RoleMiddleware>();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
