using Cnc.Shared.Messages;

namespace Cnc.Shared.Net
{
    public interface INetConnection
    {
        void SendMessage(string message);
        void SendMessage(PacketId packetId, IMessage message);
        string ReadMessage();
        void Close();

        string GetIpAddress();
    }
}