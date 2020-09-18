using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Entities;
using Calabonga.BackgroundWorker.Api.Web.Extensions;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.EventLogging;
using Calabonga.UnitOfWork;

using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Worker uses IUnitOfWork for access database (Nimble Framework)
    /// https://youtu.be/WbSwp1Aa7hM
    /// https://youtu.be/XUFphtpKZtI
    /// https://youtu.be/aIYZ92CEJN8
    /// </summary>
    public class Worker : WorkerBase, IWorker
    {
        public Worker(IUnitOfWork unitOfWork, ILogger<Worker> logger)
            : base(unitOfWork, logger)
        {
        }

        /// <summary>
        /// Appends work for worker and save it to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AppendWorkPriceCalculationAsync(CancellationToken cancellationToken)
        {
            var work = new Work(WorkType.PriceCalculation);
            var repository = UnitOfWork.GetRepository<Work>();
            await repository.InsertAsync(work, cancellationToken);
            await UnitOfWork.SaveChangesAsync();
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                Events.CreateWorkForWorker(Logger, WorkType.PriceCalculation.ToString(), null, UnitOfWork.LastSaveChangesResult.Exception);
                var message = UnitOfWork.LastSaveChangesResult.Exception == null
                    ? $"Cannot create work ({nameof(AppendWorkPriceCalculationAsync)})"
                    : UnitOfWork.LastSaveChangesResult.Exception.Message;

                return;
            }
            Events.CreateWorkForWorker(Logger, work.WorkType.ToString(), work.Id.ToString());
        }
    }
}