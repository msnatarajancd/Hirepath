namespace Hirepath.Infrastructure.Ai.Fake;

/// <summary>
/// Canned, schema-shaped JSON returned by the demo/fake client. Entirely
/// invented — no real person or company. Keys are camelCase to match the
/// structured-output deserialiser.
/// </summary>
public static class DemoFixtures
{
    public const string ParsedResumeJson = """
    {
      "summary": "Mid-level full-stack engineer with 5 years building .NET and Angular web apps.",
      "skills": ["C#", ".NET", "ASP.NET Core", "Angular", "TypeScript", "EF Core", "SQL Server", "Azure", "REST", "Git"],
      "experience": [
        {
          "company": "Northwind Digital",
          "title": "Software Engineer",
          "startDate": "2021",
          "endDate": "Present",
          "bullets": [
            "Built ASP.NET Core APIs serving 2M requests/day.",
            "Led migration of an Angular 12 app to standalone components.",
            "Introduced EF Core and cut query times by 40%."
          ]
        },
        {
          "company": "Contoso Apps",
          "title": "Junior Developer",
          "startDate": "2019",
          "endDate": "2021",
          "bullets": [
            "Shipped features across a C# and Angular stack.",
            "Wrote unit and integration tests raising coverage to 70%."
          ]
        }
      ],
      "education": [
        { "institution": "State University", "credential": "B.Sc. Computer Science", "year": "2019" }
      ],
      "projects": ["Open-source Angular component library"],
      "certifications": ["Microsoft Certified: Azure Fundamentals"]
    }
    """;

    public const string ParsedJdJson = """
    {
      "responsibilities": [
        "Design and build cloud-native web services.",
        "Collaborate with product to ship customer-facing features.",
        "Mentor junior engineers and uphold code quality."
      ],
      "requiredSkills": ["C#", ".NET", "ASP.NET Core", "Angular", "SQL", "REST"],
      "preferredSkills": ["Azure", "Kubernetes", "Terraform", "GraphQL"],
      "yearsRequired": 5,
      "techStack": ["C#", ".NET", "Angular", "TypeScript", "Azure", "SQL Server"],
      "keywords": ["C#", ".NET", "ASP.NET Core", "Angular", "Azure", "Kubernetes", "REST", "microservices", "CI/CD"]
    }
    """;
}
