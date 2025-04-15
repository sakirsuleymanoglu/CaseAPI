using CaseAPI.Entities;
using CaseAPI.Models.Jwt;

namespace CaseAPI.Abstractions.Jwt;

public interface IJwtService
{
    Task<CreatedJwt> CreateAsync(AppUser user);
}
