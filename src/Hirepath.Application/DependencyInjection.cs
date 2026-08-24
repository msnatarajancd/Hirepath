using Hirepath.Application.Intake;
using Hirepath.Application.Jobs;
using Hirepath.Application.Profiles;
using Hirepath.Application.Prompts;
using Hirepath.Application.Resumes;
using Hirepath.Application.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Hirepath.Application;

/// <summary>Registers the use-case services and prompt library. These are the
/// single implementations shared by the REST layer and the chat tools.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<PromptLibrary>();

        services.AddScoped<ResumeParser>();
        services.AddScoped<JobDescriptionParser>();

        services.AddScoped<SettingsService>();
        services.AddScoped<ProfileService>();
        services.AddScoped<ResumeService>();
        services.AddScoped<JobDescriptionService>();

        return services;
    }
}
