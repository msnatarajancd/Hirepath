using Microsoft.Extensions.AI;

namespace Hirepath.Infrastructure.Ai.Fake;

/// <summary>What a fake script chooses to return for a turn: prose and/or a
/// structured JSON body and/or tool calls. Deterministic by construction.</summary>
public sealed record FakeReply
{
    public string Text { get; init; } = string.Empty;
    public IReadOnlyList<FunctionCallContent> ToolCalls { get; init; } = [];

    public static FakeReply Say(string text) => new() { Text = text };
    public static FakeReply Json(string json) => new() { Text = json };
}

/// <summary>
/// Decides the fake response for a set of messages. A pure function of the
/// conversation, so every call is reproducible. Tests supply their own; Demo
/// Mode uses <see cref="DemoScript"/>.
/// </summary>
public interface IFakeChatScript
{
    FakeReply Respond(IReadOnlyList<ChatMessage> messages, ChatOptions? options);
}
