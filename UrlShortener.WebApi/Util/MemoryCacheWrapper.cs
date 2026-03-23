using Microsoft.Extensions.Caching.Memory;

namespace UrlShortener.WebApi.Util;

public class MemoryCacheWrapper(IMemoryCache memoryCache) : IMemoryCacheWrapper
{
    public T Set<T>(object key, T value, MemoryCacheEntryOptions options)
    {
        return memoryCache.Set(key, value, options);
    }

    public bool TryGetValue<T>(object key, out T value)
    {
        return memoryCache.TryGetValue(key, out value);
    }

    public void Remove(object key)
    {
        memoryCache.Remove(key);
    }
}