using System;
using System.Collections.Generic;
using System.Linq;

namespace VCSVersion.Helpers
{
    /// <summary>
    /// Utility methods to work with reflection
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns a list of types that implemet <paramref name="type"/>
        /// </summary>
        /// <param name="type">Abstract/interface type</param>
        public static IEnumerable<Type> GetImplemtetions(this Type type)
        {
            return type.Assembly
                .GetExportedTypes()
                .Where(impl => !impl.IsAbstract && !impl.IsInterface)
                .Where(impl => type.IsAssignableFrom(impl));
        }
    }
}