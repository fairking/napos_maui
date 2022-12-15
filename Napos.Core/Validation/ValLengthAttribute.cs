using System;
using System.ComponentModel.DataAnnotations;

namespace Napos.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValLengthAttribute : ValidationAttribute
    {
        private readonly int _max;
        private readonly int _min;

        public ValLengthAttribute(int max, int min = 0) : base((string)null)
        {
            if (max <= 0)
                throw new ArgumentException("Max must be greater than 0.", nameof(max));

            _max = max;

            if (min < 0)
                throw new ArgumentException("Min must be greater than 0.", nameof(min));

            _min = min;
        }

        public int Max { get { return _max; } }

        public int Min { get { return _min; } }

        public override bool IsValid(object value)
        {
            if (value == null) // We are allowing nulls
                return true;

            if (value.ToString().Length > _max)
                return false;

            if (_min > 0 && value.ToString().Length < _min)
                return false;

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            if (_min > 0)
                return this.ErrorMessageString;
            else
                return this.ErrorMessageString;
        }
    }
}
