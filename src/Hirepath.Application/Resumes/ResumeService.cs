using Hirepath.Application.Abstractions;
using Hirepath.Application.Common;
using Hirepath.Application.Intake;
using Hirepath.Domain.Resumes;

namespace Hirepath.Application.Resumes;

/// <summary>
/// Resume intake and management. Upload stores the binary, extracts text, and
/// runs the model parse; the result is shown to the user, who can correct it.
/// This handler is called from both the REST endpoint and the chat tool.
/// </summary>
public sealed class ResumeService(
    IResumeRepository resumes,
    IResumeStorage storage,
    IDocumentTextExtractionService extraction,
    ResumeParser parser)
{
    public const long MaxSizeBytes = 10 * 1024 * 1024;

    public async Task<Result<ResumeDetailDto>> UploadAsync(
        UploadResumeCommand command,
        CancellationToken ct = default)
    {
        if (command.SizeBytes > MaxSizeBytes)
        {
            return AppError.Validation("Resume exceeds the 10 MB limit.");
        }

        // Buffer once so we can both store the binary and extract text from it.
        using var buffer = new MemoryStream();
        await command.Content.CopyToAsync(buffer, ct);
        if (buffer.Length == 0)
        {
            return AppError.Validation("The uploaded file is empty.");
        }

        buffer.Position = 0;
        string extractedText;
        try
        {
            extractedText = await extraction.ExtractAsync(
                buffer, command.ContentType, command.FileName, ct);
        }
        catch (UnsupportedDocumentException ex)
        {
            return ex.Error;
        }

        buffer.Position = 0;
        var storedPath = await storage.SaveAsync(command.FileName, buffer, ct);

        var parsed = await parser.ParseAsync(extractedText, ct);

        var isFirst = (await resumes.ListAsync(ct)).Count == 0;
        var resume = new Resume
        {
            Label = string.IsNullOrWhiteSpace(command.Label)
                ? Path.GetFileNameWithoutExtension(command.FileName)
                : command.Label.Trim(),
            FileName = command.FileName,
            ContentType = command.ContentType,
            StoredPath = storedPath,
            SizeBytes = command.SizeBytes,
            ExtractedText = extractedText,
            Parsed = parsed,
            IsDefault = isFirst,
        };

        await resumes.AddAsync(resume, ct);
        return ToDetail(resume);
    }

    public async Task<IReadOnlyList<ResumeSummaryDto>> ListAsync(CancellationToken ct = default)
    {
        var all = await resumes.ListAsync(ct);
        return [.. all.Select(ToSummary)];
    }

    public async Task<Result<ResumeDetailDto>> GetAsync(Guid id, CancellationToken ct = default)
    {
        var resume = await resumes.GetAsync(id, ct);
        return resume is null ? AppError.NotFound("Resume") : ToDetail(resume);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var resume = await resumes.GetAsync(id, ct);
        if (resume is null)
        {
            return AppError.NotFound("Resume");
        }

        await storage.DeleteAsync(resume.StoredPath, ct);
        await resumes.RemoveAsync(resume, ct);
        return true;
    }

    public async Task<Result<bool>> SetDefaultAsync(Guid id, CancellationToken ct = default)
    {
        var resume = await resumes.GetAsync(id, ct);
        if (resume is null)
        {
            return AppError.NotFound("Resume");
        }

        resume.IsDefault = true;
        await resumes.SaveAsync(resume, ct);
        await resumes.ClearDefaultAsync(resume.Id, ct);
        return true;
    }

    public async Task<Result<ResumeDetailDto>> UpdateParsedAsync(
        Guid id,
        ParsedResume parsed,
        CancellationToken ct = default)
    {
        var resume = await resumes.GetAsync(id, ct);
        if (resume is null)
        {
            return AppError.NotFound("Resume");
        }

        resume.Parsed = parsed;
        await resumes.SaveAsync(resume, ct);
        return ToDetail(resume);
    }

    private static ResumeSummaryDto ToSummary(Resume r) => new(
        r.Id, r.Label, r.FileName, r.ContentType, r.SizeBytes, r.IsDefault,
        r.Parsed?.Skills.Count ?? 0, r.CreatedAt);

    private static ResumeDetailDto ToDetail(Resume r) => new(
        r.Id, r.Label, r.FileName, r.ContentType, r.SizeBytes, r.IsDefault,
        r.ExtractedText, r.Parsed, r.CreatedAt);
}
