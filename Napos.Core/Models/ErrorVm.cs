using Napos.Core.Attributes;

namespace Napos.Core.Models
{
    [Api]
    public class ErrorVm
    {
        /// <summary>
        /// Property name/path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Validation message (user exception)
        /// </summary>
        public string Message { get; set; }
    }
}
