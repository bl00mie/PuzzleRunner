using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PuzzleRunner.Util
{
    public class Clipboard
    {
        public static void Copy(string text)
        {
            string cmd="", options="";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                cmd = "pbcopy";
                options = "-pboard general -Prefer txt";
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
    }
}
