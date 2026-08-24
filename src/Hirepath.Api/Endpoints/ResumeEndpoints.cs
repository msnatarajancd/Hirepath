using Hirepath.Application.Resumes;
using Hirepath.Domain.Resumes;
using Microsoft.AspNetCore.Mvc;

namespace Hirepath.Api.Endpoints;

/// <summary>Multipart upload form: the resume file plus an optional label.</summary>
public sealed class UploadResumeForm
{
    [FromForm]
    public required IFormFile File { get; init; }

    [FromForm]
    public string? Label { get; init; }
}

public static class ResumeEndpoints
{
    public static IEndpointRouteBuilder MapResumeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resumes").WithTags("Resumes");

        group.MapGet("/", async (ResumeService service, CancellationToken ct) =>
                TypedResults.Ok(await service.ListAsync(ct)))
            .WithName("ListResumes")
            .WithSummary("All uploaded resumes.");

        group.MapGet("/{id:guid}", async (Guid id, ResumeService service, CancellationToken ct) =>
                (await service.GetAsync(id, ct)).ToHttp())
            .WithName("GetResume")
            .WithSummary("One resume with its parse.")
            .Produces<ResumeDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
                [FromForm] UploadResumeForm form,
                ResumeService service,
                CancellationToken ct) =>
            {
                await using var stream = form.File.OpenReadStream();
                var command = new UploadResumeCommand(
                    form.File.FileName, form.File.ContentType, form.File.Length, stream, form.Label);
                var result = await service.UploadAsync(command, ct);
                return result.ToHttp();
            })
            .WithName("UploadResume")
            .WithSummary("Upload a resume (PDF, DOCX, or TXT; max 10 MB).")
            .Produces<ResumeDetailDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status415UnsupportedMediaType)
            .DisableAntiforgery();

        group.MapPut("/{id:guid}/parsed", async (
                Guid id,
                ParsedResume parsed,
                ResumeService service,
                CancellationToken ct) =>
                (await service.UpdateParsedAsync(id, parsed, ct)).ToHttp())
            .WithName("UpdateResumeParsed")
            .WithSummary("Save a user-corrected resume parse.")
            .Produces<ResumeDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/{id:guid}/default", async (
                Guid id, ResumeService service, CancellationToken ct) =>
                (await service.SetDefaultAsync(id, ct)).ToHttp(_ => TypedResults.NoContent()))
            .WithName("SetDefaultResume")
            .WithSummary("Mark a resume as the default.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", async (
                Guid id, ResumeService service, CancellationToken ct) =>
                (await service.DeleteAsync(id, ct)).ToHttp(_ => TypedResults.NoContent()))
            .WithName("DeleteResume")
            .WithSummary("Delete a resume and its stored file.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }
}
