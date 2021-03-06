﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Entities;
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
            var work = new Work(WorkType.PriceCalculation)
            {
                IsDeleteAfterSuccessfulCompleted = true,
                Name = WorkType.PriceCalculation.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            var repository = UnitOfWork.GetRepository<Work>();
            await repository.InsertAsync(work, cancellationToken);
            await UnitOfWork.SaveChangesAsync();
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                Events.CreateWorkForWorker(Logger, WorkType.PriceCalculation.ToString(), string.Empty, UnitOfWork.LastSaveChangesResult.Exception);
                return;
            }
            Events.CreateWorkForWorker(Logger, work.WorkType.ToString(), work.Id.ToString());
        }

        /// <summary>
        /// Appends work for worker and save it to database <see cref="WorkType.PriceGeneration"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AppendWorkPriceGenerationAsync(CancellationToken cancellationToken)
        {
            var work = new Work(WorkType.PriceGeneration)
            {
                IsDeleteAfterSuccessfulCompleted = true,
                Name = WorkType.PriceGeneration.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            var repository = UnitOfWork.GetRepository<Work>();
            await repository.InsertAsync(work, cancellationToken);
            await UnitOfWork.SaveChangesAsync();
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                Events.CreateWorkForWorker(Logger, WorkType.PriceGeneration.ToString(), string.Empty, UnitOfWork.LastSaveChangesResult.Exception);
                return;
            }
            Events.CreateWorkForWorker(Logger, work.WorkType.ToString(), work.Id.ToString());
        }

        /// <summary>
        /// Appends work for worker and save it to database <see cref="WorkType.PriceSending"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AppendWorkPriceSendingAsync(CancellationToken cancellationToken)
        {
            var work = new Work(WorkType.PriceSending)
            {
                IsDeleteAfterSuccessfulCompleted = true,
                Name = WorkType.PriceSending.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            var repository = UnitOfWork.GetRepository<Work>();
            await repository.InsertAsync(work, cancellationToken);
            await UnitOfWork.SaveChangesAsync();
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                Events.CreateWorkForWorker(Logger, WorkType.PriceSending.ToString(), string.Empty, UnitOfWork.LastSaveChangesResult.Exception);
                return;
            }
            Events.CreateWorkForWorker(Logger, work.WorkType.ToString(), work.Id.ToString());
        }

        /// <summary>
        /// Append work for getting new rates from the Bank od Russia
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AppendWorkDownloadRatesAsync(CancellationToken cancellationToken)
        {
            var work = new Work(WorkType.DownloadRates)
            {
                IsDeleteAfterSuccessfulCompleted = true,
                Name = WorkType.DownloadRates.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            var repository = UnitOfWork.GetRepository<Work>();
            await repository.InsertAsync(work, cancellationToken);
            await UnitOfWork.SaveChangesAsync();
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                Events.CreateWorkForWorker(Logger, WorkType.PriceSending.ToString(), string.Empty, UnitOfWork.LastSaveChangesResult.Exception);
                return;
            }
            Events.CreateWorkForWorker(Logger, work.WorkType.ToString(), work.Id.ToString());
        }
    }
}