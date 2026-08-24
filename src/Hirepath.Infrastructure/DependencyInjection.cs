using Hirepath.Application.Abstractions;
using Hirepath.Infrastructure.Ai.Fake;
using Hirepath.Infrastructure.Documents;
using Hirepath.Infrastructure.Persistence;
using Hirepath.Infrastructure.Persistence.Repositories;
using Hirepath.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hirepath.Infrastructure;

/// <summary>Composition of the infrastructure layer: storage, EF Core,
/// document extraction, and the AI client (fake by default in Phase 1).</summary>
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

        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();
        services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
        services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        services.AddScoped<ISettingsRepository, SettingsRepository>();

        services.AddScoped<IResumeStorage, FileResumeStorage>();

        services.AddScoped<IDocumentTextExtractor, PdfTextExtractor>();
        services.AddScoped<IDocumentTextExtractor, DocxTextExtractor>();
        services.AddScoped<IDocumentTextExtractor, PlainTextExtractor>();
        services.AddScoped<IDocumentTextExtractionService, DocumentTextExtractionService>();

        // Phase 1: the fake client is the only provider — it powers Demo Mode and
        // all tests. Phase 2 introduces the real-provider factory.
        services.AddSingleton<IFakeChatScript, DemoScript>();
        services.AddChatClient(sp => new FakeChatClient(sp.GetRequiredService<IFakeChatScript>()));

        return services;
    }
}
