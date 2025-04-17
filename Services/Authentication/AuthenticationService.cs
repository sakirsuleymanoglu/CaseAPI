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
    private static UserNameOrPasswordIncorrectException LoginException => new();

    public async Task<CreatedJwt> LoginAsync(Login model)
    {
        AppUser? user = await userManager.FindByNameAsync(model.UserName) ?? throw LoginException;

        string decryptedPassword = encryptionService.Decrypt(model.Password);

        bool checkPasswordResult = await userManager.CheckPasswordAsync(user, decryptedPassword);

        if (!checkPasswordResult)
            throw LoginException;

        return await jwtService.CreateAsync(user);
    }
}
