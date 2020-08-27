using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
namespace Quartz
{
    internal class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly QuartzHostedServiceOptions options;
        private IScheduler scheduler = null!;
     

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            QuartzHostedServiceOptions options
            )
        {
            this.schedulerFactory = schedulerFactory;
            this.options = options;
           
            Log.Information("QuartzHostedService Constructor");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            scheduler = await schedulerFactory.GetScheduler(cancellationToken);
            Log.Information("QuartzHostedService StartAsync");
            await scheduler.Start(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("QuartzHostedService StopAsync");
            return scheduler.Shutdown(options.WaitForJobsToComplete, cancellationToken);
        }
    }
}