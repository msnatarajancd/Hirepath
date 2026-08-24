using Hirepath.Domain.Resumes;

namespace Hirepath.Application.Resumes;

public sealed record ResumeSummaryDto(
    Guid Id,
    string Label,
    string FileName,
    string ContentType,
    long SizeBytes,
    bool IsDefault,
    int SkillCount,
    DateTimeOffset CreatedAt);

public sealed record ResumeDetailDto(
    Guid Id,
    string Label,
    string FileName,
    string ContentType,
    long SizeBytes,
    bool IsDefault,
    string ExtractedText,
    ParsedResume? Parsed,
    DateTimeOffset CreatedAt);

/// <summary>A resume upload: the raw bytes plus their metadata.</summary>
public sealed record UploadResumeCommand(
    string FileName,
    string ContentType,
    long SizeBytes,
    Stream Content,
    string? Label);
