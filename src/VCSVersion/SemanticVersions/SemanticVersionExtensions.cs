using VCSVersion.Configuration;
using VCSVersion.Output;

namespace VCSVersion.SemanticVersions
{
    /// <summary>
    /// <see cref="SemanticVersion"/> extension methods
    /// </summary>
    public static class SemanticVersionExtensions
    {
        /// <summary>
        /// Converts a <see cref="SemanticVersion"/> into <see cref="VersionVariables"/>
        /// </summary>
        /// <param name="version">Semantic version</param>
        /// <param name="config">Effective configuration</param>
        /// <param name="isCurrentCommitTagged">Defines, if current commit is tagged</param>
        public static VersionVariables ToVersionVariables(this SemanticVersion version, EffectiveConfiguration config, bool isCurrentCommitTagged = false)
        {
            return new VersionVariablesProvider(version, config, isCurrentCommitTagged)
                .GetVariables();
        }
        
        /// <summary>
        /// Create a copy of <see cref="SemanticVersion"/> with defined changes
        /// </summary>
        /// <param name="version">Semantic version</param>
        /// <param name="major">Major version part to change</param>
        /// <param name="minor">Minor version part to change</param>
        /// <param name="patch">Patch version part to change</param>
        /// <param name="preReleaseTag">Pre-release version tag to change</param>
        /// <param name="buildMetadata">Build metadata to change</param>
        public static SemanticVersion Copy(this SemanticVersion version, int? major = null, int? minor = null, int? patch = null, PreReleaseTag preReleaseTag = null, BuildMetadata buildMetadata = null)
        {
            return new SemanticVersion(
                major: major ?? version.Major,
                minor: minor ?? version.Minor,
                patch: patch ?? version.Patch,
                preReleaseTag: preReleaseTag ?? version.PreReleaseTag,
                buildMetadata: buildMetadata ?? version.BuildMetadata);   
        }
    }
}