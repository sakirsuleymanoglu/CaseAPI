using CaseAPI.Entities;

namespace CaseAPI.Abstractions.Users;

public interface IUserService
{
    Task<AppUser> GetAuthenticatedUserAsync();
    string GetAuthenticatedUserName();
    Guid GetAuthenticatedUserId();
}
