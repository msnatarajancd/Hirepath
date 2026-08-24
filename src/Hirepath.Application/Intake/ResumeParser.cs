using Hirepath.Application.Prompts;
using Hirepath.Domain.Resumes;
using Microsoft.Extensions.AI;

namespace Hirepath.Application.Intake;

/// <summary>
/// Turns raw resume text into a structured <see cref="ParsedResume"/> using the
/// model's schema-constrained output. One entry point, one prompt version.
/// </summary>
public sealed class ResumeParser(IChatClient chat, PromptLibrary prompts)
{
    public const string PromptId = "parse-resume";

    public async Task<ParsedResume> ParseAsync(string resumeText, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(resumeText))
        {
            return ParsedResume.Empty;
        }

        var prompt = prompts.Render(PromptId, new Dictionary<string, string>
        {
            ["resumeText"] = resumeText,
        });

        var response = await chat.GetResponseAsync<ParsedResume>(prompt.Text, cancellationToken: ct);
        return response.Result ?? ParsedResume.Empty;
    }
}
