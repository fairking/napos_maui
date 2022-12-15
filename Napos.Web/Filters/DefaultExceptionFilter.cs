using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Napos.Core.Exceptions;
using Napos.Web.ActionResults;
using System.Reflection;
using System.Threading.Tasks;

namespace Napos.Web.Filters
{
    public class DefaultExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<DefaultExceptionFilter> _logger;

        public DefaultExceptionFilter(ILogger<DefaultExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception != null && !context.ExceptionHandled)
            {
                if (context.Exception is TargetInvocationException && context.Exception.InnerException != null)
                    context.Exception = context.Exception.InnerException;

                if (context.Exception is UserException)
                {
                    _logger.LogWarning(context.Exception, context.Exception.Message);
                    context.Result = new UserErrorResult(context.Exception as UserException);
                }
                else if (context.Exception is ServiceNotFoundException)
                {
                    _logger.LogWarning(context.Exception, context.Exception.Message);
                    context.Result = new NotFoundResult();
                }
                else if (context.Exception is ForbidException)
                {
                    _logger.LogWarning(context.Exception, context.Exception.Message);
                    context.Result = new ForbidResult();
                }
                else if (context.Exception is UnauthorizedException)
                {
                    _logger.LogWarning(context.Exception, context.Exception.Message);
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    _logger.LogError(context.Exception, context.Exception.Message);
                    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                context.ExceptionHandled = true;
            }
        }
    }

    public class DefaultExceptionFilterAttribute : TypeFilterAttribute
    {
        public DefaultExceptionFilterAttribute() : base(typeof(DefaultExceptionFilter))
        {
        }
    }
}
