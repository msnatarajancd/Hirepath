using Hirepath.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hirepath.Infrastructure.Persistence;

/// <summary>
/// Design-time factory so `dotnet ef migrations add` works without booting the
/// API. Points at the same local-data database the app uses at runtime.
/// </summary>
public sealed class HirepathDbContextFactory : IDesignTimeDbContextFactory<HirepathDbContext>
{
    public HirepathDbContext CreateDbContext(string[] args)
    {
        var paths = new AppPaths(Environment.GetEnvironmentVariable("HIREPATH_DATA_ROOT"));
        var options = new DbContextOptionsBuilder<HirepathDbContext>()
            .UseSqlite(paths.ConnectionString)
            .Options;

        return new HirepathDbContext(options);
    }
}
