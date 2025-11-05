using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pharma263.Application.Contracts.Email;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Models.Email;
using Pharma263.Infrastructure.Email;
using Pharma263.Infrastructure.Logging;

namespace Pharma263.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<EmailSmtpSettings>(configuration.GetSection("EmailSmtpSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            return services;
        }
    }
}
