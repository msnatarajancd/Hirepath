using System.Text;
using Hirepath.Application.Abstractions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Hirepath.Infrastructure.Documents;

/// <summary>Extracts text from PDF resumes with PdfPig (no native deps).</summary>
public sealed class PdfTextExtractor : IDocumentTextExtractor
{
    public bool CanHandle(string contentType, string fileName) =>
        contentType.Contains("pdf", StringComparison.OrdinalIgnoreCase)
        || fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);

    public async Task<string> ExtractAsync(
        Stream content,
        string fileName,
        CancellationToken ct = default)
    {
        // PdfPig needs a seekable stream; copy if necessary.
        using var buffer = new MemoryStream();
        await content.CopyToAsync(buffer, ct);
        buffer.Position = 0;

        var builder = new StringBuilder();
        using var document = PdfDocument.Open(buffer);
        foreach (var page in document.GetPages())
        {
            builder.AppendLine(ContentOrderTextExtractor.GetText(page));
        }

        return builder.ToString().Trim();
    }
}
