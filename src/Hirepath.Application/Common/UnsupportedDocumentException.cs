namespace Hirepath.Application.Common;

/// <summary>
/// Thrown by the document extraction service when no extractor handles a file.
/// Defined in Application so use-case handlers can catch it without depending on
/// Infrastructure.
/// </summary>
public sealed class UnsupportedDocumentException(AppError error) : Exception(error.Message)
{
    public AppError Error { get; } = error;
}
