using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cnc.Client.Data;
using Cnc.Client.Handlers.Messages;
using Cnc.Client.Networking;
using Cnc.Client.SchedulerJobs;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Messages.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;

namespace Cnc.Client
{
    public class Application : BackgroundService
    {
         private IServiceScopeFactory _serviceScopeFactory;
        public Application(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                NetClient client = serviceProvider.GetRequiredService<NetClient>();
                PacketHandler handler = serviceProvider.GetRequiredService<PacketHandler>();
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
}