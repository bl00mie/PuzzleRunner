using PuzzleRunner.Caching;
using PuzzleRunner.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleRunner
{
    public class Puzzle2017_01 : AoCSolution
    {
        private IEnumerable<string> input = Enumerable.Empty<string>();

        public override void ParseInput(IEnumerable<string> lines)
        {
            input = lines.ToArray();
        }

        public override string Part1()
        {
            var stuff = DoAThing((1, 2));
            stuff = DoAThing(stuff);
            stuff = DoAThing(stuff);
            stuff = DoAThing(stuff);
            return string.Empty;
        }

        public override string Part2()
        {
            return string.Empty;
        }

        [Cache]
        public (int x, int y) DoAThing((int x, int y) input)
        {
            return new (input.y, input.x);
        }
    }
}
