using Microsoft.Extensions.DependencyInjection;
using Napos.Data;
using Napos.Domain.Tests.Fixtures;
using System;
using Xunit;

namespace Napos.Domain.Tests.Base
{
    public abstract class BaseTest : IClassFixture<DataFixture>, IClassFixture<BagFixture>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly BagFixture _bagFixture;

        protected DataContext Db => GetService<DataContext>();
        protected BagFixture Bag => _bagFixture;

        public BaseTest(DataFixture dataFixture, BagFixture bagFixture)
        {
            _scope = dataFixture.CreateScope();
            _bagFixture = bagFixture;
        }

        protected T GetService<T>()
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
