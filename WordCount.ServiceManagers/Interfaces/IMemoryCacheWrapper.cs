using Microsoft.Extensions.Caching.Memory;

namespace WordCount.ServiceManagers.Interfaces
{
    public interface IMemoryCacheWrapper
    {
        bool TryGetValue<TItem>(object key, out TItem obj);
        TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options);
    }
}