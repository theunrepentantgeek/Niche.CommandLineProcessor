using System;
using System.Collections.Generic;
using System.Linq;

namespace Niche.CommandLine
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Test to see if the passed Type implements IEnumerable{T} for any T.
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Results of the test. True if it implements IEnumerable&lt;T&gt;; false otherwise.</returns>
        public static bool IsIEnumerable(this Type type)
        {
            if (type.IsGenericType == false)
            {
                return false;
            }

            if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return true;
            }

            var interfaces = type.GetInterfaces();
            return interfaces.Any(
                i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        /// <summary>
        /// Gets the type of the argument to IEnumerable{T} 
        /// or null if the type does not implement IEnumerable{T}
        /// </summary>
        /// <param name="type">Possible <see cref="IEnumerable{T}"/> type.</param>
        /// <returns>Item type if type is enumerable, null otherwise.</returns>
        public static Type GetIEnumerableItemType(this Type type)
        {
            if (type.IsGenericType == false)
            {
                return null;
            }

            if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments().Single();
            }

            var interfaces = type.GetInterfaces();
            var enumerableType
                = interfaces.FirstOrDefault(
                    i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumerableType.GetGenericArguments().Single();
        }

        /// <summary>
        /// Test to see if the passed Type is a <see cref="KeyValuePair{TKey,TValue}"/> for any 
        /// <c>TKey</c>, <c>TValue</c>
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>True if the type is a KeyValue type</returns>
        public static bool IsKeyValuePair(this Type type)
        {
            if (type.IsGenericType == false)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
        }

        /// <summary>
        /// Get the key type used by the KeyValuePair
        /// </summary>
        /// <param name="type">Type from which to extract the key type.</param>
        /// <returns>Extracted type value.</returns>
        public static Type GetKeyValueKeyType(this Type type)
        {
            if (type.IsKeyValuePair() == false)
            {
                return null;
            }

            var arguments = type.GetGenericArguments();
            return arguments[0];
        }

        /// <summary>
        /// Get the value type used by the KeyValuePair
        /// </summary>
        /// <param name="type">Type from which to extract the value type.</param>
        /// <returns>Extracted type value.</returns>
        public static Type GetKeyValueValueType(this Type type)
        {
            if (type.IsKeyValuePair() == false)
            {
                return null;
            }

            var arguments = type.GetGenericArguments();
            return arguments[1];
        }
    }
}
