using System.Collections.Generic;
using System.Threading.Tasks;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using System.Text.Json;
using Cnc.Shared.Net;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Autofac;
using Microsoft.Extensions.Logging;
using Cnc.Shared.Extensions;

namespace Cnc.Shared.Handlers
{
    public class PacketHandler
    {
        private readonly InMemoryMessageBusSubscriptionsManager _subsManager;
        private readonly string AUTOFAC_SCOPE_NAME = "kyx_event_bus";
        private readonly ILogger<PacketHandler> _logger;

        private readonly ILifetimeScope _autofac;

        public PacketHandler(InMemoryMessageBusSubscriptionsManager subManager, ILogger<PacketHandler> logger, ILifetimeScope autofac)
        {
            _subsManager = subManager;
            _logger = logger;
            _autofac = autofac;
        }


        public void Subscribe<T, TH>()
            where T : IMessage
            where TH : IMessageHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();


            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>();
        }

        public async Task Handler(INetConnection client, string messageWrapperContent)
        {
            Console.WriteLine("Receive: " + messageWrapperContent);
            var messageWrapper = MessageWrapper.Parse(messageWrapperContent);
            var content = messageWrapper.Content;

            var eventName = content.GetType().Name;
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicMessageHandler;
                            if (handler == null) continue;

                            await Task.Yield();
                            await handler.Handle(client,content);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = content;
                            var concreteType = typeof(IMessageHandler<>).MakeGenericType(eventType);

                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { client,integrationEvent });
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
