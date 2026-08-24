namespace Hirepath.Domain.Resumes;

/// <summary>
/// Structured view of a resume, produced by the model pass and stored as a
/// JSON column. Kept as immutable records so it is trivially serialisable and
/// safe to pass into the pure scoring functions.
/// </summary>
public sealed record ParsedResume
{
    public string Summary { get; init; } = string.Empty;
    public IReadOnlyList<string> Skills { get; init; } = [];
    public IReadOnlyList<ExperienceItem> Experience { get; init; } = [];
    public IReadOnlyList<EducationItem> Education { get; init; } = [];
    public IReadOnlyList<string> Projects { get; init; } = [];
    public IReadOnlyList<string> Certifications { get; init; } = [];

    public static ParsedResume Empty { get; } = new();
}

public sealed record ExperienceItem
{
    public string Company { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? StartDate { get; init; }
    public string? EndDate { get; init; }
    public IReadOnlyList<string> Bullets { get; init; } = [];
}

public sealed record EducationItem
{
    public string Institution { get; init; } = string.Empty;
    public string Credential { get; init; } = string.Empty;
    public string? Year { get; init; }
}
