using CaseAPI.Entities;
using CaseAPI.Models.Accounts;

namespace CaseAPI.Abstractions.Accounts;

public interface IAccountService
{
    Task<Account> CreateAsync(CreateAccount model);
    Task<List<ResultAccount>> GetAllAsync();
}
