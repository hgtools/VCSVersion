using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using VCSVersion;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;
using VCSVersion.VersionCalculation;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersionTests.VersionCalculation
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class NextVersionCalculatorTests
    {
        [Test]
        public void CalculateNotTaggedCommitVersionTest()
        {
            var baseCalculatorMock = new Mock<IBaseVersionCalculator>();
            var baseCalculator = baseCalculatorMock.Object;
            baseCalculatorMock.Setup(c => c.CalculateVersion(It.IsAny<IVersionContext>()))
                .Returns(new BaseVersion("", new SemanticVersion(1), null, shouldIncrement: true));

            var metadatCalculatorMock = new Mock<IMetadataCalculator>();
            var metadataCalculator = metadatCalculatorMock.Object;
            metadatCalculatorMock.Setup(m => m.CalculateMetadata(
                    It.IsAny<IVersionContext>(),
                    It.IsAny<ICommit>()))
                .Returns((BuildMetadata) null);
            
            var tagCalculatorMock = new Mock<IPreReleaseTagCalculator>();
            var tagCalculator = tagCalculatorMock.Object;
            tagCalculatorMock.Setup(t => t.CalculateTag(
                    It.IsAny<IVersionContext>(),
                    It.IsAny<SemanticVersion>(),
                    It.IsAny<string>()))
                .Returns((PreReleaseTag)null);

            var contextMock = new Mock<IVersionContext>();
            var context = contextMock.Object;
            contextMock.Setup(c => c.IsCurrentCommitTagged).Returns(false);

            var versionCalculator = new NextVersionCalculator(
                baseCalculator,
                metadataCalculator,
                tagCalculator);
            
            var version = versionCalculator.CalculateVersion(context);
            var expected = SemanticVersion.Parse("1.0.1");
            
            Assert.That(version, Is.EqualTo(expected));
        }

        [Test]
        public void CalculateTaggedCommitVersionTest()
        {
            // todo: implement test
        }
    }
}
