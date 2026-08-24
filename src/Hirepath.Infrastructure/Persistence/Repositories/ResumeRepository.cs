using Hirepath.Application.Abstractions;
using Hirepath.Domain.Resumes;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Infrastructure.Persistence.Repositories;

public sealed class ResumeRepository(HirepathDbContext db) : IResumeRepository
{
    public async Task<Resume?> GetAsync(Guid id, CancellationToken ct = default) =>
        await db.Resumes.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IReadOnlyList<Resume>> ListAsync(CancellationToken ct = default) =>
        await db.Resumes.OrderByDescending(r => r.CreatedAt).ToListAsync(ct);

    public async Task AddAsync(Resume resume, CancellationToken ct = default)
    {
        db.Resumes.Add(resume);
        await db.SaveChangesAsync(ct);
    }

    public async Task SaveAsync(Resume resume, CancellationToken ct = default)
    {
        db.Resumes.Update(resume);
        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Resume resume, CancellationToken ct = default)
    {
        db.Resumes.Remove(resume);
        await db.SaveChangesAsync(ct);
    }

    public async Task ClearDefaultAsync(Guid exceptId, CancellationToken ct = default)
    {
        // Load and mutate through the change tracker so any already-tracked
        // instances stay consistent (ExecuteUpdate would leave them stale).
        var others = await db.Resumes
            .Where(r => r.IsDefault && r.Id != exceptId)
            .ToListAsync(ct);

        foreach (var resume in others)
        {
            resume.IsDefault = false;
        }

        await db.SaveChangesAsync(ct);
    }
}
