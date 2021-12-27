namespace PuzzleRunner
{
    public static class NumericExtensions
    {
        public static T Clamp<T>(this T value, T min, T max) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            => (value.CompareTo(min) <= 0)? min : (value.CompareTo(max) >= 0) ? max : value;

        public static IEnumerable<int> Factors(this int value)
        {
            var factors = new HashSet<int> { 1, value };
            int max = value;
            for (int i=2; i<max; i++)
                if (value % i == 0)
                {
                    max = value / i;
                    factors.Add(i);
                    factors.Add(max);
                }
            return factors;
        }
    }
}
