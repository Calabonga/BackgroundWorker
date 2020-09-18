using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.EventLogging
{
    /// <summary>
    /// Helper for Eventing
    /// </summary>
    static class EventIdHelper
    {
        public static readonly EventId WorkerNextProcessingId                 = new EventId(1000, "WorkerNextProcessing");
        public static readonly EventId WorksForWorkerNotFoundId               = new EventId(1001, "WorksForWorkerNotFound");
        public static readonly EventId TotalWorksFoundForWorkerId             = new EventId(1002, "TotalWorksFoundForWorker");
        public static readonly EventId WrongWorkTypeDetectedForWorkerId       = new EventId(1003, "WrongWorkTypeDetectedForWorker");
        public static readonly EventId CreateWorkForWorkerId                  = new EventId(1004, "CreateWorkForWorker");
        public static readonly EventId WorkByIdNotFoundId                     = new EventId(1005, "WorkByIdNotFound");
        public static readonly EventId WorkerEndedWithProcessingResultId      = new EventId(1006, "WorkerEndedWithProcessingResult");
        public static readonly EventId WorkAlreadyQueuedId                    = new EventId(1007, "WorkAlreadyQueued");
        public static readonly EventId BackgroundWorkerStartedId              = new EventId(1009, "BackgroundWorkerStarted");
        public static readonly EventId BackgroundWorkerStoppedId              = new EventId(1010, "BackgroundWorkerStopped");
        public static readonly EventId BackgroundWorkerStoppingId             = new EventId(1011, "BackgroundWorkerStopped");
        public static readonly EventId TotalReadyToStartWorksFoundForWorkerId = new EventId(1012, "TotalReadyToStartWorksFoundForWorker");
        public static readonly EventId SaveChangesFailedId                    = new EventId(1013, "SaveChangesFailed");
        public static readonly EventId NothingToMergeId                       = new EventId(1014, "NothingToMerge");
        public static readonly EventId WorkCompletedId                        = new EventId(1015, "WorkCompleted");
    }
}