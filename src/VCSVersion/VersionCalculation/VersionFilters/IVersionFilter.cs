using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion.VersionCalculation.VersionFilters
{
    public interface IVersionFilter
    {
        bool Exclude(BaseVersion baseVersion, out string reason);
    }
}
