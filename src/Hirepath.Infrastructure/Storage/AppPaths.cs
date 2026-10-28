namespace Hirepath.Infrastructure.Storage;

/// <summary>
/// Resolves where the SQLite file and the storage folder live. In normal runs
/// this is under %LOCALAPPDATA%/Hirepath (never the repo). An override lets
/// tests and the devcontainer redirect it. All paths use Path.Combine.
/// </summary>
public sealed class AppPaths
{
    public AppPaths(string? rootOverride = null)
    {
        Root = string.IsNullOrWhiteSpace(rootOverride)
            ? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Hirepath")
            : rootOverride;

        StorageRoot = Path.Combine(Root, "storage");
        Directory.CreateDirectory(Root);
        Directory.CreateDirectory(StorageRoot);
    }

    /// <summary>Base data folder for this app.</summary>
    public string Root { get; }

    /// <summary>Folder holding uploaded resume binaries.</summary>
    public string StorageRoot { get; }

    /// <summary>Full path to the SQLite database file.</summary>
    public string DatabaseFile => Path.Combine(Root, "hirepath.db");

    /// <summary>The EF connection string for the resolved database file.</summary>
    public string ConnectionString => $"Data Source={DatabaseFile}";
}
