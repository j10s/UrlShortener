using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading.Tasks;

namespace UrlShortener.WebApi.Link;

public class RedisLinkRepository(IRedisDatabase redisConnection, ILinkRepository innerLinkRepository)
    : ILinkRepository
{
    public async ValueTask<DataAccess.Link> CreateAsync(DataAccess.Link link)
    {
        return await innerLinkRepository.CreateAsync(link);
    }

    public async ValueTask<DataAccess.Link> GetByIdAsync(long linkId)
    {
        var key = linkId.ToString();
        var link = await redisConnection.GetAsync<DataAccess.Link>(key) ?? await innerLinkRepository.GetByIdAsync(linkId);
        if (link != null) await redisConnection.AddAsync(key, link, flag: CommandFlags.FireAndForget);

        return link;
    }

    public async ValueTask<DataAccess.Link> UpdateAsync(DataAccess.Link link)
    {
        var updatedLink = await innerLinkRepository.UpdateAsync(link);
        
        if (link != null)
        {
            var key = link.Id.ToString();
            await redisConnection.ReplaceAsync(key, updatedLink, When.Exists, CommandFlags.FireAndForget);
        }

        return updatedLink;
    }

    public async ValueTask<bool> DeleteByIdAsync(long linkId)
    {
        var key = linkId.ToString();
        await redisConnection.RemoveAsync(key, CommandFlags.FireAndForget);

        return await innerLinkRepository.DeleteByIdAsync(linkId);
    }
}