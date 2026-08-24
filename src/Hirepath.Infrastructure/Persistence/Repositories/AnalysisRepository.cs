using Hirepath.Application.Abstractions;
using Hirepath.Domain.Analyses;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Infrastructure.Persistence.Repositories;

public sealed class AnalysisRepository(HirepathDbContext db) : IAnalysisRepository
{
    public async Task<MatchAnalysis?> GetAsync(Guid id, CancellationToken ct = default) =>
        await db.Analyses.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IReadOnlyList<MatchAnalysis>> ListAsync(CancellationToken ct = default) =>
        await db.Analyses.OrderByDescending(a => a.CreatedAt).ToListAsync(ct);

    public async Task AddAsync(MatchAnalysis analysis, CancellationToken ct = default)
    {
        db.Analyses.Add(analysis);
        await db.SaveChangesAsync(ct);
    }

    public async Task SaveAsync(MatchAnalysis analysis, CancellationToken ct = default)
    {
        db.Analyses.Update(analysis);
        await db.SaveChangesAsync(ct);
    }
}
