using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.DataAccess;

namespace UrlShortener.WebApi.Link;

public class EntityFrameworkLinkRepository(DataContext db) : ILinkRepository
{
    public async ValueTask<DataAccess.Link> CreateAsync(DataAccess.Link link)
    {
        db.Links.Add(link);
        await db.SaveChangesAsync();

        return link;
    }

    public async ValueTask<DataAccess.Link> GetByIdAsync(long linkId)
    {
        return await db.Links.AsNoTracking().FirstOrDefaultAsync(l => l.Id == linkId);
    }

    public async ValueTask<DataAccess.Link> UpdateAsync(DataAccess.Link link)
    {
        var linkToUpdate = await db.Links.FindAsync(link.Id);
        if (linkToUpdate == null) return null;

        linkToUpdate.TargetUri = link.TargetUri;
        linkToUpdate.UpdatedAt = link.UpdatedAt;
        await db.SaveChangesAsync();

        return linkToUpdate;
    }

    public async ValueTask<bool> DeleteByIdAsync(long linkId)
    {
        var rowsDeleted = await db.Links.Where(l => l.Id == linkId).ExecuteDeleteAsync();

        return rowsDeleted > 0;
    }
}