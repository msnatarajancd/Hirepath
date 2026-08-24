using Hirepath.Application.Prompts;
using Hirepath.Domain.Jobs;
using Microsoft.Extensions.AI;

namespace Hirepath.Application.Intake;

/// <summary>Turns raw JD text into a structured <see cref="ParsedJd"/>.</summary>
public sealed class JobDescriptionParser(IChatClient chat, PromptLibrary prompts)
{
    public const string PromptId = "parse-jd";

    public async Task<ParsedJd> ParseAsync(string jdText, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(jdText))
        {
            return ParsedJd.Empty;
        }

        var prompt = prompts.Render(PromptId, new Dictionary<string, string>
        {
            ["jdText"] = jdText,
        });

        var response = await chat.GetResponseAsync<ParsedJd>(prompt.Text, cancellationToken: ct);
        return response.Result ?? ParsedJd.Empty;
    }
}
