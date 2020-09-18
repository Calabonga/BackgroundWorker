using System.Threading;
using System.Threading.Tasks;

using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.BackgroundWorker.Api.Web.ViewModels.CatalogViewModels;
using Calabonga.OperationResultsCore;

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

        public PriceUpdateRequestHandler(IWorker worker)
        {
            _worker = worker;
        }

        public override async Task<OperationResult<PriceUpdateResult>> Handle(PriceUpdateRequest request, CancellationToken cancellationToken)
        {
            var operation = OperationResult.CreateResult<PriceUpdateResult>();

            // Update database with new rates

            operation.Result = new PriceUpdateResult();
            operation.AddSuccess("Catalog successfully updated");

            // append work for rates generation
            await _worker.AppendWorkPriceCalculationAsync();
            operation.AppendLog("Worker job list updated");

            // return operation result
            return operation;
        }
    }
}
