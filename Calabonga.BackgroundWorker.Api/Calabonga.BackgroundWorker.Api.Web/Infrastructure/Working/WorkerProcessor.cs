using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Entities;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.EventLogging;
using Calabonga.BackgroundWorker.Api.Web.Mediator.Catalog;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Works processor system
    /// </summary>
    public class WorkerProcessor : IWorkerProcessor
    {
        public WorkerProcessor(
            ILogger<WorkerProcessor> logger,
            IWorkService workService)
        {
            _logger = logger;
            _workService = workService;
        }

        private readonly ILogger<WorkerProcessor> _logger;
        private readonly IWorkService _workService;

        public async Task ProcessActiveWorksAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var depended = _workService.GetWorksWithTypeTypeDependency().ToList();
            if (depended.Any())
            {
                await ProcessWorksAsync(depended, serviceProvider, cancellationToken);
                return;
            }

            var children = _workService.GetChildrenForCompletedWorks().ToList();
            if (children.Any())
            {
                await ProcessWorksAsync(children, serviceProvider, cancellationToken);
                return;
            }

            var rootWorks = _workService.GetRootWorks().ToList();
            if (!rootWorks.Any())
            {
                Events.WorksForWorkerNotFound(_logger);
                return;
            }

            await ProcessWorksAsync(rootWorks, serviceProvider, cancellationToken);
        }

        #region privates

        /// <summary>
        /// Work processing selector
        /// </summary>
        /// <param name="works"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ProcessWorksAsync(IReadOnlyCollection<Work> works, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            Events.TotalWorksFoundForWorker(_logger, works.Count);

            var workToStart = works.Where(x => x.IsTimeToStart).ToList();

            Events.TotalReadyToStartWorksFoundForWorker(_logger, workToStart.Count);
            foreach (var work in workToStart)
            {
                if (WorkerQueue.Instance.HasKey(work.Id))
                {
                    // We should skip work that is already in process
                    Events.WorkAlreadyQueued(_logger, work.Id.ToString());
                    continue;
                }

                // We should place work to the queue to  protect against second start for processing
                WorkerQueue.Instance.Add(work.Id, work);

                using var scope = serviceProvider.CreateScope();
                switch (work.WorkType)
                {
                    case WorkType.None:
                        Events.WrongWorkTypeDetectedForWorker(_logger);
                        break;

                    case WorkType.PriceCalculation:
                        await ProcessWorkPriceCalculationAsync(scope, work.Id, cancellationToken);
                        break;
                    
                    case WorkType.PriceGeneration:
                        await ProcessWorkPriceGenerationAsync(scope, work.Id, cancellationToken);
                        break;
                    
                    case WorkType.PriceSending:
                        await ProcessWorkPriceSendingAsync(scope, work.Id, cancellationToken);
                        break; 
                    
                    case WorkType.DownloadRates:
                        await ProcessWorkDownloadRatesAsync(scope, work.Id, cancellationToken);
                        break;
                        
                    default:
                        Events.WrongWorkTypeDetectedForWorker(_logger);
                        break;
                }
            }
        }

        private async Task ProcessWorkDownloadRatesAsync(IServiceScope scope, Guid workId, CancellationToken cancellationToken)
        {
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator.Send(new DownloadRatesRequest(workId), cancellationToken);
        }

        private async Task ProcessWorkPriceSendingAsync(IServiceScope scope, Guid workId, CancellationToken cancellationToken)
        {
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator.Send(new PriceSendRequest(workId), cancellationToken);
        }

        private async Task ProcessWorkPriceGenerationAsync(IServiceScope scope, Guid workId, CancellationToken cancellationToken)
        {
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator.Send(new PriceGenerateRequest(workId), cancellationToken);
        }

        private async Task ProcessWorkPriceCalculationAsync(IServiceScope scope, Guid workId, CancellationToken cancellationToken)
        {
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator.Send(new PriceCalculateRequest(workId), cancellationToken);
        }

        #endregion
    }
}