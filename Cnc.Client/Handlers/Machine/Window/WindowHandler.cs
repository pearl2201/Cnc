using System;
using System.Runtime.InteropServices;

namespace Cnc.Client.Handlers.Machine.Window
{
    public class WindowHandler : IPlatformHandler
    {
        public string CallCommand(string command)
        {
            throw new NotImplementedException();
        }

        public void CastCommand(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "command";
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}