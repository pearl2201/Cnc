using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cnc.Client.Data;
using Cnc.Client.Handlers.Messages;
using Cnc.Client.Networking;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Messages.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnc.Client
{
    public class Application
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; }
        public Application(IServiceCollection serviceCollection)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();

            ConfigureServices(serviceCollection);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            var container = containerBuilder.Build();

            ServiceProvider = new AutofacServiceProvider(container);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(
                Configuration.GetConnectionString("Cnc.Client"));
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<NetClient>();
            services.AddSingleton<PacketHandler>();
            services.AddSingleton<InMemoryMessageBusSubscriptionsManager>();
            services.AddTransient<AskCommandResponseHandler>();
        }

        public async Task Run()
        {
            NetClient client = ServiceProvider.GetRequiredService<NetClient>();
            PacketHandler handler = ServiceProvider.GetRequiredService<PacketHandler>();
            handler.Subscribe<AskCommandResponse, AskCommandResponseHandler>();


            try
            {

                client.SendMessage(PacketId.ACK_COMMAND_REQUEST, new AskCommandRequest());



                while (true)
                {

                    var message = client.ReadMessage();
                    await handler.Handler(client, message);
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                client.Close();
            }
        }
    }
}