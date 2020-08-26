using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cnc.Client.Data;
using Cnc.Client.Handlers.Messages;
using Cnc.Client.Networking;
using Cnc.Client.SchedulerJobs;
using Cnc.Shared.Handlers;
using Cnc.Shared.Messages;
using Cnc.Shared.Messages.Requests;
using Cnc.Shared.Messages.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Extensions.Hosting;
using Serilog;

namespace Cnc.Client
{
    public class StartUp
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; }
        public StartUp(IServiceCollection serviceCollection)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();

            ConfigureServices(serviceCollection);
            var containerBuilder = new ContainerBuilder();
           /*  containerBuilder.Populate(serviceCollection);

            var container = containerBuilder.Build();

            ServiceProvider = new AutofacServiceProvider(container); */
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(
                Configuration.GetConnectionString("Cnc.Client"));
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<NetClient>();
            services.AddSingleton<PacketHandler>();
            services.AddSingleton<InMemoryMessageBusSubscriptionsManager>();
            services.AddTransient<AskCommandResponseHandler>();
            services.AddQuartz(q => {
                // handy when part of cluster or you want to otherwise identify multiple schedulers
                q.SchedulerId = "Scheduler-Core";
                
                // we take this from appsettings.json, just show it's possible
                // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";
                
                // we could leave DI configuration intact and then jobs need to have public no-arg constructor
                // the MS DI is expected to produce transient job instances 
                q.UseMicrosoftDependencyInjectionJobFactory(options =>
                {
                    // if we don't have the job in DI, allow fallback to configure via default constructor
                    options.AllowDefaultConstructor = true;
                });

                // or 
                // q.UseMicrosoftDependencyInjectionScopedJobFactory();
                
                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });

                var jobKey = new JobKey("submitclientinfo", "background");
                Log.Information("Add SubmitClientInfoJob");
                q.AddJob<SubmitClientInfoJob>(j => j
                    .StoreDurably()
                    .WithIdentity(jobKey)
                    .WithDescription("my awesome job")
                );
                q.AddTrigger(t => t
                    .WithIdentity("Simple Trigger")
                    .ForJob(jobKey)
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(1)).RepeatForever())
                    .WithDescription("Submit trigger every 30 minutes")
                );

               
            });

            services.AddHttpClient();

            
             // Quartz.Extensions.Hosting hosting
            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }

        
    }
}