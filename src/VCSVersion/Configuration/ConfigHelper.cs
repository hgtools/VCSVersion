using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VCSVersion.Helpers;

namespace VCSVersion.Configuration
{
    /// <summary>
    /// Utility methods for working with configuration
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Returns a list of configuration aliases for entities with type <typeparamref name="T"/> 
        /// </summary>
        /// <typeparam name="T">Type of entities which aliases are discovering</typeparam>
        public static IEnumerable<string> GetConfigurationAliases<T>()
        {
            return typeof(T)
                .GetImplemtetions()
                .Select(type => type.GetCustomAttribute<ConfigAliasAttribute>()?.Name)
                .Where(name => !string.IsNullOrEmpty(name));
        }

        /// <summary>
        /// Returns a list of entities with type <typeparamref name="T"/> by specific aliases
        /// </summary>
        /// <param name="aliases">A list of specific aliases</param>
        /// <typeparam name="T">Type of discoverable entities</typeparam>
        public static IEnumerable<T> GetEntities<T>(IEnumerable<string> aliases)
        {
            var mapping = CreateEntitiesMapping<T>();
            return aliases.Select(alias => mapping[alias]);
        }
        
        private static Dictionary<string, T> CreateEntitiesMapping<T>()
        {
            var types = typeof(T).GetImplemtetions();
            var mapping = new Dictionary<string, T>();

            foreach (var type in types)
            {
                var alias = type.GetCustomAttribute<ConfigAliasAttribute>()?.Name;
                if (string.IsNullOrEmpty(alias))
                    continue;
                
                mapping[alias] = (T)Activator.CreateInstance(type);
            }

            return mapping;
        }
    }
}