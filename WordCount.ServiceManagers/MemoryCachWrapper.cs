using Microsoft.Extensions.Caching.Memory;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public class MemoryCachWrapper : IMemoryCacheWrapper
    {
        private readonly IMemoryCache cache;
        public MemoryCachWrapper(IMemoryCache cache)
        {
            this.cache = cache;
        }
        public bool TryGetValue<TItem>(object key, out TItem obj)
        {
            return this.cache.TryGetValue(key, out obj);
        }

        public TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
        {
            return this.cache.Set(key, value, options);
        }
    }
}