using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hirepath.Application.Prompts;

/// <summary>A rendered prompt: its id, version, and the filled-in text.</summary>
public sealed record RenderedPrompt(string Id, int Version, string Text);

/// <summary>
/// Loads versioned prompt files (embedded .md) and renders them with named
/// parameters. No prompt strings live inline at call sites — they come through
/// here by id. The id/version header lets caching invalidate cleanly.
/// </summary>
public sealed partial class PromptLibrary
{
    private static readonly Assembly OwningAssembly = typeof(PromptLibrary).Assembly;
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public RenderedPrompt Render(string id, IReadOnlyDictionary<string, string> parameters)
    {
        var raw = _cache.GetOrAdd(id, Load);
        var (headerId, version, body) = ParseHeader(raw, id);

        var text = body;
        foreach (var (key, value) in parameters)
        {
            text = text.Replace("{{" + key + "}}", value, StringComparison.Ordinal);
        }

        return new RenderedPrompt(headerId, version, text.Trim());
    }

    private static string Load(string id)
    {
        var resourceName = $"Hirepath.Application.Prompts.{id}.md";
        using var stream = OwningAssembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Prompt '{id}' not found ({resourceName}).");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static (string Id, int Version, string Body) ParseHeader(string raw, string fallbackId)
    {
        var match = HeaderPattern().Match(raw);
        if (!match.Success)
        {
            return (fallbackId, 1, raw);
        }

        var id = match.Groups["id"].Value.Trim();
        var version = int.Parse(match.Groups["version"].Value);
        var body = raw[match.Length..];
        return (id, version, body);
    }

    [GeneratedRegex(@"^<!--\s*id:\s*(?<id>[\w-]+);\s*version:\s*(?<version>\d+)\s*-->\s*",
        RegexOptions.Singleline)]
    private static partial Regex HeaderPattern();
}
