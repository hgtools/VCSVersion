namespace VCSVersion.VCS
{
    /// <summary>
    /// Abstraction for a repository log query builder.
    /// </summary>
    public interface ILogQueryBuilder 
    {
        /// <summary>
        /// Return a <see cref="ILogQuery"/> that selects a commit based on its unique hash number.
        /// </summary>
        /// <param name="hash">The commit unique hash.</param>
        ILogQuery Single(string hash);

        /// <summary>
        /// Create a <see cref="ILogQuery"/> that finds commits that belong to the named branch.
        /// </summary>
        /// <param name="name">Branch name.</param>
        ILogQuery ByBranch(string name);

        /// <summary>
        /// Create a <see cref="ILogQuery"/> that includes the commit
        /// specified and all ancestor commits.
        /// </summary>
        /// <param name="hash">The commit hash to end with.</param>
        ILogQuery AncestorsOf(string hash);

        /// <summary>
        /// Create a <see cref="ILogQuery" /> that includes a range
        /// of commits between <paramref name="fromHash"/> and <paramref name="toHash"/>.
        /// </summary>
        /// <param name="fromHash">Hash of first commit to include.</param>
        /// <param name="toHash">Hash of last commit to include.</param>
        ILogQuery Range(string fromHash, string toHash);

        /// <summary>
        /// Create a <see cref="ILogQuery"/> that includes the commit
        /// with tags by specific tag pattern.
        /// </summary>
        /// <param name="pattern">Tag pattern</param>
        ILogQuery Tagged(string pattern);

        /// <summary>
        /// Create a <see cref="ILogQuery"/> that includes the commit with tags.
        /// </summary>
        ILogQuery Tagged();

        /// <summary>
        /// Create a <see cref="ILogQuery"/> that includes the commit
        /// intersected with <paramref name="left"/> query and <paramref name="right"/> query.
        /// </summary>
        /// <param name="left">Left part of intersection</param>
        /// <param name="right">Right part of intersection</param>
        ILogQuery Intersect(ILogQuery left, ILogQuery right);
    }
}