namespace Hirepath.Application.Abstractions;

/// <summary>
/// Extracts plain text from an uploaded resume. Implementations handle PDF
/// (PdfPig), DOCX (OpenXml), and TXT. Must not throw on messy real-world input:
/// return whatever text is recoverable.
/// </summary>
public interface IDocumentTextExtractor
{
    bool CanHandle(string contentType, string fileName);

    Task<string> ExtractAsync(Stream content, string fileName, CancellationToken ct = default);
}

/// <summary>Chooses the right extractor for a given file.</summary>
public interface IDocumentTextExtractionService
{
    Task<string> ExtractAsync(
        Stream content,
        string contentType,
        string fileName,
        CancellationToken ct = default);
}
