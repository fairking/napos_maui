using Napos.Core.Abstract;
using Napos.Core.Attributes;
using Napos.Core.Models;
using System.Diagnostics;

namespace Napos.Domain.Services.System
{
    [Api]
    public class LogService : IService
    {
        public LogService()
        {

        }

        [Api]
        public void LogClientError(ClientErrorVm model)
        {
            Debug.WriteLine("JS Error: " + model.Message);
        }
    }
}
