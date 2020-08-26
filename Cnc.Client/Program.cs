using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Cnc.Client.Networking;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using System.Text.Json;
using Cnc.Client.Handlers.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Cnc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Application application = new Application(serviceCollection);
            await application.Run();
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
