using System.Threading.Tasks;
using Cnc.Shared.Messages;
using Cnc.Shared.Net;

namespace Cnc.Shared.Handlers
{
    public interface IMessageHandler
    {
        Task Handler<T>(INetConnection connection, T message);
    }
}
