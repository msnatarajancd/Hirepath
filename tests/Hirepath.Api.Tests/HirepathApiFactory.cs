using Hirepath.Infrastructure.Persistence;
using Hirepath.Infrastructure.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Hirepath.Api.Tests;

/// <summary>
/// Boots the real API for integration tests, redirecting storage and the
/// database to a throwaway temp folder. The override is applied at the DI level
/// (ConfigureTestServices), which is reliably seen even though registration-time
/// configuration reads happen before WebApplicationFactory's config callbacks.
/// </summary>
public sealed class HirepathApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dataRoot =
        Path.Combine(Path.GetTempPath(), "Hirepath.Tests", Guid.NewGuid().ToString("N"));

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);

        builder.ConfigureTestServices(services =>
        {
            var paths = new AppPaths(_dataRoot);
            services.RemoveAll<AppPaths>();
            services.AddSingleton(paths);

            services.RemoveAll<DbContextOptions<HirepathDbContext>>();
            services.RemoveAll<HirepathDbContext>();
            services.AddDbContext<HirepathDbContext>(options => options.UseSqlite(paths.ConnectionString));
        });
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
