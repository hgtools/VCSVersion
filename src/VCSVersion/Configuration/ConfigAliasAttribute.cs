using System;

namespace VCSVersion.Configuration
{
    /// <summary>
    /// Set alias name to configurable element
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ConfigAliasAttribute : Attribute
    {
        /// <summary>
        /// Configuration alias name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Create an instance of <see cref="ConfigAliasAttribute"/>
        /// </summary>>
        public ConfigAliasAttribute(string name)
        {
            Name = name;
        }
    }
}