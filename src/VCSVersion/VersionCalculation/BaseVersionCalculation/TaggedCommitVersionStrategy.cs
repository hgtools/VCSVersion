using System;
using System.Collections.Generic;
using System.Linq;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace VCSVersion.VersionCalculation.BaseVersionCalculation
{
    /// <summary>
    /// Version is extracted from all tags on the ancestors of current commit, and not newer than the current commit.
    /// <see cref="BaseVersion.Source"/> is the tag's commit.
    /// Increments if the tag is not the current commit.
    /// </summary>
    public sealed class TaggedCommitVersionStrategy : IBaseVersionStrategy
    {
        public IEnumerable<BaseVersion> GetVersions(IVersionContext context)
        {
            return GetTaggedVersions(context, context.CurrentCommit);
        }

        private static IEnumerable<BaseVersion> GetTaggedVersions(IVersionContext context, ICommit currentCommit)
        {
            var repository = context.Repository;
            var allTags = repository.Tags()
                .Where(tag => tag.Commit.When <= currentCommit.When)
                .ToList();

            var tagsOnBranch = repository
                .Log(select => select.AncestorsOf(currentCommit.Hash))
                .SelectMany(commit => allTags.Where(tag => IsValidTag(tag, commit)))
                .Select(tag =>
                {
                    if (!SemanticVersion.TryParse(tag.Name, context.Configuration.TagPrefix, out var version))
                        return null;
                    
                    var commit = tag.Commit;
                    if (commit == null)
                        return null;

                    return new VersionTaggedCommit(commit, version, tag.Name);
                })
                .Where(commit => commit != null)
                .ToList();

            return tagsOnBranch.Select(t => CreateBaseVersion(context, t));
        }

        private static BaseVersion CreateBaseVersion(IVersionContext context, VersionTaggedCommit version)
        {
            var shouldUpdateVersion = version.Commit.Hash != context.CurrentCommit.Hash;
            return new BaseVersion(FormatType(version), version.SemVer, version.Commit, shouldUpdateVersion);
        }

        private static string FormatType(VersionTaggedCommit version)
        {
            return $"Hg tag '{version.Tag}'";
        }

        private static bool IsValidTag(ITag tag, ICommit commit)
        {
            return tag.Commit.Equals(commit);
        }

        private sealed class VersionTaggedCommit
        {
            public string Tag { get; }
            public ICommit Commit { get; }
            public SemanticVersion SemVer { get; }

            public VersionTaggedCommit(ICommit commit, SemanticVersion semVer, string tag)
            {
                Tag = tag;
                Commit = commit;
                SemVer = semVer;
            }
        }
    }
}
