using Microsoft.Extensions.Configuration;
using PuzzleRunner.Config;
using System.Net;

namespace PuzzleRunner.Solution
{
    public abstract class AoCSolution : SolutionBase
    {
        public override IEnumerable<Func<string>> Parts { get; }
        
        public AoCSolution()
        {
            Parts = new[] { Part1, Part1 };
        }

        public abstract string Part1();

        public abstract string Part2();
    }
}
