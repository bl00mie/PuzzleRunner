using Microsoft.Extensions.Configuration;
using PuzzleRunner.Config;
using System.Diagnostics;

namespace PuzzleRunner.Solution
{
    public abstract class Solution
    {
        public virtual IEnumerable<Func<string>> Parts { get; } = Enumerable.Empty<Func<string>>();

        public abstract void ParseInput(IEnumerable<string> lines);

        public static void Write<T>(T message) => Debug.Write(message);
        public static void Write(string template, params object[] os) => Debug.Write(string.Format(template, os));

        public static void WL() => Debug.WriteLine(string.Empty);
        public static void WL<T>(T message) => Debug.WriteLine(message);
        public static void WL(string template, params object[] os) => Debug.WriteLine(string.Format(template, os));

        public static bool OutOfBounds(Point c, int maxX, int maxY, int minX = 0, int minY = 0)
            => c.x < minX || c.x > maxX || c.y < minY || c.y > maxY;
        public static bool OutOfBounds(int x, int y, int maxX, int maxY, int minX = 0, int minY = 0)
            => x < minX || x > maxX || y < minY || y > maxY;
    }
}
