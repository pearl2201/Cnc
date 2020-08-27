using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Cnc.Client.Handlers.Machine.Window;
using Microsoft.Extensions.Logging;

namespace Cnc.Client.Handlers.Machine
{
    public class MachineHandler : IPlatformHandler
    {

        public IPlatformHandler handler;

        public ILogger<MachineHandler> _logger;

        public OSPlatform OSPlatform { get; set; }
        public MachineHandler(ILogger<MachineHandler> logger)
        {
            _logger = logger;
         
            OSPlatform = GetOSPlatform();
            if (OSPlatform == OSPlatform.Windows)
            {
                handler = new Linux.LinuxHandler();
            }
            else
            {
                handler = new Linux.LinuxHandler();
            }

        }

        public string GetCurrentUsername()
        {
            string userName = System.Environment.UserName;

            return userName;
        }

        public OperatingSystem GetOsVersion()
        {
            return System.Environment.OSVersion;
        }

        public string GetMachineName()
        {
            return System.Environment.MachineName;
        }

        public Architecture GetArchitexture()
        {
            return RuntimeInformation.OSArchitecture;
        }

        public OSPlatform GetOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSPlatform.FreeBSD;
            }
            return OSPlatform.Windows;
        }

        public void CastCommand(string command)
        {
            handler.CastCommand(command);
        }

        public string CallCommand(string command)
        {
            return handler.CallCommand(command);
        }

        public string GetFirstMacAddress()
        {
            String firstMacAddress = NetworkInterface
                                        .GetAllNetworkInterfaces()
                                        .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                        .Select(nic => nic.GetPhysicalAddress().ToString())
                                        .FirstOrDefault();
            return firstMacAddress;
        }
    }
}