using System;

namespace Deltics.SemanticVersioning.Tests
{
    using Xunit;
    using Deltics.SemanticVersioning;


    public class ParseTests
    {
        [Theory]
        [InlineData("0.0.1", 0, 0, 1)]
        [InlineData("0.1.0", 0, 1, 0)]
        [InlineData("1.0.0", 1, 0, 0)]
        [InlineData("1.2.3", 1, 2, 3)]
        [InlineData("1234567890.1234567890.1234567890", 1234567890, 1234567890, 1234567890)]
        public void ParseCorrectlyHandlesVersionWithNoIdentifiers(string version, int expectedMajor, int expectedMinor, int expectedPatch)
        {
            var semver = SemanticVersion.Parse(version);

            Assert.Equal(expectedMajor, semver.Major);
            Assert.Equal(expectedMinor, semver.Minor);
            Assert.Equal(expectedPatch, semver.Patch);
            Assert.Empty(semver.PreReleaseIdentifiers);
            Assert.Empty(semver.BuildIdentifiers);
        }


        [Theory]
        [InlineData("1.1.0-alpha", 1, 1, 0, new string[] { "alpha" }, new string[] { })]
        [InlineData("1.1.0-alpha.1", 1, 1, 0, new string[] { "alpha", "1" }, new string[] { })]
        [InlineData("1.1.0-alpha+20200702", 1, 1, 0, new string[] { "alpha" }, new string[] { "20200702" })]
        [InlineData("1.1.0-alpha.1+20200702", 1, 1, 0, new string[] { "alpha", "1" }, new string[] { "20200702" })]
        [InlineData("1.1.0-alpha.1+20200702.1", 1, 1, 0, new string[] { "alpha", "1" }, new string[] { "20200702", "1" })]
        [InlineData("1.1.0+20200702", 1, 1, 0, new string[] { }, new string[] { "20200702" })]
        [InlineData("1.1.0+20200702.1", 1, 1, 0, new string[] { }, new string[] { "20200702", "1" })]
        public void ParseCorrectlyHandlesVersionWithValidIdentifiers(string version, int expectedMajor, int expectedMinor, int expectedPatch, string[] expectedPreReleaseIdentifiers, string[] expectedBuildIdentifiers)
        {
            var semver = SemanticVersion.Parse(version);

            Assert.Equal(expectedMajor, semver.Major);
            Assert.Equal(expectedMinor, semver.Minor);
            Assert.Equal(expectedPatch, semver.Patch);
            Assert.Equal(expectedPreReleaseIdentifiers, semver.PreReleaseIdentifiers);
            Assert.Equal(expectedBuildIdentifiers, semver.BuildIdentifiers);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1.1")]
        [InlineData("alpha.1")]
        [InlineData("01.1.1")]
        [InlineData("1.01.1")]
        [InlineData("1.1.01")]
        [InlineData("1.0.0-alpha..rc1")]
        [InlineData("1.0.0-alpha#1")]
        [InlineData("1.0.0-001")]
        [InlineData("1.0.0+01..02")]
        [InlineData("1.0.0+#01")]
        [InlineData("a.b.c-alpha")]
        public void ParseThrowsFormatExceptionForInvalidVersion(string version)
        {
            Assert.Throws<FormatException>(() => SemanticVersion.Parse(version));
        }

        [Theory]
        [InlineData("0.0.1")]
        [InlineData("0.1.0")]
        [InlineData("1.0.0")]
        [InlineData("1.2.3")]
        [InlineData("1234567890.1234567890.1234567890")]
        [InlineData("1.1.0-alpha")]
        [InlineData("1.1.0-alpha.1")]
        [InlineData("1.1.0-alpha+20200702")]
        [InlineData("1.1.0-alpha.1+20200702")]
        [InlineData("1.1.0-alpha.1+20200702.1")]
        [InlineData("1.1.0+20200702")]
        [InlineData("1.1.0+20200702.1")]
        public void ToStringReconstructsValidSemVerFromParsedVersion(string version)
        {
            var semver = SemanticVersion.Parse(version);

            Assert.Equal(version, semver.ToString());
        }
    }
}