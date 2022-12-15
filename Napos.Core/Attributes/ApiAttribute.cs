using System;

namespace Napos.Core.Attributes
{
    /// <summary>
    /// Marked classes or enums will be added into the swagger scheme api.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Enum)]
    public class ApiAttribute : Attribute
    {
        public readonly bool Transactional;

        public ApiAttribute() { }

        public ApiAttribute(bool transactional)
        {
            Transactional = transactional;
        }
    }
}
