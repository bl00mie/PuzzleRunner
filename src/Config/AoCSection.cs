using Microsoft.Extensions.Configuration;

namespace PuzzleRunner.Config
{
    public class AoCSection : SettingsSection
    {
        public static new string SectionName => "aoc";

        public string? DefaultYear { get; set; }
        public string? DefaultDay { get; set; }
        public string? SessionCookie { get; set; }
    }
}
