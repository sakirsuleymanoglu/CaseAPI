using CaseAPI.Abstractions.Users;
using CaseAPI.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CaseAPI.Services.Users;

public sealed class UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : IUserService
{
    public async Task<AppUser> GetAuthenticatedUserAsync() => await userManager.FindByNameAsync(httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "") ?? throw new Exception();

    public Guid GetAuthenticatedUserId()
    {
        return Guid.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception());
    }

    public string GetAuthenticatedUserName()
    {
        return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception();
    }
}
