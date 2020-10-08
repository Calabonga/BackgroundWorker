using System;
using System.Threading;
using System.Threading.Tasks;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working.Jobs
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, IBackgroundJob> workItem);

        Task<Func<CancellationToken, IBackgroundJob>?> DequeueAsync(CancellationToken cancellationToken);
    }
}