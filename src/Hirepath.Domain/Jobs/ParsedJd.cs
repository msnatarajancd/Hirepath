namespace Hirepath.Domain.Jobs;

/// <summary>
/// Structured view of a job description, produced by the model pass and stored
/// as a JSON column. Immutable so it feeds cleanly into scoring.
/// </summary>
public sealed record ParsedJd
{
    public IReadOnlyList<string> Responsibilities { get; init; } = [];
    public IReadOnlyList<string> RequiredSkills { get; init; } = [];
    public IReadOnlyList<string> PreferredSkills { get; init; } = [];
    public int? YearsRequired { get; init; }
    public IReadOnlyList<string> TechStack { get; init; } = [];
    public IReadOnlyList<string> Keywords { get; init; } = [];

    public static ParsedJd Empty { get; } = new();
}
