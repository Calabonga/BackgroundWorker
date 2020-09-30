using System;
using System.Threading.Tasks;

using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services;
using Calabonga.BackgroundWorker.Api.Web.Mediator.Catalog;
using Calabonga.BackgroundWorker.Api.Web.ViewModels.CatalogViewModels;
using Calabonga.OperationResults;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Controllers
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IMediator _mediator;
        private readonly IDistributedCacheService _cacheService;

        public CatalogController(IMediator mediator, IDistributedCacheService cacheService, ILogger<CatalogController> logger)
        {
            _mediator = mediator;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public IActionResult GetCache1(string key)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            var value = _cacheService.GetOrCreate(key, options, GetPerson);
            return Ok(value);
        }

        [HttpGet("[action]")]
        public Task<IActionResult> GetCache2Async(string key)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            };

            var value = _cacheService.GetOrCreateAsync(key, options, GetPersonAsync);

            return Task.FromResult<IActionResult>(Ok(value));
        }

        private Person GetPerson()
        {
            _logger.LogInformation("Create instance");
            return new Person
            {
                Age = 32,
                FirstName = "Alex",
                LastName = "Bob"
            };
        }

        private Task<Person> GetPersonAsync()
        {
            _logger.LogInformation("Create instance");
            var result = new Person
            {
                Age = 32,
                FirstName = "Alex",
                LastName = "Bob"
            };

            return Task.FromResult(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<PriceUpdateResult>>> UpdateRates(PricesUpdateViewModel model)
        {
            return Ok(await _mediator.Send(new RatesUpdateRequest(model), HttpContext.RequestAborted));
        }
    }

    public class Person
    {
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public int Age { get; set; }

    }
}
