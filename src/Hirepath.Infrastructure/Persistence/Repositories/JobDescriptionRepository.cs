using Hirepath.Application.Abstractions;
using Hirepath.Domain.Jobs;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Infrastructure.Persistence.Repositories;

public sealed class JobDescriptionRepository(HirepathDbContext db) : IJobDescriptionRepository
{
    public async Task<JobDescription?> GetAsync(Guid id, CancellationToken ct = default) =>
        await db.JobDescriptions.FirstOrDefaultAsync(j => j.Id == id, ct);

    public async Task<IReadOnlyList<JobDescription>> ListAsync(CancellationToken ct = default) =>
        await db.JobDescriptions.OrderByDescending(j => j.CreatedAt).ToListAsync(ct);

    public async Task AddAsync(JobDescription jd, CancellationToken ct = default)
    {
        db.JobDescriptions.Add(jd);
        await db.SaveChangesAsync(ct);
    }

    public async Task SaveAsync(JobDescription jd, CancellationToken ct = default)
    {
        db.JobDescriptions.Update(jd);
        await db.SaveChangesAsync(ct);
    }
}
