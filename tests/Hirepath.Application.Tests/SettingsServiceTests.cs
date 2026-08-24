using Hirepath.Application.Settings;
using Hirepath.Application.Tests.Support;
using Hirepath.Domain.Common;
using Hirepath.Infrastructure.Persistence.Repositories;

namespace Hirepath.Application.Tests;

public sealed class SettingsServiceTests
{
    [Fact]
    public async Task Defaults_to_demo_mode_with_fake_provider()
    {
        using var ctx = new SqliteContext();
        var service = new SettingsService(new SettingsRepository(ctx.Db));

        var settings = await service.GetAsync();

        settings.DemoMode.Should().BeTrue();
        settings.Provider.Should().Be(AiProvider.Fake);
        settings.HasApiKey.Should().BeFalse();
        settings.Theme.Should().Be("dark");
    }

    [Fact]
    public async Task Update_stores_key_but_never_echoes_it()
    {
        using var ctx = new SqliteContext();
        var service = new SettingsService(new SettingsRepository(ctx.Db));

        var updated = await service.UpdateAsync(new UpdateSettingsRequest(
            AiProvider.Anthropic, "claude-opus-4-8", "sk-secret-value", null, "light", false, true));

        updated.HasApiKey.Should().BeTrue();
        updated.Provider.Should().Be(AiProvider.Anthropic);
        updated.RedactPii.Should().BeTrue();
        // The DTO type has no property that could carry the key back.
        updated.GetType().GetProperty("ApiKey").Should().BeNull();
    }

    [Fact]
    public async Task Null_key_on_update_leaves_the_stored_key_unchanged()
    {
        using var ctx = new SqliteContext();
        var service = new SettingsService(new SettingsRepository(ctx.Db));
        await service.UpdateAsync(new UpdateSettingsRequest(
            AiProvider.OpenAI, "gpt-4o", "sk-keep-me", null, "dark", false, false));

        var again = await service.UpdateAsync(new UpdateSettingsRequest(
            AiProvider.OpenAI, "gpt-4o", null, null, "dark", true, false));

        again.HasApiKey.Should().BeTrue();
        again.DemoMode.Should().BeTrue();
    }
}
