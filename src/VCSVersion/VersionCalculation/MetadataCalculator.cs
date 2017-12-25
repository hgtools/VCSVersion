using System.Linq;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace VCSVersion.VersionCalculation
{
    public sealed class MetadataCalculator : IMetadataCalculator
    {
        public BuildMetadata CalculateMetadata(IVersionContext context, ICommit baseVersionSource)
        {
            var repository = context.Repository;
            var commits = repository
                .Log(select => select.Range(
                    baseVersionSource.Hash, 
                    context.CurrentCommit.Hash));

            var count = commits.Count();
            var commitsSinceTag = count - 1;
         
            Logger.WriteInfo(
                $"{count} commits found between " +
                $"{baseVersionSource.Hash} and {context.CurrentCommit.Hash}");

            return new BuildMetadata(
                commitsSinceTag,
                context.CurrentBranch.Name,
                repository.Tip().Hash,
                repository.Tip().When);
        }
    }
}