using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    /// Factory used to create different kinds of CommandLineOption
    /// </summary>
    public static class CommandLineOptionFactory
    {
        /// <summary>
        /// Test to see if the specified method is a switch
        /// </summary>
        /// Switches are methods with no return type and no parameters that have
        /// a [Description] attribute.
        /// <param name="method">Method to test</param>
        /// <returns>True if it is a switch, false otherwise.</returns>
        public static bool IsSwitch(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            return method.ReturnType == typeof(void)
                && method.GetParameters().Length == 0
                && method.GetCustomAttribute<DescriptionAttribute>() != null;
        }

        public static IEnumerable<CommandLineSwitch> CreateSwitches(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var methods = instance.GetType().GetMethods();

            var switches
                = methods.Where(IsSwitch)
                    .Select(m => new CommandLineSwitch(instance, m))
                    .ToList();

            return switches;
        }
    }
}
