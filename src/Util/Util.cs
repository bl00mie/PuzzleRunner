using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace PuzzleRunner
{
    internal static class Util
    {
        public static IEnumerable<IEnumerable<string>> GroupInput(IEnumerable<string> input)
        {
            var groups = new List<IEnumerable<string>>();
            var group = new List<string>();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    groups.Add(group);
                    group = new List<string>();
                    continue;
                }
                group.Add(line);
            }
            if (group.Count > 0)
                groups.Add(group);
            return groups;
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list)
        {
            var items = ImmutableList.CreateRange(list);
            var stack = ImmutableStack<(ImmutableList<T> cur, int pos, ImmutableList<T> acc)>.Empty;

            var (curitems, pos, acc) = (items, 0, ImmutableList<T>.Empty);

            while (true)
            {
                if (pos >= curitems.Count)
                {
                    if (!stack.Any()) yield break;
                    if (!curitems.Any()) yield return acc;

                    (curitems, pos, acc) = stack.Peek();
                    pos += 1;
                    stack = stack.Pop();
                }
                else
                {
                    stack = stack.Push((curitems, pos, acc));
                    (curitems, pos, acc) = (curitems.RemoveAt(pos), 0, acc.Add(curitems[pos]));
                }
            }
        }

        public static IEnumerable<IList<T>> GetVariations<T>(IList<T> offers, int length)
        {
            var startIndices = new int[length];
            var variationElements = new HashSet<T>();

            while (startIndices[0] < offers.Count)
            {
                var variation = new List<T>(length);
                var valid = true;
                for (int i = 0; i < length; ++i)
                {
                    var element = offers[startIndices[i]];
                    if (variationElements.Contains(element))
                    {
                        valid = false;
                        break;
                    }
                    variation.Add(element);
                    variationElements.Add(element);
                }
                if (valid)
                    yield return variation;

                startIndices[length - 1]++;
                for (int i = length - 1; i > 0; --i)
                {
                    if (startIndices[i] >= offers.Count)
                    {
                        startIndices[i] = 0;
                        startIndices[i - 1]++;
                    }
                    else
                        break;
                }
                variationElements.Clear();
            }
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            var rnd = new Random();
            return source.OrderBy((item) => rnd.Next());
        }


        public static ulong GCD(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }

        public static ulong LCM(ulong a, ulong b)
        {
            return a / GCD(a, b) * b;
        }

        public static void Copy(string text)
        {
            string cmd = "", options = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                cmd = "pbcopy";
                options = "-pboard general -Prefer txt";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cmd = "clip";
            }
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo(cmd, options)
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = true
            };
            process.Start();
            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit();
        }

        public static void PaintGrid(IEnumerable<(int x, int y)> grid, char filled = '█', char empty = ' ')
        {
            var X = grid.Max(xy => xy.x);
            var Y = grid.Max(xy => xy.y);
            var gridSB = new StringBuilder();
            for (int y = 0; y <= Y; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x <= X; x++)
                    sb.Append(grid.Contains((x, y)) ? filled : empty);
                gridSB.AppendLine(sb.ToString());
            }
            Debug.WriteLine(gridSB);
        }

        public static void PaintGrid<T>(Dictionary<(int x, int y), T> grid, string empty = " ", string? delim = null)
        {
            var X = grid.Keys.Max(xy => xy.x);
            var Y = grid.Keys.Max(xy => xy.y);
            var gridSB = new StringBuilder();
            for (int y = 0; y <= Y; y++)
            {
                var row = new string[X + 1];
                for (int x = 0; x <= X; x++)
                    row[x] = grid[(x, y)]?.ToString() ?? empty;
                gridSB.AppendLine(string.Join(delim, row));
            }
            gridSB.AppendLine();
            Debug.WriteLine(gridSB);
        }
    }
}
