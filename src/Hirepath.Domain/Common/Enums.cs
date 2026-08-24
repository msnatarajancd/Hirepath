namespace Hirepath.Domain.Common;

/// <summary>Overall analysis verdict, derived from the weighted score.</summary>
public enum Verdict
{
    Poor = 0,
    Stretch = 1,
    Good = 2,
    Strong = 3,
}

/// <summary>How severe a gap is for this specific role.</summary>
public enum GapSeverity
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3,
}

/// <summary>Work mode of a job posting.</summary>
public enum WorkMode
{
    Unspecified = 0,
    Onsite = 1,
    Hybrid = 2,
    Remote = 3,
}

/// <summary>Seniority ladder used for both profiles and postings.</summary>
public enum SeniorityLevel
{
    Unspecified = 0,
    Intern = 1,
    Junior = 2,
    Mid = 3,
    Senior = 4,
    Staff = 5,
    Principal = 6,
    Lead = 7,
    Manager = 8,
    Director = 9,
}

/// <summary>Which chat participant produced a message.</summary>
public enum ChatRole
{
    User = 0,
    Assistant = 1,
    System = 2,
    Tool = 3,
}

/// <summary>Configured AI provider.</summary>
public enum AiProvider
{
    Fake = 0,
    Anthropic = 1,
    OpenAI = 2,
    Ollama = 3,
}
