using System.Net;
using System.Net.Http.Json;
using Hirepath.Api.Endpoints;

namespace Hirepath.Api.Tests;

public sealed class MetaEndpointsTests(HirepathApiFactory factory)
    : IClassFixture<HirepathApiFactory>
{
    private readonly HirepathApiFactory _factory = factory;

    [Fact]
    public async Task Health_returns_ok()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/health");
        var body = await response.Content.ReadFromJsonAsync<HealthResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Status.Should().Be("ok");
    }

    [Fact]
    public async Task App_info_reports_name()
    {
        var client = _factory.CreateClient();

        var body = await client.GetFromJsonAsync<AppInfoResponse>("/api/app-info");

        body!.Name.Should().Be("Hirepath");
    }
}
