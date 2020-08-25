using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Messages.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Cnc.Shared.Messages
{
    public class MessageWrapper
    {
        public PacketId PacketId { get; set; }
        public IMessage Content { get; set; }

        public static MessageWrapper Parse(string message)
        {
            MessageWrapper wr = new MessageWrapper();
            var temp = JObject.Parse(message);
            wr.PacketId = temp["PacketId"].ToObject<PacketId>();
            var content = temp["Content"].ToString();
            Console.WriteLine("Content: " + content);
            switch (wr.PacketId)
            {
                case PacketId.ACK_COMMAND_REQUEST:
                    wr.Content = JsonConvert.DeserializeObject<AskCommandRequest>(content);
                    break;
                case PacketId.ACK_COMMAND_RESPONSE:
                    wr.Content = JsonConvert.DeserializeObject<AskCommandResponse>(content);
                    break;
                case PacketId.ACK_DATETIME_RESPONSE:
                    wr.Content = JsonConvert.DeserializeObject<AskDatetimeResponse>(content);
                    break;
                default:
                    break;

            }
            if (wr.Content != null)
            {
                return wr;
            }
            throw new System.Exception($"[*] Parse Content Fail: {wr.PacketId} - {message}");

        }
    }
}