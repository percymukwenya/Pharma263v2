using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pharma263.Domain.Common;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Persistence.Configuration;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Repositories;
using Pharma263.Persistence.Shared;

namespace Pharma263.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                  configuration.GetConnectionString("Pharma263Connection")));
                   // configuration.GetConnectionString("LocalConnection")));

            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));

            services.AddMemoryCache();            

            services.AddSingleton<DapperContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountsPayableRepository, AccountsPayableRepository>();
            services.AddScoped<IAccountsReceivableRepository, AccountsReceivableRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<IPaymentMadeRepository, PaymentMadeRepository>();
            services.AddScoped<IPaymentReceivedRepository, PaymentReceivedRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IQuarantineRepository, QuarantineRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IQuotationItemRepository, QuotationItemRepository>();
            services.AddScoped<ISalesItemRepository, SalesItemRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStoreSettingRepository, StoreSettingRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IReturnRepository, ReturnRepository>();

            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            return services;
        }
    }
}
