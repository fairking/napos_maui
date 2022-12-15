using System;
using System.ComponentModel.DataAnnotations;

namespace Napos.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValRegexAttribute : ValidationAttribute
    {
        private readonly string _pattern;

        public ValRegexAttribute(string pattern) : base((string)null)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException(nameof(pattern));

            _pattern = pattern;
        }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return true;

            return new RegularExpressionAttribute(_pattern).IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessageString;
        }
    }
}
