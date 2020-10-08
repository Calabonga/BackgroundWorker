using System;
using System.Threading;
using System.Threading.Tasks;

using Calabonga.BackgroundWorker.Api.Entities;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.EventLogging;
using Calabonga.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working.Jobs
{
    public interface IBackgroundJob
    {
        Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken token);
    }

    public class BackgroundJob : IBackgroundJob
    {
        private readonly ILogger<BackgroundJob> _logger;

        public BackgroundJob(ILogger<BackgroundJob> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken token)
        {
            using var scope = serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var work = new Work(WorkType.PriceCalculation)
            {
                IsDeleteAfterSuccessfulCompleted = true,
                Name = WorkType.PriceCalculation.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            var repository = unitOfWork.GetRepository<Work>();
            await repository.InsertAsync(work, token);
            await unitOfWork.SaveChangesAsync();
            if (!unitOfWork.LastSaveChangesResult.IsOk)
            {
                Events.CreateWorkForWorker(_logger, WorkType.PriceCalculation.ToString(), string.Empty, unitOfWork.LastSaveChangesResult.Exception);
                return;
            }
            Events.CreateWorkForWorker(_logger, work.WorkType.ToString(), work.Id.ToString());
        }
    }
}
