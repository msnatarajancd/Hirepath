using System.Text;
using Hirepath.Application.Abstractions;

namespace Hirepath.Infrastructure.Documents;

/// <summary>Extracts text from TXT resumes — the trivial case, kept explicit.</summary>
public sealed class PlainTextExtractor : IDocumentTextExtractor
{
    public bool CanHandle(string contentType, string fileName) =>
        contentType.Contains("text/plain", StringComparison.OrdinalIgnoreCase)
        || fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);

    public async Task<string> ExtractAsync(
        Stream content,
        string fileName,
        CancellationToken ct = default)
    {
        using var reader = new StreamReader(content, Encoding.UTF8, leaveOpen: true);
        var text = await reader.ReadToEndAsync(ct);
        return text.Trim();
    }
}
