using VCSVersion.SemanticVersions;

namespace VCSVersion.VersionCalculation.IncrementStrategies
{
    public interface IIncrementStrategy
    {
        SemanticVersion IncrementVersion(SemanticVersion semver);
    }
}