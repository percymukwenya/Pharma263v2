using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Pharma263.Integration.Api.Configurations;
using Pharma263.Integration.Api.MessageHandlers;
using Refit;
using System;

namespace Pharma263.Integration.Api.Extensions
{
    public static class PharmaApiExtension
    {
        public static IServiceCollection AddPharmaApi(this IServiceCollection services, IConfiguration configuration)
        {
            var pharmaApiOptions = new PharmaApiOptions();
            configuration.GetSection("ServiceUrls").Bind(pharmaApiOptions);

            services
                .Configure<PharmaApiOptions>(configuration.GetSection("ServiceUrls"));

            services
                .AddScoped<PharmaApiMessageHandler>();

            services
                .AddRefitClient<IPharmaApiService>(new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            Converters = { new StringEnumConverter() },
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        })
                })
                .ConfigureHttpClient(options =>
                {
                    options.BaseAddress = new Uri(pharmaApiOptions.BaseAddress);
                })
                .AddHttpMessageHandler<PharmaApiMessageHandler>();

            return services;
        }
    }
}
