using System.Linq.Expressions;
using Hirepath.Domain.Analyses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hirepath.Infrastructure.Persistence.Configurations;

public sealed class MatchAnalysisConfiguration : IEntityTypeConfiguration<MatchAnalysis>
{
    public void Configure(EntityTypeBuilder<MatchAnalysis> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Verdict).HasConversion<string>();
        builder.Property(x => x.ModelId).HasMaxLength(200);
        builder.Property(x => x.PromptVersion).HasMaxLength(80);

        ConfigureJson(builder, x => x.Dimensions);
        ConfigureJson(builder, x => x.Strengths);
        ConfigureJson(builder, x => x.Gaps);
        ConfigureJson(builder, x => x.ResumeSuggestions);
        ConfigureJson(builder, x => x.PrepPlan);

        builder.Property(x => x.Deterministic)
            .HasConversion(JsonColumn.Converter<DeterministicScores>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<DeterministicScores>());

        builder.HasIndex(x => x.ResumeId);
        builder.HasIndex(x => x.JobDescriptionId);
    }

    private static void ConfigureJson<T>(
        EntityTypeBuilder<MatchAnalysis> builder,
        Expression<Func<MatchAnalysis, List<T>>> property)
    {
        builder.Property(property)
            .HasConversion(JsonColumn.Converter<List<T>>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<List<T>>());
    }
}
