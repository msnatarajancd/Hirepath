using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hirepath.Api.Tests;

/// <summary>
/// Boots the real API for integration tests, but redirects the data root to a
/// throwaway temp folder so tests never touch the developer's real database.
/// </summary>
public sealed class HirepathApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dataRoot =
        Path.Combine(Path.GetTempPath(), "Hirepath.Tests", Guid.NewGuid().ToString("N"));

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);
        builder.ConfigureAppConfiguration((_, config) =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Hirepath:DataRoot"] = _dataRoot,
            }));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && Directory.Exists(_dataRoot))
        {
            try
            {
                Directory.Delete(_dataRoot, recursive: true);
            }
            catch (IOException)
            {
                // Best-effort cleanup; a locked SQLite file is not a test failure.
            }
        }
    }
}
