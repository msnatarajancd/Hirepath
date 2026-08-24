using Hirepath.Domain.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hirepath.Infrastructure.Persistence.Configurations;

public sealed class JobDescriptionConfiguration : IEntityTypeConfiguration<JobDescription>
{
    public void Configure(EntityTypeBuilder<JobDescription> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Company).HasMaxLength(200);
        builder.Property(x => x.Title).HasMaxLength(200);
        builder.Property(x => x.Location).HasMaxLength(200);
        builder.Property(x => x.WorkMode).HasConversion<string>();
        builder.Property(x => x.Seniority).HasConversion<string>();

        builder.Property(x => x.Parsed)
            .HasConversion(JsonColumn.Converter<ParsedJd?>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<ParsedJd?>());
    }
}
