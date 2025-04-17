using CaseAPI.Abstractions.Authentication;
using CaseAPI.Models.Authentication;
using CaseAPI.Models.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthenticationController(
    IAuthenticationService authenticationService
   ) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(Login model)
    {
        CreatedJwt jwt = await authenticationService.LoginAsync(model);
        return Ok(jwt);
    }
}
