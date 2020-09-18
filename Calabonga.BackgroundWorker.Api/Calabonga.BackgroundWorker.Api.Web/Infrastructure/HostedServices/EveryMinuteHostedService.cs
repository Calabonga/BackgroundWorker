using System;
using System.Threading;
using System.Threading.Tasks;

using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.Microservices.BackgroundWorkers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.HostedServices
{
    public class EveryMinuteHostedService : CrontabScheduledBackgroundHostedService
    {
        public EveryMinuteHostedService(IServiceScopeFactory serviceScopeFactory, ILogger logger) : base(serviceScopeFactory, logger)
        {
        }

        protected override Task ProcessInScopeAsync(IServiceProvider serviceProvider, CancellationToken token)
        {
            var workProcessor = serviceProvider.GetRequiredService<IWorkerProcessor>();
            return workProcessor.ProcessActiveWorksAsync(serviceProvider, token);
        }

        protected override string Schedule => "* * * * *";

        protected override string DisplayName => "EveryMinutes hosted service";

        protected override bool IsExecuteOnServerRestart => false;
    }
}