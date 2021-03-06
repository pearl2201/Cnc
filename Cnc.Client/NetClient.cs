using System;
using System.IO;
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
        public bool IsConnected{get;set;}
        public Byte[] data;

        public NetClient(IOptions<AppSettings> appSettingsOptions)
        {
            client = new TcpClient(appSettingsOptions.Value.Host, appSettingsOptions.Value.Port);
            stream = client.GetStream();
            IsConnected = true;
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
            if (bytes != 0)
            {
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                return responseData;
            }
            throw new IOException();
        }

        public void Close()
        {
            IsConnected = false;
            stream.Close();
            client.Close();
        }

        public void SendMessage(PacketId packetId, IMessage message)
        {
            if (IsConnected)
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
            else
            {
                Log.Information("NetClient don't connected, so send message fail");
            }
        }

        public string GetIpAddress()
        {
            return client.Client.RemoteEndPoint.ToString();
        }
    }
}