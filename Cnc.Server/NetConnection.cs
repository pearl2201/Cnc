using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using Cnc.Shared.Net;
using Newtonsoft.Json;
using Serilog;

public class NetConnection : INetConnection
{

    private readonly NetworkStream stream;
    private readonly TcpClient client;

    private PacketHandler handler;
    public NetConnection(TcpClient client, PacketHandler handler)
    {
        // Get a stream object for reading and writing
        this.client = client;
        stream = client.GetStream();
        this.handler = handler;
    }

    public async void Run()
    {
        try
        {
            // Loop to receive all the data sent by the client.
            while (true)
            {

                string message = ReadMessage();
                await handler.Handler(this, message);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"[*] Connection may be close, reason: {e}!");
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            Close();
        }

    }

    public void Close()
    {
        // Shutdown and end connection
        stream.Close();
        client.Close();
    }

    public void SendMessage(string message)
    {
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
        stream.Write(msg, 0, msg.Length);
    }

    public string ReadMessage()
    {
        Byte[] data = new Byte[256];

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

    public void SendMessage(PacketId packetId, IMessage message)
    {
        MessageWrapper messageWrapper = new MessageWrapper()
        {
            PacketId = packetId,
            Content = message
        };
        string messageWrapperText = JsonConvert.SerializeObject(messageWrapper);
        Log.Information("Send Message: {messageWrapperText}", messageWrapperText);
        SendMessage(messageWrapperText);
    }
}