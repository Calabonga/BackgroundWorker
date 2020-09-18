using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Web.Mediator.Catalog;
using Calabonga.BackgroundWorker.Api.Web.ViewModels.CatalogViewModels;
using Calabonga.OperationResultsCore;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calabonga.BackgroundWorker.Api.Web.Controllers
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<PriceUpdateResult>>> UpdateRates(PricesUpdateViewModel model)
        {
            return Ok(await _mediator.Send(new RatesUpdateRequest(model), HttpContext.RequestAborted));
        }
    }
}
