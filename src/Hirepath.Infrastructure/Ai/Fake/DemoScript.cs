using Microsoft.Extensions.AI;

namespace Hirepath.Infrastructure.Ai.Fake;

/// <summary>
/// The default deterministic script that powers Demo Mode and the parsing tests.
/// It recognises the parse prompts by their task marker and returns canned,
/// schema-shaped JSON; anything else gets a friendly, honest reply.
/// </summary>
public sealed class DemoScript : IFakeChatScript
{
    public FakeReply Respond(IReadOnlyList<ChatMessage> messages, ChatOptions? options)
    {
        var text = string.Join("\n", messages.Select(m => m.Text));

        if (text.Contains("Task: parse-resume", StringComparison.Ordinal))
        {
            return FakeReply.Json(DemoFixtures.ParsedResumeJson);
        }

        if (text.Contains("Task: parse-jd", StringComparison.Ordinal))
        {
            return FakeReply.Json(DemoFixtures.ParsedJdJson);
        }

        return FakeReply.Say(
            "I'm running in Demo Mode with a scripted assistant, so I can show the "
            + "whole experience without an API key. Add a provider key in Settings for "
            + "live analysis.");
    }
}
