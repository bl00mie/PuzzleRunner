namespace PuzzleRunner
{
    public static class StringExtensions
    {
        public static IEnumerable<int> GetInts(this string value, char delim = ',') => value.Split(delim).Select(int.Parse);
        
        public static IEnumerable<long> GetLongs(this string value, char delim = ',') => value.Split(delim).Select(long.Parse);
        
        public static IEnumerable<double> GetDoubles(this string value, char delim = ',') => value.Split(delim).Select(double.Parse);

        public static int Int(this string value, int radix = 10) => Convert.ToInt32(value, radix);

        public static long Long(this string value, int radix = 10) => Convert.ToInt64(value, radix);
    }
}
