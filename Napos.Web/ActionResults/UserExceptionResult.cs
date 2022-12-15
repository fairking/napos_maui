using Microsoft.AspNetCore.Mvc;
using Napos.Core.Exceptions;
using Napos.Core.Models;
using System.Linq;

namespace Napos.Web.ActionResults
{
    public class UserErrorResult : BadRequestObjectResult
    {
        public UserErrorResult(string[] messages) : base(messages.Select(x => new ErrorVm() { Message = x }).ToArray()) { }

        public UserErrorResult(string[] propertyPaths, string[] messages) : base(messages.Select((x, idx) => new ErrorVm() { Path = propertyPaths[idx], Message = x }).ToArray()) { }

        public UserErrorResult(string message) : this(new[] { message }) { }

        public UserErrorResult(string propertyPath, string message) : this(new[] { propertyPath }, new[] { message }) { }

        public UserErrorResult(UserException userException) : this(userException.Properties, userException.Messages) { }
    }
}
