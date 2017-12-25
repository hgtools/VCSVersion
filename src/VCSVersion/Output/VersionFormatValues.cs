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
    public sealed class VersionFormatValues
    {
        private readonly SemanticVersion _version;
        private readonly EffectiveConfiguration _config;
        
        public string Major => _version.Major.ToString();
        public string Minor => _version.Minor.ToString();
        public string Patch => _version.Patch.ToString();
        
        public string PreReleaseTag => _version.PreReleaseTag;
        public string PreReleaseTagWithDash => _version.PreReleaseTag.IsNull() ? null : "-" + _version.PreReleaseTag;
        public string PreReleaseLabel => _version.PreReleaseTag.IsNull() ? null : _version.PreReleaseTag.Name;
        public string PreReleaseNumber => _version.PreReleaseTag.IsNull() ? null : _version.PreReleaseTag.Number.ToString();

        public string BuildMetadata => _version.BuildMetadata;
        public string BuildMetadataPadded => _version.BuildMetadata.ToString("p" + _config.BuildMetaDataPadding);
        public string FullBuildMetadata => _version.BuildMetadata.ToString("f");

        public string BranchName => _version.BuildMetadata.Branch;
        public string Sha => _version.BuildMetadata.Hash;

        public string CommitDate => 
            _version.BuildMetadata
                .CommitDate
                .UtcDateTime
                .ToString(_config.CommitDateFormat, CultureInfo.InvariantCulture);

        public string CommitsSinceVersionSource => 
            _version.BuildMetadata
                .CommitsSinceVersionSource
                .ToString(CultureInfo.InvariantCulture);

        public string CommitsSinceVersionSourcePadded => 
            _version.BuildMetadata
                .CommitsSinceVersionSource
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(_config.CommitsSinceVersionSourcePadding, '0');

        public string AssemblySemVer => _version.GetAssemblyVersion(_config.AssemblyVersioningScheme);
        public string AssemblyFileSemVer => _version.GetAssemblyFileVersion(_config.AssemblyFileVersioningScheme);

        public string MajorMinorPatch => $"{_version.Major}.{_version.Minor}.{_version.Patch}";
        public string SemVer => _version.ToString();
        public string FullSemVer => _version.ToString("f");
        public string InformationalVersion => GetInformationalVersion();
        
        public string NuGetVersion => _version.ToString("t");
        public string NuGetPreReleaseTag => _version.PreReleaseTag.IsNull() ? null : _version.PreReleaseTag.ToString("t").ToLower();
        
        public VersionFormatValues(SemanticVersion version, EffectiveConfiguration config, bool isCurrentCommitTagged = false)
        {
            _version = version.FormatVersion(config, isCurrentCommitTagged);
            _config = config;
        }
        
        private string GetInformationalVersion()
        {
            if (string.IsNullOrEmpty(_config.AssemblyInformationalFormat))
                return _version.ToString("i");
            
            try
            {
                return _config.AssemblyInformationalFormat.FormatWith(this);
            }
            catch (ArgumentException ex)
            {
                throw new WarningException(
                    $"Unable to format AssemblyInformationalVersion. " +
                    $"Check your format string: {ex.Message}");
            }
        }
    }
}