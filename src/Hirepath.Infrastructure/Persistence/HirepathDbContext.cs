using Hirepath.Domain.Analyses;
using Hirepath.Domain.Chat;
using Hirepath.Domain.Common;
using Hirepath.Domain.Jobs;
using Hirepath.Domain.Profiles;
using Hirepath.Domain.Resumes;
using Hirepath.Domain.Settings;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Infrastructure.Persistence;

/// <summary>
/// The single EF Core context. Code-first with real migrations from commit one.
/// Complex properties are stored as JSON text columns (see JsonColumn).
/// </summary>
public sealed class HirepathDbContext(DbContextOptions<HirepathDbContext> options)
    : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<JobDescription> JobDescriptions => Set<JobDescription>();
    public DbSet<MatchAnalysis> Analyses => Set<MatchAnalysis>();
    public DbSet<ChatThread> Threads => Set<ChatThread>();
    public DbSet<ChatMessage> Messages => Set<ChatMessage>();
    public DbSet<AppSettings> Settings => Set<AppSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HirepathDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        TouchTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        TouchTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void TouchTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
