using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Cnc.Client.Networking;
using Cnc.Shared.Handler;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using System.Text.Json;
using Cnc.Client.Handlers.Messages;

namespace Cnc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Connect();
        }

        static async Task Connect()
        {
            NetClient client = null;
            try
            {

                client = new NetClient("localhost", 13000);
                PacketHandler handler = new PacketHandler();
                handler.AddHandler(PacketId.ACK_COMMAND_RESPONSE, new AskCommandResponseHandler());

                client.SendMessage(PacketId.ACK_COMMAND_REQUEST, new AskCommandRequest());

                while (true)
                {

                    var message = client.ReadMessage();
                    await handler.Handler(client, message);
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                client.Close();
            }


        }
    }
}
