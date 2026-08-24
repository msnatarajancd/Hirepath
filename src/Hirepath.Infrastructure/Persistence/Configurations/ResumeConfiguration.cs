using Hirepath.Domain.Resumes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hirepath.Infrastructure.Persistence.Configurations;

public sealed class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Label).HasMaxLength(200);
        builder.Property(x => x.FileName).HasMaxLength(400);
        builder.Property(x => x.ContentType).HasMaxLength(120);
        builder.Property(x => x.StoredPath).HasMaxLength(1000);

        builder.Property(x => x.Parsed)
            .HasConversion(JsonColumn.Converter<ParsedResume?>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<ParsedResume?>());

        builder.HasIndex(x => x.IsDefault);
    }
}
