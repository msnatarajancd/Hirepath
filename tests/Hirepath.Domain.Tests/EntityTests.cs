using Hirepath.Domain.Profiles;

namespace Hirepath.Domain.Tests;

public sealed class EntityTests
{
    [Fact]
    public void New_entity_gets_a_version7_guid()
    {
        var profile = new Profile();

        profile.Id.Should().NotBe(Guid.Empty);
        profile.Id.Version.Should().Be(7);
    }

    [Fact]
    public void New_entity_stamps_created_and_updated()
    {
        var before = DateTimeOffset.UtcNow.AddSeconds(-1);

        var profile = new Profile();

        profile.CreatedAt.Should().BeAfter(before);
        profile.UpdatedAt.Should().BeAfter(before);
    }
}
