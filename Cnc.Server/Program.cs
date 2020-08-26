using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cnc.Server;
using Cnc.Server.Handlers;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CncServer
{
    class Program
    {
         static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Application application = new Application(serviceCollection);
            application.Run();
        }

        static private void ConfigureServices(IServiceCollection serviceCollection)
        {
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .CreateLogger();
            serviceCollection.AddSingleton(Log.Logger);
        }
    }

}
