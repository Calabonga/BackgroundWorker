using System.Threading;
using System.Threading.Tasks;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Public interface for worker management
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Appends work for worker and save it to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AppendWorkPriceCalculationAsync(CancellationToken cancellationToken);
    }
}