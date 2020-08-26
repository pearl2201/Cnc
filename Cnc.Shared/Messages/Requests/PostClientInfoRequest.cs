using Cnc.Shared.Entities;
using Cnc.Shared.Messages;
namespace Cnc.Shared.Messages.Requests
{
    public class PostClientInfoRequest : IRequest
    {
        public ClientInfo ClientInfo {get;set;}
    }
}