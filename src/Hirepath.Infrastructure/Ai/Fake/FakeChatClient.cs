using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;

namespace Hirepath.Infrastructure.Ai.Fake;

/// <summary>
/// A real <see cref="IChatClient"/> implementation (not a mock framework) that
/// returns deterministic scripted responses. Every automated test uses it, and
/// it powers Demo Mode. Implementing the interface directly is the clearest way
/// to learn its contract, including the streaming and function-calling paths.
/// </summary>
public sealed class FakeChatClient(IFakeChatScript script) : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var reply = script.Respond([.. messages], options);
        var message = new ChatMessage(ChatRole.Assistant, BuildContents(reply));
        var response = new ChatResponse(message)
        {
            FinishReason = reply.ToolCalls.Count > 0 ? ChatFinishReason.ToolCalls : ChatFinishReason.Stop,
            ModelId = "fake-demo",
        };
        return Task.FromResult(response);
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reply = script.Respond([.. messages], options);

        // Stream prose word-by-word so the UI's streaming path is exercised.
        foreach (var chunk in Chunk(reply.Text))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return new ChatResponseUpdate(ChatRole.Assistant, chunk);
            await Task.Yield();
        }

        if (reply.ToolCalls.Count > 0)
        {
            yield return new ChatResponseUpdate(ChatRole.Assistant, [.. reply.ToolCalls]);
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType.IsInstanceOfType(this) ? this : null;

    public void Dispose()
    {
        // Nothing to release.
    }

    private static List<AIContent> BuildContents(FakeReply reply)
    {
        var contents = new List<AIContent>();
        if (!string.IsNullOrEmpty(reply.Text))
        {
            contents.Add(new TextContent(reply.Text));
        }
        foreach (var call in reply.ToolCalls)
        {
            contents.Add(call);
        }
        return contents;
    }

    private static IEnumerable<string> Chunk(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        var words = text.Split(' ');
        for (var i = 0; i < words.Length; i++)
        {
            yield return i == 0 ? words[i] : " " + words[i];
        }
    }
}
