using CaseAPI.Abstractions.Users;
using CaseAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace CaseAPI.Services.Users;

public sealed class UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : IUserService
{
    public async Task<AppUser> GetAuthenticatedUserAsync() => await userManager.FindByNameAsync(httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "") ?? throw new Exception();
}
