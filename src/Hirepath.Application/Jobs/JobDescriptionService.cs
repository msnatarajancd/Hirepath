using Hirepath.Application.Abstractions;
using Hirepath.Application.Common;
using Hirepath.Application.Intake;
using Hirepath.Domain.Jobs;

namespace Hirepath.Application.Jobs;

/// <summary>Job description intake: paste text, parse it, store it. Called from
/// both the REST endpoint and the chat tool.</summary>
public sealed class JobDescriptionService(
    IJobDescriptionRepository jobs,
    JobDescriptionParser parser)
{
    public async Task<Result<JobDetailDto>> CreateAsync(
        CreateJobRequest request,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.RawText))
        {
            return AppError.Validation("Paste the job description text.");
        }

        var parsed = await parser.ParseAsync(request.RawText, ct);

        var jd = new JobDescription
        {
            RawText = request.RawText,
            Company = Trimmed(request.Company),
            Title = string.IsNullOrWhiteSpace(request.Title)
                ? "Untitled role"
                : request.Title.Trim(),
            Location = Trimmed(request.Location),
            WorkMode = request.WorkMode,
            Seniority = request.Seniority,
            Parsed = parsed,
        };

        await jobs.AddAsync(jd, ct);
        return ToDetail(jd);
    }

    public async Task<IReadOnlyList<JobSummaryDto>> ListAsync(CancellationToken ct = default)
    {
        var all = await jobs.ListAsync(ct);
        return [.. all.Select(ToSummary)];
    }

    public async Task<Result<JobDetailDto>> GetAsync(Guid id, CancellationToken ct = default)
    {
        var jd = await jobs.GetAsync(id, ct);
        return jd is null ? AppError.NotFound("Job description") : ToDetail(jd);
    }

    public async Task<Result<JobDetailDto>> UpdateParsedAsync(
        Guid id,
        ParsedJd parsed,
        CancellationToken ct = default)
    {
        var jd = await jobs.GetAsync(id, ct);
        if (jd is null)
        {
            return AppError.NotFound("Job description");
        }

        jd.Parsed = parsed;
        await jobs.SaveAsync(jd, ct);
        return ToDetail(jd);
    }

    private static JobSummaryDto ToSummary(JobDescription j) => new(
        j.Id, j.Company, j.Title, j.Location, j.WorkMode, j.Seniority,
        j.Parsed?.RequiredSkills.Count ?? 0, j.CreatedAt);

    private static JobDetailDto ToDetail(JobDescription j) => new(
        j.Id, j.Company, j.Title, j.Location, j.WorkMode, j.Seniority,
        j.RawText, j.Parsed, j.CreatedAt);

    private static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
