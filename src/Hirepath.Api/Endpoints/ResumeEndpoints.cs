using Hirepath.Application.Resumes;
using Hirepath.Domain.Resumes;
using Microsoft.AspNetCore.Mvc;

namespace Hirepath.Api.Endpoints;

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
            .WithSummary("One resume with its parse.");

        group.MapPost("/", async (
                [FromForm] IFormFile file,
                [FromForm] string? label,
                ResumeService service,
                CancellationToken ct) =>
            {
                await using var stream = file.OpenReadStream();
                var command = new UploadResumeCommand(
                    file.FileName, file.ContentType, file.Length, stream, label);
                var result = await service.UploadAsync(command, ct);
                return result.ToHttp();
            })
            .WithName("UploadResume")
            .WithSummary("Upload a resume (PDF, DOCX, or TXT; max 10 MB).")
            .DisableAntiforgery();

        group.MapPut("/{id:guid}/parsed", async (
                Guid id,
                ParsedResume parsed,
                ResumeService service,
                CancellationToken ct) =>
                (await service.UpdateParsedAsync(id, parsed, ct)).ToHttp())
            .WithName("UpdateResumeParsed")
            .WithSummary("Save a user-corrected resume parse.");

        group.MapPost("/{id:guid}/default", async (
                Guid id, ResumeService service, CancellationToken ct) =>
                (await service.SetDefaultAsync(id, ct)).ToHttp(_ => TypedResults.NoContent()))
            .WithName("SetDefaultResume")
            .WithSummary("Mark a resume as the default.");

        group.MapDelete("/{id:guid}", async (
                Guid id, ResumeService service, CancellationToken ct) =>
                (await service.DeleteAsync(id, ct)).ToHttp(_ => TypedResults.NoContent()))
            .WithName("DeleteResume")
            .WithSummary("Delete a resume and its stored file.");

        return app;
    }
}
