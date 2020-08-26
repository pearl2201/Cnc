using System.Threading.Tasks;
using Cnc.Shared.Messages;
using Cnc.Shared.Net;

namespace Cnc.Shared.Handlers
{
    public interface IDynamicMessageHandler
    {
        Task Handle(INetConnection connection, dynamic eventData);
    }
}
