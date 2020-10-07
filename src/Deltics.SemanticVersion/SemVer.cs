using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime;
using System.Text;

namespace Deltics.SemanticVersioning
{
    public class SemanticVersion
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        public string[] PreReleaseIdentifiers { get; }
        public string[] BuildIdentifiers { get; }

        public override string ToString()
        {
            var builder = new StringBuilder(254);
            builder.Append(Major).Append('.');
            builder.Append(Minor).Append('.');
            builder.Append(Patch);

            if (PreReleaseIdentifiers.Length > 0)
            {
                builder.Append('-');
                foreach (var identifier in PreReleaseIdentifiers)
                {
                    builder.Append(identifier).Append('.');
                }

                builder.Remove(builder.Length - 1, 1);
            }

            if (BuildIdentifiers.Length > 0)
            {
                builder.Append('+');
                foreach (var identifier in BuildIdentifiers)
                {
                    builder.Append(identifier).Append('.');
                }

                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();
        }


        private static void ValidateIdentifier(string identifier)
        {
            if (String.IsNullOrEmpty(identifier))
                throw new FormatException($"'{identifier}' is not a valid identifer.  Identifiers must not be empty.");

            const string ValidIdentifierChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";

            foreach (var c in identifier)
            {
                if (!(ValidIdentifierChars.Contains(c)))
                    throw new FormatException(
                        $"'{identifier}' is not a valid identifier.  Identifiers must be numeric or ASCII alpha-numerics (with hyphens).");
            }
        }


        private static string[] ValidatePreReleaseIdentifiers(IEnumerable<string> identifiers)
        {
            if (identifiers == null)
                return new string[0];

            var list = new List<string>();

            foreach (var identifier in identifiers)
            {
                ValidateIdentifier(identifier);

                if (int.TryParse(identifier, out var numericValue) && identifier.StartsWith("0"))
                    throw new FormatException(
                        $"'{identifier}' is not a valid identifier.  Numeric pre-release identifiers must not include leading zeroes.");

                list.Add(identifier);
            }

            return list.ToArray();
        }


        private static string[] ValidateBuildIdentifiers(IEnumerable<string> identifiers)
        {
            if (identifiers == null)
                return new string[0];

            var list = new List<string>();

            foreach (var identifier in identifiers)
            {
                ValidateIdentifier(identifier);
                list.Add(identifier);
            }

            return list.ToArray();
        }


        public SemanticVersion(int major, int minor = 0, int patch = 0,
            IEnumerable<String> prereleaseIdentifiers = null, IEnumerable<String> buildIdentifiers = null)
        {
            if (((major < 0) || (minor < 0) || (patch < 0))
                || ((major == 0) && (minor == 0) && (patch == 0)))
                throw new ArgumentOutOfRangeException(
                    $"'{major}.{minor}.{patch}' is not a valid Semantic Version number");

            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.PreReleaseIdentifiers = ValidatePreReleaseIdentifiers(prereleaseIdentifiers);
            this.BuildIdentifiers = ValidateBuildIdentifiers(buildIdentifiers);
        }


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


        public static int Compare(SemanticVersion a, SemanticVersion b)
        {
            var result = 0;

            result = b.Major - a.Major;
            if (result != 0)
                return Math.Sign(result);

            result = b.Minor - a.Minor;
            if (result != 0)
                return Math.Sign(result);

            result = b.Patch - a.Patch;
            if (result != 0)
                return Math.Sign(result);

            var aLen = a.PreReleaseIdentifiers.Length;
            var bLen = b.PreReleaseIdentifiers.Length;

            for (var i = 0; i < Math.Min(aLen, bLen); i++)
            {
                var sa = a.PreReleaseIdentifiers[i];
                var sb = b.PreReleaseIdentifiers[i];

                var aIsNumeric = Int32.TryParse(sa, out var ia);
                var bIsNumeric = Int32.TryParse(sb, out var ib);

                if (aIsNumeric && bIsNumeric)
                {
                    if (ia != ib)
                        return Math.Sign(ib - ia);
                }
                else if (aIsNumeric)
                    return 1;
                else if (bIsNumeric)
                    return -1;
                else
                {
                    result = String.Compare(sb, sa);
                    if (result != 0)
                        return result;
                }
            }

            return Math.Sign(aLen - bLen);
        }

        public static bool operator >(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) == -1;
        }

        public static bool operator <(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) == 1;
        }

        public static bool operator ==(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) == 0;
        }

        public static bool operator !=(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) != 0;
        }
    }
}