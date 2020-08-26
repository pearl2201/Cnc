using System.Threading.Tasks;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Net;
using System;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Responses;
using System.Text.Json;
using Newtonsoft.Json;

namespace Cnc.Server.Handlers
{
    public class AskCommandRequestHandler : IMessageHandler<AskCommandRequest>
    {
        public Task Handle(INetConnection connection, AskCommandRequest command)
        {
            connection.SendMessage(PacketId.ACK_COMMAND_RESPONSE, new AskCommandResponse()
            {
                Command = Command.ASK_DATETIME
            });
            return Task.CompletedTask;
        }
    }
}