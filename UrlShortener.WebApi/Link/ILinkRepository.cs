using System.Threading.Tasks;

namespace UrlShortener.WebApi.Link;

public interface ILinkRepository
{
    ValueTask<DataAccess.Link> CreateAsync(DataAccess.Link link);

    ValueTask<DataAccess.Link> GetByIdAsync(long linkId);

    ValueTask<DataAccess.Link> UpdateAsync(DataAccess.Link link);

    ValueTask<bool> DeleteByIdAsync(long linkId);
}