using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pharma263.Api.Extensions;
using Pharma263.Api.Services;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Api.Shared.Extensions;
using Pharma263.Application;
using Pharma263.Infrastructure;
using Pharma263.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddScoped<IPurchaseCalculationService, PurchaseCalculationService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();

builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
})
    .AddMemoryCache()
    .AddHttpContextAccessor()
    .RegisterAllTypes<IScopedInjectedService>(new[] { typeof(AccountsPayableService).Assembly }, ServiceLifetime.Scoped);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register exception middleware first to catch all exceptions
app.UseMiddleware<Pharma263.Api.Middleware.ExceptionMiddleware>();

//app.UseRouting();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("all");

app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerDocumention();

app.MapControllers();

app.Run();
