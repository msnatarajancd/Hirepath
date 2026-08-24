using Hirepath.Domain.Common;

namespace Hirepath.Domain.Settings;

/// <summary>
/// User-editable settings (single row). The API key lives here in the local
/// SQLite database and is never written to source or logged.
/// </summary>
public sealed class AppSettings : Entity
{
    public AiProvider Provider { get; set; } = AiProvider.Fake;
    public string? Model { get; set; }
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }

    /// <summary>"dark" or "light"; dark is the default.</summary>
    public string Theme { get; set; } = "dark";

    /// <summary>Demo Mode lets the whole app be explored without an API key.</summary>
    public bool DemoMode { get; set; } = true;

    /// <summary>Redact contact details before sending resumes to the model.</summary>
    public bool RedactPii { get; set; }
}
