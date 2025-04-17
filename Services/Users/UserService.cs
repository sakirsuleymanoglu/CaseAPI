using CaseAPI.Abstractions.Users;
using CaseAPI.Entities;
using CaseAPI.Exceptions.Common;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CaseAPI.Services.Users;

public sealed class UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : IUserService
{
    public async Task<AppUser> GetAuthenticatedUserAsync() => await userManager.FindByNameAsync(httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "") ?? throw new UnauthorizedException();

    public Guid GetAuthenticatedUserId() => Guid.Parse(httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedException());

    public string GetAuthenticatedUserName() => httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedException();
}
