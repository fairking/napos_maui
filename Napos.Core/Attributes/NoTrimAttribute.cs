using System;

namespace Napos.Core.Attributes
{
    /// <summary>
    /// Do not trim string properties for the selected view model class or particular property
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class NoTrimAttribute : Attribute
    {
    }
}
