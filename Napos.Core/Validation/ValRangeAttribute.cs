using System;
using System.ComponentModel.DataAnnotations;

namespace Napos.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValRangeAttribute : ValidationAttribute
    {
        private readonly double _max;
        private readonly double _min;

        public ValRangeAttribute(double min, double max) : base((string)null)
        {
            _min = min;
            _max = max;
        }

        public double Min { get { return _min; } }

        public double Max { get { return _max; } }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            //return (double)value >= _min && (double)value <= _max;

            return new RangeAttribute(_min, _max).IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessageString;
        }
    }
}
