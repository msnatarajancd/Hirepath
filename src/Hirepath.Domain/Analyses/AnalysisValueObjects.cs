using Hirepath.Domain.Common;

namespace Hirepath.Domain.Analyses;

/// <summary>One scored dimension (Skills, Experience, Domain, Seniority, ATS).</summary>
public sealed record Dimension
{
    public string Name { get; init; } = string.Empty;
    public int Score { get; init; }
    public double Weight { get; init; }

    /// <summary>Exact quotes from the resume that justify the score. Never empty:
    /// carries an explicit "no evidence found" marker instead.</summary>
    public IReadOnlyList<string> Evidence { get; init; } = [];

    public string Rationale { get; init; } = string.Empty;
}

/// <summary>A gap between the resume and the role, with how to close it.</summary>
public sealed record Gap
{
    public string Item { get; init; } = string.Empty;
    public GapSeverity Severity { get; init; }

    /// <summary>Why this matters to <em>this</em> employer specifically.</summary>
    public string WhyItMatters { get; init; } = string.Empty;

    public string HowToClose { get; init; } = string.Empty;
    public string EstimatedEffort { get; init; } = string.Empty;
}

/// <summary>A concrete before/after resume edit the model suggests.</summary>
public sealed record ResumeSuggestion
{
    public string TargetBullet { get; init; } = string.Empty;
    public string SuggestedRewrite { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
}

/// <summary>A single item in the 7-day prep plan, with persisted checked state.</summary>
public sealed record PrepPlanItem
{
    public int Day { get; init; }
    public string Task { get; init; } = string.Empty;
    public bool Done { get; init; }
}

/// <summary>
/// The raw, deterministic sub-scores computed in Domain before the model pass.
/// Stored alongside the narrative so any final number can be traced back.
/// </summary>
public sealed record DeterministicScores
{
    public double SkillOverlap { get; init; }
    public double KeywordCoverage { get; init; }
    public double StemmedKeywordCoverage { get; init; }
    public int YearsDelta { get; init; }
    public double DomainOverlap { get; init; }
    public double SeniorityAlignment { get; init; }

    public static DeterministicScores Empty { get; } = new();
}
