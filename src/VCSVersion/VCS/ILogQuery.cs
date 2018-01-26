namespace VCSVersion.VCS
{
    /// <summary>
    /// Abstraction for a repository log query.
    /// </summary>
    public interface ILogQuery
    {
        /// <summary>
        /// Returns a <see cref="ILogQuery"/> that selects the first "n" commits of the set.
        /// </summary>
        /// <param name="amount">The number of commits to select.</param>
        ILogQuery First(int amount = 1);
        
        /// <summary>
        /// Returns a <see cref="ILogQuery"/> that selects the last "n" commits of the set.
        /// </summary>
        /// <param name="amount">The number of commits to select.</param>
        ILogQuery Last(int amount = 1);
    }
}