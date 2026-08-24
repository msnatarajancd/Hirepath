using Hirepath.Application.Abstractions;
using Hirepath.Domain.Settings;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Infrastructure.Persistence.Repositories;

public sealed class SettingsRepository(HirepathDbContext db) : ISettingsRepository
{
    public async Task<AppSettings> GetAsync(CancellationToken ct = default)
    {
        var settings = await db.Settings.OrderBy(s => s.CreatedAt).FirstOrDefaultAsync(ct);
        if (settings is null)
        {
            settings = new AppSettings();
            db.Settings.Add(settings);
            await db.SaveChangesAsync(ct);
        }
        return settings;
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken ct = default)
    {
        db.Settings.Update(settings);
        await db.SaveChangesAsync(ct);
    }
}
