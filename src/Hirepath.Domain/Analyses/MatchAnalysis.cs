using Hirepath.Domain.Common;

namespace Hirepath.Domain.Analyses;

/// <summary>
/// The result of matching one resume against one job description. Re-running an
/// analysis creates a new row; nothing is ever overwritten, so history is kept.
/// </summary>
public sealed class MatchAnalysis : Entity
{
    public Guid ResumeId { get; set; }
    public Guid JobDescriptionId { get; set; }

    public int OverallScore { get; set; }
    public Verdict Verdict { get; set; }

    public List<Dimension> Dimensions { get; set; } = [];
    public List<string> Strengths { get; set; } = [];
    public List<Gap> Gaps { get; set; } = [];
    public List<ResumeSuggestion> ResumeSuggestions { get; set; } = [];
    public List<PrepPlanItem> PrepPlan { get; set; } = [];

    /// <summary>The raw deterministic sub-scores this analysis was built from.</summary>
    public DeterministicScores Deterministic { get; set; } = DeterministicScores.Empty;

    /// <summary>Model id + prompt version used, for reproducibility and caching.</summary>
    public string? ModelId { get; set; }
    public string? PromptVersion { get; set; }
}
