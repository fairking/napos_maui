using Napos.Core.Abstract;
using Napos.Data;
using System;

namespace Napos.Domain.Services.Base
{
    public abstract class BaseDataService : BaseService, IScopedService, IDisposable
    {
        protected DataContext Db => GetService<DataContext>();

        public BaseDataService(IServiceProvider services) : base(services)
        {
        }

        public virtual void Dispose()
        {

        }
    }
}
