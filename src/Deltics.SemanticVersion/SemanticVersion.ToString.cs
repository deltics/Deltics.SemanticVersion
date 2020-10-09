using System.Text;

namespace Deltics.SemanticVersioning
{
    public partial class SemanticVersion
    {
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
        }    }
}