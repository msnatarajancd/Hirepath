using Hirepath.Application.Abstractions;
using Hirepath.Application.Common;

namespace Hirepath.Infrastructure.Documents;

/// <summary>Picks the right extractor for a file; throws a typed error if none.</summary>
public sealed class DocumentTextExtractionService(IEnumerable<IDocumentTextExtractor> extractors)
    : IDocumentTextExtractionService
{
    private readonly IReadOnlyList<IDocumentTextExtractor> _extractors = [.. extractors];

    public async Task<string> ExtractAsync(
        Stream content,
        string contentType,
        string fileName,
        CancellationToken ct = default)
    {
        var extractor = _extractors.FirstOrDefault(e => e.CanHandle(contentType, fileName))
            ?? throw new UnsupportedDocumentException(
                AppError.Unsupported($"Unsupported file type: {fileName} ({contentType})."));

        return await extractor.ExtractAsync(content, fileName, ct);
    }
}
