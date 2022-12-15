using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Napos.Core.Abstract;
using Napos.Data.Migrations;
using System;

namespace Napos.Data
{
    public static class DataSetup
    {
        public static DataContext CreateDataContext(this IServiceProvider services, string connectionString)
        {
            var dataContext = new DataContext(
                    new SqliteConnection(connectionString),
                    new SqlKata.Compilers.SqliteCompiler(),
                    services.GetRequiredService<IDateTimeService>(),
                    snakeCase: true
                );

            return dataContext;
        }

        public static void MigrateData(string connectionString, bool webPlatform)
        {
            var services = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                // Add SQLite support to FluentMigrator
                .AddSQLite()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assembly containing the migrations
                .ScanIn(typeof(MIG_20220402_1500_Init).Assembly).For.Migrations());

            if (webPlatform)
            {
                // Enable logging to console in the FluentMigrator way
                services.AddLogging(lb => lb.AddFluentMigratorConsole()); // Android: System.PlatformNotSupportedException
            }

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider(false);

            // Migrations Run
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                var migrations = runner.MigrationLoader.LoadMigrations();
                runner.MigrateUp();
            }
        }
    }
}
