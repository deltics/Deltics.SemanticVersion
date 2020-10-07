using System;
using Xunit;


namespace Deltics.SemanticVersioning.Tests
{
    public class OperatorTests
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
        public void GreaterThanCorrectlyComparesReleaseVersion(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(a > b);
        }
        
        
        [Theory]
        [InlineData("1.0.0", "1.0.0-alpha")]
        [InlineData("1.0.0-alpha", "1.0.0-alpha.beta")]
        [InlineData("1.0.0-alpha", "1.0.0-alpha.1")]
        [InlineData("1.0.0-alpha.2", "1.0.0-alpha.1")]
        [InlineData("1.0.0-beta", "1.0.0-alpha")]
        [InlineData("1.0.0-beta", "1.0.0-alpha.1")]
        public void GreaterThanFollowsPrecedenceForPreReleaseVersions(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(a > b);
        }

        
        [Theory]
        [InlineData("1.0.0+arm64", "1.0.0")]
        [InlineData("1.0.0+x86", "1.0.0+x64")]
        [InlineData("1.0.0-alpha+x86", "1.0.0-alpha+x64")]
        public void GreaterThanIsFalseForEqualVersionsWithBuildInformation(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.False(a > b);
        }
        
        
        [Theory]
        [InlineData("0.0.2")]
        [InlineData("0.1.0")]
        [InlineData("1.0.0")]
        [InlineData("2.0.0")]
        public void GreaterThanIsFalseWhenReleaseVersionsAreEqual(string version)
        {
            var a = SemanticVersion.Parse(version);
            var b = SemanticVersion.Parse(version);

            Assert.False(a > b);
        }        

        
        [Theory]
        [InlineData("0.0.2", "0.0.1")]
        [InlineData("0.1.0", "0.0.1")]
        [InlineData("1.0.0", "0.0.1")]
        [InlineData("1.0.0", "0.9.9")]
        [InlineData("1.0.0", "0.99.99")]
        [InlineData("2.0.0", "1.999.999")]
        public void LessThanCorrectlyComparesReleaseVersions(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(b < a);
        }

        
        [Theory]
        [InlineData("1.0.0", "1.0.0-alpha")]
        [InlineData("1.0.0-alpha", "1.0.0-alpha.beta")]
        [InlineData("1.0.0-alpha", "1.0.0-alpha.1")]
        [InlineData("1.0.0-alpha.2", "1.0.0-alpha.1")]
        [InlineData("1.0.0-beta", "1.0.0-alpha")]
        [InlineData("1.0.0-beta", "1.0.0-alpha.1")]
        public void LessThanFollowsPrecedenceForPreReleaseVersions(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(b < a);
        }
        
        
        [Theory]
        [InlineData("1.0.0", "1.0.0+arm86")]
        [InlineData("1.0.0+x64", "1.0.0+x86")]
        [InlineData("1.0.0-alpha+x64", "1.0.0-alpha+x86")]
        public void LessThanIsFalseForEqualVersionsWithBuildInformation(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.False(b < a);
        }
        
        
        [Theory]
        [InlineData("0.0.2")]
        [InlineData("0.1.0")]
        [InlineData("1.0.0")]
        [InlineData("2.0.0")]
        public void LessThanIsFalseWhenReleaseVersionsAreEqual(string version)
        {
            var a = SemanticVersion.Parse(version);
            var b = SemanticVersion.Parse(version);

            Assert.False(a < b);
        }
        
        
        [Theory]
        [InlineData("0.0.2")]
        [InlineData("0.1.0")]
        [InlineData("1.0.0")]
        [InlineData("2.0.0")]
        public void EqualityIsTrueWhenReleaseVersionsAreEqual(string version)
        {
            var a = SemanticVersion.Parse(version);
            var b = SemanticVersion.Parse(version);

            Assert.True(a == b);
        }
        
        
        [Theory]
        [InlineData("0.0.2", "0.0.1")]
        [InlineData("0.1.0", "0.0.1")]
        [InlineData("1.0.0", "0.0.1")]
        [InlineData("1.0.0", "0.9.9")]
        [InlineData("1.0.0", "0.99.99")]
        [InlineData("2.0.0", "1.999.999")]
        public void EqualityIsFalseWhenReleaseVersionsDiffer(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.False(a == b);
        }


        [Theory]
        [InlineData("0.0.2", "0.0.1")]
        [InlineData("0.1.0", "0.0.1")]
        [InlineData("1.0.0", "0.0.1")]
        [InlineData("1.0.0", "0.9.9")]
        [InlineData("1.0.0", "0.99.99")]
        [InlineData("2.0.0", "1.999.999")]
        public void InequalityIsTrueWhenReleaseVersionsDiffer(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(a != b);
        }

        
        [Theory]
        [InlineData("0.0.2")]
        [InlineData("0.1.0")]
        [InlineData("1.0.0")]
        [InlineData("2.0.0")]
        public void InequalityIsFalseWhenReleaseVersionsAreEqual(string version)
        {
            var a = SemanticVersion.Parse(version);
            var b = SemanticVersion.Parse(version);

            Assert.False(a != b);
        }

        
        [Theory]
        [InlineData("1.0.0+arm86", "1.0.0+arm64")]
        public void InequalityIsFalseWhenVersionsDifferOnlyInBuildInformation(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.False(a != b);
        }
        
        
        [Theory]
        [InlineData("1.0.0+arm86", "1.0.0+arm64")]
        [InlineData("1.0.0+arm86", "1.0.0")]
        [InlineData("1.0.0", "1.0.0+arm64")]
        public void EqualityIsTrueEvenWhenVersionsDifferInBuildInformation(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            Assert.True(a == b);
        }
    }
}