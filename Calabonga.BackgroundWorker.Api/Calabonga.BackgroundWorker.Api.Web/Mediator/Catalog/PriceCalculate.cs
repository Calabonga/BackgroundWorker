using System;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.UnitOfWork;
using MediatR;

namespace Calabonga.BackgroundWorker.Api.Web.Mediator.Catalog
{
    /// <summary>
    /// Request: calculate and update prices to customer
    /// </summary>
    public class PriceCalculateRequest : RequestBase<Unit>
    {
        public PriceCalculateRequest(Guid workId)
        {
            WorkId = workId;
        }

        public Guid WorkId { get; }
    }

    /// <summary>
    /// Handler: calculate and update prices to customer
    /// </summary>
    public class PriceCalculateRequestHandler : IRequestHandler<PriceCalculateRequest, Unit>
    {
        private readonly IWorkService _workService;
        private readonly IWorker _worker;
        private readonly IUnitOfWork _unitOfWork;

        public PriceCalculateRequestHandler(
            IWorkService workService,
        IWorker worker, 
            IUnitOfWork unitOfWork)
        {
            _workService = workService;
            _worker = worker;
            _unitOfWork = unitOfWork;
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Unit> Handle(PriceCalculateRequest request, CancellationToken cancellationToken)
        {
            // Some operations with entities
            // Update database with new rates
            // using UnitOfWork instance (_unitOfWork)

            // EXAMPLE:
            // ----------------------------------------------------------------------------
            // await _unitOfWork.SaveChangesAsync();
            // if (!_unitOfWork.LastSaveChangesResult.IsOk)
            // {
            //     await _workService.FinishWorkAsync(cancellationToken, request.WorkId, _unitOfWork.LastSaveChangesResult.Exception);
            //     return Unit.Value;
            
            //     // ---------------- OR --------------------------
            
            //     await _workService.RestartWorkAsync(request.WorkId, _unitOfWork.LastSaveChangesResult.Exception.Message);
            //     return Unit.Value;
            // }

            // Finishing the work
            await _workService.FinishWorkAsync(cancellationToken, request.WorkId);

            // and append new work for price generation
            await _worker.AppendWorkPriceGenerationAsync(cancellationToken);

            // return
            return Unit.Value;
        }
    }
}
