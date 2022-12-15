using Napos.Core.Abstract;
using System;

namespace Napos.Domain.Services.System
{
    public class ConfigService
    {
        public string ConnectionString { get; private set; }

        public ConfigService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            ConnectionString = connectionString;
        }
    }
}
