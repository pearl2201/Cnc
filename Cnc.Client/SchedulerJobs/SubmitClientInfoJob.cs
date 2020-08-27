using System.Threading.Tasks;
using Cnc.Client.Handlers.Machine;
using Cnc.Client.Networking;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using Quartz;
using Serilog;

namespace Cnc.Client.SchedulerJobs
{
    public class SubmitClientInfoJob : IJob
    {

        private readonly NetClient _client;

        private readonly MachineHandler _machineHandler;

        public SubmitClientInfoJob(NetClient client, MachineHandler machineHandler)
        {
            _client = client;
            _machineHandler = machineHandler;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _client.SendMessage(PacketId.POST_CLIENTINFO_REQUEST, new PostClientInfoRequest()
            {
                ClientInfo = new Shared.Entities.ClientInfo()
                {
                    OperationSystem = _machineHandler.OSPlatform.ToString(),
                    ComputerName = _machineHandler.GetMachineName(),
                    Username = _machineHandler.GetCurrentUsername(),
                    MacAddress = _machineHandler.GetFirstMacAddress()
                }
            });
            return Task.CompletedTask;
        }
    }
}