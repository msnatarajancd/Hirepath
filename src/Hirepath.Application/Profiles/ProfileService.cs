using Hirepath.Application.Abstractions;
using Hirepath.Domain.Profiles;

namespace Hirepath.Application.Profiles;

public sealed class ProfileService(IProfileRepository repository)
{
    public async Task<ProfileDto> GetAsync(CancellationToken ct = default)
    {
        var profile = await repository.GetOrCreateAsync(ct);
        return ToDto(profile);
    }

    public async Task<ProfileDto> UpdateAsync(
        UpdateProfileRequest request,
        CancellationToken ct = default)
    {
        var profile = await repository.GetOrCreateAsync(ct);

        profile.Name = request.Name.Trim();
        profile.Email = request.Email?.Trim();
        profile.Location = request.Location?.Trim();
        profile.YearsOfExperience = request.YearsOfExperience;
        profile.Seniority = request.Seniority;
        profile.TargetRoles = [.. request.TargetRoles];
        profile.Links = [.. request.Links];

        await repository.SaveAsync(profile, ct);
        return ToDto(profile);
    }

    private static ProfileDto ToDto(Profile p) => new(
        p.Name,
        p.Email,
        p.Location,
        p.YearsOfExperience,
        p.Seniority,
        p.TargetRoles,
        p.Links);
}
