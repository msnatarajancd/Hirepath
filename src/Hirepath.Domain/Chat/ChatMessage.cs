using Hirepath.Domain.Common;

namespace Hirepath.Domain.Chat;

/// <summary>
/// One message in a thread. Carries the visible text plus the tool calls and
/// rendered widgets, so a reopened thread looks exactly as it did.
/// </summary>
public sealed class ChatMessage : Entity
{
    public Guid ThreadId { get; set; }
    public ChatRole Role { get; set; }
    public string Text { get; set; } = string.Empty;

    /// <summary>Ordering within the thread (monotonic, gap-tolerant).</summary>
    public int Sequence { get; set; }

    public List<ToolCallRecord> ToolCalls { get; set; } = [];

    /// <summary>Rendered widget blocks, in stream order. See Appendix A.</summary>
    public List<WidgetBlock> Widgets { get; set; } = [];
}

/// <summary>A record of a tool the assistant called, with arguments and result.</summary>
public sealed record ToolCallRecord
{
    public string ToolName { get; init; } = string.Empty;
    public string ArgumentsJson { get; init; } = "{}";
    public string? ResultJson { get; init; }
    public bool Failed { get; init; }
}

/// <summary>
/// A typed widget block persisted with the message. The name resolves to an
/// Angular component through the widget registry; version keeps old messages
/// rendering after a widget changes shape.
/// </summary>
public sealed record WidgetBlock
{
    public string Widget { get; init; } = string.Empty;
    public int Version { get; init; } = 1;

    /// <summary>Typed payload as raw JSON, produced server-side, never by the model.</summary>
    public string PayloadJson { get; init; } = "{}";
}
