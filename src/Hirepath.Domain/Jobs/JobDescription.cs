using Hirepath.Domain.Common;

namespace Hirepath.Domain.Jobs;

/// <summary>
/// A pasted job description. Raw text plus the structured parse used by scoring.
/// </summary>
public sealed class JobDescription : Entity
{
    public string? Company { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Location { get; set; }
    public WorkMode WorkMode { get; set; } = WorkMode.Unspecified;
    public SeniorityLevel Seniority { get; set; } = SeniorityLevel.Unspecified;

    public string RawText { get; set; } = string.Empty;

    /// <summary>Structured parse (JSON column). Null until the parse runs.</summary>
    public ParsedJd? Parsed { get; set; }
}
