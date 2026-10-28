using Hirepath.Application.Abstractions;

namespace Hirepath.Infrastructure.Storage;

/// <summary>
/// Stores resume binaries under the storage root as GUID-named files, keeping
/// the original extension. Paths returned are relative to the storage root, so
/// the database never holds a machine-specific absolute path.
/// </summary>
public sealed class FileResumeStorage(AppPaths paths) : IResumeStorage
{
    public async Task<string> SaveAsync(
        string fileName,
        Stream content,
        CancellationToken ct = default)
    {
        var extension = Path.GetExtension(fileName);
        var relativePath = $"{Guid.CreateVersion7():N}{extension}";
        var fullPath = Path.Combine(paths.StorageRoot, relativePath);

        await using var file = File.Create(fullPath);
        await content.CopyToAsync(file, ct);

        return relativePath;
    }

    public Task<Stream> OpenAsync(string relativePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(paths.StorageRoot, relativePath);
        Stream stream = File.OpenRead(fullPath);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string relativePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(paths.StorageRoot, relativePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        return Task.CompletedTask;
    }
}
