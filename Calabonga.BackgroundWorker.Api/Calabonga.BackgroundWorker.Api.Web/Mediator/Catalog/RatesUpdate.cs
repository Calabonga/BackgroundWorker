﻿using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working.Jobs;
using Calabonga.BackgroundWorker.Api.Web.ViewModels.CatalogViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace Calabonga.BackgroundWorker.Api.Web.Mediator.Catalog
{

    /// <summary>
    /// Request: rates updating
    /// </summary>
    public class RatesUpdateRequest : RequestBase<OperationResult<PriceUpdateResult>>
    {
        public RatesUpdateRequest(PricesUpdateViewModel rates)
        {
            Rates = rates;
        }

        public PricesUpdateViewModel Rates { get; }
    }

    /// <summary>
    /// Handler: rates updating
    /// </summary>
    public class RatesUpdateRequestHandler : OperationResultRequestHandlerBase<RatesUpdateRequest, PriceUpdateResult>
    {
        private readonly BackgroundJob _job;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IWorker _worker;
        private readonly IUnitOfWork _unitOfWork;

        public RatesUpdateRequestHandler(
            BackgroundJob job,
            IBackgroundTaskQueue backgroundTaskQueue,
            IWorker worker, 
            IUnitOfWork unitOfWork)
        {
            _job = job;
            _backgroundTaskQueue = backgroundTaskQueue;
            _worker = worker;
            _unitOfWork = unitOfWork;
        }

        public override async Task<OperationResult<PriceUpdateResult>> Handle(RatesUpdateRequest updateRequest, CancellationToken cancellationToken)
        {
            var operation = OperationResult.CreateResult<PriceUpdateResult>();

            // Some operations with entities
            // Update database with new rates
            // using UnitOfWork instance (_unitOfWork)

            // EXAMPLE:
            // ----------------------------------------------------------------------------
            // await _unitOfWork.SaveChangesAsync();`
            // if (!_unitOfWork.LastSaveChangesResult.IsOk)
            // {
            //     Handling DONE
            //     return Unit.Value;
            // }

            operation.Result = new PriceUpdateResult();
            operation.AddSuccess("Catalog successfully updated");

            // append work for rates generation
            // await _worker.AppendWorkPriceCalculationAsync(cancellationToken);
            _backgroundTaskQueue.QueueBackgroundWorkItem(token => _job);

            operation.AppendLog("Worker job list updated");

            // return operation result
            return operation;
        }
    }
}
