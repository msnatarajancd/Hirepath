using Hirepath.Domain.Common;

namespace Hirepath.Domain.Profiles;

/// <summary>
/// The single user of this local-first app. One row expected, but modelled as
/// an entity so nothing about the schema blocks multi-profile support later.
/// </summary>
public sealed class Profile : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Location { get; set; }
    public int? YearsOfExperience { get; set; }
    public SeniorityLevel Seniority { get; set; } = SeniorityLevel.Unspecified;

    /// <summary>Roles the user is targeting, e.g. "Senior Backend Engineer".</summary>
    public List<string> TargetRoles { get; set; } = [];

    /// <summary>External links (GitHub, LinkedIn, portfolio) keyed by label.</summary>
    public List<ProfileLink> Links { get; set; } = [];
}

public sealed record ProfileLink(string Label, string Url);
