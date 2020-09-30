using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services
{
    public interface IDistributedCacheService
    {
        /// <summary>
        /// Returns already exist entry or first put it to the cache and then return entry
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="options"></param>
        /// <param name="entryFunc"></param>
        /// <returns></returns>
        TEntry GetOrCreate<TEntry>(string key, DistributedCacheEntryOptions options, Func<TEntry> entryFunc);

        /// <summary>
        /// Returns already existing entry or first put it to the cache and then return entry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="options"></param>
        /// <param name="entryFunc"></param>
        /// <typeparam name="TEntry"></typeparam>
        /// <returns></returns>
        Task<TEntry> GetOrCreateAsync<TEntry>(string key, DistributedCacheEntryOptions options, Func<Task<TEntry>> entryFunc);

        /// <summary>
        ///  Remove object from the Cache by key identifier
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        
        /// <summary>
        /// Remove object from the Cache by key identifier
        /// </summary>
        /// <param name="key"></param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Returns true when the key exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool HasKey(string key);

    }
}