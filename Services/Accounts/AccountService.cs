using CaseAPI.Abstractions.Accounts;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Models.Accounts;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts;

public sealed class AccountService(ApplicationDbContext context) : IAccountService
{
    public async Task<Account> CreateAsync(CreateAccount model)
    {
        string code = await CreateCodeAsync();

        bool anyResult = await context.Accounts.AnyAsync(x => x.Code == code);

        if (anyResult)
            throw new Exception();

        Account account = new()
        {
            Id = Guid.NewGuid(),
            Code = code,
            AppUserId = Guid.Parse(model.AppUserId),
            Title = model.Title,
            Balance = 0,
        };

        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        return account;
    }

    private async Task<string> CreateCodeAsync()
    {
        Random random = new();
        string code;

        do
        {
            code = random.Next(10000000, 100000000).ToString();

        } while (await context.Accounts.AnyAsync(x => x.Code == code));

        return code;
    }
}
