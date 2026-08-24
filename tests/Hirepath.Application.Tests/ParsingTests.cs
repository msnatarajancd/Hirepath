using Hirepath.Application.Intake;
using Hirepath.Application.Prompts;
using Hirepath.Infrastructure.Ai.Fake;

namespace Hirepath.Application.Tests;

public sealed class ParsingTests
{
    private static readonly PromptLibrary Prompts = new();

    private static FakeChatClient FakeClient() => new(new DemoScript());

    [Fact]
    public async Task Resume_parser_returns_structured_skills_from_the_fake_client()
    {
        var parser = new ResumeParser(FakeClient(), Prompts);

        var parsed = await parser.ParseAsync("some resume text about C# and Angular");

        parsed.Skills.Should().Contain("C#");
        parsed.Experience.Should().NotBeEmpty();
        parsed.Summary.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Jd_parser_separates_required_and_preferred_skills()
    {
        var parser = new JobDescriptionParser(FakeClient(), Prompts);

        var parsed = await parser.ParseAsync("we need a senior engineer with C# and Azure");

        parsed.RequiredSkills.Should().Contain("C#");
        parsed.PreferredSkills.Should().Contain("Kubernetes");
        parsed.YearsRequired.Should().Be(5);
    }

    [Fact]
    public async Task Empty_input_short_circuits_to_empty_parse()
    {
        var parser = new ResumeParser(FakeClient(), Prompts);

        var parsed = await parser.ParseAsync("   ");

        parsed.Skills.Should().BeEmpty();
    }

    [Fact]
    public void Prompt_library_renders_named_parameters_and_reports_version()
    {
        var rendered = Prompts.Render("parse-resume", new Dictionary<string, string>
        {
            ["resumeText"] = "HELLO-RESUME",
        });

        rendered.Id.Should().Be("parse-resume");
        rendered.Version.Should().Be(1);
        rendered.Text.Should().Contain("HELLO-RESUME");
        rendered.Text.Should().NotContain("{{resumeText}}");
    }
}
