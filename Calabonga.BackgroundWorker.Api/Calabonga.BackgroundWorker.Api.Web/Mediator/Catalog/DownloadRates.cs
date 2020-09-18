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
    /// Request: send prices to customer
    /// </summary>
    public class DownloadRatesRequest : RequestBase<Unit>
    {
        public DownloadRatesRequest(Guid workId)
        {
            WorkId = workId;
        }

        public Guid WorkId { get; }
    }

    /// <summary>
    /// Handler: send prices to customer
    /// </summary>
    public class DownloadRatesRequestHandler : IRequestHandler<DownloadRatesRequest, Unit>
    {
        private readonly IWorkService _workService;
        private readonly IWorker _worker;
        private readonly IUnitOfWork _unitOfWork;

        public DownloadRatesRequestHandler(
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
        public async Task<Unit> Handle(DownloadRatesRequest request, CancellationToken cancellationToken)
        {
            // Some operations with IEmailService and IProfileService
            // Update database with new rates
            // using UnitOfWork instance (_unitOfWork)

            // EXAMPLE:
            // ----------------------------------------------------------------------------
            // await _unitOfWork.SaveChangesAsync();
            // if (!_unitOfWork.LastSaveChangesResult.IsOk)
            // {
            //     await _workService.WorkFailedAsync(cancellationToken, request.WorkId, _unitOfWork.LastSaveChangesResult.Exception);
            //     return Unit.Value;
            
            //     // ---------------- OR --------------------------
            
            //     await _workService.RestartWorkAsync(request.WorkId, _unitOfWork.LastSaveChangesResult.Exception.Message);
            //     return Unit.Value;
            // }

            // Finishing the work
            await _workService.CompleteWorkAsync(cancellationToken, request.WorkId, "Updated from Bank of Russia");

            // append work for rates generation
            await _worker.AppendWorkPriceCalculationAsync(cancellationToken);

            // return
            return Unit.Value;
        }
    }
}
