using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Cnc.Client.Handlers.Machine.Window
{
    public class WindowHandler : IPlatformHandler
    {
        public string CallCommand(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public void CastCommand(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"-c \"{escapedArgs}\"";
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}