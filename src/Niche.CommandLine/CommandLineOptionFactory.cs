﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

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
                throw new ArgumentNullException(nameof(method));
            }

            return method.ReturnType == typeof(void)
                && method.GetParameters().Length == 0
                && method.GetCustomAttribute<DescriptionAttribute>() != null;
        }

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
                throw new ArgumentNullException(nameof(method));
            }

            return method.ReturnType == typeof(void)
                && method.GetParameters().Length == 1
                && method.GetCustomAttribute<DescriptionAttribute>() != null;
        }

        /// <summary>
        /// Test to see if the specified method is a mode
        /// </summary>
        /// Modes are methods with a return value, no parameters and a [Description] attribute.
        /// <typeparam name="T">Required type for the return value</typeparam>
        /// <param name="method">Method to test.</param>
        /// <returns>True if the method is a mode, false otherwise.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool IsMode<T>(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return typeof(T).GetTypeInfo().IsAssignableFrom(method.ReturnType)
                && !method.GetParameters().Any()
                && method.GetCustomAttribute<DescriptionAttribute>() != null;
        }

        /// <summary>
        /// Create switches for the passed instance
        /// </summary>
        /// <param name="instance">Instance for which switches should be created.</param>
        /// <returns>Sequence of switches, possibly empty.</returns>
        public static IReadOnlyList<CommandLineSwitch> CreateSwitches(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var methods = instance.GetType().GetTypeInfo().GetMethods();

            var switches
                = methods.Where(IsSwitch)
                    .Select(m => new CommandLineSwitch(instance, m))
                    .ToList();

            return switches;
        }

        /// <summary>
        /// Create parameters for the passed instance
        /// </summary>
        /// <param name="instance">Instance for which parameters should be created.</param>
        /// <returns>Sequence of parameters, possibly empty.</returns>
        public static IReadOnlyList<CommandLineOptionBase> CreateParameters(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var methods = instance.GetType().GetTypeInfo().GetMethods()
                .Where(IsParameter);

            var result = new List<CommandLineOptionBase>();

            var parameterType = typeof(CommandLineParameter<>);
            foreach (var m in methods)
            {
                var p = m.GetParameters().Single();
                var valueType = p.ParameterType;
                if (valueType.IsIEnumerable())
                {
                    valueType = valueType.GetIEnumerableItemType();
                }

                var t = parameterType.MakeGenericType(valueType);
                var parameter = (CommandLineOptionBase)Activator.CreateInstance(t, instance, m);
                result.Add(parameter);
            }

            return result;
        }

        /// <summary>
        /// Create Modes for the passed instance.
        /// </summary>
        /// <typeparam name="T">Base type of all modes.</typeparam>
        /// <param name="instance">Instance for which modes should be created</param>
        /// <returns>Sequence of modes, possibly empty.</returns>
        public static IReadOnlyList<CommandLineMode> CreateModes<T>(T instance)
            where T : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var methods = instance.GetType().GetTypeInfo().GetMethods();

            var modes
                = methods.Where(IsMode<T>)
                    .Select(m => new CommandLineMode(typeof(T), instance, m))
                    .ToList();

            return modes;
        }
    }
}
