using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cnc.Insfrastructure.Data;
using Cnc.Server.Handlers;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Messages.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
namespace Cnc.Server
{
    public class Application
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; }

        
        public Application(IServiceCollection serviceCollection)
        {
            Log.Information("[*] Build Configuration");
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
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
            Log.Information("[*] ConfigureServices");
            services.AddLogging();

            services.AddOptions();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(
                Configuration.GetConnectionString("Cnc"));
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<PacketHandler>();
            services.AddSingleton<InMemoryMessageBusSubscriptionsManager>();
            services.AddSingleton<NetworkingContext>();
            services.AddTransient<AskCommandRequestHandler>();

        }

        public void Run()
        {
            Log.Information("[*] Application Run");
            TcpListener server = null;
            PacketHandler handler = ServiceProvider.GetRequiredService<PacketHandler>();
            AppSettings appSettings = ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
            handler.Subscribe<AskCommandRequest, AskCommandRequestHandler>();
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = appSettings.Port;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                Log.Information("[*] Application Start At Port: " + port);
                // Enter the listening loop.
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    NetConnection connection = new NetConnection(client, handler);


                    var thread = new Thread(new ThreadStart(connection.Run));
                    thread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("[*] Hit enter to continue...");
            Console.Read();
        }
    }
}