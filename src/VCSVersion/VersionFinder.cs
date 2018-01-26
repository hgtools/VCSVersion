using System.Linq;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion
{
    public class VersionFinder
    {
        public SemanticVersion FindVersion(IVersionContext context)
        {
            var branchName = context.CurrentBranch.Name;
            var commit = context.CurrentCommit == null ? "-" : context.CurrentCommit.Hash;
            Logger.WriteInfo($"Running against branch: {branchName} ({commit})");

            if (context.IsCurrentCommitTagged)
            {
                Logger.WriteInfo(
                    $"Current commit is tagged with version {context.CurrentCommitTaggedVersion}, " 
                    + "version calcuation is for metadata only.");
            }

            var calculator = new NextVersionCalculator(
                new BaseVersionCalculator(context
                    .Configuration
                    .BaseVersionStrategies
                    .ToArray()));
            
            return calculator.CalculateVersion(context);
        }
    }
}
