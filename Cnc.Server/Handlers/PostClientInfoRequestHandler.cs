using System.Threading.Tasks;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Net;
using System;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Responses;
using System.Text.Json;
using Newtonsoft.Json;
using Cnc.Insfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Cnc.Shared.Entities;

namespace Cnc.Server.Handlers
{
    public class PostClientInfoRequestHandler : IMessageHandler<PostClientInfoRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        public PostClientInfoRequestHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(INetConnection connection, PostClientInfoRequest command)
        {
            var clientInfo = await _dbContext.ClientInfos.FirstOrDefaultAsync(x =>x.MacAddress == command.ClientInfo.MacAddress);
            if (clientInfo == null)
            {
                clientInfo = new ClientInfo()
                {
                    MacAddress = command.ClientInfo.MacAddress
                };
                _dbContext.ClientInfos.Add(clientInfo);
            }
            else
            {
                _dbContext.Entry(clientInfo).State = EntityState.Modified;
            }
            clientInfo.Ip = connection.GetIpAddress();
            clientInfo.Username = command.ClientInfo.Username;
            clientInfo.OperationSystem = command.ClientInfo.OperationSystem;
            clientInfo.ComputerName = command.ClientInfo.ComputerName;
            clientInfo.LastOnlineAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }
    }
}