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
using Microsoft.Extensions.Hosting;
using Quartz.Extensions.Hosting;
using Quartz;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace Cnc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
            .ConfigureServices((HostBuilderContext, serviceCollection) => {
                ConfigureServices(serviceCollection);
                StartUp application = new StartUp(serviceCollection);
                serviceCollection.AddHostedService<Application>();
               
                     // Quartz.Extensions.Hosting hosting
                
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                // registering services in the Autofac ContainerBuilder
            })
            .UseConsoleLifetime()
            .Build()
            .RunAsync()
            ;
            

            
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
