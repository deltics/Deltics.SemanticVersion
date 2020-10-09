using System;

namespace Deltics.SemanticVersioning
{
    public partial class SemanticVersion
    {
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
    }
}