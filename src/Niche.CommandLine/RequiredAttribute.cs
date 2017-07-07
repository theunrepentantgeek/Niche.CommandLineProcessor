using System;

namespace Niche.CommandLine
{
    /// <summary>
    /// Mark a commandline parameter as mandatory
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RequiredAttribute : Attribute
    {
        // Nothing
    }
}
