using System;
using Xunit;


namespace Deltics.SemanticVersioning.Tests
{
    public partial class OperatorTests
    {
        [Theory]
        [InlineData("0.0.2", "0.0.1")]
        [InlineData("0.1.0", "0.0.1")]
        [InlineData("1.0.0", "0.0.1")]
        [InlineData("1.0.0", "0.9.9")]
        [InlineData("1.0.0", "0.99.99")]
        [InlineData("1.999.999", "1.999.998")]
        [InlineData("1.999.999", "1.998.999")]
        [InlineData("2.0.0", "1.999.999")]
        public void LessThanOrEqualIsTrueWhenComparingReleaseVersionsAndOneIsLesser(string versionA,
            string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(b <= a);
        }


        [Theory]
        [InlineData("0.0.2")]
        [InlineData("0.1.0")]
        [InlineData("1.0.0")]
        [InlineData("1.999.999")]
        [InlineData("2.0.0")]
        public void LessThanOrEqualIsTrueWhenComparingEqualVersions(string versionA)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionA);

            Assert.True(a <= b);
        }


        [Theory]
        [InlineData("1.0.0", "1.0.0-alpha")]
        [InlineData("1.0.0-alpha", "1.0.0-alpha.beta")]
        [InlineData("1.0.0-alpha", "1.0.0-alpha.1")]
        [InlineData("1.0.0-alpha.2", "1.0.0-alpha.1")]
        [InlineData("1.0.0-beta", "1.0.0-alpha")]
        [InlineData("1.0.0-beta", "1.0.0-alpha.1")]
        public void LessThanOrEqualIsTrueWhenComparingReleaseVersionWithPreReleaseIdentifiersAndOneIsLesser(
            string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(b <= a);
        }


        [Theory]
        [InlineData("1.0.0-alpha")]
        [InlineData("1.0.0-alpha.2")]
        [InlineData("1.0.0-beta")]
        public void LessrThanOrEqualIsTrueWhenComparingEqualVersionsWithPreReleaseIdentifiers(string versionA)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionA);

            Assert.True(a <= b);
        }


        [Theory]
        [InlineData("1.0.0+arm64", "1.0.0")]
        [InlineData("1.0.0+x86", "1.0.0+x64")]
        [InlineData("1.0.0-alpha+x86", "1.0.0-alpha+x64")]
        [InlineData("1.0.1+x86", "1.0.0+x86")]
        [InlineData("1.0.1+x64", "1.0.0+x86")]
        public void LessThanOrEqualIsTrueForVersionsThatDifferInBuildInformation(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(b <= a);
        }


        [Theory]
        [InlineData("0.0.2", "0.0.3")]
        [InlineData("0.1.0-alpha", "0.1.0")]
        public void LessThanOrEqualIsFalseWhenVersionIsGreater(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.False(b <= a);
        }
    }
}