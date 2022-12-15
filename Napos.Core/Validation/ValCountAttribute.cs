using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Napos.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValCountAttribute : ValidationAttribute
    {
        private readonly int _minCount;
        private readonly int _maxCount;
        private readonly bool _allowEmptyStringValues;

        public ValCountAttribute(int minCount, int maxCount = 0, bool allowEmptyStringValues = false) : base((string)null)
        {
            if (minCount < 0)
                throw new ArgumentException("MinCount cannot be less than 0.", nameof(minCount));

            if (maxCount < 0)
                throw new ArgumentException("MaxCount cannot be less than 0.", nameof(minCount));

            _minCount = minCount;
            _maxCount = maxCount;
            _allowEmptyStringValues = allowEmptyStringValues;
        }

        public int MinCount { get { return _minCount; } }

        public int MaxCount { get { return _maxCount; } }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var stringList = value as ICollection<string>;
            if (!_allowEmptyStringValues && stringList != null)
            {
                var count = stringList.Count(s => !string.IsNullOrWhiteSpace(s));
                return count >= _minCount && (_maxCount == 0 || count <= _maxCount);
            }

            var list = value as ICollection;
            if (list != null)
            {
                return list.Count >= _minCount && (_maxCount == 0 || list.Count <= _maxCount);
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            if (_maxCount == 0)
                return this.ErrorMessageString;
            else
                return this.ErrorMessageString;
        }
    }
}
