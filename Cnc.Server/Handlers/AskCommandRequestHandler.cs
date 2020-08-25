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
    public class AskCommandRequestHandler : IMessageHandler
    {
        public Task Handler<T>(INetConnection connection, T tempCommand)
        {
            AskCommandRequest command = tempCommand as AskCommandRequest;
            Console.WriteLine("Receiver AskCommandRequest");
            MessageWrapper messageWrapper = new MessageWrapper()
            {
                PacketId = PacketId.ACK_COMMAND_RESPONSE,
                Content = new AskCommandResponse()
                {
                    Command = Command.ASK_DATETIME
                }
            };
            string messageWrapperText = JsonConvert.SerializeObject(messageWrapper);
            connection.SendMessage(messageWrapperText);
            return Task.CompletedTask;
        }
    }
}