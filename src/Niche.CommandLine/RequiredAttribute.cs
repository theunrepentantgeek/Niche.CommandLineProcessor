using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
