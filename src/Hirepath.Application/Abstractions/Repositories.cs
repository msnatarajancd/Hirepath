using Hirepath.Domain.Analyses;
using Hirepath.Domain.Jobs;
using Hirepath.Domain.Profiles;
using Hirepath.Domain.Resumes;
using Hirepath.Domain.Settings;

namespace Hirepath.Application.Abstractions;

/// <summary>The single profile row, created on first read if absent.</summary>
public interface IProfileRepository
{
    Task<Profile> GetOrCreateAsync(CancellationToken ct = default);
    Task SaveAsync(Profile profile, CancellationToken ct = default);
}

public interface IResumeRepository
{
    Task<Resume?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Resume>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Resume resume, CancellationToken ct = default);
    Task SaveAsync(Resume resume, CancellationToken ct = default);
    Task RemoveAsync(Resume resume, CancellationToken ct = default);
    Task ClearDefaultAsync(Guid exceptId, CancellationToken ct = default);
}

public interface IJobDescriptionRepository
{
    Task<JobDescription?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<JobDescription>> ListAsync(CancellationToken ct = default);
    Task AddAsync(JobDescription jd, CancellationToken ct = default);
    Task SaveAsync(JobDescription jd, CancellationToken ct = default);
}

public interface IAnalysisRepository
{
    Task<MatchAnalysis?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<MatchAnalysis>> ListAsync(CancellationToken ct = default);
    Task AddAsync(MatchAnalysis analysis, CancellationToken ct = default);
    Task SaveAsync(MatchAnalysis analysis, CancellationToken ct = default);
}

/// <summary>The single settings row, created with defaults on first read.</summary>
public interface ISettingsRepository
{
    Task<AppSettings> GetAsync(CancellationToken ct = default);
    Task SaveAsync(AppSettings settings, CancellationToken ct = default);
}
