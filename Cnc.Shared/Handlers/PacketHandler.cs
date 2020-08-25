using System.Collections.Generic;
using System.Threading.Tasks;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using System.Text.Json;
using Cnc.Shared.Net;
using System;

namespace Cnc.Shared.Handler
{
    public class PacketHandler
    {
        public Dictionary<PacketId, IMessageHandler> handlers;

        public PacketHandler()
        {
            handlers = new Dictionary<PacketId, IMessageHandler>();
        }

        public void AddHandler(PacketId packetId, IMessageHandler messageHandler)
        {
            handlers[packetId] = messageHandler;
        }

        public async Task Handler(INetConnection client, string message)
        {
            var messageWrapper = MessageWrapper.Parse(message);
            if (handlers.TryGetValue(messageWrapper.PacketId, out IMessageHandler handler))
            {
                try
                {
                    await handler.Handler(client, messageWrapper.Content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
        }
    }
}
