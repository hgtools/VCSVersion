using System;
using System.Collections.Generic;

namespace VCSVersion.SemanticVersions
{
    /// <summary>
    /// <see cref="SemanticVersion"/> comparer
    /// </summary>
    public sealed class SemanticVersionComparer : IEqualityComparer<SemanticVersion>
    {
        public static readonly SemanticVersionComparer Default = 
            new SemanticVersionComparer(SemanticVersionComparation.Full);

        private readonly SemanticVersionComparation _comparation;

        /// <summary>
        /// Creates an instance of <see cref="SemanticVersionComparer"/>
        /// </summary>
        /// <param name="comparation"><see cref="SemanticVersion"/> comparation mode</param>
        public SemanticVersionComparer(SemanticVersionComparation comparation)
        {
            _comparation = comparation;
        }

        public bool Equals(SemanticVersion @this, SemanticVersion that)
        {
            if (ReferenceEquals(null, that)) return false;
            if (ReferenceEquals(@this, that)) return true;

            switch (_comparation)
            {
                case SemanticVersionComparation.MajorMinorPatch:
                    return @this.Major == that.Major
                           && @this.Minor == that.Minor
                           && @this.Patch == that.Patch;
                    
                case SemanticVersionComparation.Full:
                    return @this.Major == that.Major 
                           && @this.Minor == that.Minor 
                           && @this.Patch == that.Patch 
                           && @this.PreReleaseTag == that.PreReleaseTag 
                           && @this.BuildMetadata == that.BuildMetadata;
                    
                default:
                    throw new InvalidOperationException($"SemanticVersionComparation: {_comparation} is not supported.");
            }
        }

        public int GetHashCode(SemanticVersion version)
        {
            unchecked
            {
                var hashCode = version.Major;
                hashCode = (hashCode * 397) ^ version.Minor;
                hashCode = (hashCode * 397) ^ version.Patch;

                if (_comparation == SemanticVersionComparation.Full)
                {
                    hashCode = (hashCode * 397) ^ (version.PreReleaseTag?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ (version.BuildMetadata?.GetHashCode() ?? 0);
                }
                
                return hashCode;
            }
        }
    }
}