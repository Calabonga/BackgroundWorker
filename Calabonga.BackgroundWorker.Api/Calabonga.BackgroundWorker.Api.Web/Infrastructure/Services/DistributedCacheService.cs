using System;
using System.Text.Json;
using System.Threading.Tasks;
using Calabonga.Microservices.Core.Exceptions;
using Microsoft.Extensions.Caching.Distributed;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services
{
    /// <summary>
    /// DistributedCache system
    /// </summary>
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Returns already exist entry or first put it to the cache and then return entry
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="options"></param>
        /// <param name="entryFunc"></param>
        /// <returns></returns>
        public TEntry GetOrCreate<TEntry>(string key, DistributedCacheEntryOptions options, Func<TEntry> entryFunc)
        {
            if (entryFunc == null)
            {
                throw new InvalidOperationException();
            }

            if (HasKey(key))
            {
                var data = Get<TEntry>(key);
                if (data != null)
                {
                    _cache.Refresh(key);
                    return data;
                }

                return data;
            }

            var result = (entryFunc.DynamicInvoke() ?? throw new InvalidOperationException())!;
            Set(key, (TEntry)result, options);
            return (TEntry)result;
        }

        /// <summary>
        /// Returns already exist entry or first put it to the cache and then return entry
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="options"></param>
        /// <param name="entryFunc"></param>
        /// <returns></returns>
        public async Task<TEntry> GetOrCreateAsync<TEntry>(string key, DistributedCacheEntryOptions options, Func<Task<TEntry>> entryFunc)
        {
            if (entryFunc == null)
            {
                throw new InvalidOperationException();
            }

            if (await HasKeyAsync(key))
            {
                var data = await GetAsync<TEntry>(key);
                if (data != null)
                {
                    await _cache.RefreshAsync(key);
                    return data;
                }

                return data;
            }

            var result = GetFromMethod(entryFunc);
            if (result == null)
            {
                throw new InvalidOperationException("Cannot fetch object from Func<TEntry>");
            }

            await SetAsync(key, (TEntry)result, options);
            return (TEntry)result;
        }

        /// <summary>
        /// Remove object from the Cache by key identifier
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Remove object from the Cache by key identifier
        /// </summary>
        /// <param name="key"></param>
        public Task RemoveAsync(string key)
        {
            return _cache.RemoveAsync(key);
        }

        /// <summary>
        /// Sets entry cache for custom sliding expiration interval 
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        /// <param name="options"></param>
        private Task SetAsync<TEntry>(string key, TEntry cacheEntry, DistributedCacheEntryOptions options)
        {
            return _cache.SetStringAsync(key, JsonSerializer.Serialize(cacheEntry), options);
        }

        /// <summary>
        /// Extracts data from Task method
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="entryFunc"></param>
        /// <returns></returns>
        private object? GetFromMethod<TEntry>(Func<Task<TEntry>> entryFunc)
        {
            if (entryFunc.Method.ReturnType.IsSubclassOf(typeof(Task)))
            {
                if (entryFunc.Method.ReturnType.IsConstructedGenericType)
                {
                    dynamic? tmp = entryFunc.DynamicInvoke();
                    return tmp?.GetAwaiter().GetResult();
                }

                (entryFunc.DynamicInvoke() as Task)?.GetAwaiter().GetResult();
            }
            else
            {
                return entryFunc.DynamicInvoke();
            }

            return null;
        }

        /// <summary>
        /// GetTicket the entry from the cache
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <returns></returns>
        private TEntry Get<TEntry>(string key)
        {
            if (key == null)
            {
                throw new MicroserviceArgumentNullException(nameof(key));
            }

            if (HasKey(key))
            {
                var data = _cache.GetString(key);
                if (string.IsNullOrEmpty(data))
                {
                    throw new MicroserviceInvalidOperationException("Getting data from cache failed.");
                }
                return JsonSerializer.Deserialize<TEntry>(data);
            }

            return default!;
        }

        /// <summary>
        /// Sets entry cache for custom sliding expiration interval 
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        /// <param name="options"></param>
        private void Set<TEntry>(string key, TEntry cacheEntry, DistributedCacheEntryOptions options)
        {
            _cache.SetString(key, JsonSerializer.Serialize(cacheEntry), options);
        }

        public bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                var value = _cache.GetString(key);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private async Task<bool> HasKeyAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                var value = await _cache.GetStringAsync(key);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetTicket the entry from the cache
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <returns></returns>
        private async Task<TEntry> GetAsync<TEntry>(string key)
        {
            if (key == null)
            {
                throw new MicroserviceArgumentNullException(nameof(key));
            }

            var data = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(data))
            {
                throw new MicroserviceInvalidOperationException("Getting data from cache failed.");
            }

            return JsonSerializer.Deserialize<TEntry>(data);
        }
    }
}