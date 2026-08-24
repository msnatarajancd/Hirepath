namespace Hirepath.Application.Abstractions;

/// <summary>Stores resume binaries on disk; the DB keeps only the path.</summary>
public interface IResumeStorage
{
    /// <summary>Saves the bytes and returns a path relative to the storage root.</summary>
    Task<string> SaveAsync(string fileName, Stream content, CancellationToken ct = default);

    Task<Stream> OpenAsync(string relativePath, CancellationToken ct = default);

    Task DeleteAsync(string relativePath, CancellationToken ct = default);
}
