using System.Text.RegularExpressions;
using VCSVersion.Configuration;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;

namespace VCSVersion.Output
{
    /// <summary>
    /// Semantic version format methods
    /// </summary>
    public static class VersionFormatExtensions
    {
        /// <summary>
        /// Format semantic version according to current configuration
        /// </summary>
        /// <param name="version">Semantic version</param>
        /// <param name="config">Current configuration</param>
        /// <param name="isCurrentCommitTagged">Defines if current commit is tagged</param>
        public static SemanticVersion FormatVersion(this SemanticVersion version, EffectiveConfiguration config, bool isCurrentCommitTagged = false)
        {
            return version
                .EnsurePreReleaseTag(config, isCurrentCommitTagged)
                .AppendTagNumberPattern(config)
                .PromoteNumberOfCommitsToTagNumber(config, isCurrentCommitTagged);
        }
        
        private static SemanticVersion PromoteNumberOfCommitsToTagNumber(this SemanticVersion version, EffectiveConfiguration config, bool isCurrentCommitTagged)
        {
            if (IsContinuousDeploymentMode(config, isCurrentCommitTagged)
                || CanAppendTagNumberPattern(version, config)
                || config.VersioningMode == VersioningMode.Mainline)
            {
                // For continuous deployment the commits since tag gets promoted to the pre-release number
                return version.Copy(
                    preReleaseTag: new PreReleaseTag(
                        name: version.PreReleaseTag.Name, 
                        number: version.BuildMetadata.CommitsSinceTag),
                    buildMetadata: new BuildMetadata(
                        commitsSinceTag: null,
                        branch: version.BuildMetadata.Branch,
                        commitHash: version.BuildMetadata.Hash,
                        commitDate: version.BuildMetadata.CommitDate,
                        commitsSinceVersionSource: version.BuildMetadata.CommitsSinceTag ?? 0,
                        otherMetadata: version.BuildMetadata.OtherMetadata));
            }
            
            return version;
        }

        private static SemanticVersion EnsurePreReleaseTag(this SemanticVersion version, EffectiveConfiguration config, bool isCurrentCommitTagged)
        {
            // Continuous Deployment always requires a pre-release tag unless the commit is tagged
            if (config.VersioningMode != VersioningMode.ContinuousDeployment || isCurrentCommitTagged)
                return version;
            
            if (!version.PreReleaseTag.IsNull())
                return version;
            
            return version.Copy(
                preReleaseTag: new PreReleaseTag(
                    name: EnsurePreReleaseTagName(version, config), 
                    number: version.BuildMetadata.CommitsSinceTag));
        }
        
        private static string EnsurePreReleaseTagName(SemanticVersion version, EffectiveConfiguration config)
        {
            var name = PreReleaseTagCalculator.GetBranchSpecificTag(config, version.BuildMetadata.Branch, null);

            if (!string.IsNullOrEmpty(name))
                return name;

            return config.ContinuousDeploymentFallbackTag;
        }

        private static SemanticVersion AppendTagNumberPattern(this SemanticVersion version, EffectiveConfiguration config)
        {
            if (!CanAppendTagNumberPattern(version, config))
                return version;
            
            var match = Regex.Match(version.BuildMetadata.Branch, config.TagNumberPattern);
            var numberGroup = match.Groups["number"];
            if (!numberGroup.Success)
                return version;

            var tagNumberPattern = numberGroup.Value.PadLeft(config.BuildMetaDataPadding, '0');
            return version.Copy(
                preReleaseTag: new PreReleaseTag(
                    name: version.PreReleaseTag.Name + tagNumberPattern, 
                    number: version.PreReleaseTag.Number));
        }

        private static bool IsContinuousDeploymentMode(EffectiveConfiguration config, bool isCurrentCommitTagged)
        {
            return config.VersioningMode == VersioningMode.ContinuousDeployment
                && !isCurrentCommitTagged;
        }

        private static bool CanAppendTagNumberPattern(SemanticVersion version, EffectiveConfiguration config)
        {
            return !string.IsNullOrEmpty(config.TagNumberPattern) 
                && !version.PreReleaseTag.IsNull();
        }
    }
}