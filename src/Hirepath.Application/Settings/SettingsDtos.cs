using Hirepath.Domain.Common;

namespace Hirepath.Application.Settings;

/// <summary>Settings as shown to the UI. The API key is never echoed back;
/// only whether one is set.</summary>
public sealed record SettingsDto(
    AiProvider Provider,
    string? Model,
    bool HasApiKey,
    string? BaseUrl,
    string Theme,
    bool DemoMode,
    bool RedactPii);

/// <summary>Settings update from the UI. A null ApiKey leaves the stored key
/// unchanged; an empty string clears it.</summary>
public sealed record UpdateSettingsRequest(
    AiProvider Provider,
    string? Model,
    string? ApiKey,
    string? BaseUrl,
    string Theme,
    bool DemoMode,
    bool RedactPii);
