namespace Deltics.SemanticVersioning
{
    public partial class SemanticVersion
    {
        public static bool operator >(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) == -1;
        }

        public static bool operator <(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) == 1;
        }

        public static bool operator >=(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) <= 0;
        }

        public static bool operator <=(SemanticVersion a, SemanticVersion b)
        {
            return Compare(a, b) >= 0;
        }

        public static bool operator ==(SemanticVersion a, SemanticVersion b)
        {
            return (Compare(a, b) == 0);
        }

        public static bool operator !=(SemanticVersion a, SemanticVersion b)
        {
            return (Compare(a, b) != 0);
        }


        public override bool Equals(object obj)
        {
            if (obj is SemanticVersion other)
                return other == this;
            else
                return base.Equals(obj);
        }
    }
}