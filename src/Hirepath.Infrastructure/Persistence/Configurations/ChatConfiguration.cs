using Hirepath.Domain.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hirepath.Infrastructure.Persistence.Configurations;

public sealed class ChatThreadConfiguration : IEntityTypeConfiguration<ChatThread>
{
    public void Configure(EntityTypeBuilder<ChatThread> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(300);

        builder.HasMany(x => x.Messages)
            .WithOne()
            .HasForeignKey(m => m.ThreadId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Role).HasConversion<string>();

        builder.Property(x => x.ToolCalls)
            .HasConversion(JsonColumn.Converter<List<ToolCallRecord>>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<List<ToolCallRecord>>());

        builder.Property(x => x.Widgets)
            .HasConversion(JsonColumn.Converter<List<WidgetBlock>>())
            .Metadata.SetValueComparer(JsonColumn.Comparer<List<WidgetBlock>>());

        builder.HasIndex(x => new { x.ThreadId, x.Sequence });
    }
}
