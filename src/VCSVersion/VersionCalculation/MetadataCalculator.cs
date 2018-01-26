using System.Linq;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace VCSVersion.VersionCalculation
{
    public sealed class MetadataCalculator : IMetadataCalculator
    {
        public BuildMetadata CalculateMetadata(IVersionContext context, ICommit baseVersionSource)
        { 
            var commitsCount = context
                .Repository
                .Count(select => select.Range(
                    baseVersionSource.Hash, 
                    context.CurrentCommit.Hash));

            var commitsSinceTag = commitsCount - 1;
         
            Logger.WriteInfo(
                $"{commitsCount} commits found between " +
                $"{baseVersionSource.Hash} and {context.CurrentCommit.Hash}");

            return new BuildMetadata(
                commitsSinceTag,
                context.CurrentBranch.Name,
                context.CurrentCommit.Hash,
                context.CurrentCommit.When);
        }
    }
}