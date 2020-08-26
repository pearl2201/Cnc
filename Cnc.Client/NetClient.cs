using System;
using System.Net.Sockets;
using Cnc.Shared.Messages;
using Cnc.Shared.Net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace Cnc.Client.Networking
{

    public class NetClient : INetConnection
    {
        public TcpClient client;
        public NetworkStream stream;

        public Byte[] data;

        public NetClient(IOptions<AppSettings> appSettingsOptions)
        {
            client = new TcpClient(appSettingsOptions.Value.Host, appSettingsOptions.Value.Port);
            stream = client.GetStream();
        }


        public void SendMessage(string message)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

        }

        public string ReadMessage()
        {
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Receive Message: " + responseData);
            return responseData;
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public void SendMessage(PacketId packetId, IMessage message)
        {
            MessageWrapper messageWrapper = new MessageWrapper()
            {
                PacketId = packetId,
                Content = message
            };
            string messageWrapperText = JsonConvert.SerializeObject(messageWrapper);
            Log.Information("Send message to server: {messageWrapperText}", messageWrapperText);
            SendMessage(messageWrapperText);
        }
    }
}