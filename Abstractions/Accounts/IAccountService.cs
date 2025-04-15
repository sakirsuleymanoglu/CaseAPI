using CaseAPI.Entities;

namespace CaseAPI.Abstractions.Accounts;

public interface IAccountService
{
    Task<Account> CreateAsync(string appUserId);
}
