using System;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Entities;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Public interface for worker management
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Appends work for worker and save it to database <see cref="WorkType.PriceCalculation"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AppendWorkPriceCalculationAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Appends work for worker and save it to database <see cref="WorkType.PriceGeneration"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AppendWorkPriceGenerationAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Appends work for worker and save it to database <see cref="WorkType.PriceSending"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AppendWorkPriceSendingAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Append work for getting new rates from the Bank od Russia
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AppendWorkDownloadRatesAsync(CancellationToken token);
    }
}