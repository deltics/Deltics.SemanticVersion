using System;

namespace Deltics.SemanticVersioning
{
    public partial class SemanticVersion
    {
        public static SemanticVersion Parse(string value)
        {
            var hyphenPos = value.IndexOf('-');
            var plusPos = value.IndexOf('+');
            var isPreRelease = (hyphenPos != -1);
            var hasBuildInfo = (plusPos != -1);

            var version = value.Substring(0, isPreRelease ? hyphenPos : hasBuildInfo ? plusPos : value.Length);
            var prerelease = isPreRelease
                ? value.Substring(hyphenPos + 1, hasBuildInfo ? plusPos - hyphenPos - 1 : value.Length - hyphenPos - 1)
                : "";
            var buildInfo = hasBuildInfo ? value.Substring(plusPos + 1, value.Length - plusPos - 1) : "";

            var versionElements = version.Split('.');
            if (versionElements.Length != 3)
                throw new FormatException($"'{version}' is not a valid Semantic Version number");

            var major = int.Parse(versionElements[0]);
            var minor = int.Parse(versionElements[1]);
            var patch = int.Parse(versionElements[2]);

            if (((major < 0) || ((major > 0) && versionElements[0].StartsWith("0")))
                || ((minor < 0) || ((minor > 0) && versionElements[1].StartsWith("0")))
                || ((patch < 0) || ((patch > 0) && versionElements[2].StartsWith("0")))
                || ((major == 0) && (minor == 0) && (patch == 0)))
                throw new FormatException($"'{version}' is not a valid Semantic Version number");

            var prereleaseIdentifiers = isPreRelease ? prerelease.Split('.') : new string[0];
            var buildIdentifiers = hasBuildInfo ? buildInfo.Split('.') : new string[0];

            return new SemanticVersion(major, minor, patch, prereleaseIdentifiers, buildIdentifiers);
        }
    }
}