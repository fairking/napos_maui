using System;

namespace Napos.Core.Exceptions
{
    /// <summary>
    /// Only marked with [Api] Services and Methods (operations) are allowed.
    /// Othervise throw this exception. 
    /// </summary>
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string service) : base($"Service '{service}' not found.")
        {

        }

        public ServiceNotFoundException(string service, string operation) : base($"Service operation '{service}.{operation}' not found.")
        {

        }
    }
}
