using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Napos.Web.Filters;
using Napos.Web.Services;
using System.Threading.Tasks;

namespace Napos.Web.Controllers
{
    public class DefaultController : ControllerBase
    {
        private readonly ServiceExecutor _serviceExecuter;
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ServiceExecutor serviceExecuter, ILogger<DefaultController> logger)
        {
            _serviceExecuter = serviceExecuter;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        [DefaultExceptionFilter]
        public async Task<IActionResult> Index(string service, string operation, string? id)
        {
            return await _serviceExecuter.ExecuteService(this, service, operation);
        }
    }
}
