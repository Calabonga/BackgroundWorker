﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Entities;
using Calabonga.BackgroundWorker.Api.Web.Extensions;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.EventLogging;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Base method for Work management implementations
    /// </summary>
    public abstract class WorkerBase : IWorkService
    {
        protected WorkerBase(IUnitOfWork unitOfWork, ILogger<Worker> logger)
        {
            Logger = logger;
            UnitOfWork = unitOfWork;
        }

        #region Properties

        /// <summary>
        /// Logger instance
        /// </summary>
        protected ILogger<Worker> Logger { get; }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; }

        #endregion

        /// <summary>
        /// Returns uncompleted works for processing
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Work>GetRootWorks()
        {
            return UnitOfWork.GetRepository<Work>()
                .GetAll(true)
                .ToList()
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.CompletedAt == null && x.CanceledAt == null && x.ParentId == null && string.IsNullOrEmpty(x.Dependency));
        }

        /// <summary>
        /// Returns works that depend on other type works
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Work> GetWorksWithTypeTypeDependency()
        {
            var worksWithDependencies = UnitOfWork.GetRepository<Work>()
                .GetAll(true)
                .ToList()
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.CompletedAt == null && x.CanceledAt == null && !string.IsNullOrEmpty(x.Dependency))
                .ToList();

            if (!worksWithDependencies.Any())
            {
                return worksWithDependencies;
            }

            var result = new List<Work>();
            foreach (var work in worksWithDependencies)
            {
                var dependency = work.Dependency;
                var any = UnitOfWork.GetRepository<Work>()
                    .GetAll(true)
                    .Where(dependency)
                    .ToList()
                    .Any();

                if (!any)
                {
                    result.Add(work);
                }
            }

            return result;
        }

        /// <summary>
        /// Return uncompleted works witch is children for completed parents
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Work> GetChildrenForCompletedWorks()
        {
            return UnitOfWork.GetRepository<Work>()
                .GetAll(true)
                .Include(x => x.Parent)
                .ToList()
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.CompletedAt == null && x.CanceledAt == null && x.ParentId != null && x.Parent!.CompletedAt != null && string.IsNullOrEmpty(x.Dependency))
                .ToList();
        }

        /// <summary>
        /// Returns Work by identifier
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public Task<Work?> GetWorkByIdAsync(Guid workId)
        {
            return UnitOfWork.GetRepository<Work>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == workId,
                    include: i => i
                        .Include(x => x.Children)
                        .Include(x => x.Parent))!;
        }

        /// <summary>
        /// Returns UserName for work (creator name)
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public string? GetUserNameFromWork(Guid workId)
        {
            var work = UnitOfWork.GetRepository<Work>().GetFirstOrDefault(predicate: x => x.Id == workId);
            if (work != null)
            {
                return work.GetParamByName<string>(ParamsProperty.ParameterUserName);
            }

            Events.WorkByIdNotFound(Logger, workId.ToString(), new MicroserviceNotFoundException($"{nameof(GetUserNameFromWork)}: Work {workId} not found"));
            return null;
        }

        /// <summary>
        /// Restart work 
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="reason"></param>
        /// <param name="restartAfterMinutes"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public async Task RestartWorkAsync(Guid workId, string reason, int restartAfterMinutes = 15, int retryCount = 1)
        {
            var work = await UnitOfWork.GetRepository<Work>().FindAsync(workId);
            if (work == null)
            {
                Events.WorkByIdNotFound(Logger, workId.ToString(), new MicroserviceNotFoundException($"{nameof(RestartWorkAsync)}: Work {workId} not found"));
                return;
            }

            work.SetDelay(restartAfterMinutes);
            work.CancelAfterProcessingCount += retryCount;
            work.ProcessingResult = reason;
            work.MarkAsProcessed();
            UnitOfWork.GetRepository<Work>().Update(work);
            await UnitOfWork.SaveChangesAsync();
            WorkerQueue.Instance.Remove(work.Id);
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                var exception = UnitOfWork.LastSaveChangesResult?.Exception ?? new MicroserviceInvalidCastException("UnitOfWork.LastSaveChangesResult failed");
                Events.SaveChangesFailed(Logger, exception);
                throw exception;
            }
        }

        /// <summary>
        /// Complete work successfully
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="workId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task CompleteWorkAsync(CancellationToken cancellationToken, Guid workId, string message)
        {
            await UpdateWorkAsync(cancellationToken, workId, null);
        }

        /// <summary>
        /// Finish the Work according to Work settings
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="workId"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task WorkFailedAsync(CancellationToken cancellationToken, Guid workId, Exception? exception)
        {
            await UpdateWorkAsync(cancellationToken, workId, exception);
        }

        /// <summary>
        /// Finish the Work according to Work settings
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="workId"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task UpdateWorkAsync(CancellationToken cancellationToken, Guid workId, Exception? exception)
        {
            var work = await UnitOfWork.GetRepository<Work>().GetAll(true).SingleOrDefaultAsync(x=>x.Id == workId, cancellationToken);
            if (work == null)
            {
                Events.WorkByIdNotFound(Logger, workId.ToString(), new MicroserviceNotFoundException($"Work {workId} not found"));
                return;
            }

            if (exception != null)
            {
                WorkerQueue.Instance.Remove(work.Id);
                work.CanceledAt = DateTime.UtcNow;
                work.ProcessingResult = JsonSerializer.Serialize(exception, new JsonSerializerOptions { IgnoreNullValues = true });
                work.MarkAsProcessed();
                var repository = UnitOfWork.GetRepository<Work>();
                repository.Update(work);
                await UnitOfWork.SaveChangesAsync();
                if (UnitOfWork.LastSaveChangesResult.IsOk)
                {
                    return;
                }

                var exceptionSave = UnitOfWork.LastSaveChangesResult?.Exception ?? new MicroserviceInvalidCastException("UnitOfWork.LastSaveChangesResult failed");
                Events.SaveChangesFailed(Logger, exceptionSave);
                throw exceptionSave;
            }

            if (work.IsDeleteAfterSuccessfulCompleted && !work.HasChildren())
            {
                work.MarkAsProcessed();
                UnitOfWork.GetRepository<Work>().Delete(work);
            }
            else
            {
                work.CompletedAt = DateTime.UtcNow;
                work.MarkAsProcessed(true);
                UnitOfWork.GetRepository<Work>().Update(work);
            }

            await UnitOfWork.SaveChangesAsync();
            WorkerQueue.Instance.Remove(work.Id);
            Events.WorkCompleted(Logger, workId.ToString());
            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                var exceptionSave = UnitOfWork.LastSaveChangesResult?.Exception ?? new MicroserviceInvalidCastException("UnitOfWork.LastSaveChangesResult failed");
                Events.SaveChangesFailed(Logger, exceptionSave);
                throw exceptionSave;
            }
        }
    }
}