using PuzzleRunner.Caching;
using PuzzleRunner.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleRunner.AoC
{
    internal class Puzzle2021_24 : AoCSolution
    {
        string[][] cmds = Array.Empty<string[]>();
        public override void ParseInput(IEnumerable<string> lines)
            => cmds = lines.Select(line => line.Split(' ')).ToArray();

        public override string Part1()
        {
            d1 = 9; d2 = 0; delta = -1;
            return MONAD((0, 0, 0, 0, 0)).val;
        }

        public override string Part2()
        {
            d1 = 1; d2 = 10; delta = 1;
            return MONAD((0, 0, 0, 0, 0)).val;
        }

        const int Z = 9_000_000;
        int d1;
        int d2;
        int delta;

        [Cache]
        (bool success, string val) MONAD((int p, int w, int x, int y, int z) input)
        {
            var (p, w, x, y, z) = input;
            if (z > Z)
                return (false, "");
            if (p >= cmds.Length)
                return (z == 0, "");
            var vars = new[] { w, x, y, z };
            //WL($"[{w} {x} {y} {z}]");
            var cmd = cmds[p];
            var a = cmd[1][0] - 'w';
            if (cmd[0][1] == 'n')
            {
                for (int d = d1; d != d2; d += delta)
                {
                    vars[a] = d;
                    var (success, val) = MONAD((p + 1, vars[0], vars[1], vars[2], vars[3]));
                    if (success)
                    {
                        var answer = (true, $"{d}{val}");
                        WL($"{p} {w} {x} {y} {z} {answer}");
                        return answer;
                    }
                }
            }
            else
            {
                var b = char.IsLetter(cmd[2][0]) ? vars[cmd[2][0] - 'w'] : int.Parse(cmd[2]);
                vars[a] = cmd[0][1] switch
                {
                    'd' => vars[a] + b,
                    'u' => vars[a] * b,
                    'i' => vars[a] / b,
                    'o' => vars[a] % b,
                    'q' => vars[a] == b ? 1 : 0,
                    _ => throw new Exception("wtaf")
                };
            }
            return MONAD((p + 1, vars[0], vars[1], vars[2], vars[3]));
        }
    }
}
