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
    /// Wrapper class that handles a simple parameter - something that takes a value
    /// </summary>
    public class CommandLineParameter : CommandLineOptionBase
    {
        /// <summary>
        /// Gets the short form of this switch
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets the other short form of this switch
        /// </summary>
        public string AlternateShortName { get; private set; }

        /// <summary>
        /// Gets the long form of this switch
        /// </summary>
        public string LongName { get; private set; }

        /// <summary>
        /// Test to see if the specified method is a parameter
        /// </summary>
        /// Parameters are methods with no return type and exactly one parameter that have a
        /// [Description] attribute.
        /// <param name="method">Method to test.</param>
        /// <returns>True if it is a parameter, false otherwise.</returns>
        public static bool IsParameter(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            return method.ReturnType == typeof(void)
                && method.GetParameters().Length == 1
                && method.GetCustomAttribute<DescriptionAttribute>() != null;
        }


        public static IEnumerable<CommandLineParameter> CreateParameters(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var methods = instance.GetType().GetMethods();

            var parameters
                = methods.Where(IsParameter)
                    .Select(m => new CommandLineParameter(instance, m))
                    .ToList();

            return parameters;
        }
        public CommandLineParameter(object instance, MethodInfo method)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (!method.DeclaringType.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Expect method to be callable on instance");
            }

            mInstance = instance;
            mMethod = method;
            mParameterInfo = method.GetParameters().Single();

            ShortName = "-" + CamelCase.ToShortName(method.Name);
            AlternateShortName = "/" + CamelCase.ToShortName(method.Name);
            LongName = "--" + CamelCase.ToDashedName(method.Name);
        }

        /// <summary>
        /// Activate this parameter when found
        /// </summary>
        public override void Activate(Queue<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            var parameter = arguments.Dequeue();
            var parameters = new object[] { parameter };
            if (mParameterInfo.ParameterType != typeof(string))
            {
                parameters[0] = ConvertValue(parameter, mParameterInfo.ParameterType);
            }

            mMethod.Invoke(mInstance, parameters);
        }

        /// <summary>
        /// Add triggers to activate this option to the passed dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary that collects our triggers</param>
        public override void AddTo(Dictionary<string, CommandLineOptionBase> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            dictionary[ShortName] = this;
            dictionary[AlternateShortName] = this;
            dictionary[LongName] = this;
        }

        private readonly MethodInfo mMethod;
        private readonly ParameterInfo mParameterInfo;
        private readonly object mInstance;
    }
}
