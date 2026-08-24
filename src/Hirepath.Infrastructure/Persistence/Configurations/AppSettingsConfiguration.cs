using Hirepath.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hirepath.Infrastructure.Persistence.Configurations;

public sealed class AppSettingsConfiguration : IEntityTypeConfiguration<AppSettings>
{
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Provider).HasConversion<string>();
        builder.Property(x => x.Model).HasMaxLength(200);
        builder.Property(x => x.BaseUrl).HasMaxLength(500);
        builder.Property(x => x.Theme).HasMaxLength(20);
    }
}
