using Hirepath.Application.Abstractions;
using Hirepath.Domain.Profiles;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Infrastructure.Persistence.Repositories;

public sealed class ProfileRepository(HirepathDbContext db) : IProfileRepository
{
    public async Task<Profile> GetOrCreateAsync(CancellationToken ct = default)
    {
        var profile = await db.Profiles.OrderBy(p => p.CreatedAt).FirstOrDefaultAsync(ct);
        if (profile is null)
        {
            profile = new Profile { Name = string.Empty };
            db.Profiles.Add(profile);
            await db.SaveChangesAsync(ct);
        }
        return profile;
    }

    public async Task SaveAsync(Profile profile, CancellationToken ct = default)
    {
        db.Profiles.Update(profile);
        await db.SaveChangesAsync(ct);
    }
}
