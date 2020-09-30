using System;
using System.Collections.Concurrent;

using Calabonga.BackgroundWorker.Api.Entities;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services;

using Microsoft.Extensions.Caching.Distributed;


namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Scheduler calculations helper. Prevents recursively recalculations
    /// </summary>
    public sealed class WorkerQueue
    {
        private readonly ConcurrentDictionary<Guid, Work> _queue = new ConcurrentDictionary<Guid, Work>();
        private IDistributedCacheService? _cache;

        #region Singleton

        private static readonly Lazy<WorkerQueue> Lazy = new Lazy<WorkerQueue>(() => new WorkerQueue());

        private WorkerQueue() { }

        /// <summary>
        /// Default instal for current singleton
        /// </summary>
        public static WorkerQueue Instance => Lazy.Value;

        public void SetCache(IDistributedCacheService cache)
        {
            _cache = cache;
        }

        #endregion

        /// <summary>
        /// Append key to the list of working calculations.
        /// We should place work to the queue to  protect against second start for processing
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Add(Guid key, Work value)
        {
            if (_cache == null)
            {
                _queue.TryAdd(key, value);
                return;
            }

            _cache.GetOrCreate(key.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }, () => value);
        }

        /// <summary>
        /// Returns key exists
        /// </summary>
        /// <param name="key"></param>
        public bool HasKey(Guid key)
        {
            if (_cache == null)
            {
                return _queue.ContainsKey(key);
            }
            return _cache.HasKey(key.ToString());
        }

        /// <summary>
        /// Removes key from list of working calculations
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Remove(Guid key)
        {
            if (_cache == null)
            {
                if (HasKey(key))
                {
                    _queue.TryRemove(key, out _);
                }
                return;
            }
            _cache.Remove(key.ToString());
        }
    }
}
