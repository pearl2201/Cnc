using System.Threading.Tasks;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Net;
using System;
using Cnc.Shared.Messages.Responses;
using Cnc.Shared.Messages;
using System.Text.Json;
using Newtonsoft.Json;

namespace Cnc.Client.Handlers.Messages
{
    public class AskCommandResponseHandler : IMessageHandler<AskCommandResponse>
    {

        public Task Handle(INetConnection connection, AskCommandResponse command)
        {

            Console.WriteLine("[*] Receiver AskCommandResponse: " + command.Command);

            switch (command.Command)
            {
                case Command.ASK_DATETIME:
                    connection.SendMessage(PacketId.ACK_DATETIME_RESPONSE, new AskDatetimeResponse()
                    {
                        CurrentDatetime = DateTime.Now
                    });
                    break;
            }
            return Task.CompletedTask;
        }
    }
}