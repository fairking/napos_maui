using Microsoft.Extensions.DependencyInjection;
using QueryMan.Helpers;
using System;

namespace Napos.Domain.Tests.Fixtures
{
    public class DataFixture : IDisposable
    {
        private readonly string ConnectionString;

        public readonly IServiceProvider _services;

        public DataFixture()
        {
            // QueryMan Setup
            var dbId = RandomStringGenerator.WebHash();
#if DEBUG
            ConnectionString = $"Data Source=c:\\temp\\testdb_{dbId}.db;Mode=ReadWriteCreate";
#else
            ConnectionString = $"Data Source=memorydb_{dbId}.db;Mode=Memory";
#endif
            // DataContext and Services setup
            _services = Bootstrap.SetupServices(new ServiceCollection(), ConnectionString, ServiceLifetime.Scoped).BuildServiceProvider(true);

            // Migrations Setup
            Napos.Data.DataSetup.MigrateData(ConnectionString, false);
        }

        public IServiceScope CreateScope()
        {
            return _services.CreateScope();
        }

        public void Dispose()
        {
            (_services as IDisposable)?.Dispose();
        }
    }
}
