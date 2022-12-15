using Microsoft.Extensions.DependencyInjection;
using Napos.Core.Abstract;
using System;

namespace Napos.Domain.Services.Base
{
    public abstract class BaseService : IService
    {
        private readonly IServiceProvider _services;

        public BaseService(IServiceProvider services)
        {
            _services = services;
        }

        /// <summary>
        /// Get service or throw an exception if the service does not exists in the service collection.
        /// </summary>
        protected T GetService<T>()
        {
            return _services.GetRequiredService<T>();
        }
    }
}
