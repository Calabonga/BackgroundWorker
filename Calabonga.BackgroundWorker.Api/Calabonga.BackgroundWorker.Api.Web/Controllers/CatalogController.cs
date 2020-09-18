using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Calabonga.AspNetCore.Controllers.Base;
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
        public async Task<ActionResult<OperationResult<PriceUpdateResult>>> SavePriceById(PricesUpdateViewModel model)
        {
            return Ok(await _mediator.Send(new PriceUpdateRequest(model), HttpContext.RequestAborted));
        }
    }
}
