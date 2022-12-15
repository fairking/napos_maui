using System;
using System.ComponentModel.DataAnnotations;

namespace Napos.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValUrlAttribute : ValidationAttribute
    {
        public ValUrlAttribute() : base((string)null)
        {
        }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return true;

            return new UrlAttribute().IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessageString;
        }
    }
}
