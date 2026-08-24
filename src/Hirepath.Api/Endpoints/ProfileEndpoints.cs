using Hirepath.Application.Profiles;

namespace Hirepath.Api.Endpoints;

public static class ProfileEndpoints
{
    public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profile").WithTags("Profile");

        group.MapGet("/", async (ProfileService service, CancellationToken ct) =>
                TypedResults.Ok(await service.GetAsync(ct)))
            .WithName("GetProfile")
            .WithSummary("The user's profile (created on first read).");

        group.MapPut("/", async (
                UpdateProfileRequest request,
                ProfileService service,
                CancellationToken ct) =>
                TypedResults.Ok(await service.UpdateAsync(request, ct)))
            .WithName("UpdateProfile")
            .WithSummary("Update the user's profile.");

        return app;
    }
}
