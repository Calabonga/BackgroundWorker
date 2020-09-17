using System;
using System.Threading;
using System.Threading.Tasks;

namespace Calabonga.BackgroundWorker.Api.Infrastructure.Working
{
    /// <summary>
    /// Works processor system
    /// </summary>
    public interface IWorkerProcessor
    {
        /// <summary>
        /// Worker process starter
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessActiveWorksAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}
