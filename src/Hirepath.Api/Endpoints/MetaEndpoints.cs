namespace Hirepath.Api.Endpoints;

/// <summary>
/// Health and app-info endpoints. Deliberately tiny — the real surface arrives
/// in later phases. Kept as a mapping extension so Program.cs stays readable.
/// </summary>
public static class MetaEndpoints
{
    public static IEndpointRouteBuilder MapMetaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api").WithTags("Meta");

        group.MapGet("/health", () => TypedResults.Ok(new HealthResponse("ok")))
            .WithName("GetHealth")
            .WithSummary("Liveness probe.");

        group.MapGet("/app-info", () => TypedResults.Ok(new AppInfoResponse(
                Name: "Hirepath",
                Version: "0.1.0",
                Phase: "Phase 0 — Foundation")))
            .WithName("GetAppInfo")
            .WithSummary("Basic app metadata for the shell.");

        return app;
    }
}

public sealed record HealthResponse(string Status);

public sealed record AppInfoResponse(string Name, string Version, string Phase);
