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
    public async Task<CreatedJwt> LoginAsync(Login model)
    {
        bool isValid = true;

        void SetIsValidFalse() => isValid = false;

        AppUser? user = await userManager.FindByNameAsync(model.UserName);

        if (user == null)
            SetIsValidFalse();

        string? decryptedPassword = null;

        if (isValid)
        {
            decryptedPassword = encryptionService.Decrypt(model.Password);

            if (decryptedPassword == null)
                SetIsValidFalse();
        }

        if (isValid)
        {
            bool checkPasswordResult = await userManager.CheckPasswordAsync(user!, decryptedPassword!);

            if (!checkPasswordResult)
                SetIsValidFalse();
        }

        if (!isValid)
            throw new UserNameOrPasswordIncorrectException();

        return await jwtService.CreateAsync(user!);
    }
}
