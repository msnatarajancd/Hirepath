using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Hirepath.Application.Abstractions;

namespace Hirepath.Infrastructure.Documents;

/// <summary>Extracts text from DOCX resumes with DocumentFormat.OpenXml.</summary>
public sealed class DocxTextExtractor : IDocumentTextExtractor
{
    private const string DocxContentType =
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public bool CanHandle(string contentType, string fileName) =>
        contentType.Contains(DocxContentType, StringComparison.OrdinalIgnoreCase)
        || fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);

    public async Task<string> ExtractAsync(
        Stream content,
        string fileName,
        CancellationToken ct = default)
    {
        using var buffer = new MemoryStream();
        await content.CopyToAsync(buffer, ct);
        buffer.Position = 0;

        using var document = WordprocessingDocument.Open(buffer, isEditable: false);
        var body = document.MainDocumentPart?.Document?.Body;
        if (body is null)
        {
            return string.Empty;
        }

        // Join paragraphs with newlines so structure survives the extraction.
        var builder = new StringBuilder();
        foreach (var paragraph in body.Descendants<Paragraph>())
        {
            builder.AppendLine(paragraph.InnerText);
        }

        return builder.ToString().Trim();
    }
}
