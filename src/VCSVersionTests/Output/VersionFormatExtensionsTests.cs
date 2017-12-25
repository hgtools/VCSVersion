using NUnit.Framework;
using VCSVersion.Output;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;
using VCSVersionTests.Configuration;

namespace VCSVersionTests.Output
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class VersionFormatExtensionsTests
    {
        [Test]
        public void FormatVersion_ShouldPromoteNumberOfCommitsToTagNumber_WhenContinuousDeployment()
        {
            var version = SemanticVersion.Parse("1.1.0-alpha.1+3");
            var expected = SemanticVersion.Parse("1.1.0-alpha.3");
            
            var config = new TestEffectiveConfiguration(versioningMode: VersioningMode.ContinuousDeployment);
            var formatted = version.FormatVersion(config);

            Assert.That(formatted, Is.EqualTo(expected));
        }
    }
}