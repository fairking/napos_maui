using Microsoft.Extensions.DependencyInjection;
using Napos.Core.Helpers;
using Napos.Data;
using Napos.Domain.Services.System;

namespace Napos.Domain
{
    public static class Bootstrap
    {
        public static IServiceCollection SetupServices(this IServiceCollection services, string connectionString, ServiceLifetime dataContextLifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
                services = new ServiceCollection();

            services.AddSingleton(new ConfigService(connectionString));

            // Data setup
            services.Add(new ServiceDescriptor(
                typeof(DataContext), 
                s => DataSetup.CreateDataContext(s, s.GetRequiredService<ConfigService>().ConnectionString), 
                dataContextLifetime));

            // Services setup
            services.RegisterAllServices(typeof(Bootstrap).Assembly);

            // Validate
            services.ValidateAllServices();

            return services;
        }
    }
}
