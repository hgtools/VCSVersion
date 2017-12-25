using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using VCSVersion.AssemblyVersioning;
using VCSVersion.Configuration;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;

namespace VCSVersion.Output
{
    public sealed class VersionVariablesProvider
    {
        private readonly VersionFormatValues _formatValues;
        
        public VersionVariablesProvider(SemanticVersion version, EffectiveConfiguration config, bool isCurrentCommitTagged = false)
        {
            _formatValues = new VersionFormatValues(version, config, isCurrentCommitTagged);
        }

        public VersionVariables GetVariables()
        {
            return new VersionVariables
            {
                Major = _formatValues.Major,
                Minor = _formatValues.Minor,
                Patch = _formatValues.Patch,
                BuildMetadata = _formatValues.BuildMetadata,
                BuildMetadataPadded = _formatValues.BuildMetadataPadded,
                FullBuildMetadata = _formatValues.FullBuildMetadata,
                BranchName = _formatValues.BranchName,
                Sha = _formatValues.Sha,
                MajorMinorPatch = _formatValues.MajorMinorPatch,
                SemVer = _formatValues.SemVer,
                FullSemVer = _formatValues.FullSemVer,
                AssemblySemVer = _formatValues.AssemblySemVer,
                AssemblyFileSemVer = _formatValues.AssemblyFileSemVer,
                PreReleaseTag = _formatValues.PreReleaseTag,
                PreReleaseTagWithDash = _formatValues.PreReleaseTagWithDash,
                PreReleaseLabel = _formatValues.PreReleaseLabel,
                PreReleaseNumber = _formatValues.PreReleaseNumber,
                InformationalVersion = _formatValues.InformationalVersion,
                CommitDate = _formatValues.CommitDate,
                NuGetVersion = _formatValues.NuGetVersion,
                NuGetPreReleaseTag = _formatValues.NuGetPreReleaseTag,
                CommitsSinceVersionSource = _formatValues.CommitsSinceVersionSource
            };
        }
    }
}