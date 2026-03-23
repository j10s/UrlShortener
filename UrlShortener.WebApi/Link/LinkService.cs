using System;
using System.Threading.Tasks;
using Sqids;
using UrlShortener.Models;

namespace UrlShortener.WebApi.Link;

public class LinkService(ILinkRepository linkRepository, SqidsEncoder<long> hashIds) : ILinkService
{
    public async ValueTask<Models.Link> CreateAsync(CreateLinkRequest request)
    {
        var now = DateTimeOffset.UtcNow;
        var link = new DataAccess.Link
        {
            TargetUri = request.TargetUri,
            CreatedAt = now,
            UpdatedAt = now
        };
        link = await linkRepository.CreateAsync(link);

        return Convert(link);
    }

    public async ValueTask<Models.Link> GetByStubAsync(string stub)
    {
        if (!TryGetId(stub, out var linkId)) return null;

        var link = await linkRepository.GetByIdAsync(linkId);

        return Convert(link);
    }

    public async ValueTask<Models.Link> UpdateByStubAsync(string stub, UpdateLinkRequest request)
    {
        if (!TryGetId(stub, out var linkId)) return null;

        var link = new DataAccess.Link
        {
            Id = linkId,
            TargetUri = request.TargetUri,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var updatedLink = await linkRepository.UpdateAsync(link);

        return Convert(updatedLink);
    }

    public async ValueTask<bool> DeleteByStubAsync(string stub)
    {
        if (!TryGetId(stub, out var linkId)) return false;

        return await linkRepository.DeleteByIdAsync(linkId);
    }

    private bool TryGetId(string stub, out long id)
    {
        id = 0;

        // check hash validity
        var decoded = hashIds.Decode(stub);
        var encoded = hashIds.Encode(decoded);
        if (encoded != stub) return false;
            
        id = decoded[0];
        return true;
    }

    private Models.Link Convert(DataAccess.Link link)
    {
        if (link == null) return null;

        return new Models.Link
        {
            Stub = hashIds.Encode(link.Id),
            TargetUri = link.TargetUri,
            CreatedAt = link.CreatedAt,
            UpdatedAt = link.UpdatedAt
        };
    }
}