using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Entities;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Base Work operations service for internal use only for current project
    /// </summary>
    public interface IWorkService
    {
        /// <summary>
        /// Returns works that depend on other type works
        /// </summary>
        /// <returns></returns>
        IEnumerable<Work> GetWorksWithTypeTypeDependency();

        /// <summary>
        /// Return uncompleted works witch is children for completed parents
        /// </summary>
        /// <returns></returns>
        IEnumerable<Work> GetChildrenForCompletedWorks();

        /// <summary>
        /// Returns uncompleted works for processing
        /// </summary>
        /// <returns></returns>
        IEnumerable<Work> GetRootWorks();

        /// <summary>
        /// Returns Work by identifier
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        Task<Work?> GetWorkByIdAsync(Guid workId);

        /// <summary>
        /// Finish the Work according to Work settings
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        Task FinishWorkAsync(Guid workId, CancellationToken cancellationToken, Exception? exception = null);

        /// <summary>
        /// Restart work 
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="reason"></param>
        /// <param name="restartAfterMinutes"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        Task RestartWorkAsync(Guid workId, string reason, int restartAfterMinutes = 15, int retryCount = 1);

        /// <summary>
        /// Updates ProcessingResult property for Work by Id. Update processing time. Does not complete work execution
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FinishWorkAsync(Guid workId, string exceptionMessage, CancellationToken cancellationToken);

        /// <summary>
        /// Returns UserName for work (creator name)
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        string? GetUserNameFromWork(Guid workId);
    }
}