using System;
using System.Net.Sockets;
using Cnc.Shared.Messages;
using Cnc.Shared.Net;
using Newtonsoft.Json;

namespace Cnc.Client.Networking
{

    public class NetClient : INetConnection
    {
        public TcpClient client;
        public NetworkStream stream;

        public Byte[] data;

        public NetClient(string server, int port)
        {
            client = new TcpClient(server, port);
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
                PacketId = PacketId.ACK_DATETIME_RESPONSE,
                Content = message
            };
            string messageWrapperText = JsonConvert.SerializeObject(messageWrapper);
            SendMessage(messageWrapperText);
        }
    }
}