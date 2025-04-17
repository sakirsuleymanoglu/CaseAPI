using CaseAPI.Abstractions.Accounts;
using CaseAPI.Abstractions.Users;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Exceptions.Accounts;
using CaseAPI.Models.Accounts;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts;

public sealed class AccountService(ApplicationDbContext context,
    IUserService userService) : IAccountService
{
    public async Task<Account> CreateAsync(CreateAccount model)
    {
        string code = await CreateCodeAsync();

        bool anyResult = await context.Accounts.AnyAsync(x => x.Code == code);

        if (anyResult)
            throw new AccountCodeNotUniqueException();

        Account account = new()
        {
            Id = Guid.NewGuid(),
            Code = code,
            AppUserId = userService.GetAuthenticatedUserId(),
            Title = model.Title,
            Balance = 0,
        };

        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        return account;
    }

    public async Task<List<ResultAccount>> GetAllAsync()
    {
        return await context.Accounts.AsNoTracking().Where(x => x.AppUserId == userService.GetAuthenticatedUserId()).Select(x => new ResultAccount
        {
            Id = x.Id,
            Code = x.Code,
            Title = x.Title,
            Balance = x.Balance,
        }).ToListAsync();
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
