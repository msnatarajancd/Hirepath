using Hirepath.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hirepath.Infrastructure.Persistence.Configurations;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200);
        builder.Property(x => x.Seniority).HasConversion<string>();

        builder.Property(x => x.TargetRoles)
            .HasConversion(JsonColumn.Converter<List<string>>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<List<string>>());

        builder.Property(x => x.Links)
            .HasConversion(JsonColumn.Converter<List<ProfileLink>>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<List<ProfileLink>>());
    }
}
