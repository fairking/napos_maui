using System;
using System.ComponentModel.DataAnnotations;

namespace Napos.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValRequiredAttribute : ValidationAttribute
    {
        private readonly bool _notEmpty;

        public ValRequiredAttribute(bool notEmpty = true) : base((string)null)
        {
            _notEmpty = notEmpty;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (_notEmpty && string.IsNullOrWhiteSpace(value?.ToString()))
                return false;

            if (_notEmpty && value is DateTime && ((DateTime)value) == DateTime.MinValue)
                return false;

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            if (!string.IsNullOrWhiteSpace(this.ErrorMessageString))
                return this.ErrorMessageString;
            else
                return string.Format(Resources.Strings.Validation_Required, name);
        }
    }
}
