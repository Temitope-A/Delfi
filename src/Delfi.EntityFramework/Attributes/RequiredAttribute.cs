using System;

namespace Delfi.EntityFramework.Attributes
{
    /// <summary>
    /// Represents a required property of an entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : Attribute { }
}
