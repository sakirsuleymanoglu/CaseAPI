using CaseAPI.Models.Authentication;
using CaseAPI.Models.Jwt;

namespace CaseAPI.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<CreatedJwt> LoginAsync(Login model);
}
