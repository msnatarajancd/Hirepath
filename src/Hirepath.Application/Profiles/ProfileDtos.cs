using Hirepath.Domain.Common;
using Hirepath.Domain.Profiles;

namespace Hirepath.Application.Profiles;

public sealed record ProfileDto(
    string Name,
    string? Email,
    string? Location,
    int? YearsOfExperience,
    SeniorityLevel Seniority,
    IReadOnlyList<string> TargetRoles,
    IReadOnlyList<ProfileLink> Links);

public sealed record UpdateProfileRequest(
    string Name,
    string? Email,
    string? Location,
    int? YearsOfExperience,
    SeniorityLevel Seniority,
    IReadOnlyList<string> TargetRoles,
    IReadOnlyList<ProfileLink> Links);
