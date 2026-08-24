using System.Net;
using System.Net.Http.Json;
using System.Text;
using Hirepath.Application.Resumes;
using Hirepath.Application.Settings;
using Hirepath.Domain.Common;

namespace Hirepath.Api.Tests;

public sealed class IntakeEndpointsTests(HirepathApiFactory factory)
    : IClassFixture<HirepathApiFactory>
{
    private readonly HirepathApiFactory _factory = factory;

    [Fact]
    public async Task Settings_round_trip_never_returns_the_key()
    {
        var client = _factory.CreateClient();

        var update = new UpdateSettingsRequest(
            AiProvider.Anthropic, "claude-opus-4-8", "sk-should-not-echo", null, "dark", false, true);
        var put = await client.PutAsJsonAsync("/api/settings", update);
        put.EnsureSuccessStatusCode();

        var body = await put.Content.ReadAsStringAsync();
        body.Should().NotContain("sk-should-not-echo");

        var settings = await client.GetFromJsonAsync<SettingsDto>("/api/settings", TestJson.Options);
        settings!.HasApiKey.Should().BeTrue();
        settings.Provider.Should().Be(AiProvider.Anthropic);
    }

    [Fact]
    public async Task Resume_upload_parses_in_demo_mode()
    {
        var client = _factory.CreateClient();

        using var content = new MultipartFormDataContent();
        var bytes = Encoding.UTF8.GetBytes("Experienced C# and Angular engineer.");
        var file = new ByteArrayContent(bytes);
        file.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        content.Add(file, "file", "resume.txt");
        content.Add(new StringContent("Primary CV"), "label");

        var response = await client.PostAsync("/api/resumes", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var detail = await response.Content.ReadFromJsonAsync<ResumeDetailDto>();
        detail!.Label.Should().Be("Primary CV");
        detail.IsDefault.Should().BeTrue();
        detail.Parsed!.Skills.Should().Contain("C#");
    }

    [Fact]
    public async Task Unsupported_upload_returns_415()
    {
        var client = _factory.CreateClient();

        using var content = new MultipartFormDataContent();
        var file = new ByteArrayContent([0x1, 0x2, 0x3]);
        file.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        content.Add(file, "file", "photo.png");

        var response = await client.PostAsync("/api/resumes", content);

        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
    }
}
