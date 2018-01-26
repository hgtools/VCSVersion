﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCSVersion.AssemblyVersioning;
using VCSVersion.Configuration;
using VCSVersion.Output;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;
using VCSVersion.VersionCalculation.IncrementStrategies;
using VCSVersion.VersionCalculation.VersionFilters;
using VCSVersionTests.Configuration;

namespace VCSVersionTests.Output
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class VersionFormatValuesTests
    {
        [Test]
        [TestCase("yyyy-MM-dd", "2017-10-06")]
        [TestCase("dd.MM.yyyy", "06.10.2017")]
        [TestCase("yyyyMMdd", "20171006")]
        [TestCase("yyyy-MM", "2017-10")]
        public void CommitDateFormatTest(string format, string expectedOutcome)
        {
            var date = new DateTime(2017, 10, 6);
            var buildMetadata = new BuildMetadata(
                0, "master", 
                "3139d4eeb044f46057693473eacc2655b3b27e7d", 
                new DateTimeOffset(date, TimeSpan.Zero)); // assume time zone is UTC

            var config = new EffectiveConfiguration(
                    AssemblyVersioningScheme.MajorMinorPatch, 
                    AssemblyFileVersioningScheme.MajorMinorPatch, 
                    "", VersioningMode.ContinuousDelivery, "", "", "", 
                    IncrementStrategyType.Inherit,
                    "", true, "", "", false, "", "", "", "", 
                    CommitMessageIncrementMode.Enabled, 4, 4, 
                    Enumerable.Empty<IVersionFilter>(), false, true, format, null, 10);

            var version = new SemanticVersion(1, 0, 0, null, buildMetadata);
            var values = new VersionFormatValues(version, config);

            Assert.That(values.CommitDate, Is.EqualTo(expectedOutcome));
        }
    }
}
