using Hirepath.Domain.Common;
using Hirepath.Domain.Jobs;

namespace Hirepath.Application.Jobs;

public sealed record JobSummaryDto(
    Guid Id,
    string? Company,
    string Title,
    string? Location,
    WorkMode WorkMode,
    SeniorityLevel Seniority,
    int RequiredSkillCount,
    DateTimeOffset CreatedAt);

public sealed record JobDetailDto(
    Guid Id,
    string? Company,
    string Title,
    string? Location,
    WorkMode WorkMode,
    SeniorityLevel Seniority,
    string RawText,
    ParsedJd? Parsed,
    DateTimeOffset CreatedAt);

/// <summary>Create a job description from pasted text; metadata is optional and
/// can be filled from the parse.</summary>
public sealed record CreateJobRequest(
    string RawText,
    string? Company,
    string? Title,
    string? Location,
    WorkMode WorkMode,
    SeniorityLevel Seniority);
