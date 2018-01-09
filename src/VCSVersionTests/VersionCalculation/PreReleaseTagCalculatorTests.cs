using NUnit.Framework;
using VCSVersion.VersionCalculation;
using VCSVersionTests.Configuration;

namespace VCSVersionTests.VersionCalculation
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class PreReleaseTagCalculatorTests
    {
       [Test] 
       public void TagCalculator_CorrectlyHandlesRuCharsInBranchName()
       {
            var tag = PreReleaseTagCalculator.GetBranchSpecificTag(new TestEffectiveConfiguration(tag: "useBranchName"), "Ветка", null);
            Assert.That("Ветка", Is.EqualTo(tag) );
       }
    }
}
