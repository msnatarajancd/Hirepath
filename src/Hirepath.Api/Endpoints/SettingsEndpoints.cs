using Hirepath.Application.Settings;

namespace Hirepath.Api.Endpoints;

public static class SettingsEndpoints
{
    public static IEndpointRouteBuilder MapSettingsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/settings").WithTags("Settings");

        group.MapGet("/", async (SettingsService service, CancellationToken ct) =>
                TypedResults.Ok(await service.GetAsync(ct)))
            .WithName("GetSettings")
            .WithSummary("Current settings (the API key is never returned).");

        group.MapPut("/", async (
                UpdateSettingsRequest request,
                SettingsService service,
                CancellationToken ct) =>
                TypedResults.Ok(await service.UpdateAsync(request, ct)))
            .WithName("UpdateSettings")
            .WithSummary("Update provider, model, key, theme, and toggles.");

        return app;
    }
}
