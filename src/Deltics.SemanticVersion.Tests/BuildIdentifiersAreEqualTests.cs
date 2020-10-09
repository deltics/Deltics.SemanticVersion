using System;


namespace Deltics.SemanticVersioning.Tests
{
    using Xunit;
    using SemanticVersioning;


    public class BuildIdentifiersAreEqualTests
    {
        [Theory]
        [InlineData("0.0.1", "0.0.2")]
        [InlineData("0.0.1+arm86", "0.0.2+arm86")]
        [InlineData("0.0.1+arm86.linux", "0.0.2+linux.arm86")]
        public void BuildIdentifiersAreEqualIsTrueWhenIdentifiersAreEqual(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            SemanticVersion.BuildIdentifiersAreEqual(a, b);
        }

        
        [Theory]
        [InlineData("0.0.1+arm86", "0.0.1")]
        [InlineData("0.0.1+arm86", "0.0.1+arm64")]
        [InlineData("0.0.1", "0.0.1+arm64")]
        [InlineData("0.0.1+arm86.linux", "0.0.1+arm86.windows")]
        [InlineData("0.0.1+arm86.linux", "0.0.1+arm86")]
        public void BuildIdentifiersAreEqualIsFalseWhenIdentifiersAreDifferent(string versionA, string versionB)
        {
            var a = SemanticVersion.Parse(versionA);
            var b = SemanticVersion.Parse(versionB);

            SemanticVersion.BuildIdentifiersAreEqual(a, b);
        }
    }
}