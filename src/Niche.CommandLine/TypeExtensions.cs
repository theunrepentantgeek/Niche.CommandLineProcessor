using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Test to see if the passed Type implements IEnumerable&lt;T&gt for any T.
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
        /// Gets the type of the argument to IEnumerable&lt;T&gt, 
        /// or null if the type does not implement IEnumerable&lt;T&gt
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
    }
}
