using Hirepath.Application.Abstractions;
using Hirepath.Domain.Settings;

namespace Hirepath.Application.Settings;

/// <summary>Reads and updates the single settings row. The API key is stored but
/// never returned to the client.</summary>
public sealed class SettingsService(ISettingsRepository repository)
{
    public async Task<SettingsDto> GetAsync(CancellationToken ct = default)
    {
        var settings = await repository.GetAsync(ct);
        return ToDto(settings);
    }

    public async Task<SettingsDto> UpdateAsync(
        UpdateSettingsRequest request,
        CancellationToken ct = default)
    {
        var settings = await repository.GetAsync(ct);

        settings.Provider = request.Provider;
        settings.Model = Trimmed(request.Model);
        settings.BaseUrl = Trimmed(request.BaseUrl);
        settings.Theme = string.IsNullOrWhiteSpace(request.Theme) ? "dark" : request.Theme;
        settings.DemoMode = request.DemoMode;
        settings.RedactPii = request.RedactPii;

        // Null = leave unchanged; empty = clear; otherwise set.
        if (request.ApiKey is not null)
        {
            settings.ApiKey = request.ApiKey.Length == 0 ? null : request.ApiKey;
        }

        await repository.SaveAsync(settings, ct);
        return ToDto(settings);
    }

    private static SettingsDto ToDto(AppSettings s) => new(
        s.Provider,
        s.Model,
        !string.IsNullOrEmpty(s.ApiKey),
        s.BaseUrl,
        s.Theme,
        s.DemoMode,
        s.RedactPii);

    private static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
