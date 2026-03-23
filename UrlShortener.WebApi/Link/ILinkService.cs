using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.WebApi.Link;

public interface ILinkService
{
    ValueTask<Models.Link> CreateAsync(CreateLinkRequest request);

    ValueTask<Models.Link> GetByStubAsync(string stub);

    ValueTask<Models.Link> UpdateByStubAsync(string stub, UpdateLinkRequest request);

    ValueTask<bool> DeleteByStubAsync(string stub);
}