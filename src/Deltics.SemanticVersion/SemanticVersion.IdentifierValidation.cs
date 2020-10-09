using System;
using System.Collections.Generic;
using System.Linq;


namespace Deltics.SemanticVersioning
{
    public partial class SemanticVersion
    {
        public static bool BuildIdentifiersAreEqual(SemanticVersion a, SemanticVersion b)
        {
            if ((a.BuildIdentifiers == null) && (b.BuildIdentifiers == null))
                return true;

            if ((a.BuildIdentifiers == null) || (b.BuildIdentifiers == null))
                return false;

            if (a.BuildIdentifiers.Length != b.BuildIdentifiers.Length)
                return false;
            
            foreach(var ident in a.BuildIdentifiers)
                if (!b.BuildIdentifiers.Contains(ident))
                    return false;

            return true;
        }
        
        
        private static void ValidateIdentifier(string identifier)
        {
            if (String.IsNullOrEmpty(identifier))
                throw new FormatException($"'{identifier}' is not a valid identifier.  Identifiers must not be empty.");

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
    }
}