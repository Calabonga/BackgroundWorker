using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Infrastructure.EventLogging
{
    /// <summary>
    /// Microsoft ILogger system helper
    /// </summary>
    static partial class Events
    {
        #region SaveChangesFailed
        private static readonly Action<ILogger, Exception?> SaveChangesFailedExecute =
            LoggerMessage.Define(
                LogLevel.Critical,
                EventIdHelper.SaveChangesFailedId,
                "Saving to database error");

        public static void SaveChangesFailed(ILogger logger, Exception exception) => SaveChangesFailedExecute(logger, exception);
        #endregion
        
        #region TotalWorksFoundForWorker

        private static readonly Action<ILogger, int, Exception?> TotalWorksFoundForWorkerExecute =
            LoggerMessage.Define<int>(LogLevel.Information, EventIdHelper.TotalWorksFoundForWorkerId,
                "[SCHEDULER] found active works: {Count}");

        public static void TotalWorksFoundForWorker(ILogger logger, in int count)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                TotalWorksFoundForWorkerExecute(logger, count, null!);
            }
        }

        #endregion

        #region TotalReadyToStartWorksFoundForWorker

        private static readonly Action<ILogger, int, Exception?> TotalReadyToStartWorksFoundForWorkerExecute =
            LoggerMessage.Define<int>(LogLevel.Information, EventIdHelper.TotalReadyToStartWorksFoundForWorkerId,
                "[SCHEDULER] found active works ready to start: {Count}");

        public static void TotalReadyToStartWorksFoundForWorker(ILogger logger, in int count)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                TotalReadyToStartWorksFoundForWorkerExecute(logger, count, null!);
            }
        }

        #endregion

        #region WorksForWorkerNotFound

        private static readonly Action<ILogger, Exception?> WorksForWorkerNotFoundExecute =
            LoggerMessage.Define(LogLevel.Information, EventIdHelper.WorksForWorkerNotFoundId,
                "[SCHEDULER] does not found works to process");

        public static void WorksForWorkerNotFound(ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WorksForWorkerNotFoundExecute(logger, null!);
            }
        }

        #endregion

        #region WorkerStartWorkProcessing

        private static readonly Action<ILogger, Exception?> WorkerStartWorkProcessingExecute =
            LoggerMessage.Define(LogLevel.Information, EventIdHelper.WorkerNextProcessingId,
                "[SCHEDULER] processing next work");

        public static void WorkerStartWorkProcessing(ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WorkerStartWorkProcessingExecute(logger, null!);
            }
        }

        #endregion

        #region StoppingReplanning

        private static readonly Action<ILogger, Exception?> StoppingReplanningExecute =
            LoggerMessage.Define(LogLevel.Information, EventIdHelper.StoppingReplanningId,
                "[SCHEDULER] Stopping replanning");

        public static void StoppingReplanning(ILogger logger, Exception? exception = null)
        {
            StoppingReplanningExecute(logger, exception);
        }

        #endregion

        #region BackgroundWorkerStarted

        private static readonly Action<ILogger, Exception?> BackgroundWorkerStartedExecute =
            LoggerMessage.Define(LogLevel.Information, EventIdHelper.BackgroundWorkerStartedId,
                "[SCHEDULER] Service started");

        public static void BackgroundWorkerStarted(ILogger logger, Exception? exception = null)
        {
            BackgroundWorkerStartedExecute(logger, exception);
        }

        #endregion

        #region BackgroundWorkerStopped

        private static readonly Action<ILogger, Exception?> BackgroundWorkerStoppedExecute =
            LoggerMessage.Define(LogLevel.Information, EventIdHelper.BackgroundWorkerStoppedId,
                "[SCHEDULER] Service stopped");

        public static void BackgroundWorkerStopped(ILogger logger, Exception? exception = null)
        {
            BackgroundWorkerStoppedExecute(logger, exception);
        }

        #endregion

        #region BackgroundWorkerStopping

        private static readonly Action<ILogger, Exception?> BBackgroundWorkerStoppingExecute =
            LoggerMessage.Define(LogLevel.Information, EventIdHelper.BackgroundWorkerStoppingId,
                "[SCHEDULER] Service stopped");

        public static void BackgroundWorkerStopping(ILogger logger, Exception? exception = null)
        {
            BBackgroundWorkerStoppingExecute(logger, exception);
        }

        #endregion

        #region WorkByIdNotFound

        private static readonly Action<ILogger, string, Exception?> WorkByIdNotFoundExecute =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIdHelper.WorkByIdNotFoundId,
                "Work {WorkId} not found");

        public static void WorkByIdNotFound(ILogger logger, string workId, Exception? exception = null)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WorkByIdNotFoundExecute(logger, workId, exception);
            }
        }

        #endregion

        #region WorkerEndedWithProcessingResult

        private static readonly Action<ILogger, string, string, Exception?> WorkerEndedWithProcessingResultExecute =
            LoggerMessage.Define<string, string>(
                LogLevel.Information,
                EventIdHelper.WorkerEndedWithProcessingResultId,
                "Work {WorkId} is completed with result {ProcessingResult}");

        public static void WorkerEndedWithProcessingResult(ILogger logger, string workId, string processingResult,
            Exception? exception = null)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WorkerEndedWithProcessingResultExecute(logger, workId, processingResult, exception);
            }
        }

        #endregion

        #region WorkAlreadyQueued

        private static readonly Action<ILogger, string, Exception?> WorkAlreadyQueuedExecute =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIdHelper.WorkAlreadyQueuedId,
                "Work {WorkId} already queued and in progress");

        public static void WorkAlreadyQueued(ILogger logger, string workId)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WorkAlreadyQueuedExecute(logger, workId, null);
            }
        }

        #endregion

        #region CreateWorkForWorker

        private static readonly Action<ILogger, string, string, Exception?> CreateWorkForWorkerExecute =
            LoggerMessage.Define<string, string>(
                LogLevel.Information,
                EventIdHelper.CreateWorkForWorkerId,
                "New work with of type {WorkType} with id {Id} for Worker created");

        public static void CreateWorkForWorker(ILogger logger, string workType, string id, Exception? exception = null)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                CreateWorkForWorkerExecute(logger, workType, id, exception);
            }
        }

        #endregion

        #region WrongWorkTypeDetectedForWorker

        private static readonly Action<ILogger, Exception?> WrongWorkTypeDetectedForWorkerExecute =
            LoggerMessage.Define(
                LogLevel.Information,
                EventIdHelper.WrongWorkTypeDetectedForWorkerId,
                "wrongType of Worker detected!");

        public static void WrongWorkTypeDetectedForWorker(ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WrongWorkTypeDetectedForWorkerExecute(logger, null!);
            }
        }

        #endregion

    }
}