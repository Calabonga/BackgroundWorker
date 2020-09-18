using System;
using System.Collections.Generic;

namespace Calabonga.BackgroundWorker.Api.Entities
{
    /// <summary>
    /// Work for scheduler
    /// With dependency between work task 
    /// </summary>
    public class Work : ParamsProperty
    {
        private Work() { }

        /// <summary>
        /// Creates work instance with parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="retryCount"></param>
        /// <param name="minutesWaitBeforeSendUpdates"></param>
        public Work(WorkType type, int retryCount = 1, int minutesWaitBeforeSendUpdates = 0, string? name = null)
        {
            var now = DateTime.UtcNow;
            StartAfterMinutes = minutesWaitBeforeSendUpdates;
            WorkType = type;
            CancelAfterProcessingCount = retryCount;
            Name = name ?? type.ToString();
            IsDeleteAfterSuccessfulCompleted = true;
            CreatedAt = now;
            ProcessedAt = now;
        }

        #region properties

        /// <summary>
        /// Special tag for find dependencies from works. Use format for Expression tree
        /// </summary>
        public string? Dependency { get; set; }

        /// <summary>
        /// Name of the work or process name which had starts this work
        /// </summary>
        public string Name { get; } = null!;

        /// <summary>
        ///  Calculation for delay interval is based on this parameters
        /// </summary>
        public int StartAfterMinutes { get; private set; }

        /// <summary>
        /// Type of the work
        /// </summary>
        public WorkType WorkType { get; set; }

        /// <summary>
        /// Parent identifier for work
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Parent work
        /// </summary>
        public virtual Work? Parent { get; set; }

        /// <summary>
        /// Children for current work
        /// </summary>
        public virtual ICollection<Work>? Children { get; set; }

        /// <summary>
        /// Work created at
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Last processed
        /// </summary>
        public DateTime ProcessedAt { get; private set; }

        /// <summary>
        /// After work completed this should be filled
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// After work completed this should be filled
        /// </summary>
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// Indicates that the item should be deleted after successful completion.
        /// By default, all works created as works that need to delete after it has been completed
        /// </summary>
        public bool IsDeleteAfterSuccessfulCompleted { get; }

        /// <summary>
        /// Stores result of execution. Exceptions or other messages
        /// </summary>
        public string? ProcessingResult { get; set; }

        /// <summary>
        /// Total count times
        /// </summary>
        public int ProcessedCount { get; set; }

        /// <summary>
        /// Cancel processing after
        /// </summary>
        public int CancelAfterProcessingCount { get; set; }

        #endregion

        /// <summary>
        /// We should know how many times this works processed
        /// </summary>
        /// <param name="isComplete">indicate that the work should be marked as completed</param>
        public void MarkAsProcessed(bool isComplete = false)
        {
            var timestamp = DateTime.UtcNow;
            if (isComplete)
            {
                CompletedAt = timestamp;
            }

            ProcessedAt = timestamp;
            ProcessedCount++;
            if (ProcessedCount > CancelAfterProcessingCount)
            {
                CanceledAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// update property StartAfterMinutes
        /// </summary>
        /// <param name="minutesToDelay"></param>
        public void SetDelay(int minutesToDelay)
        {
            StartAfterMinutes = minutesToDelay;
        }

        /// <summary>
        /// Is minutes before start done
        /// </summary>
        public bool IsTimeToStart => ProcessedAt.AddMinutes(StartAfterMinutes) <= DateTime.UtcNow;
    }
}
