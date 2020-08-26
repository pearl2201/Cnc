using System.Threading.Tasks;
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
        public SubmitClientInfoJob(NetClient client)
        {
            Log.Information("[*] SubmitClientInfoJob Initialize");
            _client = client;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _client.SendMessage(PacketId.POST_CLIENTINFO_REQUEST,new PostClientInfoRequest(){
                ClientInfo = new Shared.Entities.ClientInfo(){
                  OperationSystem = "Window"  
                }
            });
            return Task.CompletedTask;
        }
    }
}