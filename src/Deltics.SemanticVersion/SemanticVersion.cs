using System;
using System.Collections.Generic;


namespace Deltics.SemanticVersioning
{
    public partial class SemanticVersion
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        public string[] PreReleaseIdentifiers { get; }
        public string[] BuildIdentifiers { get; }



        public SemanticVersion(int major, int minor = 0, int patch = 0,
            IEnumerable<string> prereleaseIdentifiers = null, IEnumerable<string> buildIdentifiers = null)
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
    }
}