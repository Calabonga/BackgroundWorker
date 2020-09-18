using System.Threading;
using System.Threading.Tasks;

using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.BackgroundWorker.Api.Web.ViewModels.CatalogViewModels;
using Calabonga.OperationResultsCore;
using Calabonga.UnitOfWork;

namespace Calabonga.BackgroundWorker.Api.Web.Mediator.Catalog
{

    /// <summary>
    /// Request: rates updating
    /// </summary>
    public class PriceUpdateRequest : RequestBase<OperationResult<PriceUpdateResult>>
    {
        public PriceUpdateRequest(PricesUpdateViewModel rates)
        {
            Rates = rates;
        }

        public PricesUpdateViewModel Rates { get; }
    }

    /// <summary>
    /// Handler: rates updating
    /// </summary>
    public class PriceUpdateRequestHandler : OperationResultRequestHandlerBase<PriceUpdateRequest, PriceUpdateResult>
    {
        private readonly IWorker _worker;
        private readonly IUnitOfWork _unitOfWork;

        public PriceUpdateRequestHandler(IWorker worker, IUnitOfWork unitOfWork)
        {
            _worker = worker;
            _unitOfWork = unitOfWork;
        }

        public override async Task<OperationResult<PriceUpdateResult>> Handle(PriceUpdateRequest request, CancellationToken cancellationToken)
        {
            var operation = OperationResult.CreateResult<PriceUpdateResult>();

            // Some operations with entities
            // Update database with new rates
            // using UnitOfWork instance (_unitOfWork)

            // EXAMPLE:
            // ----------------------------------------------------------------------------
            // await _unitOfWork.SaveChangesAsync();
            // if (!_unitOfWork.LastSaveChangesResult.IsOk)
            // {
            //     Handling DONE
            //     return Unit.Value;
            // }

            operation.Result = new PriceUpdateResult();
            operation.AddSuccess("Catalog successfully updated");

            // append work for rates generation
            await _worker.AppendWorkPriceCalculationAsync(cancellationToken);
            operation.AppendLog("Worker job list updated");

            // return operation result
            return operation;
        }
    }
}
