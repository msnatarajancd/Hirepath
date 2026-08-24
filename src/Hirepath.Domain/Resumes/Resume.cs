using Hirepath.Domain.Common;

namespace Hirepath.Domain.Resumes;

/// <summary>
/// An uploaded resume. The binary lives on disk under the storage folder; the
/// row stores its path plus the extracted text and the structured parse.
/// </summary>
public sealed class Resume : Entity
{
    public string Label { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Path to the stored binary, relative to the storage root.</summary>
    public string StoredPath { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    /// <summary>Raw text extracted by PdfPig / OpenXml, before the model pass.</summary>
    public string ExtractedText { get; set; } = string.Empty;

    /// <summary>Structured parse (JSON column). Null until the parse runs.</summary>
    public ParsedResume? Parsed { get; set; }

    public bool IsDefault { get; set; }
}
