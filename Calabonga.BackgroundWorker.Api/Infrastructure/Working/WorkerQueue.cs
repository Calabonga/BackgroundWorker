using System;
using System.Collections.Concurrent;
using Calabonga.BackgroundWorker.Api.Infrastructure.Entities;

namespace Calabonga.BackgroundWorker.Api.Infrastructure.Working
{
    /// <summary>
    /// Scheduler calculations helper. Prevents recursively recalculations
    /// </summary>
    public sealed class WorkerQueue
    {
        private readonly ConcurrentDictionary<Guid, Work> _queue = new ConcurrentDictionary<Guid, Work>();

        #region Singleton

        private static readonly Lazy<WorkerQueue> Lazy = new Lazy<WorkerQueue>(() => new WorkerQueue());

        private WorkerQueue() { }

        /// <summary>
        /// Default instal for current singleton
        /// </summary>
        public static WorkerQueue Instance => Lazy.Value;

        #endregion

        /// <summary>
        /// Append key to the list of working calculations.
        /// We should place work to the queue to  protect against second start for processing
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(Guid key, Work value)
        {
            return _queue.TryAdd(key, value);
        }

        /// <summary>
        /// Returns key exists
        /// </summary>
        /// <param name="key"></param>
        public bool HasKey(Guid key)
        {
            return _queue.ContainsKey(key);
        }

        /// <summary>
        /// Removes key from list of working calculations
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(Guid key)
        {
            return HasKey(key) && _queue.TryRemove(key, out _);
        }
    }
}
