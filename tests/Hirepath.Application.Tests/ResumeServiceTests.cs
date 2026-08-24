using System.Text;
using Hirepath.Application.Intake;
using Hirepath.Application.Prompts;
using Hirepath.Application.Resumes;
using Hirepath.Application.Tests.Support;
using Hirepath.Infrastructure.Ai.Fake;
using Hirepath.Infrastructure.Documents;
using Hirepath.Infrastructure.Persistence.Repositories;
using Hirepath.Infrastructure.Storage;

namespace Hirepath.Application.Tests;

public sealed class ResumeServiceTests : IDisposable
{
    private readonly SqliteContext _ctx = new();
    private readonly string _storageRoot =
        Path.Combine(Path.GetTempPath(), "Hirepath.Tests", Guid.NewGuid().ToString("N"));

    private ResumeService CreateService()
    {
        var repo = new ResumeRepository(_ctx.Db);
        var storage = new FileResumeStorage(new AppPaths(_storageRoot));
        var extraction = new DocumentTextExtractionService([new PlainTextExtractor()]);
        var parser = new ResumeParser(new FakeChatClient(new DemoScript()), new PromptLibrary());
        return new ResumeService(repo, storage, extraction, parser);
    }

    private static UploadResumeCommand TxtUpload(string text, string? label = null)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        return new UploadResumeCommand(
            "resume.txt", "text/plain", bytes.Length, new MemoryStream(bytes), label);
    }

    [Fact]
    public async Task Upload_extracts_parses_and_marks_first_resume_default()
    {
        var service = CreateService();

        var result = await service.UploadAsync(TxtUpload("C# and Angular experience", "My CV"));

        result.IsSuccess.Should().BeTrue();
        result.Value!.Label.Should().Be("My CV");
        result.Value.IsDefault.Should().BeTrue();
        result.Value.Parsed!.Skills.Should().Contain("C#");

        // Prove it persisted by reading through a fresh context.
        var fresh = new ResumeRepository(_ctx.NewContext());
        (await fresh.ListAsync()).Should().HaveCount(1);
    }

    [Fact]
    public async Task Oversized_upload_is_rejected_with_validation_error()
    {
        var service = CreateService();
        var command = new UploadResumeCommand(
            "big.txt", "text/plain", ResumeService.MaxSizeBytes + 1, new MemoryStream([1]), null);

        var result = await service.UploadAsync(command);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be(Hirepath.Application.Common.AppErrorCode.Validation);
    }

    [Fact]
    public async Task Setting_a_new_default_clears_the_previous_one()
    {
        var service = CreateService();
        var first = (await service.UploadAsync(TxtUpload("first C#"))).Value!;
        var second = (await service.UploadAsync(TxtUpload("second Angular"))).Value!;

        await service.SetDefaultAsync(second.Id);

        var list = await service.ListAsync();
        list.Single(r => r.Id == second.Id).IsDefault.Should().BeTrue();
        list.Single(r => r.Id == first.Id).IsDefault.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_removes_the_resume()
    {
        var service = CreateService();
        var uploaded = (await service.UploadAsync(TxtUpload("delete me"))).Value!;

        var deleted = await service.DeleteAsync(uploaded.Id);

        deleted.IsSuccess.Should().BeTrue();
        (await service.ListAsync()).Should().BeEmpty();
    }

    public void Dispose()
    {
        _ctx.Dispose();
        if (Directory.Exists(_storageRoot))
        {
            Directory.Delete(_storageRoot, recursive: true);
        }
    }
}
