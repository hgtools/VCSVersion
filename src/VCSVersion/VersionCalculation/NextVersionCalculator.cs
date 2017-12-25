using VCSVersion.SemanticVersions;
using VCSVersion.VCS;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion.VersionCalculation
{
    /// <inheritdoc />
    public sealed class NextVersionCalculator : IVersionCalculator
    {
        private static readonly IBaseVersionCalculator DefaultBaseVersionCalculator =
            new BaseVersionCalculator(
                new FallbackBaseVersionStrategy(),
                // todo: implement new ConfigNextVersionBaseVersionStrategy(),
                new TaggedCommitVersionStrategy(),
                new MergeMessageBaseVersionStrategy());
                // todo: implement new VersionInBranchNameBaseVersionStrategy());
                // todo: implement new TrackReleaseBranchesVersionStrategy());

        private static readonly IMetadataCalculator DefaultMetadataCalculator =
            new MetadataCalculator();

        private static readonly IPreReleaseTagCalculator DefaultTagCalculator =
            new PreReleaseTagCalculator();

        private readonly IBaseVersionCalculator _baseVersionCalculator;
        private readonly IMetadataCalculator _metadataCalculator;
        private readonly IPreReleaseTagCalculator _tagCalculator;

        public NextVersionCalculator(
            IBaseVersionCalculator baseVersionCalculator = null,
            IMetadataCalculator metadataCalculator = null,
            IPreReleaseTagCalculator tagCalculator = null)
        {
            _baseVersionCalculator = baseVersionCalculator ?? DefaultBaseVersionCalculator;
            _metadataCalculator = metadataCalculator ?? DefaultMetadataCalculator;
            _tagCalculator = tagCalculator ?? DefaultTagCalculator;
        }

        /// <inheritdoc />
        public SemanticVersion CalculateVersion(IVersionContext context)
        {
            if (context.IsCurrentCommitTagged)
            {
                return CalculateTaggedCommitVersion(context);
            }

            return CalculateNonTaggedCommitVersion(context);
        }

        private SemanticVersion CalculateTaggedCommitVersion(IVersionContext context)
        {
            // If current commit is tagged, don't do anything except add build metadata
            // Will always be 0, don't bother with the +0 on tags

            var buildMetadata = _metadataCalculator.CalculateMetadata(context, context.CurrentCommit);
            var nonTaggedVersion = CalculateNonTaggedCommitVersion(context);
            var taggedVersion = context.CurrentCommitTaggedVersion;
            var commitsSinceVersionSource = nonTaggedVersion.BuildMetadata.CommitsSinceVersionSource;

            return new SemanticVersion(
                major: taggedVersion.Major,
                minor: taggedVersion.Minor,
                patch: taggedVersion.Patch,
                preReleaseTag: taggedVersion.PreReleaseTag,
                buildMetadata: new BuildMetadata(
                    commitsSinceTag: null,
                    branch: buildMetadata.Branch,
                    commitHash: buildMetadata.Hash,
                    commitDate: buildMetadata.CommitDate,
                    commitsSinceVersionSource: commitsSinceVersionSource,
                    otherMetadata: buildMetadata.OtherMetadata));
        }

        private SemanticVersion CalculateNonTaggedCommitVersion(IVersionContext context)
        {
            var baseVersion = _baseVersionCalculator.CalculateVersion(context);

            // todo: implement MainlineVersionCalculator
            //if (context.Configuration.VersioningMode == VersioningMode.Mainline)
            //{
            //    var mainlineMode = new MainlineVersionCalculator(_metadataCalculator);
            //    return mainlineMode.CalculateVersion(baseVersion, context);
            //}

            var semver = baseVersion.MaybeIncrement(context);
            var buildMetadata = _metadataCalculator.CalculateMetadata(context, baseVersion.Source);
            var preReleaseTag = _tagCalculator.CalculateTag(context, semver, baseVersion.BranchNameOverride);

            return new SemanticVersion(
                semver.Major,
                semver.Minor,
                semver.Patch,
                preReleaseTag,
                buildMetadata);
        }

    }
}