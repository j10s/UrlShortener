using Microsoft.Extensions.Caching.Memory;

namespace UrlShortener.WebApi.Util;

public interface IMemoryCacheWrapper
{
    public T Set<T>(object key, T value, MemoryCacheEntryOptions options);
    
    public bool TryGetValue<T>(object key, out T value);
    
    public void Remove(object key);
}