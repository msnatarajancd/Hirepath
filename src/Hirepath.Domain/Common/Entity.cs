namespace Hirepath.Domain.Common;

/// <summary>
/// Base for every persisted entity. Ids are GUID v7 (time-ordered) so rows
/// sort by creation without a separate sequence column. Timestamps are UTC.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
