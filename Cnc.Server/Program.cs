using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cnc.Server.Handlers;
using Cnc.Shared.Handler;
using Cnc.Shared.Messages;

namespace CncServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            PacketHandler handler = new PacketHandler();
            handler.AddHandler(PacketId.ACK_COMMAND_REQUEST, new AskCommandRequestHandler());
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("[*] Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    NetConnection connection = new NetConnection(client, handler);


                    var thread = new Thread(new ThreadStart(connection.Run));
                    thread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("[*] Hit enter to continue...");
            Console.Read();
        }
    }

}
