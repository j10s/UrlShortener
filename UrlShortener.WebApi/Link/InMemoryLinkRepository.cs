using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using UrlShortener.WebApi.Util;

namespace UrlShortener.WebApi.Link;

public class InMemoryLinkRepository(IMemoryCacheWrapper memoryCache, ILinkRepository innerLinkRepository)
    : ILinkRepository
{
    private static readonly MemoryCacheEntryOptions MemoryCacheEntryOptions = new() { Size = 1 };

    public async ValueTask<DataAccess.Link> CreateAsync(DataAccess.Link link)
    {
        return await innerLinkRepository.CreateAsync(link);
    }

    public async ValueTask<DataAccess.Link> GetByIdAsync(long linkId)
    {
        if (memoryCache.TryGetValue(linkId, out DataAccess.Link link)) return link;
        link = await innerLinkRepository.GetByIdAsync(linkId);
        if (link != null) memoryCache.Set(linkId, link, MemoryCacheEntryOptions);

        return link;
    }

    public async ValueTask<DataAccess.Link> UpdateAsync(DataAccess.Link link)
    {
        var updatedLink = await innerLinkRepository.UpdateAsync(link);
        if (updatedLink != null && memoryCache.TryGetValue<DataAccess.Link>(link.Id, out _))
            memoryCache.Set(link.Id, updatedLink, MemoryCacheEntryOptions);

        return updatedLink;
    }

    public ValueTask<bool> DeleteByIdAsync(long linkId)
    {
        memoryCache.Remove(linkId);

        return innerLinkRepository.DeleteByIdAsync(linkId);
    }
}