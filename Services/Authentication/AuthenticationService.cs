using CaseAPI.Abstractions.Authentication;
using CaseAPI.Abstractions.Encryption;
using CaseAPI.Abstractions.Jwt;
using CaseAPI.Entities;
using CaseAPI.Exceptions.Authentication;
using CaseAPI.Models.Authentication;
using CaseAPI.Models.Jwt;
using Microsoft.AspNetCore.Identity;

namespace CaseAPI.Services.Authentication;

public sealed class AuthenticationService(
    UserManager<AppUser> userManager,
    IEncryptionService encryptionService,
    IJwtService jwtService) : IAuthenticationService
{
    private void LoginException() => throw new UserNameOrPasswordIncorrectException();

    public async Task<CreatedJwt> LoginAsync(Login model)
    {
        AppUser? user = await userManager.FindByNameAsync(model.UserName);

        if (user == null) LoginException();

        string? decryptedPassword = encryptionService.Decrypt(model.Password);

        if (decryptedPassword == null) LoginException();

        bool checkPasswordResult = await userManager.CheckPasswordAsync(user, decryptedPassword);

        if (!checkPasswordResult) LoginException();

        return await jwtService.CreateAsync(user);
    }
}
