using Hirepath.Infrastructure.Persistence;
using Hirepath.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hirepath.Infrastructure;

/// <summary>Composition of the infrastructure layer: storage paths + EF Core.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Override with Hirepath:DataRoot (config) or HIREPATH_DATA_ROOT (env).
        var dataRoot = configuration["Hirepath:DataRoot"]
            ?? Environment.GetEnvironmentVariable("HIREPATH_DATA_ROOT");
        var paths = new AppPaths(dataRoot);
        services.AddSingleton(paths);

        services.AddDbContext<HirepathDbContext>(options =>
            options.UseSqlite(paths.ConnectionString));

        return services;
    }
}
