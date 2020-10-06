using System;
using System.Threading;
using System.Threading.Tasks;

using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.Microservices.BackgroundWorkers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.HostedServices
{
    public class RateUpdateHostedService : CrontabScheduledBackgroundHostedService
    {
        public RateUpdateHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<RateUpdateHostedService> logger) 
            : base(serviceScopeFactory, logger)
        {
        }

        protected override Task ProcessInScopeAsync(IServiceProvider serviceProvider, CancellationToken token)
        {
            var worker = serviceProvider.GetRequiredService<IWorker>();
            return worker.AppendWorkDownloadRatesAsync(token);
        }

        protected override string Schedule => "0 0 * * *";

        protected override string DisplayName => "At minute 0:00 every day";

        protected override bool IsExecuteOnServerRestart => false;
    }
}