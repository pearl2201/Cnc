using System;
using System.Runtime.InteropServices;

namespace Cnc.Client.Handlers.Machine
{
    public interface IPlatformHandler
    {
        void CastCommand(string command);
        string CallCommand(string command);
    }
}