using Hirepath.Application.Jobs;
using Hirepath.Domain.Jobs;

namespace Hirepath.Api.Endpoints;

public static class JobEndpoints
{
    public static IEndpointRouteBuilder MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/jobs").WithTags("Jobs");

        group.MapGet("/", async (JobDescriptionService service, CancellationToken ct) =>
                TypedResults.Ok(await service.ListAsync(ct)))
            .WithName("ListJobs")
            .WithSummary("All saved job descriptions.");

        group.MapGet("/{id:guid}", async (
                Guid id, JobDescriptionService service, CancellationToken ct) =>
                (await service.GetAsync(id, ct)).ToHttp())
            .WithName("GetJob")
            .WithSummary("One job description with its parse.")
            .Produces<JobDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
                CreateJobRequest request,
                JobDescriptionService service,
                CancellationToken ct) =>
                (await service.CreateAsync(request, ct)).ToHttp())
            .WithName("CreateJob")
            .WithSummary("Create a job description from pasted text.")
            .Produces<JobDetailDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}/parsed", async (
                Guid id,
                ParsedJd parsed,
                JobDescriptionService service,
                CancellationToken ct) =>
                (await service.UpdateParsedAsync(id, parsed, ct)).ToHttp())
            .WithName("UpdateJobParsed")
            .WithSummary("Save a user-corrected JD parse.")
            .Produces<JobDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }
}
