using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Infrastructure.Entities;
using Calabonga.BackgroundWorker.Api.Infrastructure.EventLogging;
using Calabonga.BackgroundWorker.Api.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Infrastructure.Working
{
    /// <summary>
    /// Works processor system
    /// </summary>
    public class WorkerProcessor : IWorkerProcessor
    {
        public WorkerProcessor(
            IMediator mediator,
            ILogger<WorkerProcessor> logger,
            IWorkService workService)
        {
            _mediator = mediator;
            _logger = logger;
            _workService = workService;
        }

        private readonly IMediator _mediator;
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

                    case WorkType.CleanUploadsFolder:
                        await ProcessWorkCleanUploadsFolderAsync(scope, work.Id, cancellationToken);
                        break;
                    
                    case WorkType.ProcessUploadsFolder:
                        await ProcessWorkProcessUploadsFolderAsync(scope, work.Id, cancellationToken);
                        break;


                    default:
                        Events.WrongWorkTypeDetectedForWorker(_logger);
                        break;
                }
            }
        }

        private async Task ProcessWorkProcessUploadsFolderAsync(IServiceScope scope, Guid workId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new MergeFilesRequest(),cancellationToken);
        }

        private async Task ProcessWorkCleanUploadsFolderAsync(IServiceScope scope, Guid workId, CancellationToken cancellationToken)
        {
            
        }

        #endregion
    }
}